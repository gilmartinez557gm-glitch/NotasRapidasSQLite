using SQLite;

namespace NotasRapidasSQLite.Models;

public class Historial
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int NotaId { get; set; }

    [NotNull]
    public string TituloNota { get; set; } = string.Empty;

    [NotNull]
    public string Accion { get; set; } = string.Empty;

    public DateTime Fecha { get; set; }
}
