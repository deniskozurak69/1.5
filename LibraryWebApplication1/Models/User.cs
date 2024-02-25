using System;
using System.Collections.Generic;

namespace LibraryWebApplication1.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual ICollection<SearchRequest> SearchRequests { get; set; } = new List<SearchRequest>();
}
