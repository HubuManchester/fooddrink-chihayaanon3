using SQLite;
using TasteNote.Models;

namespace TasteNote.Services;

public class TastingRepository
{
    private readonly DatabaseService _dbService;
    private SQLiteAsyncConnection _db => _dbService.GetConnectionAsync().GetAwaiter().GetResult();

    public TastingRepository(DatabaseService dbService)
    {
        _dbService = dbService;
    }

    private async Task<SQLiteAsyncConnection> GetDb() => await _dbService.GetConnectionAsync();

    public async Task<List<TastingRecord>> GetAllAsync()
    {
        var db = await GetDb();
        return await db.Table<TastingRecord>().OrderByDescending(r => r.CreatedAt).ToListAsync();
    }

    public async Task<TastingRecord?> GetByIdAsync(int id)
    {
        var db = await GetDb();
        return await db.Table<TastingRecord>().FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<TastingRecord>> SearchAsync(string keyword)
    {
        var db = await GetDb();
        return await db.Table<TastingRecord>()
            .Where(r => r.Title.Contains(keyword) || (r.Notes != null && r.Notes.Contains(keyword)))
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<TastingRecord>> GetByCategoryAsync(string category)
    {
        var db = await GetDb();
        if (category == "全部") return await GetAllAsync();
        return await db.Table<TastingRecord>()
            .Where(r => r.Category == category)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<TastingRecord>> GetFavoritesAsync()
    {
        var db = await GetDb();
        return await db.Table<TastingRecord>().Where(r => r.IsFavorite).ToListAsync();
    }

    public async Task<List<TastingRecord>> GetByDateRangeAsync(DateTime start, DateTime end)
    {
        var db = await GetDb();
        return await db.Table<TastingRecord>()
            .Where(r => r.CreatedAt >= start && r.CreatedAt <= end)
            .ToListAsync();
    }

    public async Task<int> SaveAsync(TastingRecord record)
    {
        var db = await GetDb();
        record.UpdatedAt = DateTime.Now;
        if (record.Id == 0)
        {
            record.CreatedAt = DateTime.Now;
            return await db.InsertAsync(record);
        }
        return await db.UpdateAsync(record);
    }

    public async Task<int> DeleteAsync(TastingRecord record)
    {
        var db = await GetDb();
        return await db.DeleteAsync(record);
    }

    public async Task<int> GetCountAsync()
    {
        var db = await GetDb();
        return await db.Table<TastingRecord>().CountAsync();
    }

    public async Task<int> GetMonthlyCountAsync()
    {
        var db = await GetDb();
        var now = DateTime.Now;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        return await db.Table<TastingRecord>()
            .Where(r => r.CreatedAt >= startOfMonth)
            .CountAsync();
    }

    public async Task<int> GetFavoriteCountAsync()
    {
        var db = await GetDb();
        return await db.Table<TastingRecord>().Where(r => r.IsFavorite).CountAsync();
    }

    public async Task<double> GetAverageRatingAsync()
    {
        var db = await GetDb();
        var records = await db.Table<TastingRecord>().ToListAsync();
        return records.Count == 0 ? 0 : records.Average(r => r.Rating);
    }
}
