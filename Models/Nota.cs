using SQLite;

namespace NotasRapidasSQLite.Models;

public class Nota
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull, MaxLength(100)]
    public string Titulo { get; set; } = string.Empty;

    public string Contenido { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }
}
