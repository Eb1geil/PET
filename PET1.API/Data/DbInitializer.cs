using Microsoft.EntityFrameworkCore;
using PET1.Domain.Entities;

namespace PET1.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Применение миграций перед заполнением данными
            await context.Database.MigrateAsync();

            // Проверка наличия данных в таблицах
            if (context.Games.Any() || context.Categories.Any())
            {
                return; // Данные уже добавлены
            }

            // Добавление категорий
            var categories = new List<Category>
    {
        new Category { Name = "Category 1", NormalizedName = "category1", GroupName = "Group 1" },
        new Category { Name = "Category 2", NormalizedName = "category2", GroupName = "Group 2" }
        // Добавьте другие категории
    };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();

            // Добавление игр
            var games = new List<Game>
    {
        new Game { Name = "Game 1", ImgType = "png", CategoryId = categories[0].Id },
        new Game { Name = "Game 2", ImgType = "jpg", CategoryId = categories[1].Id }
        // Добавьте другие игры с корректными значениями CategoryId
    };

            context.Games.AddRange(games);
            await context.SaveChangesAsync();
        }

    }
}
