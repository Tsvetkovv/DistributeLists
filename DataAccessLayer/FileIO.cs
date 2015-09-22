using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Entities;

namespace DataAccessLayer
{
    public class FileIO
    {
        private static readonly string FolderPath = string.Format(AppDomain.CurrentDomain.BaseDirectory);
        private static readonly string UsersFileName = string.Format(FolderPath + "users.dat");
        private static readonly string DistributeListsFileName = string.Format(FolderPath + "distribute_List.dat");

        public FileIO()
        {
            if (!File.Exists(DistributeListsFileName))
                File.Create(DistributeListsFileName);
            if (!File.Exists(UsersFileName))
                File.Create(UsersFileName);
        }

        /// <summary>
        ///  Get users list from file
        /// </summary>
        internal List<User> GetUsers()
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

        internal List<DistributeList> GetDistributeLists()
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

            return distributeLists;
        }

        internal void SaveDistributeLists(List<DistributeList> savingDistributeLists)
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

        internal void SaveUserList(List<User> savingUsers)
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

        internal void SaveAll(List<User> savingUsers, List<DistributeList> savingDistributeLists)
        {
            SaveUserList(savingUsers);
            SaveDistributeLists(savingDistributeLists);
        }
    }
}
