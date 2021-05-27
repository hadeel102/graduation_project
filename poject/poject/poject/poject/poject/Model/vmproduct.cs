using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace poject.Model
{
    public class vmproduct
    {
        public int ID { get; set; }
        public string Ename { get; set; }
        public string Aname { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Image1 { get; set; }
        public Nullable<int> TotalQty { get; set; }
        public string Description { get; set; }
        public Nullable<int> UserID { get; set; }
        public bool? IsService { get; set; }

        public string AdminComment { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }   
    }
}