
using NotasRapidasSQLite.ViewModels;

namespace NotasRapidasSQLite.Views;

public partial class DetalleNotaPage : ContentPage, IQueryAttributable
{
    private readonly DetalleNotaViewModel _viewModel;
    private Task _cargaPendiente = Task.CompletedTask;

    public DetalleNotaPage(DetalleNotaViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("notaId", out var valor) &&
            int.TryParse(valor?.ToString(), out int id))
        {
            _cargaPendiente = _viewModel.CargarNotaAsync(id);
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _cargaPendiente;

        BotonEliminar.IsVisible = !_viewModel.EsNuevaNota;
    }

    private async void Eliminar_Clicked(object sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert(
            "Eliminar nota",
            "¿Está seguro de que desea eliminar esta nota?",
            "Sí",
            "No");

        if (confirmar)
        {
            await _viewModel.EliminarCommand.ExecuteAsync(null);
        }
    }
}
