using SQLite;
using TasteNote.Models;

namespace TasteNote.Services;

public class TasteProfileRepository
{
    private readonly DatabaseService _dbService;

    public TasteProfileRepository(DatabaseService dbService)
    {
        _dbService = dbService;
    }

    private async Task<SQLiteAsyncConnection> GetDb() => await _dbService.GetConnectionAsync();

    public async Task<TasteProfile?> GetByDateAsync(string date)
    {
        var db = await GetDb();
        return await db.Table<TasteProfile>().FirstOrDefaultAsync(p => p.ProfileDate == date);
    }

    public async Task<TasteProfile?> GetLatestAsync()
    {
        var db = await GetDb();
        return await db.Table<TasteProfile>().OrderByDescending(p => p.ProfileDate).FirstOrDefaultAsync();
    }

    public async Task<int> SaveAsync(TasteProfile profile)
    {
        var db = await GetDb();
        var existing = await GetByDateAsync(profile.ProfileDate);
        if (existing != null)
        {
            profile.Id = existing.Id;
            return await db.UpdateAsync(profile);
        }
        return await db.InsertAsync(profile);
    }

    public async Task<List<TasteProfile>> GetAllAsync()
    {
        var db = await GetDb();
        return await db.Table<TasteProfile>().OrderByDescending(p => p.ProfileDate).ToListAsync();
    }
}
