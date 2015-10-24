using System.Collections.Generic;
using Entities;

namespace InterfacesLibrary
{
    public interface IDataAsccessLayer
    {
        void AddUser(User addingUser);
        void AddUserToDistributeList(User addingUser, DistributeList distributeList);
        List<DistributeList> GetDistributeLists();
        List<User> GetUsers();
        void Save();
    }
}