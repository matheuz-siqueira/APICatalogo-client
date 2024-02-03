using APICatalogo_client.Models;

namespace APICatalogo_client.Services.Contracts;

public interface IAuthenticateService
{
    Task<TokenViewModel> Authenticate(UserViewModel model);
}
