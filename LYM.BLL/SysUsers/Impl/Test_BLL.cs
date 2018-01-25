using LYM.DAL.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace LYM.BLL.SysUsers.Impl
{
    public class Test_BLL : ITest_BLL
    {
        private ITest_DAL _test_DAL;
        public Test_BLL(IServiceProvider serviceProvider, ITest_DAL test_DAL)
        {
            _test_DAL = test_DAL;
        }
        public string SayHello(string name)
        {

            return _test_DAL.SayHello(name);
        }

    }
}
