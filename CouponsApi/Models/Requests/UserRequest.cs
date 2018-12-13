using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CouponsApi.Models.Requests
{
    public class UserRequest
    {
        public String Name { get; set; }
        [EmailAddress]
        public String Email { get; set; }
        public String Password { get; set; }
        public String Type { get; set; }
        public long CompanyId { get; set; }
    }
}