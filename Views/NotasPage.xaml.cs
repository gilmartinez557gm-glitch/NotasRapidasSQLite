
using NotasRapidasSQLite.ViewModels;
using NotasRapidasSQLite.Views;

namespace NotasRapidasSQLite.Views;

public partial class NotasPage : ContentPage
{
    private readonly NotasViewModel _viewModel;

    public NotasPage(NotasViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.CargarNotasCommand.ExecuteAsync(null);
    }

    private async void AbrirNota_Clicked(object sender, EventArgs e)
    {
        if (sender is Button boton &&
            boton.CommandParameter is int notaId)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(DetalleNotaPage)}?notaId={notaId}");
        }
    }
}
