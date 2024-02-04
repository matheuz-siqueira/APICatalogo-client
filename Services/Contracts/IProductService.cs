using APICatalogo_client.Models;

namespace APICatalogo_client.Services.Contracts;

public interface IProductService
{
    Task<IEnumerable<ProductViewModel>> GetAll(string token); 
    Task<ProductViewModel> GetById(int id, string token); 
    Task<ProductViewModel> Create(ProductViewModel model, string token); 
    Task<bool> UpdateProduct(int id, ProductViewModel model, string token); 
    Task<bool> RemoveProduct(int id, string token);
}
