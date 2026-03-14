using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotasMaui.Data;
using NotasMaui.Models;

namespace NotasMaui.ViewModels
{
    public partial class NotesViewModel : ObservableObject
    {
        private readonly Database _db = new();

        [ObservableProperty]
        private ObservableCollection<Note> notes = new();

        [ObservableProperty]
        private string searchText = string.Empty;

        private ObservableCollection<Note>? _allNotes;

        public NotesViewModel()
        {
            LoadNotesCommand.Execute(null);
        }

        [RelayCommand]
        public async Task LoadNotes()
        {
            var list = await _db.GetNotesAsync();
            Notes.Clear();
            foreach (var note in list) Notes.Add(note);

            _allNotes = new ObservableCollection<Note>(Notes);
        }

        public async Task RefreshAsync()
        {
            await LoadNotes();
        }

        [RelayCommand]
        private async Task AddNote()
        {
            try
            {
                
                await Shell.Current.GoToAsync("NoteDetailPage");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task EditNote(Note note)
        {
            try
            {
                if (note == null) return;
                
                await Shell.Current.GoToAsync($"NoteDetailPage?id={note.Id}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task DeleteNote(Note note)
        {
            bool confirm = await Shell.Current.DisplayAlert("Eliminar", "¿Seguro?", "Sí", "No");
            if (confirm)
            {
                await _db.DeleteNoteAsync(note);
                await LoadNotes();
            }
        }

        partial void OnSearchTextChanged(string value)
        {
            if (_allNotes == null || !_allNotes.Any())
            {
                _allNotes = new ObservableCollection<Note>(Notes);
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                Notes = new ObservableCollection<Note>(_allNotes);
                return;
            }

            var filtered = _allNotes.Where(n =>
                (n.Title?.Contains(value, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (n.Content?.Contains(value, StringComparison.OrdinalIgnoreCase) ?? false))
                .ToList();

            Notes = new ObservableCollection<Note>(filtered);
        }
    }
}