using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace poject.Model
{
    public class vmRequestedIncubation
    {

        public int ID { get; set; }
        public int RequestedUser { get; set; }
        [DataType(DataType.MultilineText)]
        public string RequestedDescription { get; set; }
        public string AttchedFile { get; set; }
        public int Status { get; set; }

        public string AdminComment { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}