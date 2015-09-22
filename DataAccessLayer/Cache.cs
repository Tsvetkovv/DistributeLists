using System;
using System.Collections.Generic;
using Entities;

namespace DataAccessLayer
{
    public class Cache: IDisposable
    {
        readonly FileIO _dataFile = new FileIO();
        public readonly List<User> Users;
        public readonly List<DistributeList> DistributeLists;

        public Cache()
        {
            Users = _dataFile.GetUsers();
            DistributeLists = _dataFile.GetDistributeLists();
        }

        /// <summary>
        /// Save cache in a file
        /// </summary>
        public void Dispose()
        {
            _dataFile.SaveAll(Users, DistributeLists);
        }
    }
}
