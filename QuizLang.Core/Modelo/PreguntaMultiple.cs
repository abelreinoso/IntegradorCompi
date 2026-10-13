using System.Collections.Generic;

namespace QuizLang.Core.Modelo;

/// <summary>
/// Pregunta de opción múltiple con varias alternativas y exactamente una correcta.
/// </summary>
public class PreguntaMultiple : Pregunta
{
    public override TipoPregunta Tipo => TipoPregunta.Multiple;

    /// <summary>
    /// Lista de opciones disponibles.
    /// </summary>
    public List<Opcion> Opciones { get; set; } = new();
}
