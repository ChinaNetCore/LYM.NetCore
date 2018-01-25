using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.CAP;
using LYM.Core.Service;

namespace LYM.ApiServices.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        private IUserService _userService;


        public ValuesController(IUserService userService)
        { 
            _userService = userService;
      
        }
        /// <summary>
        /// 消息服务
        /// </summary>
        /// <param name="usermsg"></param>
        /// <returns></returns>
        [CapSubscribe("lymtest.services")]
        [HttpPost("CheckReceivedMessage")]
        //[ProducesResponseType(typeof(LYM.Core.Model.User.UserLoginModel), 200)]
        public void CheckReceivedMessage(LYM.Core.Model.User.UserLoginModel usermsg)
        {
            _userService.MessageInsert(usermsg);
        }


        /// <summary>
        /// 取数据
        /// </summary>
        /// <returns></returns>
        // GET api/values

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
