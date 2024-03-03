using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryWebApplication1.Models
{
    public enum ArticleStatus
    {
        draft,
        requested,
        declined,
        published
    }
    public partial class Article
    {
        [Key]
        public int ArticleId { get; set; }

        public int? AuthorId { get; set; }

        [Display(Name = "Author Username")]
        public string? AuthorUsername { get; set; }

        [Required(ErrorMessage = "Article Name is required.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Article Name")]
        public string? ArticleName { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int? CategoryId { get; set; }

        public string? Category { get; set; }

        [Display(Name = "Publish Date")]
        public DateOnly? PublishDate { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 1)]
        public string? Status { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User? Author { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? CategoryNavigation { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}