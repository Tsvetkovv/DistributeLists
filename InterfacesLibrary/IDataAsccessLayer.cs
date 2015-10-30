using System;
using System.Collections.Generic;
using Entities;

namespace InterfacesLibrary
{
    public interface IDataAsccessLayer
    {
        bool Create(User addingUser);
        bool Create(DistributeList addingDistributeList);
        bool Update(DistributeList updatingDistributeList);
        bool DeleteDistributeList(Guid id);
        DistributeList GetDistributeListById(Guid id);
        List<DistributeList> GetDistributeLists();
        List<User> GetUsers();
        void Save();
    }
}