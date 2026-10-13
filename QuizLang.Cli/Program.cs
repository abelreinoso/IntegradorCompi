using System;
using System.IO;
using QuizLang.Core;

namespace QuizLang.Cli;

internal class Program
{
    private static int Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        if (args.Length == 0 || args[0] == "-h" || args[0] == "--help" || args[0] == "/?")
        {
            Console.WriteLine("QuizLang Compiler CLI - Compilador de exámenes interactivos");
            Console.WriteLine("Uso: QuizLang.Cli <archivo_entrada.quiz> [-o <archivo_salida.html>]");
            return 1;
        }

        string? entrada = null;
        string? salida = null;

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-o" && i + 1 < args.Length)
            {
                salida = args[i + 1];
                i++;
            }
            else if (entrada == null)
            {
                entrada = args[i];
            }
            else
            {
                Console.WriteLine($"Parámetro desconocido o redundante: {args[i]}");
                return 1;
            }
        }

        if (string.IsNullOrWhiteSpace(entrada))
        {
            Console.WriteLine("Debe especificar un archivo de entrada .quiz");
            return 1;
        }

        if (string.IsNullOrWhiteSpace(salida))
        {
            salida = Path.ChangeExtension(entrada, ".html");
        }

        if (!File.Exists(entrada))
        {
            Console.WriteLine($"linea 0 col 0: No se encontró el archivo '{entrada}'");
            return 1;
        }

        var result = Compiler.CompileFile(entrada, salida);

        if (result.Success)
        {
            // Mostrar advertencias si hubiere
            foreach (var warning in result.Warnings)
            {
                Console.WriteLine($"linea {warning.Line} col {warning.Column}: advertencia: {warning.Message}");
            }

            Console.WriteLine($"Compilación exitosa. HTML generado en: {salida}");
            return 0;
        }
        else
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"linea {error.Line} col {error.Column}: {error.Message}");
            }

            return 1;
        }
    }
}
