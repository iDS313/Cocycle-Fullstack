using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cocycle.Models
{
    public class RouteGroup
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public string RequestBy { get; set; }
        public string ApproveBy { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ApproveDate { get; set; }
        public Boolean IsApproved { get; set; }

        [ForeignKey("ApproveBy")]
        public ApplicationUser applicationUserApproved { get; set; }

        [ForeignKey("RequestBy")]
        public ApplicationUser applicationUserRequested { get; set; }

        [ForeignKey("RouteId")]
        public Routes routes { get; set; }
        public Boolean IsActive { get; set; }

        public RouteGroup()
        {
            this.ApproveDate = DateTime.Now;
            this.IsActive = true;

        }
    }
}