using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Entities;
using InterfacesLibrary;

namespace DataAccessLayer
{
    public class File: IDataAsccessLayer
    {
        private static readonly string FolderPath = string.Format(AppDomain.CurrentDomain.BaseDirectory);
        private static readonly string UsersFileName = string.Format(FolderPath + "users.dat");
        private static readonly string DistributeListsFileName = string.Format(FolderPath + "distribute_List.dat");

        private readonly List<User> _users;
        private readonly List<DistributeList> _distributeLists;

        public File()
        {
            if (!System.IO.File.Exists(DistributeListsFileName))
                System.IO.File.Create(DistributeListsFileName);
            if (!System.IO.File.Exists(UsersFileName))
                System.IO.File.Create(UsersFileName);

            _users = GetUsers();
            _distributeLists = GetDistributeLists();
        }

        public void AddUser(User addingUser)
        {
            _users.Add(addingUser);
        }

        public void AddUserToDistributeList(User addingUser, DistributeList distributeList)
        {
            distributeList.SubscribersList.Add(addingUser);
        }

        /// <summary>
        ///  Get users list from file
        /// </summary>
        public List<User> GetUsers()
        {
            var userList = new List<User>();

            // Read the file and display it line by line.
            // and parsing users' data
            using (var file = new StreamReader(UsersFileName, Encoding.UTF8))
            {
                string currentLine;

                while ((currentLine = file.ReadLine()) != null)
                {
                    string[] currentUserData = currentLine.Split();

                    string login = currentUserData[0];
                    string firstName = currentUserData[1];
                    string lastName = currentUserData[2];
                    string middleName = currentUserData[3];

                    // check unique login in userList
                    if (userList.Any(user => user.Login.Equals(login)))
                    {
                        // #TODO it may be better. 
                        throw new IOException("login is nonunique");
                    }

                    userList.Add(new User(login, firstName, lastName, middleName));
                }

            }

            return userList;
        }

        public List<DistributeList> GetDistributeLists()
        {
            //it need for send user-instance using only user's login
            var userList = GetUsers();
            var distributeLists = new List<DistributeList>();

            // Read the file and display it line by line.
            // and parsing data
            // Fields separated using ;
            // Logins separated using space
            using (var file = new StreamReader(DistributeListsFileName, Encoding.UTF8))
            {
                string currentLine;

                while ((currentLine = file.ReadLine()) != null)
                {
                    string[] currentSubscribingData = currentLine.Split(';');

                    string title = currentSubscribingData[0];
                    string description = currentSubscribingData[1];
                    string[] logins = currentSubscribingData[2].Split();

                    distributeLists.Add(new DistributeList(title, description));
                    foreach (var login in logins)
                    {
                        if (login != "")
                            distributeLists.Last().SubscribersList.Add(userList.Find(user => user.Login.Equals(login)));
                    }

                }

            }

            return new List<DistributeList>(distributeLists); ;
        }

        public void SaveDistributeLists(List<DistributeList> savingDistributeLists)
        {
            using (var file = new StreamWriter(DistributeListsFileName, false, Encoding.UTF8))
            {
                foreach (var savingList in savingDistributeLists)
                {
                    StringBuilder logins = new StringBuilder();
                    foreach (var user in savingList.SubscribersList)
                    {
                        logins.AppendFormat("{0} ", user.Login);
                    }

                    file.WriteLine("{0};{1};{2}", savingList.Title, savingList.Description, logins);
                }
                file.Flush();
            }

        }

        public void SaveUserList(List<User> savingUsers)
        {
            using (var file = new StreamWriter(UsersFileName, false, Encoding.UTF8))
            {
                foreach (var currentUser in savingUsers)
                {
                    file.WriteLine("{0} {1} {2} {3}", currentUser.Login, currentUser.FirstName, currentUser.LastName, currentUser.MiddleName);
                }
                file.Flush();
            }
        }

        public void SaveAllFromCache()
        {
            SaveUserList(_users);
            SaveDistributeLists(_distributeLists);
        }
    }
}
