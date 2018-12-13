using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouponsApi.Models.Requests
{
    public class CouponRequest
    {
        public string Name { get; set; }
        public long CompanyId { get; set; }
        public long Start_date { get; set; }
        public long End_date { get; set; }
        public int Amount { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
    }
}