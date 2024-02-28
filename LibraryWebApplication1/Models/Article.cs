using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryWebApplication1.Models;

public partial class Article
{
    public int ArticleId { get; set; }

    public int? AuthorId { get; set; }

    public string? AuthorUsername { get; set; }

    public string? ArticleName { get; set; }

    public int? CategoryId { get; set; }

    public string? Category { get; set; }

    public DateOnly? PublishDate { get; set; }

    public string? Status { get; set; }

    public virtual User? Author { get; set; }

    public virtual Category CategoryNavigation { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}

