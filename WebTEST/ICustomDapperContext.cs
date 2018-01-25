using LYM.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTEST
{
    public interface ICustomDapperContext 
    {

        void Insert(string sql);
       
    }
}
