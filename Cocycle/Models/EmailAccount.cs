using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cocycle.Models
{
    public class EmailAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public Boolean EnableSSL { get; set; }
        public Boolean UseDefaultCredentials { get; set; }
        public DateTime Modified { get; set; }

    }
}