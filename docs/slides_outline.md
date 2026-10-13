# Esquema de Diapositivas para la Defensa Oral del Trabajo Final

**Materia:** Compiladores 2026  
**Proyecto:** Compilador de QuizLang y Estudio de Coco/R  
**Integrantes del Grupo:**
* Albornoz Joaquín
* Díaz Guadalupe
* Gallardo Mateo
* Reinoso Abel  
**Duración Estimada:** 15 a 20 minutos (14 diapositivas)  

---

## Estructura General y Distribución de Expositores

| N° | Título de la Diapositiva | Expositor Asignado | Eje Temático |
| :-: | :--- | :--- | :--- |
| **1** | Portada y Presentación del Proyecto | **Albornoz Joaquín** | Introducción institucional y miembros |
| **2** | Introducción a Coco/R y Filosofía de Diseño | **Díaz Guadalupe** | Marco teórico de la herramienta |
| **3** | Arquitectura Interna de Coco/R y Caso Taste | **Gallardo Mateo** | Pipeline de Coco/R y compilador didáctico |
| **4** | Motivación y Dominio de QuizLang | **Reinoso Abel** | Problema y concepción del DSL |
| **5** | Especificación Léxica y Gramática EBNF | **Albornoz Joaquín** | Definición formal en Cocol/R |
| **6** | Demostración de la Propiedad LL(1) | **Díaz Guadalupe** | Teoría de lenguajes formales y First/Follow |
| **7** | Análisis Semántico y Tabla de Símbolos | **Gallardo Mateo** | Reglas contextuales y AST |
| **8** | Generación de Código Web Interactivo | **Reinoso Abel** | Emisión de HTML5/CSS3/JavaScript |
| **9** | Arquitectura del Software e Interfaz Gráfica | **Albornoz Joaquín** | Estructura modular .NET 10 y WinForms |
| **10** | Verificación Experimental y Suite de Pruebas | **Díaz Guadalupe** | Batería de 13 pruebas y automatización |
| **11** | Metodología con IA: Enfoque Agentic y Bitácora | **Gallardo Mateo** | Organización con Claude Opus y Gemini Flash |
| **12** | Depuración Real y Resolución de Conflictos | **Reinoso Abel** | Ejemplos concretos de depuración en C# y Coco/R |
| **13** | Reflexión Crítica: Alucinaciones y Verificación Humana | **Albornoz Joaquín & Díaz Guadalupe** | Citas alucinadas y límites de la IA |
| **14** | Conclusiones y Demostración en Vivo | **Gallardo Mateo & Reinoso Abel** | Síntesis del proyecto y demo funcional |

---

## Detalle Diapositiva por Diapositiva

### Diapositiva 1: Portada y Presentación del Proyecto
* **Expositor:** **Albornoz Joaquín**
* **Contenido en Viñetas:**
  * **Título Principal:** *QuizLang: Compilador de Exámenes Interactivos*.
  * **Subtítulo:** Generación de Páginas Web Autocontenidas mediante Coco/R y .NET 10.
  * **Cátedra:** Compiladores 2026.
  * **Equipo de Desarrollo:** Albornoz Joaquín, Díaz Guadalupe, Gallardo Mateo, Reinoso Abel.
  * **Objetivo de la Presentación:** Exponer la investigación sobre el generador de compiladores Coco/R, la formalización matemática y semántica de un DSL propio, y el análisis crítico del desarrollo asistido por Inteligencia Artificial.
* **Sugerencia Visual:** Logotipo institucional, captura estética de la aplicación gráfica y foto del equipo.
* **Guía del Discurso:** Saludo inicial, encuadre académico de la defensa y hoja de ruta de la exposición.

---

### Diapositiva 2: Introducción a Coco/R y Filosofía de Diseño
* **Expositor:** **Díaz Guadalupe**
* **Contenido en Viñetas:**
  * **Origen y Evolución:** Creado por Hanspeter Mössenböck (ETH Zürich / JKU Linz) bajo la influencia de Niklaus Wirth; popularizado internacionalmente por Pat Terry.
  * **Filosofía Distintiva:** Frente a generadores ascendentes LALR(1) como Lex/Yacc o Flex/Bison, Coco/R adopta **análisis sintáctico por descenso recursivo** sobre gramáticas atribuidas $LL(1)$.
  * **Ventajas Prácticas:** Código generado altamente legible y depurable en C#, correspondencia directa 1:1 entre no terminales y métodos procedurales, y excelente rendimiento lineal $\mathcal{O}(N)$.
  * **Dialecto Cocol/R:** Especificación unificada en un único archivo `.ATG` que integra alfabeto, tokens, comentarios y producciones con acciones semánticas.
* **Sugerencia Visual:** Comparativa conceptual entre autómata ascendente (tabla LR) vs. métodos descendentes recursivos en C#.
* **Guía del Discurso:** Explicar por qué Mössenböck eligió LL(1) y cómo la versión de C# aprovecha tipos fuertemente tipados y parámetros `out`.

---

### Diapositiva 3: Arquitectura Interna de Coco/R y Caso Taste
* **Expositor:** **Mateo Gallardo**
* **Contenido en Viñetas:**
  * **Pipeline de Generación:** `.ATG` + Frames (`Scanner.frame`, `Parser.frame`) $\rightarrow$ Validación formal (completitud, no circularidad) $\rightarrow$ Cálculo de conjuntos directores $\rightarrow$ DFA + Descenso Recursivo $\rightarrow$ `Scanner.cs` y `Parser.cs`.
  * **El Compilador Pedagógico Taste:** Lenguaje imperativo didáctico con tipos `int` y `bool`, procedimientos y estructuras `if`/`while`.
  * **Compilación en una Sola Pasada (*One-Pass*):** Generación directa de bytecode sin construir AST intermedio.
  * **Técnica de Backpatching:** Resolución de saltos hacia adelante (`FJMP`, `JMP`) y dimensionamiento del marco de pila (`ENTER`) mediante la primitiva `Patch(adr, val)`.
* **Sugerencia Visual:** Diagrama de flujo de datos de Coco/R y esquema de memoria en tiempo de ejecución de la máquina virtual de pila de Taste.
* **Guía del Discurso:** Destacar la elegancia del backpatching en compiladores de una pasada y las lecciones aprendidas de Taste.

---

### Diapositiva 4: Motivación y Dominio de QuizLang
* **Expositor:** **Reinoso Abel**
* **Contenido en Viñetas:**
  * **El Problema:** Creación y mantenimiento de cuestionarios y evaluaciones técnicas sin depender de plataformas cerradas (Moodle, Google Forms) que requieren conectividad constante o servidores dedicados.
  * **La Solución – QuizLang:** Lenguaje de dominio específico (DSL) conciso, legible y declarativo para docentes y evaluadores.
  * **Tipología de Preguntas Soportadas:**
    1. Opción Múltiple (`PREGUNTA_MULTIPLE`) con alternativas delimitadas y única respuesta correcta.
    2. Verdadero / Falso (`PREGUNTA_VF`).
    3. Respuesta Libre (`PREGUNTA_TEXTO`) con autocorrecion insensible a mayúsculas/minúsculas.
  * **Salida Autocontenida:** Generación de un archivo `.html` cliente puro que funciona offline en cualquier navegador moderno.
* **Sugerencia Visual:** Ejemplo de código fuente `.quiz` a la izquierda y el examen HTML interactivo resultante a la derecha.
* **Guía del Discurso:** Justificar la elección temática y cómo un DSL ahorra horas de maquetación manual a un docente.

---

### Diapositiva 5: Especificación Léxica y Gramática EBNF
* **Expositor:** **Albornoz Joaquín**
* **Contenido en Viñetas:**
  * **Definición Léxica:**
    * `CHARACTERS`: `letter`, `digit`, `stringChar` (excluyendo comillas y escapes) y `printable`.
    * `TOKENS`: `letra` (identificador de alternativa), `number` y `string` delimitado con soporte de secuencias de escape (`\"`, `\\`, etc.).
    * `COMMENTS` e `IGNORE`: Comentarios de línea (`//`), comentarios de bloque (`/* ... */`) y descarte de espacios y saltos de línea.
  * **Gramática EBNF Concisa:**
    * Axioma: `QuizLang = "EXAMEN" string [ Config ] { Pregunta } .`
    * `Config = "PUNTAJE_MINIMO" number .`
    * `Pregunta = Multiple | Texto | VerdaderoFalso .`
    * Producciones para alternativas, opciones con `[CORRECTA]` y asignaciones `= VERDADERO | FALSO`.
* **Sugerencia Visual:** Fragmentos exactos del archivo `QuizLang.atg` destacando la separación léxica y las reglas sintácticas.
* **Guía del Discurso:** Enfatizar la claridad y compacidad de la especificación EBNF de Wirth adoptada por Coco/R.

---

### Diapositiva 6: Demostración de la Propiedad LL(1)
* **Expositor:** **Díaz Guadalupe**
* **Contenido en Viñetas:**
  * **Requisito Matemático LL(1):** Los conjuntos de selección ($First$ y $Follow$) de toda alternativa deben ser estrictamente disjuntos con $k=1$ símbolo de anticipación.
  * **Análisis de Producciones Clave:**
    * *Regla `Pregunta`:* $\text{FIRST}(Multiple) \cap \text{FIRST}(Texto) \cap \text{FIRST}(VF) = \emptyset$ (tokens directores: `"PREGUNTA_MULTIPLE"`, `"PREGUNTA_TEXTO"`, `"PREGUNTA_VF"`).
    * *Bloque de Opciones:* $\text{FIRST}(Opcion) = \{\text{letra}\}$ vs. delimitador de cierre $\{\text{"}"}\}$. No existe ambigüedad de parada.
    * *Cláusula `Config`:* Lookahead `"PUNTAJE_MINIMO"` frente a símbolos de inicio de pregunta o fin de archivo.
  * **Verificación de Coco/R:** Ejecución de `Coco.exe` arrojó **0 errores y 0 conflictos LL(1)**.
* **Sugerencia Visual:** Tabla de conjuntos $First$ y $Follow$ de cada símbolo no terminal, demostrando intersecciones vacías.
* **Guía del Discurso:** Explicar cómo la estructuración del lenguaje evitó la necesidad de resolutores sintácticos `IF(...)`.

---

### Diapositiva 7: Análisis Semántico y Tabla de Símbolos
* **Expositor:** **Mateo Gallardo**
* **Contenido en Viñetas:**
  * **Rol de `SymbolTable.cs`:** Encapsula las reglas contextuales y de negocio del examen desacopladas del parser sintáctico.
  * **Modelo de Objetos (AST):** Clases fuertemente tipadas: `Examen`, `Pregunta` (abstracta), `PreguntaMultiple`, `PreguntaTexto`, `PreguntaVF`, `Opcion`.
  * **Chequeos Semánticos Implementados:**
    1. *Enunciados Duplicados:* Detección mediante `HashSet<string>` insensible a mayúsculas.
    2. *Exactitud de Opciones:* Valida que haya **exactamente una** opción marcada con `[CORRECTA]` (reporta error si hay 0 o más de 1).
    3. *Letras Repetidas y Secuencia:* Error ante letras duplicadas ('a', 'a'); advertencia (`Warning`) si no son consecutivas ('a', 'c').
    4. *Coherencia de Puntajes:* Error si `PUNTAJE_MINIMO` excede el puntaje total acumulado.
    5. *Examen No Vacío:* Error si el archivo no contiene preguntas.
* **Sugerencia Visual:** Diagrama de clases en Mermaid que ilustre la jerarquía del AST y la relación con `SymbolTable`.
* **Guía del Discurso:** Explicar cómo los errores semánticos preservan la línea y columna exactas mediante llamadas a `_errors.SemErr`.

---

### Diapositiva 8: Generación de Código Web Interactivo
* **Expositor:** **Reinoso Abel**
* **Contenido en Viñetas:**
  * **Módulo `CodeGen.cs`:** Transforma el AST validado en una aplicación web interactiva autocontenida.
  * **Características del HTML Generado:**
    * *Cero Dependencias:* No requiere conexión a internet, frameworks pesados (React, Angular) ni CDNs externas.
    * *Diseño Responsivo:* Tarjetas visuales modernas (`pregunta-card`), tipografía limpia y badges de puntaje.
    * *Seguridad:* Sanitización contra inyección de código mediante `WebUtility.HtmlEncode`.
  * **Motor JavaScript del Lado del Cliente:**
    * Evaluación reactiva instantánea al presionar el botón "Corregir".
    * Resaltado visual en verde (aciertos) y rojo (errores con muestra de respuesta correcta).
    * Normalización inteligente para preguntas de texto (`trim().toLowerCase()`).
    * Panel de veredicto final: aprobación según `PUNTAJE_MINIMO` o umbral del 60%.
* **Sugerencia Visual:** Captura del examen web abierto en el navegador mostrando una respuesta correcta y otra incorrecta corregidas en vivo.
* **Guía del Discurso:** Detallar la lógica de corrección en el cliente y la portabilidad del artefacto generado.

---

### Diapositiva 9: Arquitectura del Software e Interfaz Gráfica
* **Expositor:** **Albornoz Joaquín**
* **Contenido en Viñetas:**
  * **Modularización en .NET 10:**
    * `QuizLang.Core`: Biblioteca central con Parser, Scanner, AST, Tabla de Símbolos y Fachada `Compiler.cs`.
    * `QuizLang.Cli`: Herramienta de consola para compilación por lotes (`entrada.quiz -o salida.html`).
    * `QuizLang.Gui`: Entorno de desarrollo interactivo en Windows Forms.
  * **Características de `QuizLang.Gui`:**
    * Editor de texto monoespaciado con números de línea dibujados en tiempo real vía GDI+.
    * Selector de plantillas de examen precargadas (básicas, avanzadas y con errores deliberados).
    * Grilla inferior de diagnósticos coloreada (rojo para errores, amarillo para advertencias).
    * **Navegación Interactiva:** Doble clic en el error salta automáticamente a la línea y columna exacta en el editor.
    * Visor web integrado para previsualizar y responder el examen sin salir de la aplicación.
* **Sugerencia Visual:** Diagrama arquitectónico de capas y captura de pantalla completa de la GUI WinForms en funcionamiento.
* **Guía del Discurso:** Describir la separación de responsabilidades y la experiencia de usuario orientada a la productividad.

---

### Diapositiva 10: Verificación Experimental y Suite de Pruebas
* **Expositor:** **Díaz Guadalupe**
* **Contenido en Viñetas:**
  * **Filosofía de Verificación:** Ningún compilador es confiable sin una suite de pruebas automatizada de regresión.
  * **Banco de Pruebas de 13 Casos:**
    * *4 Casos Válidos:* `basico`, `completo`, `compiladores2026`, `comentarios` (Exit Code 0, salida HTML válida).
    * *9 Casos Inválidos:* Errores léxicos, sintácticos (falta de llave, falta de título) y semánticos (duplicados, sin correcta, múltiples correctas, letras repetidas, puntaje excesivo, examen vacío) (Exit Code 1).
  * **Automatización con PowerShell (`run_tests.ps1`):**
    * Compilación previa de la solución con `dotnet build`.
    * Ejecución sistemática comparando el código de retorno y los mensajes exactos contra los archivos `.expected`.
  * **Resultado Verificado:** **13/13 pruebas exitosas (PASS)**.
* **Sugerencia Visual:** Tabla resumen de los 13 casos y captura de la ejecución en consola mostrando el reporte en verde.
* **Guía del Discurso:** Resaltar la cobertura de casos límite y la importancia de validar mensajes de error exactos con línea y columna.

---

### Diapositiva 11: Metodología con IA: Enfoque Agentic y Bitácora
* **Expositor:** **Mateo Gallardo**
* **Contenido en Viñetas:**
  * **Entorno Agentic Antigravity:**
    * *Supervisor (Claude Opus):* Planificación estratégica, auditoría de código, detección de alucinaciones y verificación de compilaciones.
    * *Subagentes (Gemini Flash):* Prototipado rápido, formulación de borradores de código y redacción preliminar.
  * **Decisiones Clave del Grupo:**
    * Selección de QuizLang frente a otras 4 ideas generadas en el brainstorming inicial.
    * Elección de C# (.NET 10) frente a C++ por madurez de tooling, GUI y estabilidad de Coco/R.
  * **Transparencia y Bitácora Obligatoria:** Registro cronológico de prompts, respuestas, adaptaciones y validaciones empíricas.
* **Sugerencia Visual:** Esquema del flujo de trabajo agentic (Supervisor $\leftrightarrow$ Subagentes $\leftrightarrow$ Equipo Humano).
* **Guía del Discurso:** Explicar cómo se gestionó el uso de IA de forma profesional, estructurada y transparente.

---

### Diapositiva 12: Depuración Real y Resolución de Desafíos Técnicos
* **Expositor:** **Reinoso Abel**
* **Contenido en Viñetas:**
  * **Casos Críticos de Depuración:**
    1. *Restricción Léxica en Coco/R:* Error `invalid TokenFactor` al intentar restar conjuntos en `TOKENS`; resuelto declarando `stringChar` en `CHARACTERS`.
    2. *Regla Raíz en Cocol/R:* Error `missing production for grammar name` por discrepancia entre `COMPILER QuizLang` y la regla `Quiz`.
    3. *Ubicación de `using` en `.ATG`:* Error de compilación en C# por inyectar directivas dentro de la clase `Parser`; resuelto ubicándolas antes de `COMPILER`.
    4. *Traducción de Errores:* Confusión entre `??? expected` y mensajes sintácticos; priorización del error léxico en español.
  * **Lección Principal:** Los generadores de compiladores imponen reglas sutiles de metaprogramación que la IA a menudo pasa por alto si no se cuenta con conocimiento de base.
* **Sugerencia Visual:** Fragmentos de código en formato "Antes vs. Después" con los mensajes de error de compilación.
* **Guía del Discurso:** Mostrar solvencia técnica explicando los fallos reales y cómo se diagnosticaron mediante la lectura de plantillas y código generado.

---

### Diapositiva 13: Reflexión Crítica: Alucinaciones y Verificación Humana
* **Expositor:** **Albornoz Joaquín & Díaz Guadalupe**
* **Contenido en Viñetas:**
  * **Detección de una Alucinación Bibliográfica:**
    * La IA citó el libro: *Mössenböck, H. (2018). Compilerbau: Grundlagen... Hanser Verlag*.
    * La auditoría del supervisor reveló que la obra no existe en el catálogo de Hanser ni en la biografía del autor.
    * **Postura ética del grupo:** Se mantuvo marcada como `[VERIFICAR]` en el informe en lugar de ocultarla, evidenciando el riesgo de citas sintéticas.
  * **La Falsa Certeza ("Verificado al 100%"):**
    * Los subagentes tendían a declarar éxito absoluto ignorando advertencias de compilación y casos no contemplados.
  * **El Rol Insustituible del Criterio Humano:** La IA acelera la escritura, pero la validación matemática, la ejecución experimental y la comprensión conceptual residen exclusivamente en los estudiantes.
* **Sugerencia Visual:** Imagen que contraste la cita alucinada con la advertencia de verificación y un decálogo de buenas prácticas éticas.
* **Guía del Discurso:** Reflexionar honestamente sobre el uso responsable de modelos generativos en el ámbito universitario.

---

### Diapositiva 14: Conclusiones y Demostración en Vivo
* **Expositor:** **Mateo Gallardo & Reinoso Abel**
* **Contenido en Viñetas:**
  * **Logros Alcanzados:**
    * Compilador robusto, determinista y completamente funcional bajo gramática LL(1).
    * Integración exitosa de análisis léxico, sintáctico, semántico y backend web interactivo.
    * Entorno gráfico ergonómico y suite de pruebas con 100% de efectividad (13/13 PASS).
  * **Paso a la Demostración en Vivo:**
    1. Carga y edición de un examen en `QuizLang.Gui`.
    2. Demostración de detección de errores sintácticos y semánticos con salto interactivo.
    3. Compilación exitosa, apertura en navegador y resolución interactiva.
* **Sugerencia Visual:** Ventana de la aplicación lista para la demostración interactiva en pantalla gigante.
* **Guía del Discurso:** Conclusión final, agradecimiento al cuerpo docente y transición a la demo en tiempo real.
