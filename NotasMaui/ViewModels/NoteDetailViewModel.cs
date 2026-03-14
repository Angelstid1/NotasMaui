using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotasMaui.Data;
using NotasMaui.Models;

namespace NotasMaui.ViewModels
{
    [QueryProperty(nameof(NoteId), "id")]
    public partial class NoteDetailViewModel : ObservableObject
    {
        private readonly Database _db = new();

        [ObservableProperty]
        private Note currentNote = new();

        public int NoteId { get; set; }

        public async Task LoadNoteAsync()
        {
            if (NoteId > 0)
            {
                var note = await _db.GetNoteByIdAsync(NoteId);
                CurrentNote = note ?? new Note();
            }
            else
            {
                CurrentNote = new Note
                {
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CurrentNote.Title))
                {
                    await Shell.Current.DisplayAlert("Error", "El título no puede estar vacío", "OK");
                    return;
                }

                CurrentNote.UpdatedAt = DateTime.UtcNow;
                await _db.SaveNoteAsync(CurrentNote);

                // Volver a la página principal 
                await Shell.Current.GoToAsync("//NotesPage");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            try
            {
                // Volver a la página principal 
                await Shell.Current.GoToAsync("//NotesPage");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}