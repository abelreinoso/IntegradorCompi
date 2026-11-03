# INFORME DE PRUEBAS DEL COMPILADOR QUIZLANG

## Verificación Integral del Compilador Coco/R en C# (.NET 10)

---

**Cátedra:** Compiladores  
**Ciclo Lectivo:** 2026  
**Carrera:** Ingeniería en Sistemas de Información / Ciencias de la Computación  
**Equipo de Trabajo:**
* **Albornoz, Joaquín**
* **Díaz, Guadalupe**
* **Gallardo, Mateo**
* **Reinoso, Abel**

---

## 1. Introducción y Estrategia de Pruebas

El aseguramiento de la calidad y la verificación de la corrección sintáctica y semántica constituyen etapas críticas en la construcción de un compilador. Para el lenguaje de dominio específico **QuizLang**, implementado mediante el generador de compiladores **Coco/R** sobre la plataforma **.NET 10**, diseñamos una suite de pruebas automatizadas concebida bajo una metodología rigurosa de **caja negra** (*black-box testing*).

### 1.1 Objetivo del Juego de Pruebas
El objetivo primordial de la batería de pruebas consiste en someter al compilador a un conjunto representativo de entradas estructuradas para validar sistemáticamente:
1. El reconocimiento léxico de tokens válidos, comentarios y secuencias de escape, junto con el rechazo inmediato de caracteres ilegales.
2. El análisis sintáctico determinista por descenso recursivo $LL(1)$, verificando tanto la aceptación de estructuras gramaticales válidas como la detección exacta y recuperación ante omisiones de tokens obligatorios.
3. El análisis contextual y semántico administrado por la tabla de símbolos (`SymbolTable`), certificando que las restricciones de unicidad, consistencia lógica y coherencia numérica sean aplicadas estrictamente.
4. La generación de código final (`CodeGen`), garantizando que ante fuentes válidos se sintetice un artefacto web interactivo (`HTML5 + CSS3 + JavaScript`) autocontenido, completamente funcional y fiel al modelo del examen.

### 1.2 Estrategia de Cobertura
Nuestra estrategia contempla un equilibrio metódico entre:
* **Casos Positivos (Pruebas Válidas):** Diseñados para comprobar el correcto flujo transversal del compilador desde el escaneo inicial hasta la emisión del archivo HTML. Estos casos cubren las tres modalidades de preguntas (`PREGUNTA_MULTIPLE`, `PREGUNTA_VF` y `PREGUNTA_TEXTO`), la fijación de puntajes específicos con la cláusula `PUNTOS`, el establecimiento de umbrales con `PUNTAJE_MINIMO` y el manejo transparente de comentarios unilínea (`//`) y multilínea (`/* ... */`).
* **Casos Negativos (Pruebas de Diagnóstico y Error):** Formulados deliberadamente para disparar condiciones de falla en cada una de las fases internas de la arquitectura:
  * *Fase Léxica:* Detección de caracteres no contemplados en el alfabeto del compilador.
  * *Fase Sintáctica:* Desviaciones respecto a las producciones de la gramática EBNF en `QuizLang.atg`.
  * *Fase Semántica:* Inconsistencias lógicas no capturables por la gramática libre de contexto, tales como preguntas duplicadas, ausencia de opciones correctas, multiplicidad indebida de respuestas marcadas, letras de opción repetidas o configuraciones de puntaje inalcanzables.

---

## 2. Metodología y Entorno de Ejecución

### 2.1 Comando de Ejecución
La ejecución automatizada del banco de pruebas se lleva a cabo desde la raíz del proyecto `TrabajoFinal` invocando el script de PowerShell provisto:

```powershell
powershell -ExecutionPolicy Bypass -File run_tests.ps1
```

Alternativamente, el script puede ser invocado desde el subdirectorio `tests\` mediante su wrapper homónimo:
```powershell
powershell -ExecutionPolicy Bypass -File tests\run_tests.ps1
```

### 2.2 Arquitectura y Funcionamiento Interno de `run_tests.ps1`
A partir del análisis directo del código fuente del script `run_tests.ps1`, el proceso se desglosa en las siguientes etapas secuenciales:

1. **Configuración del Entorno y Codificación UTF-8:**
   ```powershell
   $ErrorActionPreference = "Stop"
   $scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
   $tfDir = $scriptDir
   $slnPath = Join-Path $tfDir "QuizLang.sln"
   $testsDir = Join-Path $tfDir "tests"
   $cliExe = Join-Path $tfDir "QuizLang.Cli\bin\Debug\net10.0\QuizLang.Cli.exe"
   [Console]::OutputEncoding = [System.Text.Encoding]::UTF8
   ```
   Se asegura la detención ante errores imprevistos, se resuelven rutas relativas absolutas y se establece la consola en UTF-8 para garantizar la fidelidad de caracteres en español y signos de interrogación invertidos (`¿`).

2. **Compilación Previa de la Solución:**
   ```powershell
   $buildResult = dotnet build $slnPath --nologo
   if ($LASTEXITCODE -ne 0) {
       Write-Host "Error al compilar la solución QuizLang.sln" -ForegroundColor Red
       exit 1
   }
   ```
   El script garantiza que los binarios ejecutados correspondan siempre a la versión más reciente del código fuente, abortando la ejecución si la solución de C# presenta errores de compilación.

3. **Descubrimiento y Procesamiento de Pruebas:**
   ```powershell
   $quizFiles = Get-ChildItem -Path $testsDir -Filter "*.quiz" | Sort-Object Name
   ```
   Se recopilan todos los archivos con extensión `.quiz` ordenados alfabéticamente. Para cada archivo se localiza su respectivo archivo hermano `.expected`.

4. **Invocación del CLI y Captura de Salida:**
   ```powershell
   $expectedText = (Get-Content -Path $expectedFile -Raw -Encoding UTF8).Trim()
   $output = (& $cliExe $quiz.FullName 2>&1) -join "`n"
   $exitCode = $LASTEXITCODE
   $actualTrimmed = $output.Trim()
   ```
   Se ejecuta el binario de consola `QuizLang.Cli.exe` redirigiendo tanto la salida estándar como el flujo de error (`2>&1`).

### 2.3 Criterio Formal PASS / FAIL
El script discrimina dos modalidades de evaluación:
* **Prueba Válida (`$expectedText -eq "OK"`):**
  Se evalúa si el código de retorno es `0` (`$exitCode -eq 0`). Si el compilador finalizó sin registrar errores léxicos, sintácticos o semánticos, el caso se califica como **PASS**; en caso contrario, se marca **FAIL**.
* **Prueba de Error (`$expectedText -ne "OK"`):**
  Se evalúa una condición doble estricta:
  ```powershell
  if ($exitCode -eq 1 -and $actualTrimmed.Contains($expectedText)) {
      $isPass = $true
  }
  ```
  El compilador debe retornar código de terminación `1` (indicando anomalía) y la salida textual emitida en consola debe contener la subcadena diagnóstica exacta estipulada en el archivo `.expected`.

### 2.4 Formato Estandarizado de Diagnósticos
En cumplimiento con los estándares de herramientas de desarrollo en línea de comandos, todos los diagnósticos de QuizLang respetan el formato unificado:
$$\text{linea } X \text{ col } Y\text{: } \text{mensaje}$$

Este formato se implementa en el núcleo del compilador (`Compiler.cs`) mediante la configuración de la plantilla de Coco/R:
```csharp
parser.errors.errMsgFormat = "linea {0} col {1}: {2}";
```
Y es renderizado por la consola en `QuizLang.Cli\Program.cs`:
```csharp
Console.WriteLine($"linea {error.Line} col {error.Column}: {error.Message}");
```
Donde $X$ representa el número de línea (base 1), $Y$ el número de columna (base 1) y el texto posterior detalla el motivo del rechazo.

### 2.5 Procedimiento para Agregar una Nueva Prueba
Para incorporar un nuevo caso de prueba al banco automatizado, el desarrollador o evaluador debe seguir un procedimiento directo y sin necesidad de modificar el código del script:
1. Crear el archivo fuente con el examen en la carpeta `TrabajoFinal\tests\`, por ejemplo `mi_prueba.quiz`.
2. Crear en la misma carpeta el archivo de verificación homónimo con extensión `.expected`, en este caso `mi_prueba.expected`.
3. Definir el contenido de `mi_prueba.expected`:
   * Si se espera que el archivo compile exitosamente y genere HTML, escribir únicamente `OK`.
   * Si se espera que sea rechazado por el compilador, escribir la cadena exacta de diagnóstico esperada (por ejemplo: `linea 4 col 5: se esperaba "="`).
4. Ejecutar nuevamente `powershell -ExecutionPolicy Bypass -File run_tests.ps1`. El script descubrirá el archivo automáticamente y lo incluirá en el reporte general.

---

## 3. Matriz de Cobertura de la Suite de Pruebas

A continuación se resume la correlación entre cada caso de prueba, las fases del compilador puestas a prueba y las construcciones sintácticas del lenguaje evaluadas:

| N° | Archivo de Prueba | Categoría | Fase Evaluada | `MULTIPLE` | `VF` | `TEXTO` | `PUNTOS` | `PUNTAJE_MINIMO` | Comentarios | `RESPUESTA` |
| :-: | :--- | :--- | :--- | :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| **1** | `basico.quiz` | Válida | Todas + CodeGen | Sí | Sí | No | Sí | Sí | No | No |
| **2** | `comentarios.quiz` | Válida | Léxica (Comentarios) + CodeGen | Sí | Sí | No | Sí | Sí | Sí (`//`, `/* */`) | No |
| **3** | `compiladores2026.quiz` | Válida | Todas + CodeGen | Sí | Sí | Sí | Sí | Sí | No | Sí |
| **4** | `completo.quiz` | Válida | Todas + CodeGen | Sí | Sí | Sí | Sí | Sí | No | Sí |
| **5** | `error_lexico.quiz` | Error | Léxica (Scanner / DFA) | No | Sí | No | No | No | No | No |
| **6** | `error_sintactico_falta_llave.quiz` | Error | Sintáctica (Parser / EBNF) | Sí | No | No | No | No | No | No |
| **7** | `error_sintactico_falta_titulo.quiz` | Error | Sintáctica (Parser / EBNF) | No | Sí | No | No | No | No | No |
| **8** | `sem_enunciado_duplicado.quiz` | Error | Semántica (`SymbolTable`) | No | Sí | No | Sí | Sí | No | No |
| **9** | `sem_examen_sin_preguntas.quiz` | Error | Semántica (`SymbolTable`) | No | No | No | No | Sí | No | No |
| **10** | `sem_multiple_sin_correcta.quiz` | Error | Semántica (`SymbolTable`) | Sí | No | No | Sí | Sí | No | No |
| **11** | `sem_multiple_varias_correctas.quiz` | Error | Semántica (`SymbolTable`) | Sí | No | No | Sí | Sí | No | No |
| **12** | `sem_opcion_letra_repetida.quiz` | Error | Semántica (`SymbolTable`) | Sí | No | No | Sí | Sí | No | No |
| **13** | `sem_puntaje_minimo_excesivo.quiz` | Error | Semántica (`SymbolTable`) | No | Sí | No | Sí | Sí | No | No |

---

## 4. Fichas Técnicas Detalladas de las 13 Pruebas

A continuación se expone la ficha monográfica de cada uno de los 13 casos de prueba, transcribiendo fielmente el código fuente `.quiz`, la salida esperada `.expected` y el análisis formal del mecanismo de detección involucrado.

---

### Ficha 1: `basico.quiz`
* **Objetivo:** Verificar la compilación exitosa de un examen elemental con configuración de puntaje mínimo, combinando preguntas de opción múltiple y de verdadero/falso con puntaje explícito.
* **Fase del Compilador Evaluada:** Análisis Léxico, Análisis Sintáctico, Análisis Semántico y Generación de Código Web (`CodeGen`).
* **Código Fuente Completo (`basico.quiz`):**
  ```quiz
  EXAMEN "Examen Básico"
  PUNTAJE_MINIMO 2

  PREGUNTA_MULTIPLE "¿Cuál es la capital de Francia?" PUNTOS 1 {
    a) "Madrid"
    b) "París" [CORRECTA]
    c) "Roma"
  }

  PREGUNTA_VF "¿El sol es una estrella?" PUNTOS 1 = VERDADERO
  ```
* **Salida Esperada (`basico.expected`):**
  ```text
  OK
  ```
* **Justificación Teórica y Reglas Aplicadas:**
  El código satisface rigurosamente la producción axioma de `QuizLang.atg`:
  ```csharp
  QuizLang =
    (. symbolTable = new SymbolTable(errors); .)
    "EXAMEN" string
    (. symbolTable.SetExamen(Unquote(t.val), t.line, t.col); .)
    [ Config ]
    { Pregunta }
    (. symbolTable.ValidarExamen(); .)
    .
  ```
  La cláusula `PUNTAJE_MINIMO 2` es parseada por `Config`. Las producciones `Multiple` y `VerdaderoFalso` consumen sus respectivos tokens sin ambigüedad. En la tabla de símbolos, las llamadas a `ValidarYAgregarPreguntaMultiple` y `ValidarYAgregarPreguntaVF` constatan que los enunciados no están duplicados, que la pregunta de opción múltiple contiene exactamente una respuesta `[CORRECTA]` (`b) París`) y que las letras de opción (`a`, `b`, `c`) son consecutivas y únicas. Finalmente, `ValidarExamen()` corrobora que la sumatoria total de puntos ($1 + 1 = 2$) es mayor o igual al puntaje mínimo configurado ($2 \le 2$).
* **Generación de Código HTML:**
  Se sintetiza el archivo `basico.html` con un total de 2 preguntas y un total de 2 puntos disponibles. Se incrusta la variable de configuración `puntajeMinimo = 2`. Para aprobar, el alumno debe responder correctamente ambas preguntas obteniendo 2 puntos (100% de efectividad).

---

### Ficha 2: `comentarios.quiz`
* **Objetivo:** Comprobar que el analizador léxico (`Scanner`) reconoce, filtra y descarta de manera transparente los comentarios de una sola línea (`//`) y los comentarios de bloque multilínea (`/* ... */`) en cualquier ubicación del archivo.
* **Fase del Compilador Evaluada:** Análisis Léxico (directivas de comentarios) y Generación de Código.
* **Código Fuente Completo (`comentarios.quiz`):**
  ```quiz
  // Comentario inicial de una sola línea
  EXAMEN "Examen de Verificación de Comentarios"
  /* Comentario de bloque
     multilínea antes de la configuración */
  PUNTAJE_MINIMO 2

  // Pregunta 1
  PREGUNTA_MULTIPLE "¿Se ignoran correctamente los comentarios en línea y en bloque?" PUNTOS 1 {
    a) "Sí" [CORRECTA] // Comentario al final de opción
    b) "No"
  }

  /* Pregunta 2 de tipo VF */
  PREGUNTA_VF "¿El Scanner de Coco/R descarta comentarios configurados con COMMENTS FROM...?" PUNTOS 1 = VERDADERO
  // Fin del examen
  ```
* **Salida Esperada (`comentarios.expected`):**
  ```text
  OK
  ```
* **Justificación Teórica y Reglas Aplicadas:**
  En `QuizLang.atg`, la especificación léxica instruye a Coco/R a tratar estas secuencias como comentarios ignorados:
  ```csharp
  COMMENTS FROM "//" TO "\n"
  COMMENTS FROM "/*" TO "*/"
  IGNORE ' ' + '\t' + '\r' + '\n'
  ```
  Durante el escaneo, el método `Scan()` de `Scanner.cs` intercepta las secuencias iniciales `//` y `/*` mediante transiciones específicas del autómata, consumiendo los caracteres sucesivos hasta encontrar el delimitador de cierre (`\n` o `*/`) sin generar ningún objeto `Token` para el parser. Los comentarios aparecen antes de la cabecera, entre directivas, al final de una opción, precediendo a una pregunta y al final del flujo, demostrando que ningún token espurio interfiere en el análisis sintáctico.
* **Generación de Código HTML:**
  Genera `comentarios.html` con 2 preguntas de 1 punto cada una (total 2 puntos). El puntaje mínimo para aprobar es 2. Ningún texto de los comentarios es volcado al archivo HTML resultante.

---

### Ficha 3: `compiladores2026.quiz`
* **Objetivo:** Validar un examen técnico exhaustivo con 5 preguntas teóricas de la cátedra que cubre simultáneamente los tres tipos de preguntas soportados (`PREGUNTA_MULTIPLE`, `PREGUNTA_VF`, `PREGUNTA_TEXTO`), con enunciados extensos y puntuaciones asignadas explícitamente.
* **Fase del Compilador Evaluada:** Todas las fases del compilador (Léxico, Sintaxis $LL(1)$, Semántica y Generación de Código).
* **Código Fuente Completo (`compiladores2026.quiz`):**
  ```quiz
  EXAMEN "Examen de Compiladores 2026"
  PUNTAJE_MINIMO 8

  PREGUNTA_MULTIPLE "¿Qué tipo de analizador sintáctico genera Coco/R a partir de una especificación ATG?" PUNTOS 2 {
    a) "Descendente recursivo LL(1)" [CORRECTA]
    b) "Ascendente LR(1)"
    c) "Ascendente LALR(1)"
    d) "Tabla SLR(1)"
  }

  PREGUNTA_VF "¿Un analizador LL(1) puede resolver gramáticas que contienen recursión por izquierda directa sin transformarlas?" PUNTOS 2 = FALSO

  PREGUNTA_MULTIPLE "¿Qué condición deben cumplir las opciones alternativas A -> alfa | beta para que la gramática sea LL(1)?" PUNTOS 2 {
    a) "FIRST(alfa) y FIRST(beta) deben ser conjuntos disjuntos" [CORRECTA]
    b) "FOLLOW(A) debe ser igual a FIRST(alfa)"
    c) "La gramática debe ser ambigua"
  }

  PREGUNTA_VF "¿La tabla de símbolos permite almacenar atributos y validar reglas de contexto durante el análisis semántico?" PUNTOS 2 = VERDADERO

  PREGUNTA_TEXTO "Nombre de la fase del compilador que transforma el flujo de caracteres en una secuencia de tokens" PUNTOS 2 RESPUESTA "Scanner"
  ```
* **Salida Esperada (`compiladores2026.expected`):**
  ```text
  OK
  ```
* **Justificación Teórica y Reglas Aplicadas:**
  Este caso pone a prueba la alternancia determinista de la regla `Pregunta`:
  ```csharp
  Pregunta =
    Multiple | Texto | VerdaderoFalso .
  ```
  La producción `Texto` consume la cláusula `RESPUESTA "Scanner"`:
  ```csharp
  Texto =
    "PREGUNTA_TEXTO" string
    [ "PUNTOS" number ]
    [ "RESPUESTA" string (. pt.RespuestaCorrecta = Unquote(t.val); .) ]
    (. symbolTable.ValidarYAgregarPreguntaTexto(pt); .)
    .
  ```
  La tabla de símbolos valida 5 preguntas únicas, cada una con `PUNTOS 2`, acumulando un total de $5 \times 2 = 10$ puntos. El puntaje mínimo establecido es `8`, cumpliendo la condición $8 \le 10$ en `ValidarExamen()`.
* **Generación de Código HTML:**
  Genera `compiladores2026.html`. En el script JavaScript client-side se inyectan las 5 estructuras de validación. Para la pregunta de texto, la función `corregirExamen()` compara la entrada del alumno normalizando espacios y mayúsculas (`trim().toLowerCase() === "scanner"`). El estudiante aprueba si obtiene $\ge 8$ puntos (al menos 4 respuestas correctas sobre 5).

---

### Ficha 4: `completo.quiz`
* **Objetivo:** Verificar la versatilidad del compilador ante un examen de ciencias y computación con 4 preguntas, variedad de puntajes diferenciados (preguntas de 1 y 2 puntos) y múltiples modalidades.
* **Fase del Compilador Evaluada:** Todas las fases del compilador y Generación de Código Web.
* **Código Fuente Completo (`completo.quiz`):**
  ```quiz
  EXAMEN "Examen Completo de Ciencias y Computación"
  PUNTAJE_MINIMO 5

  PREGUNTA_MULTIPLE "¿Cuál de las siguientes estructuras es de tipo FIFO?" PUNTOS 2 {
    a) "Pila"
    b) "Cola" [CORRECTA]
    c) "Árbol"
    d) "Grafo"
  }

  PREGUNTA_VF "¿HTTP es un protocolo de capa de transporte?" PUNTOS 1 = FALSO

  PREGUNTA_TEXTO "Nombre de la unidad central de procesamiento (siglas)" PUNTOS 2 RESPUESTA "CPU"

  PREGUNTA_MULTIPLE "¿Cuál es el valor binario del decimal 5?" PUNTOS 1 {
    a) "101" [CORRECTA]
    b) "110"
    c) "011"
  }
  ```
* **Salida Esperada (`completo.expected`):**
  ```text
  OK
  ```
* **Justificación Teórica y Reglas Aplicadas:**
  El compilador reconoce exitosamente dos preguntas de selección múltiple (una con 4 alternativas y 2 puntos, otra con 3 alternativas y 1 punto), una pregunta de verdadero/falso (1 punto) y una de respuesta textual abierta (2 puntos). El puntaje total acumulado es $2 + 1 + 2 + 1 = 6$ puntos. Como `PUNTAJE_MINIMO 5` verifica $5 \le 6$, el análisis semántico es enteramente satisfactorio.
* **Generación de Código HTML:**
  Genera `completo.html`. La interfaz web presenta las cuatro tarjetas renderizadas con sus respectivos badges de ponderación (`2 pts` y `1 pt`). Para aprobar se requiere un mínimo de 5 puntos, lo que exige responder correctamente al menos ambas preguntas de 2 puntos y una de 1 punto, o acertar las 4 preguntas completas.

---

### Ficha 5: `error_lexico.quiz`
* **Objetivo:** Verificar que el analizador léxico (`Scanner`) detecte caracteres inválidos que no pertenecen al alfabeto del lenguaje ni forman parte de ningún token regular, abortando la compilación con la posición precisa del lexema anómalo.
* **Fase del Compilador Evaluada:** Análisis Léxico (Scanner / DFA).
* **Código Fuente Completo (`error_lexico.quiz`):**
  ```quiz
  EXAMEN "Examen con Error Léxico"
  PREGUNTA_VF "¿Es válido?" = VERDADERO
  @
  ```
* **Salida Esperada (`error_lexico.expected`):**
  ```text
  linea 3 col 1: caracter o simbolo lexico no reconocido
  ```
* **Explicación Formal del Rechazo y Localización:**
  * **Regla violada:** En `QuizLang.atg`, la sección `TOKENS` declara:
    ```csharp
    TOKENS
      letra  = letter .
      number = digit { digit } .
      string = '"' { stringChar | '\\' printable } '"' .
    ```
    Y la gramática define los símbolos y palabras clave reconocidos (`"EXAMEN"`, `"PREGUNTA_VF"`, `"VERDADERO"`, `"="{`, etc.).
  * **Mecanismo de detección:** El carácter `@` no pertenece a ningún token regular ni símbolo terminal de la gramática. Al procesar la línea 3, la función `Scan()` de `Scanner.cs` inspecciona el carácter `@`, constata que no existe ninguna transición válida en el autómata determinista (DFA) a partir de ese símbolo, y retorna un token especial de error léxico (`_EOF` o token no reconocido).
  * **Coordenadas de diagnóstico:** El carácter `@` está ubicado al inicio absoluto de la tercera línea del archivo fuente, en el índice de carácter 0 (base 0) de esa línea. Por lo tanto, el escáner asigna con absoluta precisión `line = 3` y `col = 1`, emitiendo el mensaje estandarizado:
    `linea 3 col 1: caracter o simbolo lexico no reconocido`.

---

### Ficha 6: `error_sintactico_falta_llave.quiz`
* **Objetivo:** Verificar que el analizador sintáctico descendente recursivo (`Parser`) detecte la transgresión estructural consistente en omitir la llave de cierre `}` de una pregunta de opción múltiple antes de alcanzar el fin de archivo.
* **Fase del Compilador Evaluada:** Análisis Sintáctico (Parser por Descenso Recursivo).
* **Código Fuente Completo (`error_sintactico_falta_llave.quiz`):**
  ```quiz
  EXAMEN "Examen con Error Sintáctico"

  PREGUNTA_MULTIPLE "¿Pregunta sin cerrar llave?" {
    a) "Opción A" [CORRECTA]
    b) "Opción B"
  ```
* **Salida Esperada (`error_sintactico_falta_llave.expected`):**
  ```text
  linea 6 col 1: se esperaba "}"
  ```
* **Explicación Formal del Rechazo y Localización:**
  * **Regla violada:** La producción sintáctica `Multiple` en `QuizLang.atg` impone:
    ```csharp
    Multiple =
      "PREGUNTA_MULTIPLE" string [ "PUNTOS" number ]
      "{"
        Opcion<out op>
        { Opcion<out op> }
      "}"
      ...
    ```
  * **Mecanismo de detección:** En `Parser.cs`, el método `Multiple()` consume la llave de apertura `"{"`, la primera opción `a)`, la segunda opción `b)`, y al iterar en el bucle `{ Opcion }` observa que el siguiente token ya no es una `letra`. Por consiguiente, sale del ciclo e intenta ejecutar la validación obligatoria:
    `Expect(12); // Token correspondiente a "}"`
  * **Coordenadas de diagnóstico:** El flujo de entrada se agota de inmediato tras el token `"Opción B"`, por lo que el token de anticipación (`la`) es el final de archivo (`_EOF`), ubicado en la línea 6, columna 1 (línea subsiguiente donde debió figurar la llave). El método `Expect()` falla, invoca a `SynErr()` con el código del token esperado, y produce el mensaje:
    `linea 6 col 1: se esperaba "}"`.

---

### Ficha 7: `error_sintactico_falta_titulo.quiz`
* **Objetivo:** Verificar la estricta obligatoriedad de los argumentos sintácticos en el encabezado del examen, detectando la ausencia del literal de cadena con el título inmediatamente posterior a la palabra clave `EXAMEN`.
* **Fase del Compilador Evaluada:** Análisis Sintáctico (Parser).
* **Código Fuente Completo (`error_sintactico_falta_titulo.quiz`):**
  ```quiz
  EXAMEN
  PREGUNTA_VF "¿Pregunta sin título de examen?" = VERDADERO
  ```
* **Salida Esperada (`error_sintactico_falta_titulo.expected`):**
  ```text
  linea 2 col 1: se esperaba string
  ```
* **Explicación Formal del Rechazo y Localización:**
  * **Regla violada:** La producción inicial `QuizLang` en `QuizLang.atg` exige:
    ```csharp
    QuizLang =
      (. symbolTable = new SymbolTable(errors); .)
      "EXAMEN" string
      (. symbolTable.SetExamen(Unquote(t.val), t.line, t.col); .)
      [ Config ]
      { Pregunta }
      ...
    ```
  * **Mecanismo de detección:** El parser consume el token terminal `"EXAMEN"` en la línea 1 y a continuación ejecuta `Expect(_string)`. Sin embargo, el token de preanálisis (`la`) obtenido del escáner es `"PREGUNTA_VF"`, correspondiente al inicio de la siguiente sentencia.
  * **Coordenadas de diagnóstico:** El token inesperado `"PREGUNTA_VF"` comienza en la línea 2, columna 1 del código fuente. Dado que `la.kind != _string`, el parser dispara `SynErr(3)` (código de token `string`), informando:
    `linea 2 col 1: se esperaba string`.

---

### Ficha 8: `sem_enunciado_duplicado.quiz`
* **Objetivo:** Comprobar la restricción semántica que prohíbe formular dos preguntas con idéntico enunciado dentro del mismo examen, garantizando la unicidad de las consignas independientemente de su puntuación o tipo.
* **Fase del Compilador Evaluada:** Análisis Semántico (`SymbolTable.cs`).
* **Código Fuente Completo (`sem_enunciado_duplicado.quiz`):**
  ```quiz
  EXAMEN "Examen con Preguntas Duplicadas"
  PUNTAJE_MINIMO 2

  PREGUNTA_VF "¿La Tierra gira alrededor del Sol?" PUNTOS 1 = VERDADERO
  PREGUNTA_VF "¿La Tierra gira alrededor del Sol?" PUNTOS 1 = VERDADERO
  ```
* **Salida Esperada (`sem_enunciado_duplicado.expected`):**
  ```text
  linea 5 col 13: Enunciado de pregunta duplicado: "¿La Tierra gira alrededor del Sol?".
  ```
* **Explicación Formal del Rechazo y Localización:**
  * **Regla violada y método involucrado:** El chequeo reside en el método privado `ValidarEnunciado(Pregunta p)` de `SymbolTable.cs`:
    ```csharp
    private void ValidarEnunciado(Pregunta p)
    {
        string enunciadoNormalizado = p.Enunciado.Trim();
        if (!_enunciados.Add(enunciadoNormalizado))
        {
            _errors.SemErr(p.Line, p.Col,
                $"Enunciado de pregunta duplicado: \"{p.Enunciado}\".");
        }
    }
    ```
  * **Mecanismo de detección:** `SymbolTable` mantiene una tabla hash `HashSet<string> _enunciados` con comparador insensible a mayúsculas/minúsculas. La primera pregunta en la línea 4 es agregada satisfactoriamente. Al procesarse la segunda pregunta en la línea 5, `_enunciados.Add(...)` retorna `false` por colisión. Se invoca de inmediato `_errors.SemErr(p.Line, p.Col, ...)`.
  * **Coordenadas de diagnóstico:** En la regla gramatical `VerdaderoFalso`:
    ```csharp
    "PREGUNTA_VF" string (. pvf.Enunciado = Unquote(t.val); pvf.Line = t.line; pvf.Col = t.col; .)
    ```
    El token `string` en la línea 5 está precedido por `"PREGUNTA_VF "` (12 caracteres). Por consiguiente, la comilla inicial del enunciado duplicado se sitúa exactamente en la columna 13 (`linea 5 col 13`).

---

### Ficha 9: `sem_examen_sin_preguntas.quiz`
* **Objetivo:** Verificar la regla semántica global que impide compilar exámenes vacíos que no contengan al menos una pregunta evaluable.
* **Fase del Compilador Evaluada:** Análisis Semántico (`SymbolTable.cs`).
* **Código Fuente Completo (`sem_examen_sin_preguntas.quiz`):**
  ```quiz
  EXAMEN "Examen Vacío Sin Preguntas"
  PUNTAJE_MINIMO 1
  ```
* **Salida Esperada (`sem_examen_sin_preguntas.expected`):**
  ```text
  linea 1 col 8: El examen no contiene preguntas.
  ```
* **Explicación Formal del Rechazo y Localización:**
  * **Regla violada y método involucrado:** En `QuizLang.atg`, al concluir el análisis sintáctico de todo el archivo en la producción `QuizLang`, se invoca la acción semántica:
    ```csharp
    (. symbolTable.ValidarExamen(); .)
    ```
    Y en `SymbolTable.cs`:
    ```csharp
    public void ValidarExamen()
    {
        // 1. Examen sin preguntas
        if (Examen.Preguntas.Count == 0)
        {
            _errors.SemErr(Examen.Line, Examen.Col,
                "El examen no contiene preguntas.");
            return;
        }
        ...
    }
    ```
  * **Mecanismo de detección:** Dado que el bucle `{ Pregunta }` no iteró ninguna vez, la lista `Examen.Preguntas` permanece con cardinalidad cero (`Count == 0`), disparando el error semántico.
  * **Coordenadas de diagnóstico:** El error se asocia conceptualmente al objeto global `Examen`, reportándose en `Examen.Line` y `Examen.Col`. Dichos atributos fueron registrados cuando el parser procesó el token `string` del título en la línea 1:
    `"EXAMEN "` ocupa 7 caracteres, comenzando la cadena `"Examen Vacío Sin Preguntas"` exactamente en la columna 8. Por tal motivo, el diagnóstico señala con exactitud `linea 1 col 8`.

---

### Ficha 10: `sem_multiple_sin_correcta.quiz`
* **Objetivo:** Verificar la restricción semántica que exige que toda pregunta de selección múltiple posea obligatoriamente al menos una alternativa marcada como correcta mediante la etiqueta `[CORRECTA]`.
* **Fase del Compilador Evaluada:** Análisis Semántico (`SymbolTable.cs`).
* **Código Fuente Completo (`sem_multiple_sin_correcta.quiz`):**
  ```quiz
  EXAMEN "Examen Sin Opción Correcta"
  PUNTAJE_MINIMO 1

  PREGUNTA_MULTIPLE "¿Cuál es la respuesta correcta?" PUNTOS 1 {
    a) "Primera opción"
    b) "Segunda opción"
  }
  ```
* **Salida Esperada (`sem_multiple_sin_correcta.expected`):**
  ```text
  linea 4 col 19: La pregunta múltiple "¿Cuál es la respuesta correcta?" no tiene ninguna opción marcada como [CORRECTA].
  ```
* **Explicación Formal del Rechazo y Localización:**
  * **Regla violada y método involucrado:** En `SymbolTable.ValidarYAgregarPreguntaMultiple(PreguntaMultiple p)`:
    ```csharp
    int correctas = p.Opciones.Count(o => o.EsCorrecta);
    if (correctas == 0)
    {
        _errors.SemErr(p.Line, p.Col,
            $"La pregunta múltiple \"{p.Enunciado}\" no tiene ninguna opción marcada como [CORRECTA].");
    }
    ```
  * **Mecanismo de detección:** Cada objeto `Opcion` tiene su propiedad booleana `EsCorrecta` inicializada en `false`, la cual solo pasa a `true` si la regla sintáctica `Opcion` detecta el marcador opcional `[ "[CORRECTA]" ]`. Al iterar sobre las opciones `a)` y `b)`, ninguna posee dicho modificador, por lo que `correctas == 0`.
  * **Coordenadas de diagnóstico:** El error se notifica sobre el enunciado de la pregunta cuestionada (`p.Line` y `p.Col`). En la línea 4, tras la palabra clave `"PREGUNTA_MULTIPLE "` (18 caracteres), el enunciado `"¿Cuál es la respuesta correcta?"` arranca en la columna 19 (`linea 4 col 19`).

---

### Ficha 11: `sem_multiple_varias_correctas.quiz`
* **Objetivo:** Comprobar que el compilador impida la ambigüedad en preguntas de selección múltiple, rechazando preguntas que marquen dos o más alternativas simultáneas con la etiqueta `[CORRECTA]`.
* **Fase del Compilador Evaluada:** Análisis Semántico (`SymbolTable.cs`).
* **Código Fuente Completo (`sem_multiple_varias_correctas.quiz`):**
  ```quiz
  EXAMEN "Examen Con Múltiples Opciones Correctas"
  PUNTAJE_MINIMO 1

  PREGUNTA_MULTIPLE "¿Cuál es la respuesta correcta?" PUNTOS 1 {
    a) "Primera opción" [CORRECTA]
    b) "Segunda opción" [CORRECTA]
  }
  ```
* **Salida Esperada (`sem_multiple_varias_correctas.expected`):**
  ```text
  linea 4 col 19: La pregunta múltiple "¿Cuál es la respuesta correcta?" tiene más de una opción marcada como [CORRECTA] (se encontraron 2).
  ```
* **Explicación Formal del Rechazo y Localización:**
  * **Regla violada y método involucrado:** En `SymbolTable.ValidarYAgregarPreguntaMultiple`:
    ```csharp
    int correctas = p.Opciones.Count(o => o.EsCorrecta);
    ...
    else if (correctas > 1)
    {
        _errors.SemErr(p.Line, p.Col,
            $"La pregunta múltiple \"{p.Enunciado}\" tiene más de una opción marcada como [CORRECTA] (se encontraron {correctas}).");
    }
    ```
  * **Mecanismo de detección:** Ambas opciones `a)` y `b)` poseen la marca `[CORRECTA]`, resultando en `correctas == 2`. Como QuizLang modela preguntas de respuesta única para su posterior corrección determinista mediante radio buttons HTML, la existencia de múltiples claves correctas es una anomalía semántica explícitamente vedada.
  * **Coordenadas de diagnóstico:** El error se ancla en las coordenadas del enunciado de la pregunta `p.Line = 4` y `p.Col = 19`, coincidiendo con el inicio de la cadena en la línea 4 tras el prefijo `"PREGUNTA_MULTIPLE "`.

---

### Ficha 12: `sem_opcion_letra_repetida.quiz`
* **Objetivo:** Verificar el control semántico de etiquetas identificadoras en preguntas de selección múltiple, detectando y rechazando la duplicación de letras de opción (por ejemplo, dos alternativas etiquetadas con `a)`).
* **Fase del Compilador Evaluada:** Análisis Semántico (`SymbolTable.cs`).
* **Código Fuente Completo (`sem_opcion_letra_repetida.quiz`):**
  ```quiz
  EXAMEN "Examen Con Letra de Opción Repetida"
  PUNTAJE_MINIMO 1

  PREGUNTA_MULTIPLE "¿Pregunta con letras repetidas?" PUNTOS 1 {
    a) "Primera opción" [CORRECTA]
    a) "Segunda opción con letra a repetida"
  }
  ```
* **Salida Esperada (`sem_opcion_letra_repetida.expected`):**
  ```text
  linea 6 col 3: Letra de opción duplicada 'a' en la pregunta "¿Pregunta con letras repetidas?".
  ```
* **Explicación Formal del Rechazo y Localización:**
  * **Regla violada y método involucrado:** En `SymbolTable.cs`:
    ```csharp
    var letrasVistas = new HashSet<char>();
    foreach (var op in p.Opciones)
    {
        char letraNormalizada = char.ToLowerInvariant(op.Letra);
        if (!letrasVistas.Add(letraNormalizada))
        {
            _errors.SemErr(op.Line, op.Col,
                $"Letra de opción duplicada '{op.Letra}' en la pregunta \"{p.Enunciado}\".");
        }
        ...
    }
    ```
  * **Mecanismo de detección:** Al recorrer las opciones de la pregunta, la primera alternativa inscribe el carácter `'a'` en el conjunto `letrasVistas`. Al evaluar la segunda opción en la línea 6, que también declara la letra `'a'`, el método `Add` falla y emite el diagnóstico semántico.
  * **Coordenadas de diagnóstico:** La coordenada utilizada es `op.Line` y `op.Col`, capturada en la producción `Opcion` de `QuizLang.atg`:
    ```csharp
    Opcion<out Opcion op> =
      letra (. op.Letra = t.val[0]; op.Line = t.line; op.Col = t.col; .)
      ")" ...
    ```
    En la línea 6, la letra `'a'` está precedida por 2 espacios de sangría (`  a)`). Por lo tanto, el token `letra` inicia con total precisión en la columna 3 (`linea 6 col 3`).

---

### Ficha 13: `sem_puntaje_minimo_excesivo.quiz`
* **Objetivo:** Verificar la validación de coherencia aritmética global del examen, impidiendo que el docente configure un `PUNTAJE_MINIMO` numéricamente superior al total de puntos acumulados por todas las preguntas declaradas.
* **Fase del Compilador Evaluada:** Análisis Semántico (`SymbolTable.cs`).
* **Código Fuente Completo (`sem_puntaje_minimo_excesivo.quiz`):**
  ```quiz
  EXAMEN "Examen Con Puntaje Mínimo Excesivo"
  PUNTAJE_MINIMO 10

  PREGUNTA_VF "¿Pregunta de un solo punto?" PUNTOS 1 = VERDADERO
  ```
* **Salida Esperada (`sem_puntaje_minimo_excesivo.expected`):**
  ```text
  linea 2 col 16: El puntaje mínimo (10) es mayor al total de puntos del examen (1).
  ```
* **Explicación Formal del Rechazo y Localización:**
  * **Regla violada y método involucrado:** En `SymbolTable.cs` dentro del método de cierre `ValidarExamen()`:
    ```csharp
    if (Examen.PuntajeMinimo.HasValue && Examen.PuntajeMinimo.Value > Examen.TotalPuntos)
    {
        _errors.SemErr(Examen.PuntajeMinimoLine, Examen.PuntajeMinimoCol,
            $"El puntaje mínimo ({Examen.PuntajeMinimo.Value}) es mayor al total de puntos del examen ({Examen.TotalPuntos}).");
    }
    ```
  * **Mecanismo de detección:** El examen define `PUNTAJE_MINIMO 10`, pero la única pregunta contenida es de 1 punto (`Examen.TotalPuntos = 1`). La condición $10 > 1$ se cumple, evidenciando un examen imposible de aprobar para cualquier alumno.
  * **Coordenadas de diagnóstico:** Se reporta en las coordenadas capturadas durante el procesamiento de la directiva `Config`:
    ```csharp
    Config =
      "PUNTAJE_MINIMO" number
      (.
         int puntajeMin = int.Parse(t.val);
         symbolTable.SetConfig(puntajeMin, t.line, t.col);
      .)
    ```
    En la línea 2, el texto `"PUNTAJE_MINIMO "` contiene 15 caracteres. El token numérico `"10"` arranca en la columna 16 (`linea 2 col 16`).

---

## 5. Resultados de la Ejecución Real del Banco de Pruebas

La batería completa fue ejecutada el **3 de noviembre de 2026** sobre el entorno de desarrollo oficial (Windows 11, PowerShell, .NET 10.0.400 SDK). 

A continuación se transcribe la salida real e íntegra producida por la ejecución de `run_tests.ps1`:

```text
=============================================
 Compilando QuizLang.sln...
=============================================

Ejecutando suite de pruebas en: C:\Users\abelr\source\repos\compi2026\TrabajoFinal\tests

  [PASS] basico.quiz
  [PASS] comentarios.quiz
  [PASS] compiladores2026.quiz
  [PASS] completo.quiz
  [PASS] error_lexico.quiz
  [PASS] error_sintactico_falta_llave.quiz
  [PASS] error_sintactico_falta_titulo.quiz
  [PASS] sem_enunciado_duplicado.quiz
  [PASS] sem_examen_sin_preguntas.quiz
  [PASS] sem_multiple_sin_correcta.quiz
  [PASS] sem_multiple_varias_correctas.quiz
  [PASS] sem_opcion_letra_repetida.quiz
  [PASS] sem_puntaje_minimo_excesivo.quiz

---------------------------------------------
Resultados: 13/13 pruebas pasaron (PASS).
```

### 5.1 Balance Cuantitativo
* **Total de Pruebas Ejecutadas:** 13
* **Pruebas Aprobadas (PASS):** 13 (100%)
* **Pruebas Fallidas (FAIL):** 0 (0%)
* **Tiempo de Ejecución Total:** < 4 segundos (incluyendo compilación incremental previa de la solución .NET).

---

## 6. Limitaciones Detectadas y Pruebas Futuras

El análisis exhaustivo de los resultados obtenidos permitió identificar las siguientes particularidades técnicas, limitaciones operativas y oportunidades de ampliación para futuras versiones del compilador:

### 6.1 Reporte del Primer Error Sintáctico
El parser descendente recursivo generado por Coco/R emplea una estrategia de recuperación con ventana de sincronización de al menos dos tokens válidos. Cuando ocurre un error sintáctico severo (como la omisión de un delimitador estructural o una palabra clave faltante), el compilador emite el primer diagnóstico encontrado y detiene o aborta la producción corriente. Si bien esto previene avalanchas de errores falsos o en cascada (*cascading errors*), impide al usuario visualizar múltiples fallas sintácticas independientes en una sola pasada.

### 6.2 Casos Borde y Pruebas Semánticas No Cubiertas Actualmente
Existen situaciones de frontera no incorporadas formalmente en los 13 casos actuales que ameritan pruebas adicionales en futuras iteraciones:
1. **Letras de Opción No Consecutivas:**
   La tabla de símbolos (`SymbolTable.cs`) contiene lógica para emitir advertencias no fatales (`Warning`) cuando las opciones no siguen un orden correlativo (por ejemplo, opción `a)` seguida de `c)`):
   ```csharp
   if (siguienteEsperada.HasValue && letraNormalizada != siguienteEsperada.Value)
   {
       _errors.Warning(op.Line, op.Col,
           $"Letra de opción no consecutiva: se esperaba '{siguienteEsperada.Value}' pero se encontró '{op.Letra}'.");
   }
   ```
   Actualmente la suite no cuenta con un archivo `.quiz` dedicado a verificar que el compilador emita la advertencia sin fallar ni abortar el código de salida (exit code 0).
2. **Puntajes Nulos o Inválidos:**
   La regla `number` reconoce secuencias de dígitos sin signo. Casos con `PUNTOS 0` no son rechazados sintácticamente y podrían distorsionar la ponderación del examen, lo que sugiere la conveniencia de una regla semántica que exija puntajes estrictamente positivos ($\text{puntos} \ge 1$).
3. **Cadenas Vacías:**
   Un enunciado o título definido como `""` es válido para la expresión regular `string`, pero carece de sentido pedagógico. Podría incorporarse una validación semántica que exija longitud mínima o contenido imprimible no blanco.
4. **Límite de Opciones por Pregunta:**
   El alfabeto latino comprende 26 letras (`a` a `z`). Una pregunta con más de 26 alternativas generaría desbordamiento en la correlatividad alfabética.

### 6.3 Naturaleza Manual de las Pruebas sobre la Interfaz Gráfica (`QuizLang.Gui`)
El script automatizado `run_tests.ps1` opera exclusivamente sobre el compilador de línea de comandos (`QuizLang.Cli.exe`). Las características particulares del entorno de desarrollo gráfico (como el cálculo visual de números de línea sincronizado con el scroll, el salto automático al código fuente al hacer doble clic sobre un error del DataGridView y la previsualización interactiva dentro del control WebBrowser) se validan mediante pruebas manuales exploratorias. En una etapa posterior, podrían implementarse pruebas de interfaz automatizadas empleando frameworks de automatización de UI en Windows.
