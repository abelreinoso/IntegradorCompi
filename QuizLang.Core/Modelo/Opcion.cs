namespace QuizLang.Core.Modelo;

/// <summary>
/// Representa una opción dentro de una pregunta de opción múltiple.
/// </summary>
public class Opcion
{
    /// <summary>
    /// Letra identificadora de la opción (p. ej. 'a', 'b', etc.).
    /// </summary>
    public char Letra { get; set; }

    /// <summary>
    /// Texto descriptivo de la opción.
    /// </summary>
    public string Texto { get; set; } = string.Empty;

    /// <summary>
    /// Indica si esta opción es la respuesta correcta ([CORRECTA]).
    /// </summary>
    public bool EsCorrecta { get; set; }

    /// <summary>
    /// Línea en el código fuente.
    /// </summary>
    public int Line { get; set; }

    /// <summary>
    /// Columna en el código fuente.
    /// </summary>
    public int Col { get; set; }
}
