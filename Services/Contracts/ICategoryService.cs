using APICatalogo_client.Models;

namespace APICatalogo_client.Services.Contracts;

public interface ICategoryService
{
    Task<IEnumerable<CategoryViewModel>> GetAll(string token) ;
    Task<CategoryViewModel> GetById(int id, string token); 
    Task<CategoryViewModel> Create(CategoryViewModel model, string token); 
    Task<bool> Update(int id, CategoryViewModel model, string token); 
    Task<bool> Remove(int id, string token);
}
