using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using QuizLang.Core;

namespace QuizLang.Gui;

public partial class Form1 : Form
{
    private string? _archivoActual = null;
    private string? _ultimoHtmlGenerado = null;

    public Form1()
    {
        InitializeComponent();
        CargarEjemplosEnCombo();
        cmbEjemplos.SelectedIndex = 0; // Cargar ejemplo básico por defecto
    }

    private void CargarEjemplosEnCombo()
    {
        cmbEjemplos.Items.AddRange(new object[]
        {
            "Examen Básico",
            "Examen Completo",
            "Compiladores 2026 (Examen Real)",
            "Examen con Comentarios",
            "Inválido: Error Sintáctico",
            "Inválido: Error Semántico"
        });
    }

    private void CmbEjemplos_SelectedIndexChanged(object? sender, EventArgs e)
    {
        string seleccionado = cmbEjemplos.SelectedItem?.ToString() ?? "";
        string codigo = seleccionado switch
        {
            "Examen Básico" =>
@"EXAMEN ""Evaluación Rápida de Programación""
PUNTAJE_MINIMO 2

PREGUNTA_MULTIPLE ""¿Cuál es el lenguaje orientado a objetos desarrollado por Microsoft?"" PUNTOS 1 {
  a) ""Python""
  b) ""C#"" [CORRECTA]
  c) ""C""
}

PREGUNTA_VF ""¿C# compila a Common Intermediate Language (CIL)?"" PUNTOS 1 = VERDADERO

PREGUNTA_TEXTO ""Nombre del framework de ejecución multiplataforma moderno (.NET)"" PUNTOS 1 RESPUESTA "".NET""
",
            "Examen Completo" =>
@"EXAMEN ""Examen Integral de Algoritmos y Estructuras""
PUNTAJE_MINIMO 6

PREGUNTA_MULTIPLE ""¿Cuál es la complejidad temporal promedio de búsqueda en una tabla hash?"" PUNTOS 2 {
  a) ""O(1)"" [CORRECTA]
  b) ""O(n)""
  c) ""O(log n)""
  d) ""O(n^2)""
}

PREGUNTA_VF ""¿Un árbol binario de búsqueda mantiene sus claves ordenadas in-order?"" PUNTOS 2 = VERDADERO

PREGUNTA_VF ""¿El algoritmo Dijkstra funciona directamente con pesos negativos en aristas?"" PUNTOS 2 = FALSO

PREGUNTA_TEXTO ""Estructura de datos LIFO (Last In First Out) en español o inglés"" PUNTOS 2 RESPUESTA ""Pila""

PREGUNTA_MULTIPLE ""¿Qué estructura se utiliza comúnmente para un recorrido en anchura (BFS)?"" PUNTOS 2 {
  a) ""Cola"" [CORRECTA]
  b) ""Pila""
  c) ""Árbol""
}
",
            "Compiladores 2026 (Examen Real)" =>
@"EXAMEN ""Parcial Práctico: Compiladores 2026""
PUNTAJE_MINIMO 7

PREGUNTA_MULTIPLE ""¿Cuál es la herramienta generadora de compiladores basada en gramáticas atribuidas utilizada en QuizLang?"" PUNTOS 2 {
  a) ""Coco/R"" [CORRECTA]
  b) ""Bison / Flex""
  c) ""ANTLR v4""
  d) ""Yacc""
}

PREGUNTA_VF ""¿Una gramática con recursión por izquierda directa puede ser parseada directamente por un parser descendente recursivo LL(1)?"" PUNTOS 2 = FALSO

PREGUNTA_MULTIPLE ""¿Qué propiedad garantiza que un parser LL(1) no tenga conflictos de decisión con k=1 símbolo de anticipación?"" PUNTOS 2 {
  a) ""Conjuntos FIRST y FOLLOW disjuntos en alternativas"" [CORRECTA]
  b) ""Gramática ambigua""
  c) ""Recursión izquierda mutua""
}

PREGUNTA_VF ""¿El analizador léxico (Scanner) transforma un flujo de caracteres en una secuencia de tokens?"" PUNTOS 2 = VERDADERO

PREGUNTA_TEXTO ""Estructura de datos que almacena identificadores, tipos y alcances durante la compilación"" PUNTOS 2 RESPUESTA ""Tabla de Simbolos""
",
            "Examen con Comentarios" =>
@"// ==========================================
// Examen de Demostración con Comentarios
// Materia: Compiladores
// ==========================================

EXAMEN ""Quiz de Comentarios y Directivas""
PUNTAJE_MINIMO 1

/* Pregunta de opción múltiple
   que demuestra comentarios de bloque
   multilínea en QuizLang */
PREGUNTA_MULTIPLE ""¿Los comentarios son ignorados por el analizador sintáctico?"" PUNTOS 1 {
  a) ""Sí, el Scanner los descarta"" [CORRECTA] // Comentario de línea
  b) ""No, generan tokens de comentario""
}

/* Fin del examen */
",
            "Inválido: Error Sintáctico" =>
@"EXAMEN ""Examen con Error Sintáctico""

PREGUNTA_MULTIPLE ""Falta cerrar la llave de las opciones"" {
  a) ""Opción A"" [CORRECTA]
  b) ""Opción B""
// Falta cerrar la llave '}'
",
            "Inválido: Error Semántico" =>
@"EXAMEN ""Examen con Errores Semánticos""
PUNTAJE_MINIMO 10

PREGUNTA_MULTIPLE ""Pregunta sin ninguna opción correcta"" PUNTOS 1 {
  a) ""Primera opción""
  b) ""Segunda opción""
}

PREGUNTA_MULTIPLE ""Pregunta sin ninguna opción correcta"" PUNTOS 1 {
  a) ""Opción duplicada A"" [CORRECTA]
  a) ""Opción duplicada letra A""
}
",
            _ => ""
        };

        if (!string.IsNullOrEmpty(codigo))
        {
            txtEditor.Text = codigo;
            _archivoActual = null;
            ActualizarTituloVentana();
            LimpiarErrores();
        }
    }

    private void ActualizarTituloVentana()
    {
        string nombre = string.IsNullOrEmpty(_archivoActual) ? "Sin título" : Path.GetFileName(_archivoActual);
        Text = $"QuizLang IDE - [{nombre}]";
    }

    private void LimpiarErrores()
    {
        dgvErrores.Rows.Clear();
        lblEstado.Text = "Listo";
        lblEstado.ForeColor = Color.Black;
    }

    private void BtnAbrir_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "Archivos QuizLang (*.quiz)|*.quiz|Todos los archivos (*.*)|*.*",
            Title = "Abrir archivo QuizLang"
        };

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            _archivoActual = ofd.FileName;
            txtEditor.Text = File.ReadAllText(_archivoActual, Encoding.UTF8);
            ActualizarTituloVentana();
            LimpiarErrores();
        }
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_archivoActual))
        {
            BtnGuardarComo_Click(sender, e);
        }
        else
        {
            File.WriteAllText(_archivoActual, txtEditor.Text, Encoding.UTF8);
            lblEstado.Text = $"Guardado en {_archivoActual}";
        }
    }

    private void BtnGuardarComo_Click(object? sender, EventArgs e)
    {
        using var sfd = new SaveFileDialog
        {
            Filter = "Archivos QuizLang (*.quiz)|*.quiz|Todos los archivos (*.*)|*.*",
            Title = "Guardar archivo QuizLang",
            DefaultExt = "quiz"
        };

        if (sfd.ShowDialog() == DialogResult.OK)
        {
            _archivoActual = sfd.FileName;
            File.WriteAllText(_archivoActual, txtEditor.Text, Encoding.UTF8);
            ActualizarTituloVentana();
            lblEstado.Text = $"Guardado en {_archivoActual}";
        }
    }

    private void BtnCompilar_Click(object? sender, EventArgs e)
    {
        dgvErrores.Rows.Clear();

        string outHtmlPath = Path.Combine(Path.GetTempPath(), "quizlang_preview.html");
        _ultimoHtmlGenerado = outHtmlPath;

        var result = Compiler.CompileSource(txtEditor.Text, outHtmlPath);

        foreach (var msg in result.Messages)
        {
            string tipo = msg.IsWarning ? "Advertencia" : "Error";
            int rowIndex = dgvErrores.Rows.Add(tipo, msg.Line, msg.Column, msg.Message);

            if (msg.IsWarning)
            {
                dgvErrores.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 199);
            }
            else
            {
                dgvErrores.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(254, 226, 226);
            }
        }

        if (result.Success)
        {
            lblEstado.Text = $"✓ Compilación exitosa: {result.Examen.Preguntas.Count} preguntas, {result.Examen.TotalPuntos} puntos en total.";
            lblEstado.ForeColor = Color.DarkGreen;

            // Mostrar en el visor HTML integrado
            webBrowser.DocumentText = result.Html;
        }
        else
        {
            int errCount = result.Errors.Count();
            lblEstado.Text = $"✗ Compilación fallida: se encontraron {errCount} error(es).";
            lblEstado.ForeColor = Color.DarkRed;
        }
    }

    private void BtnVerHtml_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_ultimoHtmlGenerado) || !File.Exists(_ultimoHtmlGenerado))
        {
            BtnCompilar_Click(sender, e);
        }

        if (!string.IsNullOrEmpty(_ultimoHtmlGenerado) && File.Exists(_ultimoHtmlGenerado))
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = _ultimoHtmlGenerado,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el navegador: {ex.Message}", "QuizLang", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void DgvErrores_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= dgvErrores.Rows.Count) return;

        var row = dgvErrores.Rows[e.RowIndex];
        if (int.TryParse(row.Cells[1].Value?.ToString(), out int linea) && linea > 0)
        {
            int col = 1;
            int.TryParse(row.Cells[2].Value?.ToString(), out col);

            // Posicionar el cursor en la línea y columna
            int charIndex = txtEditor.GetFirstCharIndexFromLine(linea - 1);
            if (charIndex >= 0)
            {
                int targetPos = Math.Min(charIndex + Math.Max(0, col - 1), txtEditor.TextLength);
                txtEditor.Focus();
                txtEditor.Select(targetPos, 0);

                // Seleccionar la línea completa para visibilidad
                int lineaSigIndex = txtEditor.GetFirstCharIndexFromLine(linea);
                int len = (lineaSigIndex >= 0 ? lineaSigIndex : txtEditor.TextLength) - charIndex;
                if (len > 0)
                {
                    txtEditor.Select(charIndex, len);
                }
                txtEditor.ScrollToCaret();
            }
        }
    }

    private void TxtEditor_TextChanged(object? sender, EventArgs e)
    {
        picLineNumbers.Invalidate();
        ActualizarPosicionCursor();
    }

    private void TxtEditor_VScroll(object? sender, EventArgs e)
    {
        picLineNumbers.Invalidate();
    }

    private void TxtEditor_SelectionChanged(object? sender, EventArgs e)
    {
        ActualizarPosicionCursor();
    }

    private void ActualizarPosicionCursor()
    {
        int index = txtEditor.SelectionStart;
        int linea = txtEditor.GetLineFromCharIndex(index) + 1;
        int primerChar = txtEditor.GetFirstCharIndexFromLine(linea - 1);
        int col = (index - primerChar) + 1;
        lblPosicion.Text = $"Línea: {linea}, Col: {col}";
    }

    private void PicLineNumbers_Paint(object? sender, PaintEventArgs e)
    {
        e.Graphics.Clear(Color.FromArgb(245, 245, 245));

        int firstIndex = txtEditor.GetCharIndexFromPosition(new Point(0, 0));
        int firstLine = txtEditor.GetLineFromCharIndex(firstIndex);

        int lastIndex = txtEditor.GetCharIndexFromPosition(new Point(txtEditor.ClientRectangle.Width, txtEditor.ClientRectangle.Height));
        int lastLine = txtEditor.GetLineFromCharIndex(lastIndex);

        using var font = new Font("Consolas", 9F, FontStyle.Regular);
        using var brush = new SolidBrush(Color.FromArgb(120, 120, 120));
        using var borderPen = new Pen(Color.FromArgb(220, 220, 220));

        e.Graphics.DrawLine(borderPen, picLineNumbers.Width - 1, 0, picLineNumbers.Width - 1, picLineNumbers.Height);

        for (int line = firstLine; line <= lastLine + 1; line++)
        {
            int lineCharIndex = txtEditor.GetFirstCharIndexFromLine(line);
            if (lineCharIndex < 0) break;

            Point pt = txtEditor.GetPositionFromCharIndex(lineCharIndex);
            string lineStr = (line + 1).ToString();
            SizeF size = e.Graphics.MeasureString(lineStr, font);

            float x = picLineNumbers.Width - size.Width - 6;
            float y = pt.Y + 2;

            e.Graphics.DrawString(lineStr, font, brush, x, y);
        }
    }
}
