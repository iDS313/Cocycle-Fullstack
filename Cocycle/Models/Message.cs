using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cocycle.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public int ParentId { get; set; }
        public Boolean IsReminder { get; set; }
        public DateTime Created { get; set; }
        

    }
}