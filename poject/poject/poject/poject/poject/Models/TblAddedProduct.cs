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
    
    public partial class TblAddedProduct
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<int> ProductionID { get; set; }
        public Nullable<int> IsApproved { get; set; }
    }
}