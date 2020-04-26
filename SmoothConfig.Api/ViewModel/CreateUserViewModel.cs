using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.ViewModel
{
    public class CreateUserViewModel
    {
        public CreateUserViewModel()
        {
            Roles = new List<string>();
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public List<string> Roles { get; set; }
    }
}
