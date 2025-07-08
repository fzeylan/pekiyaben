using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekiYaBen.API.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ConnectionString { get; set; }
        public string FromEmail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }
    }
}
