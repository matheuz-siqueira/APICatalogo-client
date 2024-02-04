using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using APICatalogo_client.Models;
using APICatalogo_client.Services.Contracts;

namespace APICatalogo_client.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _clientFactory;
    private const string endpoint = "/api/v1/products/"; 
    private readonly JsonSerializerOptions _options; 
    private ProductViewModel model; 
    private IEnumerable<ProductViewModel> models;

    public ProductService(IHttpClientFactory clientFactory)
    {
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true }; 
        _clientFactory = clientFactory; 
    }

    public async Task<IEnumerable<ProductViewModel>> GetAll(string token)
    {
        var client = _clientFactory.CreateClient("ProductsAPI"); 
        PutTokenInHeaderAuthorization(token, client); 
        using var response = await client.GetAsync(endpoint); 
        if(response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStreamAsync(); 
            models = await JsonSerializer
                .DeserializeAsync<IEnumerable<ProductViewModel>>(apiResponse, _options); 
        }
        else 
        {
            return null; 
        }
        return models; 
    }

    public async Task<ProductViewModel> GetById(int id, string token)
    {
        var client = _clientFactory.CreateClient("ProductsAPI"); 
        PutTokenInHeaderAuthorization(token, client); 
        using var response = await client.GetAsync(endpoint + id); 
        if(response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStreamAsync(); 
            model = await 
                JsonSerializer.DeserializeAsync<ProductViewModel>(apiResponse, _options);
        }
        else 
        {
            return null; 
        }
        return model; 
    }
    public async Task<ProductViewModel> Create(ProductViewModel model, string token)
    {
        var client = _clientFactory.CreateClient("ProductsAPI");
        PutTokenInHeaderAuthorization(token, client); 
        var product = JsonSerializer.Serialize(model); 
        StringContent content = new(product, Encoding.UTF8, "application/json"); 
        using var response = await client.PostAsync(endpoint, content); 
        if(response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStreamAsync();
            model = await JsonSerializer
                .DeserializeAsync<ProductViewModel>(apiResponse, _options);
        }
        else
        {
            return null; 
        }
        return model; 
    }
    public async Task<bool> UpdateProduct(int id, ProductViewModel model, string token)
    {
        var client = _clientFactory.CreateClient("ProductsAPI"); 
        PutTokenInHeaderAuthorization(token, client); 
        using var response = await client.PutAsJsonAsync(endpoint + id, model);
        if(response.IsSuccessStatusCode)
            return true; 
        else 
            return false;
    }
    public async Task<bool> RemoveProduct(int id, string token)
    {
        var client = _clientFactory.CreateClient("ProductsAPI"); 
        PutTokenInHeaderAuthorization(token, client); 
        using var response = await client.DeleteAsync(endpoint + id); 
        if(response.IsSuccessStatusCode)
            return true; 
        else 
            return false;
    }

    private static void PutTokenInHeaderAuthorization(string token, HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);        
    }
}
