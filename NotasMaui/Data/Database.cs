using SQLite;
using Microsoft.Maui.Storage;
using NotasMaui.Models;

namespace NotasMaui.Data
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;

        public Database()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "notes.db3");
            _database = new SQLiteAsyncConnection(dbPath);
        }

        private async Task InitAsync()
        {
            await _database.CreateTableAsync<Note>();
        }

        public async Task<List<Note>> GetNotesAsync()
        {
            await InitAsync();
            return await _database.Table<Note>()
                .OrderByDescending(n => n.IsPinned)
                .ThenByDescending(n => n.UpdatedAt)
                .ToListAsync();
        }

       
        public async Task<Note?> GetNoteByIdAsync(int id)
        {
            await InitAsync();
            return await _database.Table<Note>()
                .Where(n => n.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> SaveNoteAsync(Note note)
        {
            await InitAsync();
            note.UpdatedAt = DateTime.UtcNow;
            if (note.Id != 0)
                return await _database.UpdateAsync(note);
            else
            {
                note.CreatedAt = DateTime.UtcNow;
                return await _database.InsertAsync(note);
            }
        }

        public async Task<int> DeleteNoteAsync(Note note)
        {
            await InitAsync();
            return await _database.DeleteAsync(note);
        }
    }
}