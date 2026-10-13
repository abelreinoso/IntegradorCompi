namespace QuizLang.Core.Modelo;

/// <summary>
/// Pregunta de Verdadero o Falso.
/// </summary>
public class PreguntaVF : Pregunta
{
    public override TipoPregunta Tipo => TipoPregunta.VerdaderoFalso;

    /// <summary>
    /// Valor booleano correcto (true = VERDADERO, false = FALSO).
    /// </summary>
    public bool RespuestaCorrecta { get; set; }
}
