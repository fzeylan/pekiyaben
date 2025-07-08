using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PekiYaBen.WebSite.Models
{
    public class MethodResult
    {
        public MethodResult()
        {
            this.Succeed = false;
            this.Message = "";
            this.Data = null;
        }
        public bool Succeed { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}