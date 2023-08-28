using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cocycle.Models
{
    public class ProfileViewModel
    {
        public ApplicationUser User { get; set; }
        public string RoleName { get; set; }
        public ICollection<Routes> routes { get; set; }
        public ICollection<Routes> Requestedroutes { get; set; }
        public ICollection<RouteGroup> routeGroups { get; set; }
        public ICollection<RouteGroup> joinedroutes { get; set; }
        public ICollection<Arranged> arrangeds  { get; set; }
        public ICollection<Arranged> RequestedRides { get; set; }
        public ICollection<RouteGroup> MyRequests { get; set; }
        public Area area { get; set; }
        public State state { get; set; }
    //    public RouteGroup routeGroup { get; set; }
        public RouteSchedule routeSchedule { get; set; }
        public ICollection<FeedBack> RideFeedbacks { get; set; }

    }
}