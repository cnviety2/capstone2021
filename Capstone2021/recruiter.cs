//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Capstone2021
{
    using System;
    using System.Collections.Generic;
    
    public partial class recruiter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public recruiter()
        {
            this.companies = new HashSet<company>();
            this.jobs = new HashSet<job>();
            this.manager_deny_job = new HashSet<manager_deny_job>();
        }
    
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string gmail { get; set; }
        public string phone { get; set; }
        public string avatar { get; set; }
        public Nullable<System.DateTime> create_date { get; set; }
        public string role { get; set; }
        public bool sex { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public Nullable<int> status { get; set; }
        public string forgot_password_string { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<company> companies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<job> jobs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<manager_deny_job> manager_deny_job { get; set; }
    }
}
