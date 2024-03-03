using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryWebApplication1.Models
{
    public partial class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

        public virtual ICollection<SearchRequest> SearchRequests { get; set; } = new List<SearchRequest>();
    }
}
