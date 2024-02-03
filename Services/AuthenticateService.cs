using System.Text;
using System.Text.Json;
using APICatalogo_client.Models;
using APICatalogo_client.Services.Contracts;

namespace APICatalogo_client.Services;

public class AuthenticateService : IAuthenticateService
{
    private readonly IHttpClientFactory _clientFactory;
    const string endpoint = "/api/v1/login/"; 
    private readonly JsonSerializerOptions _options;
    private TokenViewModel token; 
    public AuthenticateService(IHttpClientFactory clientFactory)
    {
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true }; 
        _clientFactory = clientFactory; 
    }
    public async Task<TokenViewModel> Authenticate(UserViewModel model)
    {
        var client = _clientFactory.CreateClient("AuthenticationAPI");
        var user = JsonSerializer.Serialize(model); 
        StringContent content = new StringContent(user, Encoding.UTF8, "application/json");
        using var response = await client.PostAsync(endpoint, content); 
        if(response.IsSuccessStatusCode)
        {
            var apiReponse = await response.Content.ReadAsStreamAsync(); 
            token = await JsonSerializer
                .DeserializeAsync<TokenViewModel>(apiReponse, _options);
        }
        else 
        {
            return null; 
        }
        return token; 
    }
}
