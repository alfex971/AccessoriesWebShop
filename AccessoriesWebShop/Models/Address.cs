//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccessoriesWebShop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Address
    {
        public int id { get; set; }
        public int userID { get; set; }
        public string country { get; set; }
        public int postCode { get; set; }
        public string city { get; set; }
        public string street { get; set; }
    
        public virtual User User { get; set; }
    }
}
