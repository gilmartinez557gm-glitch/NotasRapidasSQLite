
using SQLite;
using NotasRapidasSQLite.Models;

namespace NotasRapidasSQLite.Services;

public class NotasDatabase
{
    private readonly SQLiteAsyncConnection _database;
    private readonly Task _inicializacion;

    public NotasDatabase()
    {
        string dbPath = Path.Combine(
            FileSystem.AppDataDirectory,
            "NotasRapidas.db3"
        );

        _database = new SQLiteAsyncConnection(dbPath);

        _inicializacion = InicializarBaseDatosAsync();
    }

    private async Task InicializarBaseDatosAsync()
    {
        await _database.CreateTableAsync<Nota>();
        await _database.CreateTableAsync<Historial>();
    }

    public async Task<List<Nota>> ObtenerNotasAsync()
    {
        await _inicializacion;

        return await _database.Table<Nota>()
            .OrderByDescending(n => n.FechaModificacion)
            .ToListAsync();
    }

    public async Task<Nota?> ObtenerNotaAsync(int id)
    {
        await _inicializacion;

        return await _database.Table<Nota>()
            .Where(n => n.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<int> CrearNotaAsync(Nota nota)
    {
        await _inicializacion;

        nota.FechaCreacion = DateTime.Now;
        nota.FechaModificacion = DateTime.Now;

        await _database.InsertAsync(nota);

        await RegistrarHistorialAsync(
            nota.Id, nota.Titulo, "Creada"
        );

        return nota.Id;
    }

    public async Task<int> ActualizarNotaAsync(Nota nota)
    {
        await _inicializacion;

        nota.FechaModificacion = DateTime.Now;

        int resultado = await _database.UpdateAsync(nota);

        if (resultado > 0)
        {
            await RegistrarHistorialAsync(
                nota.Id, nota.Titulo, "Editada"
            );
        }

        return resultado;
    }

    public async Task<int> EliminarNotaAsync(Nota nota)
    {
        await _inicializacion;

        int resultado = await _database.DeleteAsync(nota);

        if (resultado > 0)
        {
            await RegistrarHistorialAsync(
                nota.Id, nota.Titulo, "Eliminada"
            );
        }

        return resultado;
    }

    private async Task RegistrarHistorialAsync(
        int notaId,
        string titulo,
        string accion)
    {
        var historial = new Historial
        {
            NotaId = notaId,
            TituloNota = titulo,
            Accion = accion,
            Fecha = DateTime.Now
        };

        await _database.InsertAsync(historial);
    }

    public async Task<List<Historial>> ObtenerHistorialAsync()
    {
        await _inicializacion;

        return await _database.Table<Historial>()
            .OrderByDescending(h => h.Fecha)
            .ToListAsync();
    }
}
