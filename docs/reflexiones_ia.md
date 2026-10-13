# Reflexiones sobre el Uso de la Inteligencia Artificial Generativa

**Materia:** Compiladores 2026  
**Autores:** Albornoz Joaquín, Díaz Guadalupe, Gallardo Mateo, Reinoso Abel  
**Fecha:** Octubre de 2026  

---

## 1. Motivos de la Incorporación de la IA y Objetivos Perseguidos

La incorporación de herramientas de Inteligencia Artificial generativa en el desarrollo de este Trabajo Final obedeció a una decisión metodológica planificada, orientada a experimentar con flujos de trabajo contemporáneos de ingeniería de software asistida (*pair programming* y arquitecturas *agentic*). 

Nuestros objetivos centrales fueron:
1. **Acelerar la curva de aprendizaje inicial:** Comprender en profundidad el ecosistema del generador de compiladores Coco/R, su dialecto Cocol/R y los detalles de implementación del compilador pedagógico Taste en C#, cuya documentación original está dispersa en artículos y manuales en inglés y alemán.
2. **Mitigar tareas mecánicas y código repetitivo (*boilerplate*):** Automatizar la escritura de clases de soporte del AST, mapeos de colecciones, generación de código para la interfaz web (HTML5/CSS3/JavaScript) y la configuración de soluciones bajo .NET 10.
3. **Explorar alternativas de diseño y gramática:** Evaluar diferentes formalizaciones gramaticales para asegurar el cumplimiento estricto de la condición $LL(1)$ sin ambigüedades ni conflictos directores.
4. **Construir una batería rigurosa de pruebas y automatización:** Diseñar casos de prueba que desafiaran exhaustivamente las etapas léxica, sintáctica y semántica del compilador.
5. **Aprender a auditar y verificar críticamente la IA:** Desarrollar en el grupo un criterio técnico independiente capaz de detectar errores sutiles, suposiciones infundadas o alucinaciones generadas por los modelos lingüísticos.

---

## 2. Herramientas Empleadas y Entorno de Trabajo

El proyecto no se basó en consultas aisladas mediante interfaces web genéricas, sino en un entorno integrado y estructurado mediante la plataforma agentic **Antigravity**:

* **Agente Supervisor (Claude Opus):** Actuó como auditor y planificador arquitectónico de alto nivel. Supervisó la descomposición del proyecto en tareas, auditó las decisiones estructurales del lenguaje, contrastó las implementaciones con la bibliografía canónica y ejecutó revisiones críticas de código y fuentes bibliográficas.
* **Subagentes Operativos (Gemini Flash):** Se utilizaron como agentes de ejecución rápida para la investigación preliminar, la formulación de borradores de gramática Cocol/R, el prototipado de clases del AST, la construcción de la interfaz gráfica en Windows Forms y la redacción de plantillas de prueba.
* **Subagente de Redacción Técnica:** Modelo especializado en documentación técnica académica en español, encargado de consolidar los informes finales, verificar la coherencia estilística en primera persona del plural y asegurar la correspondencia exacta entre la teoría y los archivos reales del repositorio.
* **Entorno de Compilación y Shell:** PowerShell bajo Windows 11, integrando el SDK de .NET 10 (`dotnet build`, `dotnet run`) y la versión de Coco/R para C# (distribución oficial del SSW de JKU Linz).

---

## 3. Tareas Asistidas por Fase del Ciclo de Vida

El empleo de IA cubrió de manera sistemática las distintas fases del proyecto:

1. **Fase 0 – Brainstorming y Selección Temática (2026-10-05):**
   * *Asistencia de IA:* Generación de 5 propuestas de DSL con distintos grados de complejidad gramatical (dibujo vectorial tipo Logo, aventuras conversacionales, manipulador de matrices, QuizLang para exámenes interactivos y seudocódigo algorítmico).
   * *Decisión humana:* Evaluación en equipo de los pros y contras; selección unánime de QuizLang por su utilidad académica directa y claridad formal.
2. **Fase 1 – Investigación Teórica y Marco de Referencia (2026-10-13):**
   * *Asistencia de IA:* Síntesis de la historia y arquitectura de Coco/R, análisis del compilador pedagógico Taste de Mössenböck y elaboración de un banco de 15 preguntas de examen oral para la defensa.
   * *Intervención humana:* Cotejo contra el manual de usuario de Coco/R y el código fuente de Taste; detección de una referencia bibliográfica no verificada.
3. **Fase 2 – Diseño Formal y Gramática:**
   * *Asistencia de IA:* Formulación de producciones EBNF y cálculo predictivo de los conjuntos $First$ y $Follow$.
   * *Intervención humana:* Reformulación de reglas léxicas para respetar las restricciones de conjuntos de Coco/R y validación de ausencia de recursión por izquierda.
4. **Fase 3 – Implementación del Compilador:**
   * *Asistencia de IA:* Escritura de `SymbolTable.cs`, backend de generación web en `CodeGen.cs`, fachada `Compiler.cs`, cliente de línea de comandos `QuizLang.Cli` e interfaz Windows Forms `QuizLang.Gui`.
   * *Intervención humana:* Integración de delegados para posicionamiento interactivo de errores en el editor, diseño visual y compatibilidad con tipos anulables de C#.
5. **Fase 4 – Verificación, Depuración y Pruebas:**
   * *Asistencia de IA:* Creación de casos de prueba `.quiz` y script de automatización `run_tests.ps1`.
   * *Intervención humana:* Ejecución real en consola, ajuste de aserciones en los archivos `.expected` y validación de códigos de salida.
6. **Fase 5 – Documentación y Preparación de la Defensa:**
   * *Asistencia de IA:* Redacción unificada del informe final, diapositivas para la defensa y guion técnico para el video explicativo.
   * *Intervención humana:* Revisión integral, incorporación de fotos y personalización de las reflexiones éticas y académicas.

---

## 4. Asistencia en la Construcción de Artefactos del Compilador

La IA colaboró activamente en la construcción de cada etapa del pipeline del compilador:

* **Analizador Léxico (`Scanner.cs` vía `QuizLang.atg`):**
  La IA colaboró en la partición formal entre `CHARACTERS` y `TOKENS`. Por ejemplo, modeló las expresiones regulares para cadenas con caracteres de escape (`\"`, `\\`, etc.) y distinguió adecuadamente los identificadores de opción de una sola letra de los literales reservados.
* **Analizador Sintáctico (`Parser.cs` vía `QuizLang.atg`):**
  Ayudó a estructurar las producciones en EBNF garantizando la compatibilidad con el motor de descenso recursivo de Coco/R. Asistió en la formulación de atributos semánticos sintetizados mediante parámetros `out` de C# (por ejemplo, `Opcion<out op>`), permitiendo recolectar los nodos del árbol durante el parseo descendente.
* **Tabla de Símbolos y Chequeo Semántico (`SymbolTable.cs`):**
  Generó la estructura de validación en memoria, encapsulando colecciones eficientes (`HashSet<string>`) para detectar preguntas duplicadas sin distinción de mayúsculas/minúsculas, validar que exactamente una opción esté marcada como `[CORRECTA]` y contrastar el puntaje acumulado frente al `PUNTAJE_MINIMO`.
* **Generador de Código (`CodeGen.cs`):**
  Construyó la plantilla de salida HTML5 con CSS moderno responsivo y lógica JavaScript pura. Se incorporó sanitización de cadenas con `WebUtility.HtmlEncode` para impedir fallas de seguridad por inyección.
* **Herramientas y Automatización (`tools\generar.ps1`, `run_tests.ps1`):**
  Generó los scripts PowerShell que invocan a `Coco.exe` con los parámetros correspondientes (`-namespace`, `-frames`, `-o`) y el script de prueba automatizado que ejecuta el compilador sobre los 13 casos de prueba y compara la salida contra los archivos `.expected`.

---

## 5. Depuración y Resolución de Errores Técnicos Reales

Durante la fase de construcción nos enfrentamos a múltiples errores concretos donde la interacción con la IA y la comprensión del equipo fueron puestas a prueba. A continuación se detallan los 4 casos más representativos documentados en la bitácora:

### Caso 1: Restricción de operaciones de conjuntos en TOKENS de Coco/R
* **Error emitido por Coco/R:**
  ```text
  -- line 10 col 18: invalid TokenFactor / contents of {...} must not be deletable
  ```
* **Fragmento de código conflictivo propuesto inicialmente:**
  ```cocol
  TOKENS
    string = '"' { ANY - '"' } '"' .
  ```
* **Causa técnica:** En Coco/R, la operación de diferencia de conjuntos (`ANY - ...`) solo es válida dentro de la sección `CHARACTERS`. La sección `TOKENS` únicamente admite expresiones regulares sobre clases de caracteres predefinidas.
* **Solución aplicada:**
  Se reestructuró la gramática declarando en `CHARACTERS`:
  ```cocol
  CHARACTERS
    stringChar = ANY - '"' - '\\' - cr - lf .
    printable  = ' ' .. '~' .
  TOKENS
    string = '"' { stringChar | '\\' printable } '"' .
  ```

### Caso 2: Discrepancia entre la directiva COMPILER y el axioma inicial
* **Error emitido por Coco/R:**
  ```text
  -- line 119 col 5: missing production for grammar name
  ```
* **Causa técnica:** En Cocol/R, el nombre del compilador declarado en la cabecera (`COMPILER QuizLang`) debe coincidir unívocamente con el nombre de la regla sintáctica inicial. En el borrador inicial, la regla raíz se había nombrado `Quiz`.
* **Solución aplicada:** Se renombró la producción inicial a `QuizLang = ...`.

### Caso 3: Ubicación de directivas `using` en las plantillas de C#
* **Error emitido por el compilador de C# (Roslyn):**
  ```text
  Parser.cs(24,27): error CS1513: Se esperaba }
  Parser.cs(26,1): error CS1529: Una cláusula using debe preceder al resto de elementos definidos en el espacio de nombres
  ```
* **Causa técnica:** Las plantillas de Coco/R (`Parser.frame`) inyectan todo el código situado entre `COMPILER Nombre` y `CHARACTERS` dentro de la definición de la clase `Parser`. Al colocar `using QuizLang.Core.Modelo;` después de `COMPILER`, la directiva quedaba atrapada en el cuerpo de la clase en C#.
* **Solución aplicada:** Se movió la directiva a la primerísima línea de `QuizLang.atg`, **antes** de `COMPILER QuizLang`, de modo que Coco/R la ubique en el preámbulo del archivo generado.

### Caso 4: Ambigüedad en la traducción de errores sintácticos vs. léxicos
* **Comportamiento defectuoso:** Un carácter no reconocido en el código fuente (error léxico) emitía en consola el mensaje `linea X col Y: se esperaba ???` en lugar de una descripción comprensible.
* **Causa técnica:** Coco/R asocia a los caracteres ilegales el token especial `noSym = maxT`, y la plantilla generaba la cadena `"??? expected"`. La rutina de traducción al español aplicaba la condición genérica `s.EndsWith(" expected")` que la convertía ingenuamente a `"se esperaba ???"`.
* **Solución aplicada:** Se modificó la rutina de traducción en `Parser.frame` para dar prioridad estricta al caso léxico:
  ```csharp
  if (s == "??? expected") return "caracter o simbolo lexico no reconocido";
  if (s.EndsWith(" expected")) return "se esperaba " + s.Substring(0, s.Length - 9);
  ```

---

## 6. Usabilidad y Diseño de la Interfaz Gráfica (GUI) y Salida Web

La IA brindó un soporte significativo en la concepción y programación de la experiencia de usuario:

### Entorno Gráfico (`QuizLang.Gui`)
* **Editor de Código Ergonómico:** Se programó un control `RichTextBox` con fuente monoespaciada (`Consolas`) y un panel lateral sincronizado (`picLineNumbers`) que calcula y dibuja en tiempo real los números de línea basándose en las métricas de renderizado gráfico de GDI+.
* **Navegación Interactiva de Diagnósticos:** Se diseñó una grilla (`DataGridView`) que categoriza errores y advertencias con resaltado en colores pastel (rojo suave y amarillo claro). Al hacer doble clic sobre cualquier fila, el evento invoca `txtEditor.GetFirstCharIndexFromLine(linea - 1)` y selecciona la línea correspondiente, emulando la usabilidad de entornos profesionales como Visual Studio o VS Code.
* **Selector de Plantillas Integrado:** Un menú desplegable permite cargar instantáneamente exámenes válidos de complejidad creciente o ejemplos diseñados deliberadamente con fallas sintácticas y semánticas para demostración didáctica.

### Aplicación Web Generada (`CodeGen.cs`)
* **Diseño Moderno y Responsivo:** La página generada adopta un estilo visual contemporáneo basado en tarjetas elevadas con sombras suaves (`pregunta-card`), tipografía del sistema y paleta de colores accesible.
* **Evaluación Inmediata del Lado del Cliente:** El motor JavaScript incorporado evalúa las respuestas instantáneamente sin requerir recarga de página. Proporciona retroalimentación visual directa: bordes verdes y cartel de felicitaciones ante aciertos; bordes rojos, mensaje de error y visualización de la respuesta correcta esperada ante fallos.
* **Cálculo de Aprobación Global:** Computa el puntaje acumulado y lo compara con el `PUNTAJE_MINIMO` explícito (o el 60% por defecto), desplegando un panel destacado de "APROBADO" o "DESAPROBADO".

---

## 7. Ejemplos de Prompts Reales y Respuestas Relevantes

A continuación se transcriben dos ejemplos representativos de interacciones reales mantenidas durante el proyecto:

### Prompt 1: Modelado de validación semántica en C#
> **Prompt:**  
> *"Necesitamos implementar `SymbolTable.cs` para QuizLang en C#. Debe conectarse con `Errors` de Coco/R y realizar las siguientes validaciones: que no se repitan enunciados de preguntas (insensible a mayúsculas), que cada pregunta de opción múltiple tenga exactamente una opción marcada como [CORRECTA], que las letras de opciones no se repitan (y emitir una advertencia si no son consecutivas 'a', 'b', 'c'), que el PUNTAJE_MINIMO no sea mayor a la suma total de puntos y que el examen no esté vacío. ¿Cómo estructuramos esta clase de forma limpia y desacoplada del parser?"*

> **Respuesta Relevante de la IA (Subagente):**  
> *"Podemos diseñar `SymbolTable` manteniendo internamente el objeto `Examen` (que actúa como raíz del AST) y una referencia a `Errors`. Las validaciones se ejecutan en dos momentos: de manera inmediata al terminar de parsear cada pregunta (`ValidarYAgregarPreguntaMultiple`), y de forma global al cerrar la producción del examen (`ValidarExamen`). Para las preguntas duplicadas usamos un `HashSet<string>(StringComparer.OrdinalIgnoreCase)`. Para las letras de opción usamos un `HashSet<char>` y una variable `char? siguienteEsperada` que nos permite llamar a `_errors.Warning` si `letra != siguienteEsperada` sin frenar la compilación..."*  
*(El código resultante se integró con adaptaciones menores en `QuizLang.Core\SymbolTable.cs`).*

### Prompt 2: Diagnóstico del error de espacios de nombres en Cocol/R
> **Prompt:**  
> *"Al colocar `using QuizLang.Core.Modelo;` dentro del archivo `QuizLang.atg`, el compilador de C# genera: `Parser.cs(26,1): error CS1529: Una cláusula using debe preceder al resto de elementos definidos en el espacio de nombres`. ¿Dónde debe ubicarse exactamente la cláusula `using` en un archivo ATG de Coco/R?"*

> **Respuesta Relevante de la IA (Subagente):**  
> *"En Coco/R para C#, la sección comprendida entre `COMPILER Nombre` y `CHARACTERS` se copia literalmente en la sección interna de la clase `Parser` (definida en `Parser.frame` en el marcador `-->declarations`). Por lo tanto, si colocas el `using` ahí, estás intentando declarar una directiva using adentro de una clase en C#, lo cual es ilegal según la especificación de C#. Debes colocar `using QuizLang.Core.Modelo;` en la primerísima línea de tu archivo `.atg`, ANTES de la directiva `COMPILER QuizLang`. De ese modo, Coco/R lo inyecta en el marcador `-->imports` o al inicio del archivo generado."*

---

## 8. Contribución de la IA a la Documentación Técnica

La IA operó como un eficiente estructurador y catalizador de la documentación del proyecto:
1. **Elaboración de Diagramas Mermaid:** Tradujo la arquitectura de capas y el pipeline de Coco/R en diagramas vectoriales legibles que ilustran con claridad el flujo de datos desde el archivo `.quiz` hasta el HTML final.
2. **Tabulación Exhaustiva del Banco de Pruebas:** Compiló la tabla comparativa de los 13 casos de prueba, vinculando archivos fuente, códigos de retorno y mensajes de error esperados.
3. **Estructuración Didáctica de Materiales de Defensa:** Organizó el compendio de preguntas técnicas para la defensa del compilador Taste, permitiendo al equipo preparar argumentos sólidos sobre conceptos complejos como registros de activación, variables globales vs. locales en la máquina virtual y backpatching de instrucciones.
4. **Mantenimiento de la Coherencia Textual:** Aseguró que la terminología técnica (lexemas, tokens, autómatas deterministas, atributos sintetizados, gramáticas L-atribuidas) se utilizara de manera consistente y rigurosa en todos los entregables.

---

## 9. Reflexión Crítica, Verificación y Desafíos

El aspecto más formativo del uso de Inteligencia Artificial en este proyecto no radicó en sus aciertos, sino en el análisis de sus fallas, limitaciones y sesgos cognitivos artificiales. Identificamos tres dimensiones críticas:

### 1. La Referencia Bibliográfica Probablemente Alucinada
Durante la redacción del marco teórico sobre Coco/R, el subagente de IA incluyó con absoluta naturalidad la siguiente cita bibliográfica:
> *Mössenböck, Hanspeter. (2018). Compilerbau: Grundlagen, Methoden, Werkzeuge und Anwendungen. Hanser Verlag.*

Al auditar la bibliografía, nuestro agente supervisor (Claude Opus) alertó que dicha referencia no figuraba en el catálogo oficial de Hanser Verlag ni en la lista de publicaciones de la cátedra del Prof. Mössenböck en la Universidad de Linz. La IA había generado una cita verosímil pero inexistente en la realidad (una alucinación clásica).  
En lugar de ocultar el hecho o eliminarla silenciosamente, **el equipo decidió mantenerla señalizada explícitamente con la marca `[VERIFICAR]` en todos los documentos**. Esta decisión constituye una lección académica fundamental: **ninguna afirmación bibliográfica o técnica emitida por una IA puede darse por válida sin contraste directo contra fuentes primarias verificadas.**

### 2. La Falsa Sensación de Completitud ("Verificado al 100%")
En múltiples etapas, los subagentes afirmaron taxativamente que las tareas estaban *"concluidas con éxito y verificadas al 100%"*. Sin embargo, la auditoría humana e independiente reveló detalles omitidos: advertencias de nulabilidad en C# que ensuciaban la compilación, falta de manejo de codificación UTF-8 en la consola de Windows y la ausencia de validación cruzada en la regeneración limpia de los archivos con `Coco.exe`. La IA tiende a presentar conclusiones optimistas y autocomplacientes; confiar ciegamente en sus declaraciones de éxito induce a un riesgo crítico de fallas no detectadas.

### 3. La Necesidad Insustituible del Criterio Humano y la Verificación Experimental
La generación de compiladores es una disciplina regida por la exactitud matemática y la semántica formal: un solo carácter de más en un conjunto de selección o un conflicto no resuelto destruye el determinismo del analizador.  
La IA puede sugerir código y proponer gramáticas, pero carece de comprensión semántica real:
* No puede reemplazar el entendimiento del alumno sobre por qué una gramática no es $LL(1)$ ante la recursión por izquierda.
* No puede sustituir la verificación empírica consistente en ejecutar `run_tests.ps1`, inspeccionar los binarios resultantes y probar la interfaz en Windows.
* No puede defender oralmente el proyecto ante el tribunal docente.

### Conclusión Ética y Pedagógica
La Inteligencia Artificial generativa demostró ser un amplificador de capacidades técnicas y un copiloto extraordinario para agilizar el desarrollo de software. No obstante, su valor reside exclusivamente en la capacidad del equipo humano para guiarla, cuestionarla, auditarla y corregirla. Como futuros ingenieros y profesionales de la informática, reafirmamos que la responsabilidad intelectual, el rigor metodológico y el conocimiento de fondo residen indelegablemente en nosotros.
