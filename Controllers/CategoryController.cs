using APICatalogo_client.Models;
using APICatalogo_client.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo_client.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryService _service;
    private string token = string.Empty; 
    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    public async Task<ActionResult<IEnumerable<CategoryViewModel>>> Index()
    {
        var result = await _service.GetAll(GetToken());
        if(result is null)
            return View("Error");

        return View(result); 
    }

    [HttpGet]
    public IActionResult NewCategory()
    {
        return View(); 
    }

    [HttpPost]
    public async Task<ActionResult<CategoryViewModel>> NewCategory(
        CategoryViewModel model)
    {
        if(ModelState.IsValid)
        {
            var result = await _service.Create(model, GetToken());
            if(result is not null)
                return RedirectToAction(nameof(Index));
        }
        ViewBag.Error = "Erro ao criar categoria"; 
        return View(model); 
    }

    [HttpGet]
    public async Task<IActionResult> UpdateCategory(int id)
    {
        var result = await _service.GetById(id, GetToken()); 
        if(result is null)
            return View("Error"); 
        return View(result);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryViewModel>> UpdateCategory
        (int id, CategoryViewModel model)
    {
        if(ModelState.IsValid)
        {
            var result = await _service.Update(id, model, GetToken());  
            if(result)
                return RedirectToAction(nameof(Index)); 
        }
        ViewBag.Erro = "Erro ao atualizar categoria"; 
        return View(model); 
    }

    [HttpGet]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _service.GetById(id, GetToken()); 
        if(result is null)
            return View("Error"); 
        return View(result) ;
    }

    [HttpPost(), ActionName("DeleteCategory")]
    public async Task<ActionResult<CategoryViewModel>> DeleteConfirmed(int id)
    {
        var result = await _service.Remove(id, GetToken());
        if(result)
            return RedirectToAction(nameof(Index));
        return View("Error");

    }

    private string GetToken()
    {
        if(HttpContext.Request.Cookies.ContainsKey("X-Access-Token"))
            token =  HttpContext.Request.Cookies["X-Access-Token"].ToString();
        return token; 
    }


}
