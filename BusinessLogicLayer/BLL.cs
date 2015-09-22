using System.Collections.Generic;
using System.Linq;
using Entities;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class BLL
    {
        readonly Cache _data = new Cache();

        public void AddUser(User addingUser)
        {
            _data.Users.Add(addingUser);
        }

        public void AddUserToDistributeList(User addingUser, DistributeList distributeList)
        {
            distributeList.SubscribersList.Add(addingUser);
        }

        public List<DistributeList> GetDistributeLists()
        {
            return _data.DistributeLists;
        }

        public List<User> GetUsers()
        {
            return _data.Users;
        }

        public List<DistributeList> GetDistributeListsOfUser(User user)
        {
            var distributeListsOfUser = new List<DistributeList>(_data.DistributeLists.Where(list => list.SubscribersList.Exists(user1 => user1.Login == user.Login)));

            return distributeListsOfUser;
        }

        /// <summary>
        /// Кандидатом на удаление считается список рассылки, в котором есть только одна запись пользователя
        /// </summary>
        public List<DistributeList> GetCandidatesForDeletion()
        {
            return new List<DistributeList>(_data.DistributeLists.Where(list => list.SubscribersList.Count == 1));
        }


        public void SaveChanges()
        {
            _data.Dispose();
        }
    }
}
