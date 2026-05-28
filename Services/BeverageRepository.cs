using SQLite;
using TasteNote.Models;

namespace TasteNote.Services;

public class BeverageRepository
{
    private readonly DatabaseService _dbService;

    public BeverageRepository(DatabaseService dbService)
    {
        _dbService = dbService;
    }

    private async Task<SQLiteAsyncConnection> GetDb() => await _dbService.GetConnectionAsync();

    public async Task<List<Beverage>> GetAllAsync()
    {
        var db = await GetDb();
        return await db.Table<Beverage>().OrderBy(b => b.Category).ThenBy(b => b.Name).ToListAsync();
    }

    public async Task<Beverage?> GetByIdAsync(int id)
    {
        var db = await GetDb();
        return await db.Table<Beverage>().FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Beverage>> SearchAsync(string keyword)
    {
        var db = await GetDb();
        return await db.Table<Beverage>()
            .Where(b => b.Name.Contains(keyword) || (b.Description != null && b.Description.Contains(keyword)))
            .ToListAsync();
    }

    public async Task<List<Beverage>> GetByCategoryAsync(string category)
    {
        var db = await GetDb();
        if (category == "全部") return await GetAllAsync();
        return await db.Table<Beverage>().Where(b => b.Category == category).ToListAsync();
    }

    public async Task<List<Beverage>> GetRandomRecommendationsAsync(int count = 3)
    {
        var db = await GetDb();
        var all = await db.Table<Beverage>().ToListAsync();
        var random = new Random();
        return all.OrderBy(_ => random.Next()).Take(count).ToList();
    }

    public async Task<int> SaveAsync(Beverage beverage)
    {
        var db = await GetDb();
        beverage.UpdatedAt = DateTime.Now;
        if (beverage.Id == 0)
        {
            return await db.InsertAsync(beverage);
        }
        return await db.UpdateAsync(beverage);
    }

    public async Task ToggleFavoriteAsync(Beverage beverage)
    {
        var db = await GetDb();
        beverage.IsFavorite = !beverage.IsFavorite;
        beverage.UpdatedAt = DateTime.Now;
        await db.UpdateAsync(beverage);
    }
}
