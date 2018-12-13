using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouponsApi.Infrastructures
{
    public class ActionResultModel<T> : ActionResult
    {
        public T Result { get; set; }
    }
}