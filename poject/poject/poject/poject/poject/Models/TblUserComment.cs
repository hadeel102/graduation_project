//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace poject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TblUserComment
    {
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> ServiceID { get; set; }
        public Nullable<int> IncubID { get; set; }
        public string Comment { get; set; }
    }
}
