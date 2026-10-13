using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using QuizLang.Core.Modelo;

namespace QuizLang.Core;

/// <summary>
/// Generador de código HTML/CSS/JS autocontenido a partir del AST de QuizLang.
/// </summary>
public static class CodeGen
{
    public static string GenerarHtml(Examen examen)
    {
        if (examen == null) throw new ArgumentNullException(nameof(examen));

        var sb = new StringBuilder();

        // Estructura de datos para evaluación en JavaScript
        var preguntasEvaluacion = examen.Preguntas.Select((p, idx) =>
        {
            object respuesta = p switch
            {
                PreguntaMultiple pm => pm.Opciones.FirstOrDefault(o => o.EsCorrecta)?.Letra.ToString() ?? "",
                PreguntaVF pvf => pvf.RespuestaCorrecta ? "VERDADERO" : "FALSO",
                PreguntaTexto pt => pt.RespuestaCorrecta ?? "",
                _ => ""
            };

            return new
            {
                id = idx + 1,
                tipo = p.Tipo.ToString(),
                puntos = p.Puntos,
                correcta = respuesta
            };
        }).ToList();

        string datosJson = JsonSerializer.Serialize(preguntasEvaluacion);
        int puntajeMinimo = examen.PuntajeMinimo ?? (int)Math.Ceiling(examen.TotalPuntos * 0.6);
        bool tienePuntajeMinimoExplicito = examen.PuntajeMinimo.HasValue;

        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang=\"es\">");
        sb.AppendLine("<head>");
        sb.AppendLine("  <meta charset=\"UTF-8\">");
        sb.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        sb.AppendLine($"  <title>{WebUtility.HtmlEncode(examen.Titulo)}</title>");
        sb.AppendLine("  <style>");
        sb.AppendLine(@"    :root {
      --primary: #4f46e5;
      --primary-hover: #4338ca;
      --bg: #f8fafc;
      --card-bg: #ffffff;
      --text: #1e293b;
      --text-muted: #64748b;
      --border: #e2e8f0;
      --success: #16a34a;
      --success-bg: #dcfce7;
      --error: #dc2626;
      --error-bg: #fee2e2;
    }
    * { box-sizing: border-box; margin: 0; padding: 0; }
    body {
      font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
      background-color: var(--bg);
      color: var(--text);
      line-height: 1.6;
      padding: 2rem 1rem;
    }
    .container {
      max-width: 820px;
      margin: 0 auto;
    }
    .header {
      background: var(--card-bg);
      padding: 2rem;
      border-radius: 12px;
      box-shadow: 0 4px 6px -1px rgba(0,0,0,0.06), 0 2px 4px -2px rgba(0,0,0,0.06);
      margin-bottom: 2rem;
      border-top: 5px solid var(--primary);
    }
    .header h1 {
      font-size: 1.85rem;
      font-weight: 700;
      color: #0f172a;
      margin-bottom: 0.75rem;
    }
    .meta-badges {
      display: flex;
      flex-wrap: wrap;
      gap: 0.75rem;
      font-size: 0.9rem;
    }
    .badge {
      display: inline-flex;
      align-items: center;
      padding: 0.25rem 0.75rem;
      border-radius: 9999px;
      font-weight: 500;
      background: #e0e7ff;
      color: #3730a3;
    }
    .pregunta-card {
      background: var(--card-bg);
      padding: 1.75rem;
      border-radius: 10px;
      margin-bottom: 1.5rem;
      box-shadow: 0 2px 4px rgba(0,0,0,0.04);
      border: 1px solid var(--border);
      transition: all 0.2s ease;
    }
    .pregunta-card.correcta {
      border-color: var(--success);
      background-color: #f0fdf4;
    }
    .pregunta-card.incorrecta {
      border-color: var(--error);
      background-color: #fef2f2;
    }
    .pregunta-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      gap: 1rem;
      margin-bottom: 1rem;
    }
    .pregunta-titulo {
      font-weight: 600;
      font-size: 1.1rem;
      color: #1e293b;
    }
    .puntos-tag {
      background: #f1f5f9;
      color: var(--text-muted);
      padding: 0.2rem 0.5rem;
      border-radius: 6px;
      font-size: 0.85rem;
      font-weight: 600;
      white-space: nowrap;
    }
    .opciones-list {
      display: flex;
      flex-direction: column;
      gap: 0.6rem;
      margin-top: 0.5rem;
    }
    .opcion-label {
      display: flex;
      align-items: center;
      padding: 0.65rem 0.9rem;
      border-radius: 8px;
      border: 1px solid var(--border);
      cursor: pointer;
      background: #fff;
      transition: background 0.15s, border-color 0.15s;
    }
    .opcion-label:hover {
      background: #f8fafc;
      border-color: #cbd5e1;
    }
    .opcion-label input {
      margin-right: 0.75rem;
    }
    .opcion-label.correct-highlight {
      background: var(--success-bg);
      border-color: var(--success);
      font-weight: 600;
    }
    .opcion-label.wrong-highlight {
      background: var(--error-bg);
      border-color: var(--error);
    }
    .input-texto {
      width: 100%;
      padding: 0.75rem 1rem;
      border: 1px solid var(--border);
      border-radius: 8px;
      font-size: 1rem;
      outline: none;
      transition: border-color 0.2s;
    }
    .input-texto:focus {
      border-color: var(--primary);
      box-shadow: 0 0 0 3px rgba(79, 70, 229, 0.15);
    }
    .feedback {
      margin-top: 1rem;
      padding: 0.75rem;
      border-radius: 6px;
      font-size: 0.9rem;
      font-weight: 500;
      display: none;
    }
    .feedback.ok {
      display: block;
      background: var(--success-bg);
      color: #15803d;
    }
    .feedback.err {
      display: block;
      background: var(--error-bg);
      color: #b91c1c;
    }
    .acciones {
      margin-top: 2rem;
      display: flex;
      gap: 1rem;
      justify-content: center;
    }
    .btn {
      padding: 0.85rem 2rem;
      border-radius: 8px;
      border: none;
      font-size: 1.05rem;
      font-weight: 600;
      cursor: pointer;
      transition: all 0.2s ease;
    }
    .btn-primary {
      background: var(--primary);
      color: white;
    }
    .btn-primary:hover {
      background: var(--primary-hover);
    }
    .btn-secondary {
      background: #e2e8f0;
      color: var(--text);
    }
    .btn-secondary:hover {
      background: #cbd5e1;
    }
    .resultado-panel {
      margin-top: 2rem;
      background: var(--card-bg);
      padding: 2rem;
      border-radius: 12px;
      box-shadow: 0 4px 6px rgba(0,0,0,0.06);
      text-align: center;
      display: none;
      border: 2px solid var(--border);
    }
    .resultado-panel.aprobado {
      border-color: var(--success);
      background: #f0fdf4;
    }
    .resultado-panel.desaprobado {
      border-color: var(--error);
      background: #fef2f2;
    }
    .resultado-panel h2 {
      font-size: 1.6rem;
      margin-bottom: 0.5rem;
    }
    .resultado-panel .puntaje-display {
      font-size: 1.25rem;
      font-weight: 600;
      margin-bottom: 0.5rem;
    }
  </style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        sb.AppendLine("  <div class=\"container\">");
        sb.AppendLine("    <header class=\"header\">");
        sb.AppendLine($"      <h1>{WebUtility.HtmlEncode(examen.Titulo)}</h1>");
        sb.AppendLine("      <div class=\"meta-badges\">");
        sb.AppendLine($"        <span class=\"badge\">Total: {examen.TotalPuntos} {(examen.TotalPuntos == 1 ? "punto" : "puntos")}</span>");
        if (tienePuntajeMinimoExplicito)
        {
            sb.AppendLine($"        <span class=\"badge\">Puntaje mínimo de aprobación: {puntajeMinimo}</span>");
        }
        sb.AppendLine($"        <span class=\"badge\">{examen.Preguntas.Count} {(examen.Preguntas.Count == 1 ? "pregunta" : "preguntas")}</span>");
        sb.AppendLine("      </div>");
        sb.AppendLine("    </header>");

        sb.AppendLine("    <form id=\"quiz-form\" onsubmit=\"return false;\">");

        for (int i = 0; i < examen.Preguntas.Count; i++)
        {
            var p = examen.Preguntas[i];
            int num = i + 1;
            sb.AppendLine($"      <section class=\"pregunta-card\" id=\"pregunta-{num}\">");
            sb.AppendLine("        <div class=\"pregunta-header\">");
            sb.AppendLine($"          <div class=\"pregunta-titulo\">{num}. {WebUtility.HtmlEncode(p.Enunciado)}</div>");
            sb.AppendLine($"          <span class=\"puntos-tag\">{p.Puntos} {(p.Puntos == 1 ? "pto" : "pts")}</span>");
            sb.AppendLine("        </div>");

            switch (p)
            {
                case PreguntaMultiple pm:
                    sb.AppendLine("        <div class=\"opciones-list\">");
                    foreach (var op in pm.Opciones)
                    {
                        string opId = $"p{num}_{op.Letra}";
                        sb.AppendLine($"          <label class=\"opcion-label\" id=\"label_{opId}\">");
                        sb.AppendLine($"            <input type=\"radio\" name=\"preg_{num}\" value=\"{op.Letra}\" id=\"{opId}\">");
                        sb.AppendLine($"            <span><strong>{op.Letra})</strong> {WebUtility.HtmlEncode(op.Texto)}</span>");
                        sb.AppendLine("          </label>");
                    }
                    sb.AppendLine("        </div>");
                    break;

                case PreguntaVF pvf:
                    sb.AppendLine("        <div class=\"opciones-list\">");
                    sb.AppendLine($"          <label class=\"opcion-label\" id=\"label_p{num}_VERDADERO\">");
                    sb.AppendLine($"            <input type=\"radio\" name=\"preg_{num}\" value=\"VERDADERO\" id=\"p{num}_VERDADERO\">");
                    sb.AppendLine("            <span>Verdadero</span>");
                    sb.AppendLine("          </label>");
                    sb.AppendLine($"          <label class=\"opcion-label\" id=\"label_p{num}_FALSO\">");
                    sb.AppendLine($"            <input type=\"radio\" name=\"preg_{num}\" value=\"FALSO\" id=\"p{num}_FALSO\">");
                    sb.AppendLine("            <span>Falso</span>");
                    sb.AppendLine("          </label>");
                    sb.AppendLine("        </div>");
                    break;

                case PreguntaTexto pt:
                    sb.AppendLine("        <div style=\"margin-top: 0.75rem;\">");
                    sb.AppendLine($"          <input type=\"text\" class=\"input-texto\" id=\"input_p{num}\" name=\"preg_{num}\" placeholder=\"Escribe tu respuesta aquí...\" autocomplete=\"off\">");
                    sb.AppendLine("        </div>");
                    break;
            }

            sb.AppendLine($"        <div class=\"feedback\" id=\"feedback-{num}\"></div>");
            sb.AppendLine("      </section>");
        }

        sb.AppendLine("      <div class=\"acciones\">");
        sb.AppendLine("        <button type=\"button\" class=\"btn btn-primary\" id=\"btn-corregir\" onclick=\"corregirExamen()\">Corregir</button>");
        sb.AppendLine("        <button type=\"button\" class=\"btn btn-secondary\" onclick=\"reiniciarExamen()\">Reiniciar</button>");
        sb.AppendLine("      </div>");
        sb.AppendLine("    </form>");

        sb.AppendLine("    <div class=\"resultado-panel\" id=\"resultado-panel\">");
        sb.AppendLine("      <h2 id=\"resultado-titulo\"></h2>");
        sb.AppendLine("      <div class=\"puntaje-display\" id=\"resultado-puntaje\"></div>");
        sb.AppendLine("      <p id=\"resultado-detalle\"></p>");
        sb.AppendLine("    </div>");
        sb.AppendLine("  </div>");

        // Script JavaScript
        sb.AppendLine("  <script>");
        sb.AppendLine($"    const preguntas = {datosJson};");
        sb.AppendLine($"    const puntajeMinimo = {puntajeMinimo};");
        sb.AppendLine($"    const totalPuntos = {examen.TotalPuntos};");
        sb.AppendLine(@"
    function corregirExamen() {
      let puntajeObtenido = 0;

      preguntas.forEach(p => {
        const card = document.getElementById(`pregunta-${p.id}`);
        const feedback = document.getElementById(`feedback-${p.id}`);
        card.classList.remove('correcta', 'incorrecta');
        feedback.className = 'feedback';
        feedback.innerHTML = '';

        // Reset highlights
        card.querySelectorAll('.opcion-label').forEach(lbl => {
          lbl.classList.remove('correct-highlight', 'wrong-highlight');
        });

        let esCorrecta = false;
        let respuestaDada = null;

        if (p.tipo === 'Multiple') {
          const seleccionada = document.querySelector(`input[name=""preg_${p.id}""]:checked`);
          respuestaDada = seleccionada ? seleccionada.value : null;

          const labelCorrecto = document.getElementById(`label_p${p.id}_${p.correcta}`);
          if (labelCorrecto) labelCorrecto.classList.add('correct-highlight');

          if (respuestaDada && respuestaDada.toLowerCase() === p.correcta.toLowerCase()) {
            esCorrecta = true;
          } else if (respuestaDada) {
            const labelSeleccionado = document.getElementById(`label_p${p.id}_${respuestaDada}`);
            if (labelSeleccionado) labelSeleccionado.classList.add('wrong-highlight');
          }
        } else if (p.tipo === 'VerdaderoFalso') {
          const seleccionada = document.querySelector(`input[name=""preg_${p.id}""]:checked`);
          respuestaDada = seleccionada ? seleccionada.value : null;

          const labelCorrecto = document.getElementById(`label_p${p.id}_${p.correcta}`);
          if (labelCorrecto) labelCorrecto.classList.add('correct-highlight');

          if (respuestaDada && respuestaDada === p.correcta) {
            esCorrecta = true;
          } else if (respuestaDada) {
            const labelSeleccionado = document.getElementById(`label_p${p.id}_${respuestaDada}`);
            if (labelSeleccionado) labelSeleccionado.classList.add('wrong-highlight');
          }
        } else if (p.tipo === 'Texto') {
          const input = document.getElementById(`input_p${p.id}`);
          respuestaDada = input ? input.value.trim() : '';

          if (p.correcta && p.correcta.trim().length > 0) {
            if (respuestaDada.toLowerCase() === p.correcta.trim().toLowerCase()) {
              esCorrecta = true;
            }
          } else {
            // Si la pregunta no tiene respuesta esperada, se considera correcta si no está vacía
            esCorrecta = (respuestaDada.length > 0);
          }
        }

        if (esCorrecta) {
          puntajeObtenido += p.puntos;
          card.classList.add('correcta');
          feedback.classList.add('ok');
          feedback.innerHTML = `✓ ¡Correcto! (+${p.puntos} ${p.puntos === 1 ? 'punto' : 'puntos'})`;
        } else {
          card.classList.add('incorrecta');
          feedback.classList.add('err');
          let detalle = `✗ Incorrecto (0/${p.puntos} puntos).`;
          if (p.tipo === 'Texto' && p.correcta) {
            detalle += ` Respuesta correcta esperada: ""${p.correcta}""`;
          } else if (p.tipo === 'Multiple' || p.tipo === 'VerdaderoFalso') {
            detalle += ` Opción correcta: ${p.correcta}`;
          }
          feedback.innerHTML = detalle;
        }
      });

      // Panel de resultados
      const panel = document.getElementById('resultado-panel');
      const titulo = document.getElementById('resultado-titulo');
      const display = document.getElementById('resultado-puntaje');
      const detalle = document.getElementById('resultado-detalle');

      panel.style.display = 'block';
      panel.classList.remove('aprobado', 'desaprobado');

      const aprobado = (puntajeObtenido >= puntajeMinimo);
      if (aprobado) {
        panel.classList.add('aprobado');
        titulo.innerText = '🎉 ¡Felicitaciones! Has APROBADO el examen.';
        titulo.style.color = '#15803d';
      } else {
        panel.classList.add('desaprobado');
        titulo.innerText = '❌ Has DESAPROBADO el examen.';
        titulo.style.color = '#b91c1c';
      }

      display.innerText = `Puntaje obtenido: ${puntajeObtenido} / ${totalPuntos} puntos`;
      detalle.innerText = `Puntaje mínimo requerido: ${puntajeMinimo} puntos. Porcentaje alcanzado: ${Math.round((puntajeObtenido / totalPuntos) * 100)}%.`;

      panel.scrollIntoView({ behavior: 'smooth' });
    }

    function reiniciarExamen() {
      document.getElementById('quiz-form').reset();
      document.querySelectorAll('.pregunta-card').forEach(c => c.classList.remove('correcta', 'incorrecta'));
      document.querySelectorAll('.opcion-label').forEach(l => l.classList.remove('correct-highlight', 'wrong-highlight'));
      document.querySelectorAll('.feedback').forEach(f => { f.className = 'feedback'; f.innerHTML = ''; });
      const panel = document.getElementById('resultado-panel');
      panel.style.display = 'none';
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  </script>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        return sb.ToString();
    }
}
