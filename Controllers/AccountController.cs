using APICatalogo_client.Models;
using APICatalogo_client.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo_client.Controllers;

public class AccountController : Controller
{
    private readonly IAuthenticateService _service; 
    public AccountController(IAuthenticateService service)
    {
        _service = service; 
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(); 
    }

    [HttpPost]
    public async Task<ActionResult> Login(UserViewModel model)
    {
        if(!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Login inválido..."); 
            return View(model); 
        }
        var result = await _service.Authenticate(model); 
        if(result is null)
        {
            ModelState.AddModelError(string.Empty, "Login inválido...");
            return View(model); 
        }

        Response.Cookies.Append("X-Access-Token", result.Token, new CookieOptions()
        {
            // Secure = true; 
            HttpOnly = true, 
            SameSite = SameSiteMode.Strict
        });
        return Redirect("/"); 
    }
    
}
