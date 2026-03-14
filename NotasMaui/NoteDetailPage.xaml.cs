using NotasMaui.ViewModels;

namespace NotasMaui;

public partial class NoteDetailPage : ContentPage
{
    public NoteDetailPage(NoteDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is NoteDetailViewModel vm)
        {
            await vm.LoadNoteAsync();
        }
    }
}