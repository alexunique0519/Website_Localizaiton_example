//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OEC06.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class crop
    {
        public crop()
        {
            this.varieties = new HashSet<variety>();
        }
    
        public int cropId { get; set; }
        public string name { get; set; }
        public string image { get; set; }
    
        public virtual ICollection<variety> varieties { get; set; }
    }
}
