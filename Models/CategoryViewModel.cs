using System.ComponentModel.DataAnnotations;

namespace APICatalogo_client.Models;

public class CategoryViewModel
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome da categoria é obrigatório")]
    public string Name { get; set; }

    [Required]
    [Display(Name = "Imagem")]
    public string ImageUrl { get; set; }   
}
