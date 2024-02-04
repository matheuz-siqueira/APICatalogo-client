using System.Net.Http.Headers;
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

    public async Task<IEnumerable<CategoryViewModel>> GetAll(string token)
    {
        var client = _httpClientFactory.CreateClient("CategoriesAPI");
        PutTokenInHeaderAuthorization(client, token); 
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

    public async Task<CategoryViewModel> GetById(int id, string token)
    {
        var client = _httpClientFactory.CreateClient("CategoriesAPI");
        PutTokenInHeaderAuthorization(client, token);
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
    public async Task<CategoryViewModel> Create(CategoryViewModel model, string token)
    {
        var client = _httpClientFactory.CreateClient("CategoriesAPI");
        PutTokenInHeaderAuthorization(client, token);  
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
    public async Task<bool> Update(int id, CategoryViewModel model, string token)
    {
        var client = _httpClientFactory.CreateClient("CategoriesAPI");
        PutTokenInHeaderAuthorization(client, token); 
        using var response = await client.PutAsJsonAsync(apiEndpoint + id, model); 
        if(response.IsSuccessStatusCode)
            return true; 
        else 
            return false; 
    }
    public async Task<bool> Remove(int id, string token)
    {
        var client = _httpClientFactory.CreateClient("CategoriesAPI"); 
        PutTokenInHeaderAuthorization(client, token); 
        using var response = await client.DeleteAsync(apiEndpoint + id); 
        if(response.IsSuccessStatusCode)
            return true; 
        else 
            return false; 
    }

    private static void PutTokenInHeaderAuthorization(HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new 
            AuthenticationHeaderValue("Bearer",token); 
    }
}
