using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Entities;
using InterfacesLibrary;

namespace DataAccessLayer
{
    public class File : IDataAsccessLayer
    {
        private static readonly string FolderPath = string.Format(AppDomain.CurrentDomain.BaseDirectory);
        private static readonly string UsersFileName = string.Format(FolderPath + "users.dat");
        private static readonly string DistributeListsFileName = string.Format(FolderPath + "distribute_List.dat");
        private static readonly FileInfo UsersFile = new FileInfo(UsersFileName);
        private static readonly FileInfo DistribureListsFile = new FileInfo(DistributeListsFileName);
        private readonly List<User> _users;
        private readonly List<DistributeList> _distributeLists;

        public File()
        {
            _users = new List<User>();
            _distributeLists = new List<DistributeList>();

            if (UsersFile.Exists)
            {
                _users = GetUsersFromFile();
            }
            else
            {
                UsersFile.Create().Dispose();
            }

            if (DistribureListsFile.Exists)
            {
                _distributeLists = GetDistributeListsFromFile();
            }
            else
            {
                DistribureListsFile.Create().Dispose();
            }
        }

        public bool Create(User addingUser)
        {
            _users.Add(addingUser);
            return true;
        }

        public bool Create(DistributeList addingDistributeList)
        {
            _distributeLists.Add(addingDistributeList);
            return true;
        }

        public bool Update(DistributeList updatingDistributeList)
        {
            var list = _distributeLists.First(list1 => list1.Id.Equals(updatingDistributeList.Id));
            list.Description = updatingDistributeList.Description;
            list.SubscribersList = updatingDistributeList.SubscribersList;
            list.Title = updatingDistributeList.Title;
            return true;
        }

        public bool DeleteDistributeList(Guid idOfList)
        {
            _distributeLists.RemoveAll(list => list.Id.Equals(idOfList));
            return true;
        }

        public DistributeList GetDistributeListById(Guid id)
        {
            return _distributeLists.FirstOrDefault(list => list.Id.Equals(id));
        }

        public bool AddUserToDistributeList(User addingUser, DistributeList distributeList)
        {
            _distributeLists.First(list => list.Id.Equals(distributeList.Id)).SubscribersList.Add(addingUser);
            return true;
        }

        private List<User> GetUsersFromFile()
        {
            var userList = new List<User>();

            // Read the file and display it line by line.
            // and parsing users' data
            using (var file = UsersFile.OpenText())
            {
                string currentLine;

                while (!string.IsNullOrWhiteSpace(currentLine = file.ReadLine()))
                {
                    string[] currentUserData = currentLine.Split(';');

                    Guid userGuid = Guid.Parse(currentUserData[0]);
                    string login = currentUserData[1];
                    string firstName = currentUserData[2];
                    string lastName = currentUserData[3];
                    string middleName = currentUserData[4];

                    // check unique login in userList
                    if (userList.Any(user => user.Login.Equals(login)))
                    {
                        // #TODO it may be better. 
                        throw new IOException("login is nonunique");
                    }

                    userList.Add(new User(userGuid, login, firstName, lastName, middleName));
                }
            }

            return userList;
        }

        public List<User> GetUsers()
        {
            return _users;
        }

        private List<DistributeList> GetDistributeListsFromFile()
        {
            //it need for send user-instance using only user's login
            var userList = GetUsers();
            var distributeLists = new List<DistributeList>();

            // Read the file and display it line by line.
            // and parsing data
            // Fields separated using ;
            //Guids separated using space
            using (var file = DistribureListsFile.OpenText())
            {
                string currentLine;

                while ((currentLine = file.ReadLine()) != null)
                {
                    string[] currentSubscribingData = currentLine.Split(';');

                    Guid guid = Guid.Parse(currentSubscribingData[0]);
                    string title = currentSubscribingData[1];
                    string description = currentSubscribingData[2];
                    string[] ids = currentSubscribingData[3].Split();

                    distributeLists.Add(new DistributeList(guid, title, description));
                    foreach (var id in ids.Where(id => id != ""))
                    {
                        distributeLists.Last().SubscribersList.Add(userList.Find(user => user.Id.Equals(Guid.Parse(id))));
                    }
                }
            }

            return new List<DistributeList>(distributeLists);
        }

        public List<DistributeList> GetDistributeLists()
        {
            return _distributeLists;
        }

        private void SaveDistributeLists(List<DistributeList> savingDistributeLists)
        {

            DistribureListsFile.Delete();
            using (var file = DistribureListsFile.CreateText())
            {
                foreach (var savingList in savingDistributeLists)
                {
                    StringBuilder idsOfSubscribers = new StringBuilder();
                    foreach (var user in savingList.SubscribersList)
                    {
                        idsOfSubscribers.AppendFormat("{0} ", user.Id);
                    }

                    file.WriteLine("{0};{1};{2};{3}", savingList.Id, savingList.Title, savingList.Description, idsOfSubscribers);
                }
                file.Flush();
            }

        }

        private void SaveUserList(List<User> savingUsers)
        {
            UsersFile.Delete();
            using (var file = UsersFile.CreateText())
            {
                foreach (var currentUser in savingUsers)
                {
                    file.WriteLine("{0};{1};{2};{3};{4}", currentUser.Id, currentUser.Login, currentUser.FirstName, currentUser.LastName, currentUser.MiddleName);
                }
                file.Flush();
            }
        }

        public void Save()
        {
            SaveUserList(_users);
            SaveDistributeLists(_distributeLists);
        }
    }
}
