using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cocycle.Models
{
    public class RouteSchedule
    {

        public int Id { get; set; }
        public int RouteId { get; set; }
        public int DayId { get; set; }

        //[DataType(DataType.Time)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        //public DateTime StartTime { get; set; }

        //[DataType(DataType.Time)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        //public DateTime EndTime { get; set; }

        [ForeignKey("RouteId")]
        public Routes routes { get; set; }
    }
}