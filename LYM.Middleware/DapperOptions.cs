using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace LYM.Middleware
{

    public class Options1 {

        public string UserName { get; set; }
    }
    public class Options2
    {


    }
    
    public class DapperOptions
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        public Func<Options1,Task> OnCustomOptionSet { get; set; }


       
        

    }
}
