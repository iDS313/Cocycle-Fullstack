using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cocycle.Models
{
    public class Area
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string AreaName { get; set; }
        //public System.DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        // public virtual State State{ get; set; }
        public ICollection<Routes> RoutesList { get; set; }
        public ICollection<Arranged> Arranged { get; set; }
        public State state { get; set; }


    }


}