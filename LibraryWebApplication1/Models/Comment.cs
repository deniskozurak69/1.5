using System;
using System.Collections.Generic;

namespace LibraryWebApplication1.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int? ArticleId { get; set; }
        public int? AuthorId { get; set; }  
        public string? AuthorUsername { get; set; }
        public DateOnly? PublishDate { get; set; }
        public virtual Article? Article { get; set; }
        public virtual User? Author { get; set; }  
        public string? Text { get; set; }
    }
}