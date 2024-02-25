using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace LibraryWebApplication1.Models;

public partial class Category
{
    public int CategoryId { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name="Категорія")]

    public string? Name { get; set; }
    [Display(Name = "Інформація про категорію")]
    public string? Description { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
