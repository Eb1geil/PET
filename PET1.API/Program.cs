using Microsoft.EntityFrameworkCore;
using PET1.API.Data;
using PET1.API.Services.CategoryService;
using PET1.API.Services.MovieService;
using PET1.API.Services.ProductService;
using PET1.Services.MovieServices;
using PET1.Services.ProductServices;

var builder = WebApplication.CreateBuilder(args);

// �������� ������ ����������� �� ������������
var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// �������� ������ �������
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IMovieService, MovieService>();

var app = builder.Build();
//await DbInitializer.SeedData(app);

// ���������������� HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await DbInitializer.SeedData(app);

app.Run();
