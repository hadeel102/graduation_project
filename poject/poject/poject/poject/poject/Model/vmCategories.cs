using poject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace poject.Model
{
    public class vmCategories
    {
        public vmCategories()
        {
            this.Category = new HashSet<TblCategory>();
        }
        public int ID { get; set; }
        public string Ename { get; set; }
        public string AName { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Image { get; set; }
        public Nullable<bool> IsService { get; set; }
        public virtual ICollection<TblCategory> Category { get; set; }

    }
}