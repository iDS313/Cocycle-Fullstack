using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cocycle.Models
{
    public class PostCode
    {
       
        public int Id { get; set; }
        public int AreaId  { get; set; }
        public int StateId { get; set; }
        public string PostCodeName { get; set; }
        public DateTime Created { get; set; }
        public State State { get; set; }
        public Area Area { get; set; }
        public Boolean IsActive { get; set; }


        public PostCode()
        {
            this.Created = DateTime.Now;
            this.IsActive = true;
        }

    }
}