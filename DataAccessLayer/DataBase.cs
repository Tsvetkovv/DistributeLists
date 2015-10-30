using System;
using System.Collections.Generic;
using System.Configuration;
using Entities;
using InterfacesLibrary;

namespace DataAccessLayer
{
    class DataBase: IDataAsccessLayer
    {
        private string _connectionString;

        public DataBase()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["NoteDB"].ConnectionString;
        }


        public bool Create(User addingUser)
        {
            throw new NotImplementedException();
        }

        public bool Create(DistributeList addingDistributeList)
        {
            throw new NotImplementedException();
        }

        public bool Update(DistributeList updatingDistributeList)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDistributeList(Guid id)
        {
            throw new NotImplementedException();
        }

        public DistributeList GetDistributeListById(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<DistributeList> GetDistributeLists()
        {
            throw new NotImplementedException();
        }

        public List<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
