//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Profile.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Comment()
        {
            this.Comment_Reaction = new HashSet<Comment_Reaction>();
        }
    
        public int comment_id { get; set; }
        public int profile_id { get; set; }
        public int picture_id { get; set; }
        public string comment1 { get; set; }
        public System.DateTime time_stamp { get; set; }
        public bool seen { get; set; }
    
        public virtual Picture Picture { get; set; }
        public virtual Profile Profile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment_Reaction> Comment_Reaction { get; set; }
    }
}
