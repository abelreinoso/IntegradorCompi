using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using QuizLang.Core.Modelo;

namespace QuizLang.Core;

/// <summary>
/// Resultado del proceso de compilación de QuizLang.
/// </summary>
public class CompilerResult
{
    public bool Success { get; set; }
    public Examen Examen { get; set; }
    public string Html { get; set; } = string.Empty;
    public List<Errors.ErrorItem> Messages { get; set; } = new();

    public IEnumerable<Errors.ErrorItem> Errors => Messages.FindAll(m => !m.IsWarning);
    public IEnumerable<Errors.ErrorItem> Warnings => Messages.FindAll(m => m.IsWarning);
}

/// <summary>
/// Fachada principal del compilador de QuizLang.
/// </summary>
public static class Compiler
{
    /// <summary>
    /// Compila un código fuente de QuizLang provisto como cadena de texto.
    /// </summary>
    public static CompilerResult CompileSource(string sourceCode, string outputPath = null)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(sourceCode ?? string.Empty);
        using var stream = new MemoryStream(bytes);
        return CompileStream(stream, outputPath);
    }

    /// <summary>
    /// Compila un archivo .quiz desde el disco.
    /// </summary>
    public static CompilerResult CompileFile(string filePath, string outputPath = null)
    {
        if (!File.Exists(filePath))
        {
            var res = new CompilerResult { Success = false };
            res.Messages.Add(new Errors.ErrorItem
            {
                Line = 0,
                Column = 0,
                Message = $"No se encontró el archivo de entrada: '{filePath}'",
                IsWarning = false
            });
            return res;
        }

        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var result = CompileStream(stream, outputPath);

        // Si la compilación fue exitosa y se especificó ruta de salida, escribir archivo
        if (result.Success && !string.IsNullOrWhiteSpace(outputPath))
        {
            string outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }
            File.WriteAllText(outputPath, result.Html, Encoding.UTF8);
        }

        return result;
    }

    /// <summary>
    /// Ejecuta el Scanner y Parser sobre un Stream.
    /// </summary>
    public static CompilerResult CompileStream(Stream stream, string outputPath = null)
    {
        var result = new CompilerResult();
        var errorWriter = new StringWriter();

        var scanner = new Scanner(stream);
        var parser = new Parser(scanner);

        // Redirigir el flujo de texto de errores de Coco
        parser.errors.errorStream = errorWriter;
        parser.errors.errMsgFormat = "linea {0} col {1}: {2}";

        parser.Parse();

        result.Messages.AddRange(parser.errors.Items);
        result.Examen = parser.Examen;

        if (parser.errors.count == 0 && result.Examen != null)
        {
            result.Success = true;
            result.Html = CodeGen.GenerarHtml(result.Examen);

            if (!string.IsNullOrWhiteSpace(outputPath))
            {
                string outDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
                {
                    Directory.CreateDirectory(outDir);
                }
                File.WriteAllText(outputPath, result.Html, Encoding.UTF8);
            }
        }
        else
        {
            result.Success = false;
        }

        return result;
    }
}
