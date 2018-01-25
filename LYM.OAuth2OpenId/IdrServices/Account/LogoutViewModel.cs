using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LYM.OAuth2OpenId.IdrServices.Account
{
    public class LogoutViewModel : LogoutInputModel
    {
        public bool ShowLogoutPrompt { get; set; }
    }
}
