# Manual de Usuario de QuizLang

**Cátedra:** Compiladores 2026  
**Integrantes:** Albornoz Joaquín, Díaz Guadalupe, Gallardo Mateo, Reinoso Abel  

---

## 1. Introducción

**QuizLang** es un lenguaje simple y expresivo para crear exámenes interactivos autocorregibles. A partir de un archivo de texto con extensión `.quiz`, el compilador genera un archivo `.html` moderno y autocontenido que puede ser visualizado en cualquier navegador web sin necesidad de servidores ni conexión a internet.

---

## 2. Guía de Sintaxis de QuizLang

### 2.1. Estructura General

Todo examen comienza con la palabra clave `EXAMEN` seguida del título entre comillas dobles. De forma opcional, puede definirse un puntaje mínimo para aprobar (`PUNTAJE_MINIMO`). A continuación se escriben las preguntas del examen.

```quiz
EXAMEN "Mi Primer Examen"
PUNTAJE_MINIMO 4

// Aquí van las preguntas...
```

### 2.2. Tipos de Preguntas

#### A) Pregunta de Opción Múltiple (`PREGUNTA_MULTIPLE`)
Contiene un enunciado, puntaje opcional (por defecto 1 punto) y una lista de alternativas entre llaves `{}`. Cada alternativa se identifica con una letra y un paréntesis. Exactamente una de las opciones debe marcarse con `[CORRECTA]`.

```quiz
PREGUNTA_MULTIPLE "¿Cuál es la capital de Argentina?" PUNTOS 2 {
  a) "Córdoba"
  b) "Buenos Aires" [CORRECTA]
  c) "Rosario"
}
```

#### B) Pregunta de Verdadero / Falso (`PREGUNTA_VF`)
Contiene un enunciado, puntaje opcional y una asignación con `=` a `VERDADERO` o `FALSO`.

```quiz
PREGUNTA_VF "¿C# es un lenguaje fuertemente tipado?" PUNTOS 1 = VERDADERO
PREGUNTA_VF "¿El sol gira alrededor de la Tierra?" PUNTOS 1 = FALSO
```

#### C) Pregunta de Respuesta Textual (`PREGUNTA_TEXTO`)
Permite al estudiante ingresar su respuesta en una caja de texto. Si se especifica `RESPUESTA "..."`, el sistema autocorregirá comparando el texto ingresado sin distinguir entre mayúsculas y minúsculas y recortando espacios sobrantes.

```quiz
PREGUNTA_TEXTO "¿Cómo se denomina la fase que agrupa caracteres en tokens?" PUNTOS 2 RESPUESTA "Scanner"
```

### 2.3. Comentarios
QuizLang permite documentar el código fuente mediante comentarios:
* **De una sola línea:** utilizando `//`
* **De bloque multilínea:** utilizando `/* ... */`

```quiz
// Comentario de encabezado
EXAMEN "Examen de Prueba"
/* Comentario explicativo
   de varias líneas */
PREGUNTA_VF "¿Es válido?" = VERDADERO // Comentario al final de línea
```

---

## 3. Uso de la Herramienta de Consola (`QuizLang.Cli`)

El compilador de línea de comandos permite automatizar la generación de exámenes.

### 3.1. Sintaxis del Comando

```powershell
QuizLang.Cli <archivo_entrada.quiz> [-o <archivo_salida.html>]
```

* `<archivo_entrada.quiz>`: ruta al archivo fuente.
* `-o <archivo_salida.html>`: (Opcional) ruta donde se guardará el HTML generado. Si no se especifica, se guardará en la misma ubicación del archivo de entrada cambiando la extensión a `.html`.

### 3.2. Códigos de Retorno
* **0 (Éxito):** El archivo se compiló correctamente y se generó el HTML.
* **1 (Error):** Se detectaron errores léxicos, sintácticos o semánticos.

### 3.3. Formato de Errores
Los errores se muestran en formato unificado indicando la posición exacta:
```text
linea <N> col <M>: <mensaje descriptivo en español>
```

**Ejemplo de uso:**
```powershell
# Compilación básica
dotnet run --project TrabajoFinal\QuizLang.Cli\QuizLang.Cli.csproj -- tests\basico.quiz

# Compilación con nombre de salida personalizado
dotnet run --project TrabajoFinal\QuizLang.Cli\QuizLang.Cli.csproj -- tests\compiladores2026.quiz -o parcial.html
```

---

## 4. Uso del Entorno Gráfico (`QuizLang.Gui`)

El entorno visual de QuizLang ofrece una experiencia integral para la edición y prueba de exámenes:

```powershell
# Ejecución de la aplicación gráfica
dotnet run --project TrabajoFinal\QuizLang.Gui\QuizLang.Gui.csproj
```

### 4.1. Características de la Interfaz:
1. **Editor de Código:** área con fuente monoespaciada y panel lateral de numeración de líneas sincronizado.
2. **Ejemplos Precargados:** selector en la barra superior con exámenes listos para probar:
   * *Examen Básico*
   * *Examen Completo*
   * *Compiladores 2026 (Examen Real de la Cátedra)*
   * *Examen con Comentarios*
   * *Casos de Error (Sintáctico y Semántico)*
3. **Botón ▶ Compilar:** compila el código en memoria y actualiza la lista de mensajes y la vista previa.
4. **Navegación de Errores con Doble Clic:** al hacer doble clic sobre cualquier fila del panel inferior de errores, el cursor del editor salta y selecciona automáticamente la línea y columna del error.
5. **Vista Previa Integrada:** permite responder y corregir el examen directamente dentro de la ventana de la aplicación.
6. **Botón 🌐 Ver HTML:** abre el examen generado en el navegador web predeterminado del sistema operativo.

---

## 5. Interacción con el Examen en el Navegador

El archivo HTML generado funciona de la siguiente manera:
1. **Encabezado:** Muestra el título del examen, el total de puntos posibles y el puntaje mínimo requerido.
2. **Responder:** El estudiante selecciona opciones en los botones de radio o escribe sus respuestas en los campos de texto.
3. **Botón "Corregir":**
   * Evalúa cada respuesta.
   * Pinta en **verde** las respuestas correctas (`✓ ¡Correcto!`).
   * Pinta en **rojo** las respuestas incorrectas y muestra la solución esperada.
   * Despliega un panel inferior con el puntaje total obtenido, el porcentaje de aciertos y un cartel destacado indicando si el examen fue **APROBADO** o **DESAPROBADO**.
4. **Botón "Reiniciar":** Limpia las respuestas y el resultado para permitir un nuevo intento.
