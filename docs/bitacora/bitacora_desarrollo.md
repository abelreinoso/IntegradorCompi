# Bitácora de Desarrollo - QuizLang

**Proyecto:** Compilador de QuizLang para Exámenes Interactivos  
**Cátedra:** Compiladores 2026  
**Integrantes:** Albornoz Joaquín, Díaz Guadalupe, Gallardo Mateo, Reinoso Abel  
**Fecha:** 2026-10-13  
**Herramienta:** Antigravity – subagente modelo flash  

---

## 1. Registro Cronológico de Tareas y Prompts

| Fecha | Herramienta | Tarea / Prompt | Resumen | Errores Encontrados (Mensaje Exacto) y Resolución | Verificación |
| :--- | :--- | :--- | :--- | :--- | :--- |
| 2026-10-13 | Antigravity – subagente modelo flash | Obtención de herramientas y configuración de Coco/R C# | Descarga de `Coco.exe`, `Scanner.frame` y `Parser.frame` desde SSW Linz. Verificación de ejecución en Windows. | Ninguno. `Coco.exe` ejecutó correctamente en el entorno Windows con .NET 10. | Ejecución de `Coco.exe` sin argumentos reportó ayuda de sintaxis con código 1. |
| 2026-10-13 | Antigravity – subagente modelo flash | Creación de solución y proyectos .NET 10 | Creación de `QuizLang.sln`, `QuizLang.Core` (classlib), `QuizLang.Cli` (console) y `QuizLang.Gui` (winforms). | Al ejecutar `dotnet new winforms -f net10.0-windows` el CLI rechazó el parámetro `-f` con error `Opción u opciones no válidas: -f net10.0-windows`. Se resolvió usando `dotnet new winforms` que adopta automáticamente el SDK 10 de Windows Forms. También se generó `.slnx` inicialmente, reemplazado por `.sln` clásico mediante `-f sln`. | `dotnet build QuizLang.sln` compiló con 0 advertencias y 0 errores. |
| 2026-10-13 | Antigravity – subagente modelo flash | Especificación léxica de caracteres y tokens en Coco/R | Definición de tokens para strings con escapes, letras para opciones, números y palabras clave. | Error Coco/R: `-- line 10 col 18: invalid TokenFactor / contents of {...} must not be deletable` al intentar definir `ANY - '"'` dentro de `TOKENS`. En Coco/R, la diferencia de conjuntos `ANY - ...` pertenece a `CHARACTERS`. Se resolvió declarando `stringChar = ANY - '"' - '\\' - cr - lf` en `CHARACTERS`. | Coco/R generó scanner y parser de prueba sin errores. |
| 2026-10-13 | Antigravity – subagente modelo flash | Definición de gramática LL(1) `QuizLang.atg` | Implementación de las producciones para `QuizLang`, `Config`, `Pregunta`, `Multiple`, `Opcion`, `Texto` y `VerdaderoFalso`. | Error Coco/R: `-- line 119 col 5: missing production for grammar name` porque la producción raíz se llamaba `Quiz` pero la cabecera era `COMPILER QuizLang`. Se resolvió renombrando la producción raíz a `QuizLang`. | Coco/R verificó la gramática LL(1) con 0 conflictos y 0 advertencias. |
| 2026-10-13 | Antigravity – subagente modelo flash | Integración de directivas using en ATG | Inclusión del espacio de nombres `QuizLang.Core.Modelo` en el archivo ATG. | Errores de compilación C#: `Parser.cs(24,27): error CS1513: Se esperaba }` y `Parser.cs(26,1): error CS1529: Una cláusula using debe preceder al resto de elementos definidos en el espacio de nombres`. Ocurrió porque `using` se ubicó después de `COMPILER QuizLang`. En Coco/R, los `using` deben ubicarse antes de la directiva `COMPILER` para colocarse en el preámbulo global del archivo generado. | Tras mover el `using` antes de `COMPILER`, `Parser.cs` compiló sin errores. |
| 2026-10-13 | Antigravity – subagente modelo flash | Compatibilidad con Nullable Reference Types de .NET 10 | Ajuste de configuración de tipos anulables para los archivos generados por Coco/R. | Advertencias C#: `warning CS8618: El elemento campo "val" que no acepta valores NULL debe contener un valor distinto...` debido a que el código base de Coco/R no incluye anotaciones de nulabilidad de C# 8+. Se resolvió configurando `<Nullable>annotations</Nullable>` en `QuizLang.Core.csproj`. | Compilación limpia de `QuizLang.Core` con 0 advertencias y 0 errores. |
| 2026-10-13 | Antigravity – subagente modelo flash | Soporte UTF-8 y traducción de errores léxicos/sintácticos | Adaptación de `Scanner.frame` y `Parser.frame` para admitir UTF-8 sin BOM y traducir mensajes de Coco/R al español. | 1) Acentos en consola se corrompían por la codificación OEM de Windows; se resolvió configurando `Console.OutputEncoding = Encoding.UTF8` en el CLI. 2) La regla `s.EndsWith(" expected")` capturaba `??? expected` y producía `se esperaba ???` en lugar del mensaje de error léxico; se resolvió priorizando la comprobación de `??? expected` para emitir `caracter o simbolo lexico no reconocido`. | Mensajes de error claros, en español y con formato exacto `linea X col Y: mensaje`. |
| 2026-10-13 | Antigravity – subagente modelo flash | Implementación de `SymbolTable.cs` y chequeos semánticos | Validaciones semánticas completas con `errors.SemErr`: preguntas duplicadas, opciones correctas, letras repetidas, letras consecutivas, puntaje mínimo y examen sin preguntas. | Ninguno. La separación entre el AST (`Examen`, `Pregunta`, etc.) y la tabla de símbolos permitió modularizar las validaciones. | Pruebas semánticas unitarias y en CLI funcionando correctamente. |
| 2026-10-13 | Antigravity – subagente modelo flash | Generador de código `CodeGen.cs` | Generación de página HTML autocontenida con estilos modernos, formularios interactivos, corrección y badges. | Se requería asegurar que los textos estuvieran escapados para evitar inyección y que las preguntas de texto no fueran sensibles a mayúsculas/espacios. Se implementó `WebUtility.HtmlEncode` y funciones JS de normalización (`trim().toLowerCase()`). | Apertura e interacción de los HTML generados (`basico.html`, `completo.html`, etc.) en el navegador. |
| 2026-10-13 | Antigravity – subagente modelo flash | Implementación de `QuizLang.Cli` y `QuizLang.Gui` | Interfaz de línea de comandos (`entrada.quiz [-o salida.html]`) e interfaz gráfica WinForms con editor de código, números de línea, visor de errores y vista previa web. | Se requirió conectar el doble clic del DataGridView de errores para que seleccionara y posicionara la línea exacta en el `RichTextBox`. Se implementó mediante `GetFirstCharIndexFromLine` y `Select`. | Ejecución exitosa de CLI y GUI con ejemplos precargados. |
| 2026-10-13 | Antigravity – subagente modelo flash | Creación de suite de pruebas y script `run_tests.ps1` | 4 pruebas válidas y 9 pruebas inválidas con sus correspondientes archivos `.expected`. Script automatizado que compila y valida código de retorno y mensajes de error. | Ninguno. Se verificaron tanto los casos de éxito (exit code 0 y 'OK') como los casos de fallo (exit code 1 y coincidencia de error). | Ejecución de `run_tests.ps1`: `Resultados: 13/13 pruebas pasaron (PASS).` |

---

## 2. Sección de Depuración y Aprendizajes Técnicos

Durante el desarrollo del compilador QuizLang con Coco/R y C#, se identificaron puntos críticos de depuración fundamentales para comprender el funcionamiento de generadores de compiladores:

### Caso 1: Restricción en la definición de TOKENS en Coco/R
* **Síntoma:** Al definir `string = '"' { ANY - '"' } '"'` en la sección `TOKENS`, Coco/R emitió errores `invalid TokenFactor` y `contents of {...} must not be deletable`.
* **Causa técnica:** En la arquitectura de Coco/R, las operaciones de teoría de conjuntos sobre caracteres (unión `+` y diferencia `-`) solo pueden definirse en la sección `CHARACTERS`. La sección `TOKENS` únicamente admite expresiones regulares sobre los conjuntos ya definidos.
* **Solución aplicada:** Se definió en `CHARACTERS` el conjunto `stringChar = ANY - '"' - '\\' - cr - lf` y `printable = ' ' .. '~'`, permitiendo que `TOKENS` utilice `string = '"' { stringChar | '\\' printable } '"'`.

### Caso 2: Correspondencia obligatoria entre COMPILER y la producción raíz
* **Síntoma:** Error de Coco/R `missing production for grammar name`.
* **Causa técnica:** Coco/R exige por convención que la regla sintáctica inicial tenga exactamente el mismo identificador que la cláusula `COMPILER <Nombre>`. Al nombrar la regla como `Quiz` pero el compilador como `QuizLang`, el generador no pudo determinar el símbolo inicial de la gramática.
* **Solución aplicada:** Se renombró la producción inicial a `QuizLang`.

### Caso 3: Ámbito de directivas de preprocesamiento y `using` en frames de Coco/R
* **Síntoma:** El compilador de C# falló con errores `CS1513` y `CS1529` al encontrar una directiva `using` dentro del cuerpo de la clase `Parser`.
* **Causa técnica:** Todo el código colocado entre `COMPILER QuizLang` y la sección `CHARACTERS` es inyectado por Coco/R dentro de la clase `Parser` (en el punto de anclaje `-->declarations`). Para que una directiva `using` se sitúe a nivel de archivo/espacio de nombres, debe ubicarse antes de la palabra reservada `COMPILER`.
* **Solución aplicada:** Se movió `using QuizLang.Core.Modelo;` a la primera línea del archivo `.atg`.

### Caso 4: Ambigüedad en la traducción de mensajes de error de Coco/R
* **Síntoma:** Un token no reconocido (error léxico) reportaba el mensaje sintáctico genérico `se esperaba ???`.
* **Causa técnica:** Coco/R asigna el token `noSym = maxT` a caracteres no reconocidos y genera en `Parser.cs` la cláusula `case 18: s = "??? expected"`. La función de traducción al español comprobaba `s.EndsWith(" expected")` antes de evaluar el caso específico `??? expected`.
* **Solución aplicada:** Se reordenaron las condiciones para que `if (s == "??? expected") return "caracter o simbolo lexico no reconocido";` se evalúe prioritariamente.
