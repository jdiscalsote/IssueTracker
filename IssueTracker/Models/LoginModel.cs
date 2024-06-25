using System;
using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    public class LoginModel
    {
        public string AccessCode { get; set; }
        public string Password { get; set; }
    }
}
