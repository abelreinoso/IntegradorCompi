namespace QuizLang.Core.Modelo;

/// <summary>
/// Pregunta de respuesta abierta o texto libre.
/// </summary>
public class PreguntaTexto : Pregunta
{
    public override TipoPregunta Tipo => TipoPregunta.Texto;

    /// <summary>
    /// Respuesta textual esperada para la autocorrección (opcional).
    /// </summary>
    public string? RespuestaCorrecta { get; set; }
}
