using System.Net.Http.Headers;
using APICatalogo_client.Services;
using APICatalogo_client.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("CategoriesAPI", c => 
{
    c.BaseAddress = new Uri(builder.Configuration["ServiceUri:APICatalogo"]);
});

builder.Services.AddHttpClient("AuthenticationAPI", c=> 
{
    c.BaseAddress = new Uri(builder.Configuration["ServiceUri:APICatalogo"]); 
    c.DefaultRequestHeaders.Accept.Clear();
    c.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddScoped<ICategoryService, CategoryService>(); 
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>(); 

var app = builder.Build();

// app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
