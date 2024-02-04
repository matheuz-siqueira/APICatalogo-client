using System.Text;
using System.Text.Json;
using APICatalogo_client.Models;
using APICatalogo_client.Services.Contracts;

namespace APICatalogo_client.Services;

public class CategoryService : ICategoryService
{
    private const string apiEndpoint = "/api/v1/categories/";
    private readonly JsonSerializerOptions _options; 
    private readonly IHttpClientFactory _httpClientFactory;
    private CategoryViewModel model; 
    private IEnumerable<CategoryViewModel> models; 

    public CategoryService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory; 
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true }; 
    }

    public async Task<IEnumerable<CategoryViewModel>> GetAll()
    {
        var client = _httpClientFactory.CreateClient("CategoriesAPI");
        using var response = await client.GetAsync(apiEndpoint);
        if(response.IsSuccessStatusCode)
        {   
            var apiResponse = await response.Content.ReadAsStreamAsync(); 
            models = await JsonSerializer
                .DeserializeAsync<IEnumerable<CategoryViewModel>>
                (apiResponse, _options); 
        }
        else 
        {
            return null; 
        }
        return models;
    }

    public async Task<CategoryViewModel> GetById(int id)
    {
        var client = _httpClientFactory.CreateClient("CategoriesAPI");
        using var response = await client.GetAsync(apiEndpoint + id);
        if(response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStreamAsync();
            model = await JsonSerializer
                            .DeserializeAsync<CategoryViewModel>(apiResponse, _options);
        }
        else 
        {
            return null; 
        }
        return model; 

    }
    public async Task<CategoryViewModel> Create(CategoryViewModel model)
    {
        var client = _httpClientFactory.CreateClient("CategoriesAPI"); 
        var category = JsonSerializer.Serialize(model); 
        StringContent content = new(category, Encoding.UTF8, "application/json");
        using var response = await client.PostAsync(apiEndpoint, content);
        if(response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStreamAsync();
            model = await JsonSerializer
                .DeserializeAsync<CategoryViewModel>(apiResponse, _options);
        }
        else 
        {
            return null; 
        }
        return model;
    }
    public async Task<bool> Update(int id, CategoryViewModel model)
    {
        var client = _httpClientFactory.CreateClient("CategoriesAPI");
        using var response = await client.PutAsJsonAsync(apiEndpoint + id, model); 
        if(response.IsSuccessStatusCode)
            return true; 
        else 
            return false; 
    }
    public async Task<bool> Remove(int id)
    {
        var client = _httpClientFactory.CreateClient("CategoriesAPI"); 
        using var response = await client.DeleteAsync(apiEndpoint + id); 
        if(response.IsSuccessStatusCode)
            return true; 
        else 
            return false; 
    }
}
