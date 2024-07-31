using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PET1.Data;
using PET1.Services.CategoryServices;
using PET1.Services.MovieServices;
using PET1.Services.ProductServices;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<MovieContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("MovieContext") ?? throw new InvalidOperationException("Connection string 'MovieContext' not found.")));
        builder.Services.AddDbContext<GameContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("GameContext") ?? throw new InvalidOperationException("Connection string 'GameContext' not found.")));

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddLogging();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession();
      builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
        builder.Services.AddScoped<IProductService, MemoryProductService>();
        builder.Services.AddScoped<IMovieService, MemoryMovieService>();
        //UriData uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

        //builder.Services.AddHttpClient("ApiHttpClient", client =>
        //{
        //    client.BaseAddress = new Uri(uriData.ApiUri);
        //});

        //builder.Services.AddScoped<IProductService, ApiProductService>(provider =>
        //{
        //    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
        //    var httpClient = httpClientFactory.CreateClient("ApiHttpClient");
        //    var configuration = provider.GetRequiredService<IConfiguration>();
        //    var logger = provider.GetRequiredService<ILogger<ApiProductService>>();
        //    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();

        //    return new ApiProductService(httpClient, configuration, logger, httpContextAccessor);
        //});
        var app = builder.Build();
        
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
    }
}