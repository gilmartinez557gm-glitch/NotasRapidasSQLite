using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotasRapidasSQLite.Models;
using NotasRapidasSQLite.Services;
using System.Collections.ObjectModel;

namespace NotasRapidasSQLite.ViewModels;

public partial class HistorialViewModel : ObservableObject
{
    private readonly NotasDatabase _database;

    public HistorialViewModel(NotasDatabase database)
    {
        _database = database;
    }

    public ObservableCollection<Historial> Registros { get; } = new();

    [ObservableProperty]
    private bool estaCargando;

    // Cargar los registros del historial
    [RelayCommand]
    private async Task CargarHistorialAsync()
    {
        if (EstaCargando)
            return;

        try
        {
            EstaCargando = true;

            var historial = await _database.ObtenerHistorialAsync();

            Registros.Clear();

            foreach (var registro in historial)
            {
                Registros.Add(registro);
            }
        }
        finally
        {
            EstaCargando = false;
        }
    }
}

