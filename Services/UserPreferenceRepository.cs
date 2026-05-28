using SQLite;
using TasteNote.Models;

namespace TasteNote.Services;

public class UserPreferenceRepository
{
    private readonly DatabaseService _dbService;

    public UserPreferenceRepository(DatabaseService dbService)
    {
        _dbService = dbService;
    }

    private async Task<SQLiteAsyncConnection> GetDb() => await _dbService.GetConnectionAsync();

    public async Task<string?> GetAsync(string key)
    {
        var db = await GetDb();
        var pref = await db.Table<UserPreference>().FirstOrDefaultAsync(p => p.Key == key);
        return pref?.Value;
    }

    public async Task SetAsync(string key, string value)
    {
        var db = await GetDb();
        var existing = await db.Table<UserPreference>().FirstOrDefaultAsync(p => p.Key == key);
        if (existing != null)
        {
            existing.Value = value;
            await db.UpdateAsync(existing);
        }
        else
        {
            await db.InsertAsync(new UserPreference { Key = key, Value = value });
        }
    }

    public async Task<string> GetAsync(string key, string defaultValue)
    {
        var result = await GetAsync(key);
        return result ?? defaultValue;
    }
}
