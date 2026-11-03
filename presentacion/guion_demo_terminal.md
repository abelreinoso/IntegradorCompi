# Guion Técnico de Demostración en Vivo por Terminal

**Asignatura:** Compiladores 2026  
**Evaluación:** Parcial 2 – Avances del Trabajo Final (Parte 2)  
**Proyecto:** QuizLang – Compilador de Exámenes Interactivos  
**Responsable de Terminal:** Mateo Gallardo (con soporte de Joaquín Albornoz, Guadalupe Díaz y Abel Reinoso)  
**Tiempo Estimado:** 3:00 a 4:00 minutos  
**Directorio de Trabajo Base:** `c:\Users\abelr\source\repos\compi2026\TrabajoFinal\` (todas las rutas son relativas a este directorio)

---

## 1. Preparación Previa (Antes de Comenzar la Exposición)

1. Abrir una consola de **PowerShell** en el directorio raíz del proyecto:
   ```powershell
   cd c:\Users\abelr\source\repos\compi2026\TrabajoFinal
   ```
2. Asegurar que la codificación de la consola sea UTF-8 para visualizar caracteres acentuados y símbolos correctamente:
   ```powershell
   [Console]::OutputEncoding = [System.Text.Encoding]::UTF8
   ```
3. Limpiar la pantalla de la terminal (`Clear-Host` o `cls`) y tener el navegador web por defecto (Edge o Chrome) abierto en segundo plano.
4. Asegurar que la carpeta `presentacion\demo\` exista:
   ```powershell
   if (-not (Test-Path "presentacion\demo")) { New-Item -ItemType Directory -Path "presentacion\demo" }
   ```

---

## 2. Secuencia Paso a Paso de la Demostración

### Paso 1: Regeneración del Analizador Léxico y Sintáctico con Coco/R
* **Objetivo:** Demostrar que el compilador no utiliza analizadores escritos a mano ni cajas negras, sino que genera el código C# de `Scanner.cs` y `Parser.cs` de manera determinista a partir de la especificación formal `QuizLang.atg`.
* **Comando a ejecutar:**
  ```powershell
  powershell -ExecutionPolicy Bypass -File tools\generar.ps1
  ```
* **Salida real obtenida en consola:**
  ```text
  Regenerando Scanner y Parser con Coco/R...
    Gramática: C:\Users\abelr\source\repos\compi2026\TrabajoFinal\QuizLang.Core\QuizLang.atg
    Frames:    C:\Users\abelr\source\repos\compi2026\TrabajoFinal\tools\coco
    Destino:   C:\Users\abelr\source\repos\compi2026\TrabajoFinal\QuizLang.Core
  Coco/R (Apr 19, 2011)
  checking
  parser + scanner generated
  0 errors detected
  Generación exitosa sin errores.
  ```
* **Qué decir mientras se ejecuta:**
  > *"Acá invocamos nuestro script de generación. Coco/R procesa `QuizLang.atg` junto a las plantillas `Scanner.frame` y `Parser.frame`. Fíjense en la línea `0 errors detected`: el generador verifica formalmente que la gramática no contiene recursión por izquierda, es circularmente consistente y cumple de forma estricta la condición LL(1) sin ningún tipo de ambigüedades."*

---

### Paso 2: Compilación de un Examen Válido y Demostración en el Navegador
* **Objetivo:** Demostrar la compilación exitosa de un archivo `.quiz` real y la generación del artefacto final: una página web HTML5 autocontenida, con estilos y lógica de autoevaluación cliente pura.
* **Comando de compilación:**
  ```powershell
  dotnet run --project QuizLang.Cli\QuizLang.Cli.csproj -- tests\compiladores2026.quiz -o presentacion\demo\demo_valido.html
  ```
* **Salida real obtenida en consola:**
  ```text
  Compilación exitosa. HTML generado en: presentacion\demo\demo_valido.html
  ```
* **Comando para abrir el HTML generado:**
  ```powershell
  Start-Process presentacion\demo\demo_valido.html
  ```
* **Qué decir y mostrar en el navegador:**
  > *"El compilador procesó el archivo fuente `compiladores2026.quiz` y generó una página web autocontenida de unos 16 kilobytes. Al abrirla en el navegador, vemos el título del examen, las insignias con el puntaje total y mínimo de aprobación, y las distintas tarjetas de preguntas.*  
  > *(Acción interactiva): Voy a marcar una opción correcta en la primera pregunta múltiple y una opción incorrecta en la segunda. Hago clic en el botón inferior 'Corregir Examen'.*  
  > *Inmediatamente el motor de JavaScript incorporado evalúa las respuestas en el navegador: pinta las respuestas correctas en verde con tilde, las incorrectas en rojo con cruz indicando cuál era la alternativa correcta, y en la cabecera nos muestra el puntaje obtenido y el estado de aprobación. Todo esto funciona 100% offline sin necesidad de un servidor backend."*

---

### Paso 3: Detección y Localización de Error Léxico
* **Objetivo:** Demostrar cómo el escáner generado por Coco/R rechaza caracteres que no pertenecen al alfabeto del lenguaje y emite el mensaje con la línea y columna exactas.
* **Archivo de prueba:** `tests\error_lexico.quiz` (contiene un caracter `@` en la línea 3, columna 1).
* **Comando a ejecutar:**
  ```powershell
  dotnet run --project QuizLang.Cli\QuizLang.Cli.csproj -- tests\error_lexico.quiz
  ```
* **Salida real obtenida en consola:**
  ```text
  linea 3 col 1: caracter o simbolo lexico no reconocido
  ```
* **Código de salida verificado:** `1` (falla controlada).
* **Qué decir:**
  > *"Ahora pasamos a verificar el manejo de errores. En este archivo `error_lexico.quiz` incluimos un caracter arroba que no forma parte de ningún token permitido ni de los identificadores. El analizador léxico detiene el procesamiento e informa con precisión milimétrica: `linea 3 col 1: caracter o simbolo lexico no reconocido`, saliendo con código de error 1."*

---

### Paso 4: Detección y Localización de Error Sintáctico
* **Objetivo:** Demostrar cómo el parser descendente recursivo detecta una violación a la estructura EBNF de la gramática.
* **Archivo de prueba:** `tests\error_sintactico_falta_llave.quiz` (pregunta de opción múltiple abierta con `{` pero que llega al final del archivo sin la correspondiente `}`).
* **Comando a ejecutar:**
  ```powershell
  dotnet run --project QuizLang.Cli\QuizLang.Cli.csproj -- tests\error_sintactico_falta_llave.quiz
  ```
* **Salida real obtenida en consola:**
  ```text
  linea 6 col 1: se esperaba "}"
  ```
* **Código de salida verificado:** `1`.
* **Qué decir:**
  > *"Acá compilamos un caso con error sintáctico: una pregunta múltiple donde se omitió la llave de cierre. Al llegar a la línea 6, el parser esperaba el delimitador `}` para cerrar el bloque de opciones según la producción EBNF. Como encuentra el fin de archivo, emite de forma clara e inequívoca: `linea 6 col 1: se esperaba "}"`."*

---

### Paso 5: Detección y Localización de Errores Semánticos
* **Objetivo:** Demostrar que el compilador no se limita a verificar la sintaxis, sino que implementa reglas de contexto en `SymbolTable.cs`.
* **Caso 5A: Pregunta de opción múltiple sin ninguna alternativa correcta.**
  * **Comando a ejecutar:**
    ```powershell
    dotnet run --project QuizLang.Cli\QuizLang.Cli.csproj -- tests\sem_multiple_sin_correcta.quiz
    ```
  * **Salida real obtenida en consola:**
    ```text
    linea 4 col 19: La pregunta múltiple "¿Cuál es la respuesta correcta?" no tiene ninguna opción marcada como [CORRECTA].
    ```
  * **Qué decir:**
    > *"Sintácticamente esta pregunta es impecable: tiene opciones, letras y llaves válidas. Pero semánticamente no tiene sentido evaluar una pregunta múltiple sin respuesta correcta. Nuestra tabla de símbolos recorre las opciones del AST, detecta que la cantidad de marcas `[CORRECTA]` es cero y reporta el error contextual indicando el enunciado y la posición de la pregunta."*

* **Caso 5B (opcional / complementario): Puntaje mínimo superior al acumulado.**
  * **Comando a ejecutar:**
    ```powershell
    dotnet run --project QuizLang.Cli\QuizLang.Cli.csproj -- tests\sem_puntaje_minimo_excesivo.quiz
    ```
  * **Salida real obtenida en consola:**
    ```text
    linea 2 col 16: El puntaje mínimo (10) es mayor al total de puntos del examen (1).
    ```
  * **Qué decir:**
    > *"Otro chequeo semántico global: si el docente especifica un `PUNTAJE_MINIMO` de 10 puntos en un examen cuyas preguntas solo suman 1 punto, el compilador valida la suma total del examen y advierte la inconsistencia antes de generar un examen matemáticamente imposible de aprobar."*

---

### Paso 6: Ejecución de la Suite Completa de Pruebas Automatizadas
* **Objetivo:** Demostrar la robustez de todo el pipeline y la cobertura de pruebas de regresión automatizadas.
* **Comando a ejecutar:**
  ```powershell
  powershell -ExecutionPolicy Bypass -File run_tests.ps1
  ```
* **Salida real obtenida en consola:**
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
* **Qué decir:**
  > *"Para cerrar la demo en consola, ejecutamos nuestro script de pruebas automatizado `run_tests.ps1`. El script compila la solución completa y ejecuta el compilador sobre los trece casos de prueba, comparando los códigos de retorno y los mensajes de error contra los archivos `.expected`. Como pueden ver, el cien por ciento de las pruebas resultan en [PASS]."*

---

## 3. Plan de Contingencia (Plan B)

Si durante la presentación en vivo ocurre algún imprevisto técnico:

| Escenario de Falla | Causa Probable | Acción Inmediata (Plan B) |
| :--- | :--- | :--- |
| **`dotnet run` demora o reporta bloqueo de archivo:** | El proceso previo quedó abierto o el antivirus de Windows analiza el binario. | Invocar directamente el binario compilado: `.\QuizLang.Cli\bin\Debug\net10.0\QuizLang.Cli.exe tests\compiladores2026.quiz -o presentacion\demo\demo_valido.html`. No requiere recompilación de .NET. |
| **`Start-Process` no abre el navegador:** | La asociación de extensiones `.html` en Windows está deshabilitada o en segundo plano. | Abrir manualmente el navegador y arrastrar el archivo `presentacion\demo\demo_valido.html` a una pestaña nueva, o ejecutar: `Start-Process msedge (Resolve-Path "presentacion\demo\demo_valido.html")`. |
| **Caracteres extraños en la consola (mojibake):** | La terminal de PowerShell perdió la configuración UTF-8 activa. | Ejecutar rápidamente: `[Console]::OutputEncoding = [System.Text.Encoding]::UTF8; chcp 65001`. |
| **Falla en `tools\generar.ps1` por permisos:** | La política de ejecución de PowerShell bloqueó el script no firmado. | Ejecutar con flag de bypass explícito: `powershell -ExecutionPolicy Bypass -File tools\generar.ps1`, o invocar directamente a Coco: `.\tools\coco\Coco.exe QuizLang.Core\QuizLang.atg -namespace QuizLang.Core -frames tools\coco -o QuizLang.Core`. |
