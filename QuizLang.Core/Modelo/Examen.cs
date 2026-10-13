using System.Collections.Generic;
using System.Linq;

namespace QuizLang.Core.Modelo;

/// <summary>
/// Representa el examen completo en QuizLang.
/// </summary>
public class Examen
{
    /// <summary>
    /// Título general del examen.
    /// </summary>
    public string Titulo { get; set; } = string.Empty;

    /// <summary>
    /// Puntaje mínimo necesario para aprobar (opcional).
    /// </summary>
    public int? PuntajeMinimo { get; set; }

    /// <summary>
    /// Línea donde se declaró el puntaje mínimo.
    /// </summary>
    public int PuntajeMinimoLine { get; set; }

    /// <summary>
    /// Columna donde se declaró el puntaje mínimo.
    /// </summary>
    public int PuntajeMinimoCol { get; set; }

    /// <summary>
    /// Línea donde se declaró el examen.
    /// </summary>
    public int Line { get; set; }

    /// <summary>
    /// Columna donde se declaró el examen.
    /// </summary>
    public int Col { get; set; }

    /// <summary>
    /// Lista ordenada de preguntas del examen.
    /// </summary>
    public List<Pregunta> Preguntas { get; set; } = new();

    /// <summary>
    /// Sumatoria total de puntos de todas las preguntas del examen.
    /// </summary>
    public int TotalPuntos => Preguntas.Sum(p => p.Puntos);
}
