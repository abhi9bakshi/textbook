//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace testbook
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comment
    {
        public long CommentId { get; set; }
        public System.DateTime CreationDateTime { get; set; }
        public long LikesCount { get; set; }
        public long PostId { get; set; }
    
        public virtual Post Post { get; set; }
    }
}
