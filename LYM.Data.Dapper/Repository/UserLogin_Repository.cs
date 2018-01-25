using System;
using System.Collections.Generic;
using System.Text;
using LYM.DomainDapper.Repository;
using Dapper;
using System.Data;

namespace LYM.Data.Dapper.Repository
{
   public  class UserLogin_Repository:IUserLogin_Repository
    {
        IDbConnection _connection;
        public UserLogin_Repository(IDbConnection connection) {

            _connection = connection;
            _connection.ConnectionString = "";
        }

        public void TestSay() {

        

        }
    }
}
