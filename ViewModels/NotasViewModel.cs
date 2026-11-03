
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotasRapidasSQLite.Models;
using NotasRapidasSQLite.Services;
using NotasRapidasSQLite.Views;
using System.Collections.ObjectModel;

namespace NotasRapidasSQLite.ViewModels;

public partial class NotasViewModel : ObservableObject
{
    private readonly NotasDatabase _database;

    private List<Nota> _todasLasNotas = new();

    public NotasViewModel(NotasDatabase database)
    {
        _database = database;
    }

    public ObservableCollection<Nota> Notas { get; } = new();

    [ObservableProperty]
    private string textoBusqueda = string.Empty;

    [ObservableProperty]
    private bool estaCargando;

    // Cargar todas las notas
    [RelayCommand]
    private async Task CargarNotasAsync()
    {
        if (EstaCargando)
            return;

        try
        {
            EstaCargando = true;

            _todasLasNotas = await _database.ObtenerNotasAsync();

            AplicarFiltro();
        }
        finally
        {
            EstaCargando = false;
        }
    }

    // Crear una nota nueva
    [RelayCommand]
    private async Task NuevaNotaAsync()
    {
        await Shell.Current.GoToAsync(
            nameof(DetalleNotaPage));
    }

    // Buscar notas por título
    partial void OnTextoBusquedaChanged(string value)
    {
        AplicarFiltro();
    }

    private void AplicarFiltro()
    {
        var resultado = string.IsNullOrWhiteSpace(TextoBusqueda)
            ? _todasLasNotas
            : _todasLasNotas
                .Where(n => n.Titulo.Contains(
                    TextoBusqueda.Trim(),
                    StringComparison.OrdinalIgnoreCase))
                .ToList();

        Notas.Clear();

        foreach (var nota in resultado)
        {
            Notas.Add(nota);
        }
    }
}
