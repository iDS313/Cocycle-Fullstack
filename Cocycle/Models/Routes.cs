using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cocycle.Models
{
    public class Routes
    {
        public int Id { get; set; }
        public int RouteType { get; set; }
        [Display(Name ="Add Image")]
        public string PosterImage { get; set; }
        //public string PostCode { get; set; }
        
        public string StartingPoint { get; set; }
        
        public string EndPoint { get; set; }

       
        //[DisplayFormat(DataFormatString = "{0:t}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{DD-MM-yyyy}")]
        public DateTime StartTime { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:t}")]
        public DateTime EndTime { get; set; }
        
        public int AreaId { get; set; }
        
        public int StateId { get; set; }
        
        public int? PostCodeId { get; set; }
        public string Message { get; set; }
        public  string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestedDate { get; set; }
        public Boolean IsApproved { get; set; }
        public Boolean  Addreturn { get; set; }
        public State State { get; set; }
        public Area Area { get; set; }
        public Boolean IsActive { get; set; }

        [ForeignKey("CreatedBy")]
        public ApplicationUser applicationUserCreated { get; set; }

        [ForeignKey("RequestedBy")]
        public ApplicationUser applicationUserRequested { get; set; }

        public List<RouteSchedule> RouteSchedule { get; set; }
   
        [Required(ErrorMessage = "please set schedule")]
        [NotMapped]
        public string hidInput { get; set; }
        public ICollection<FeedBack> FeedBack { get; set; }

        [ForeignKey("PostCodeId")]
        public virtual PostCode PostCode { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public Routes()
        {
            this.RequestedDate = DateTime.Now;
            this.Created = DateTime.Now;
        }

    }

}