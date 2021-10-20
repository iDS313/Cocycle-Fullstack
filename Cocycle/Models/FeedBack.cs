using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cocycle.Models
{
    public class FeedBack
    {
        public int Id { get; set; }
        public int? RideId { get; set; }
        public int? RouteId { get; set; }
        public string UserId { get; set; }

        [DataType(DataType.MultilineText)]
        public string  description { get; set; }
        public DateTime Created { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser user { get; set; }

        [ForeignKey("RideId")]
        public Arranged Rides { get; set; }

        [ForeignKey("RouteId")]
        public Routes Routes { get; set; }

        public FeedBack()
        {
            this.Created = DateTime.Now;
        }
        [NotMapped]
        public Boolean RideHappened { get; set; }
    }
}
