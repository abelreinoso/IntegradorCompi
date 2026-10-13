using System;
using System.Collections.Generic;
using System.Linq;
using QuizLang.Core.Modelo;

namespace QuizLang.Core;

/// <summary>
/// Tabla de símbolos y verificador semántico de QuizLang.
/// </summary>
public class SymbolTable
{
    private readonly Errors _errors;
    private readonly HashSet<string> _enunciados = new(StringComparer.OrdinalIgnoreCase);

    public Examen Examen { get; } = new();

    public SymbolTable(Errors errors)
    {
        _errors = errors ?? throw new ArgumentNullException(nameof(errors));
    }

    /// <summary>
    /// Registra el título y posición del examen principal.
    /// </summary>
    public void SetExamen(string titulo, int line, int col)
    {
        Examen.Titulo = titulo;
        Examen.Line = line;
        Examen.Col = col;
    }

    /// <summary>
    /// Configura el puntaje mínimo de aprobación y su posición.
    /// </summary>
    public void SetConfig(int puntajeMinimo, int line, int col)
    {
        Examen.PuntajeMinimo = puntajeMinimo;
        Examen.PuntajeMinimoLine = line;
        Examen.PuntajeMinimoCol = col;
    }

    /// <summary>
    /// Valida y registra una pregunta de opción múltiple.
    /// </summary>
    public void ValidarYAgregarPreguntaMultiple(PreguntaMultiple p)
    {
        ValidarEnunciado(p);

        // 1. Chequeo de opciones: letras repetidas y consecutivas
        var letrasVistas = new HashSet<char>();
        char? siguienteEsperada = null;

        foreach (var op in p.Opciones)
        {
            char letraNormalizada = char.ToLowerInvariant(op.Letra);

            if (!letrasVistas.Add(letraNormalizada))
            {
                _errors.SemErr(op.Line, op.Col,
                    $"Letra de opción duplicada '{op.Letra}' en la pregunta \"{p.Enunciado}\".");
            }

            if (siguienteEsperada.HasValue && letraNormalizada != siguienteEsperada.Value)
            {
                _errors.Warning(op.Line, op.Col,
                    $"Letra de opción no consecutiva: se esperaba '{siguienteEsperada.Value}' pero se encontró '{op.Letra}'.");
            }

            siguienteEsperada = (char)(letraNormalizada + 1);
        }

        // 2. Chequeo de respuestas correctas: exactamente una [CORRECTA]
        int correctas = p.Opciones.Count(o => o.EsCorrecta);
        if (correctas == 0)
        {
            _errors.SemErr(p.Line, p.Col,
                $"La pregunta múltiple \"{p.Enunciado}\" no tiene ninguna opción marcada como [CORRECTA].");
        }
        else if (correctas > 1)
        {
            _errors.SemErr(p.Line, p.Col,
                $"La pregunta múltiple \"{p.Enunciado}\" tiene más de una opción marcada como [CORRECTA] (se encontraron {correctas}).");
        }

        Examen.Preguntas.Add(p);
    }

    /// <summary>
    /// Valida y registra una pregunta de respuesta textual.
    /// </summary>
    public void ValidarYAgregarPreguntaTexto(PreguntaTexto p)
    {
        ValidarEnunciado(p);
        Examen.Preguntas.Add(p);
    }

    /// <summary>
    /// Valida y registra una pregunta de Verdadero/Falso.
    /// </summary>
    public void ValidarYAgregarPreguntaVF(PreguntaVF p)
    {
        ValidarEnunciado(p);
        Examen.Preguntas.Add(p);
    }

    /// <summary>
    /// Valida que el enunciado no se encuentre repetido en el examen.
    /// </summary>
    private void ValidarEnunciado(Pregunta p)
    {
        string enunciadoNormalizado = p.Enunciado.Trim();
        if (!_enunciados.Add(enunciadoNormalizado))
        {
            _errors.SemErr(p.Line, p.Col,
                $"Enunciado de pregunta duplicado: \"{p.Enunciado}\".");
        }
    }

    /// <summary>
    /// Valida condiciones globales del examen al finalizar el análisis sintáctico.
    /// </summary>
    public void ValidarExamen()
    {
        // 1. Examen sin preguntas
        if (Examen.Preguntas.Count == 0)
        {
            _errors.SemErr(Examen.Line, Examen.Col,
                "El examen no contiene preguntas.");
            return;
        }

        // 2. PUNTAJE_MINIMO mayor al total de puntos del examen
        if (Examen.PuntajeMinimo.HasValue && Examen.PuntajeMinimo.Value > Examen.TotalPuntos)
        {
            _errors.SemErr(Examen.PuntajeMinimoLine, Examen.PuntajeMinimoCol,
                $"El puntaje mínimo ({Examen.PuntajeMinimo.Value}) es mayor al total de puntos del examen ({Examen.TotalPuntos}).");
        }
    }
}
