using SQLite;
using TasteNote.Models;

namespace TasteNote.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _database;
    private readonly string _dbPath;

    public DatabaseService()
    {
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, "tastenote.db3");
    }

    private async Task<SQLiteAsyncConnection> GetDatabaseAsync()
    {
        if (_database != null) return _database;
        _database = new SQLiteAsyncConnection(_dbPath);
        await _database.CreateTableAsync<TastingRecord>();
        await _database.CreateTableAsync<Beverage>();
        await _database.CreateTableAsync<FavoritePlace>();
        await _database.CreateTableAsync<TasteProfile>();
        await _database.CreateTableAsync<UserPreference>();
        await _database.CreateTableAsync<RecognitionLog>();
        return _database;
    }

    public async Task<SQLiteAsyncConnection> GetConnectionAsync()
    {
        return await GetDatabaseAsync();
    }
}
