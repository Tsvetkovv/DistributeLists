using System.Collections.Generic;
using System.Linq;
using Entities;
using DataAccessLayer;
using InterfacesLibrary;

namespace BusinessLogicLayer
{
    public class BLL
    {
        readonly IDataAsccessLayer _data = new File();

        public void AddUser(User addingUser)
        {
            _data.AddUser(addingUser);
        }

        public void AddUserToDistributeList(User addingUser, DistributeList distributeList)
        {
            _data.AddUserToDistributeList(addingUser, distributeList);
        }

        public List<DistributeList> GetDistributeLists()
        {
            return _data.GetDistributeLists();
        }

        public List<User> GetUsers()
        {
            return _data.GetUsers();
        }

        public List<DistributeList> GetDistributeListsOfUser(User user)
        {
            var distributeListsOfUser = new List<DistributeList>(_data.GetDistributeLists().Where(list => list.SubscribersList.Exists(user1 => user1.Login == user.Login)));

            return distributeListsOfUser;
        }

        /// <summary>
        /// Кандидатом на удаление считается список рассылки, в котором есть только одна запись пользователя
        /// </summary>
        public List<DistributeList> GetCandidatesForDeletion()
        {
            return new List<DistributeList>(_data.GetDistributeLists().Where(list => list.SubscribersList.Count == 1));
        }


        public void SaveChanges()
        {
            _data.SaveAllFromCache();
        }
    }
}
