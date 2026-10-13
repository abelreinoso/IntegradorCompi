namespace QuizLang.Core.Modelo;

/// <summary>
/// Clase base abstracta para todas las preguntas de QuizLang.
/// </summary>
public abstract class Pregunta
{
    /// <summary>
    /// Enunciado de la pregunta.
    /// </summary>
    public string Enunciado { get; set; } = string.Empty;

    /// <summary>
    /// Puntaje asignado a la pregunta (por defecto: 1).
    /// </summary>
    public int Puntos { get; set; } = 1;

    /// <summary>
    /// Tipo de pregunta.
    /// </summary>
    public abstract TipoPregunta Tipo { get; }

    /// <summary>
    /// Línea en el código fuente donde inicia la pregunta.
    /// </summary>
    public int Line { get; set; }

    /// <summary>
    /// Columna en el código fuente donde inicia la pregunta.
    /// </summary>
    public int Col { get; set; }
}
