using APICatalogo_client.Models;

namespace APICatalogo_client.Services.Contracts;

public interface ICategoryService
{
    Task<IEnumerable<CategoryViewModel>> GetAll() ;
    Task<CategoryViewModel> GetById(int id); 
    Task<CategoryViewModel> Create(CategoryViewModel model); 
    Task<bool> Update(int id, CategoryViewModel model); 
    Task<bool> Remove(int id);
}
