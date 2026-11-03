using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotasRapidasSQLite.Models;
using NotasRapidasSQLite.Services;


namespace NotasRapidasSQLite.ViewModels;

public partial class DetalleNotaViewModel : ObservableObject
{
    private readonly NotasDatabase _database;

    public DetalleNotaViewModel(NotasDatabase database)
    {
        _database = database;
    }

    [ObservableProperty]
    private int notaId;

    [ObservableProperty]
    private string titulo = string.Empty;

    [ObservableProperty]
    private string contenido = string.Empty;

    [ObservableProperty]
    private string mensajeError = string.Empty;

    [ObservableProperty]
    private bool estaCargando;

    public bool EsNuevaNota => NotaId == 0;

    // Cargar una nota existente para editarla
    public async Task CargarNotaAsync(int id)
    {
        if (id <= 0)
            return;

        try
        {
            EstaCargando = true;

            var nota = await _database.ObtenerNotaAsync(id);

            if (nota is not null)
            {
                NotaId = nota.Id;
                Titulo = nota.Titulo;
                Contenido = nota.Contenido;
            }
        }
        finally
        {
            EstaCargando = false;
        }

        OnPropertyChanged(nameof(EsNuevaNota));
    }

    // Guardar una nota nueva o actualizar una existente
    [RelayCommand]
    private async Task GuardarAsync()
    {
        MensajeError = string.Empty;

        if (string.IsNullOrWhiteSpace(Titulo))
        {
            MensajeError = "El título es obligatorio.";
            return;
        }

        if (Titulo.Trim().Length > 100)
        {
            MensajeError = "El título no puede superar los 100 caracteres.";
            return;
        }

        try
        {
            EstaCargando = true;

            if (NotaId == 0)
            {
                var nuevaNota = new Nota
                {
                    Titulo = Titulo.Trim(),
                    Contenido = Contenido.Trim()
                };

                await _database.CrearNotaAsync(nuevaNota);
            }
            else
            {
                var nota = await _database.ObtenerNotaAsync(NotaId);

                if (nota is null)
                {
                    MensajeError = "No se encontró la nota.";
                    return;
                }

                nota.Titulo = Titulo.Trim();
                nota.Contenido = Contenido.Trim();

                await _database.ActualizarNotaAsync(nota);
            }

            await Shell.Current.GoToAsync("..");
        }
        finally
        {
            EstaCargando = false;
        }
    }

    // Eliminar una nota
    [RelayCommand]
    private async Task EliminarAsync()
    {
        if (NotaId <= 0)
            return;

        try
        {
            EstaCargando = true;

            var nota = await _database.ObtenerNotaAsync(NotaId);

            if (nota is not null)
            {
                await _database.EliminarNotaAsync(nota);
                await Shell.Current.GoToAsync("..");
            }
        }
        finally
        {
            EstaCargando = false;
        }
    }
}

