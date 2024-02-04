using System.ComponentModel.DataAnnotations;

namespace APICatalogo_client.Models;

public class ProductViewModel
{
    [Required]
    public int Id { get; set; }
    [Required(ErrorMessage = "O nome do produto é obrigatório")]
    public string Name { get; set; }
    [Required(ErrorMessage = "A descrição do produto é obrigatório")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Informe o preço do produto")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Informe o caminho da imagem")]
    public string ImageUrl { get; set; }

    [Display(Name = "Categoria")]
    public int CategoryId { get; set; }
}
