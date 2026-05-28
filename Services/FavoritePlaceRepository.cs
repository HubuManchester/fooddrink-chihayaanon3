using SQLite;
using TasteNote.Models;

namespace TasteNote.Services;

public class FavoritePlaceRepository
{
    private readonly DatabaseService _dbService;

    public FavoritePlaceRepository(DatabaseService dbService)
    {
        _dbService = dbService;
    }

    private async Task<SQLiteAsyncConnection> GetDb() => await _dbService.GetConnectionAsync();

    public async Task<List<FavoritePlace>> GetAllAsync()
    {
        var db = await GetDb();
        return await db.Table<FavoritePlace>().OrderByDescending(p => p.Rating).ToListAsync();
    }

    public async Task<FavoritePlace?> GetByIdAsync(int id)
    {
        var db = await GetDb();
        return await db.Table<FavoritePlace>().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<FavoritePlace>> GetByCategoryAsync(string category)
    {
        var db = await GetDb();
        return await db.Table<FavoritePlace>().Where(p => p.Category == category).ToListAsync();
    }

    public async Task<int> SaveAsync(FavoritePlace place)
    {
        var db = await GetDb();
        if (place.Id == 0)
        {
            return await db.InsertAsync(place);
        }
        return await db.UpdateAsync(place);
    }

    public async Task<int> DeleteAsync(FavoritePlace place)
    {
        var db = await GetDb();
        return await db.DeleteAsync(place);
    }

    public async Task<int> GetCountAsync()
    {
        var db = await GetDb();
        return await db.Table<FavoritePlace>().CountAsync();
    }
}
