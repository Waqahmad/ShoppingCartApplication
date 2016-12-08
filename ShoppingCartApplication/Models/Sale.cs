//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShoppingCartApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sale
    {
        public int ID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> SaleQty { get; set; }
        public Nullable<decimal> SaleAmount { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<System.DateTime> Createdon { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<decimal> EmployeeCommission { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
