using System;
using System.ComponentModel.DataAnnotations;


namespace CouponsApi.Models.Requests
{
    public class CompanyRequest
    {
        public String Name { get; set; }
        [EmailAddress]
        public String Email { get; set; }
    }
}