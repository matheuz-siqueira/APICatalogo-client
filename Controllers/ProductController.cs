using APICatalogo_client.Models;
using APICatalogo_client.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace APICatalogo_client.Controllers;

public class ProductController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;
    private string token = string.Empty; 
    public ProductController(IProductService productService, 
        ICategoryService categoryService)
    {
        _productService = productService; 
        _categoryService = categoryService; 
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductViewModel>>> Index()
    {
        var result = await _productService.GetAll(GetToken()); 
        if(result is null)
            return View("Error"); 
        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> NewProduct()
    {
        ViewBag.CategoryId = new 
            SelectList(await _categoryService.GetAll(), "Id", "Name");
        return View();
    }   

    [HttpPost]
    public async Task<ActionResult<ProductViewModel>> NewProduct(
        ProductViewModel model)
    {
        if(ModelState.IsValid)
        {
            var result = await _productService.Create(model, GetToken()); 
            if(result is not null)
                return RedirectToAction(nameof(Index)); 
        }
        else 
        {
            ViewBag.CategoryId = 
                new SelectList(await _categoryService.GetAll(), "Id", "Name");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<ActionResult> Details(int id)
    {
        var result = await _productService.GetById(id, GetToken()); 
        if(result is null) 
            return View("Erro"); 
        return View(result);
    } 

    [HttpGet]
    public async Task<ActionResult> UpdateProduct(int id)
    {
        var result = await _productService.GetById(id, GetToken()) ;
        if(result is null)
            return View("Erro");

        ViewBag.CategoryId = new 
            SelectList(await _categoryService.GetAll(), "Id", "Name"); 
        return View(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProductViewModel>> UpdateProduct(int id, ProductViewModel model)
    {
        if(ModelState.IsValid)
        {
            var result = await _productService.UpdateProduct(id, model, GetToken()); 
            if(result)
                return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    [HttpGet]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var result = await _productService.GetById(id, GetToken()); 
        if(result is null)
            return View("Erro"); 
        return View(result); 
    }

    [HttpPost(), ActionName("DeleteProduct")]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        var result = await _productService.RemoveProduct(id, GetToken()); 
        if(result)
            return RedirectToAction(nameof(Index)); 
        return View("Erro");
    }


    private string GetToken()
    { 
        if(HttpContext.Request.Cookies.ContainsKey("X-Access-Token"))
            token =  HttpContext.Request.Cookies["X-Access-Token"].ToString();
        return token; 
    }

}
