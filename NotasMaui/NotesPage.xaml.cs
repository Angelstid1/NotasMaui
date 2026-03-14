using NotasMaui.ViewModels;

namespace NotasMaui;

public partial class NotesPage : ContentPage
{
    public NotesPage(NotesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is NotesViewModel vm)
        {
            await vm.RefreshAsync();  
        }
    }
}