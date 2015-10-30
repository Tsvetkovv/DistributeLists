using System;
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

        public void Create(User addingUser)
        {
            _data.Create(addingUser);
        }
        public void Create(DistributeList addingDistributeList)
        {
            _data.Create(addingDistributeList);
        }

        public bool AddUserToDistributeList(User addingUser, DistributeList distributeList)
        {
            distributeList.SubscribersList.Add(addingUser);
            return _data.Update(distributeList);
        }

        public bool ChangeTitle(Guid idOfList, string newTitle)
        {
            var newList = GetDistributeLists().FirstOrDefault(list => list.Id.Equals(idOfList));
            return _data.Update(newList);
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
            var distributeListsOfUser = new List<DistributeList>(_data.GetDistributeLists().Where(list => list.SubscribersList.Exists(user1 => user1.Id == user.Id)));

            return distributeListsOfUser;
        }

        public List<DistributeList> GetDistrListWithoutUsers(List<DistributeList> lists, User selectedUser)
        {
            var selectedLists =
                lists.Where(list => list.SubscribersList.TrueForAll(user => user.Id != selectedUser.Id));
            return selectedLists.ToList();
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
            _data.Save();
        }
    }
}
