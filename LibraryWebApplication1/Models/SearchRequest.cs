using System;
using System.Collections.Generic;

namespace LibraryWebApplication1.Models;

public partial class SearchRequest
{
    public int SearchRequestId { get; set; }

    public int? UserId { get; set; }

    public string? RequestedName { get; set; }

    public string? RequestedCategory { get; set; }

    public string? RequestedAuthor { get; set; }

    public DateOnly? RequestedDate { get; set; }

    public virtual User? User { get; set; }
}
