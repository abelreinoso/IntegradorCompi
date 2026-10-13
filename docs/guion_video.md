# Guion Técnico del Video Demostrativo y Explicativo de QuizLang

**Materia:** Compiladores 2026  
**Proyecto:** Compilador de QuizLang y Generador de Exámenes Interactivos  
**Autores:** Albornoz Joaquín, Díaz Guadalupe, Gallardo Mateo, Reinoso Abel  
**Duración Total Estimada:** 6 minutos y 15 segundos (Rango requerido: 5 a 7 minutos)  
**Formato de Grabación:** Captura de pantalla de alta definición (1080p / 60 fps) con narración de voz en off clara y profesional.  

---

## Estructura General del Video

| Bloque | Intervalo de Tiempo | Escena / Foco Temático | Responsable Principal de Narración |
| :-: | :---: | :--- | :--- |
| **1** | 0:00 – 0:45 | Presentación del grupo, objetivos y visión general | **Albornoz Joaquín** |
| **2** | 0:45 – 1:45 | Arquitectura interna: Coco/R, gramática LL(1) y .NET 10 | **Díaz Guadalupe** |
| **3** | 1:45 – 3:00 | Demostración de `QuizLang.Gui`: compilación y visor integrado | **Gallardo Mateo** |
| **4** | 3:00 – 4:15 | Interacción web en navegador: resolución y corrección en vivo | **Reinoso Abel** |
| **5** | 4:15 – 5:20 | Detección de errores semánticos y navegación por doble clic | **Gallardo Mateo & Reinoso Abel** |
| **6** | 5:20 – 6:15 | Suite de pruebas en consola (`run_tests.ps1`) y conclusiones | **Albornoz Joaquín & Díaz Guadalupe** |

---

## Guion Minuto a Minuto: Video, Pantalla y Narración

### Bloque 1: Presentación Institucional y Motivación (0:00 – 0:45)

* **Tiempo:** 0:00 – 0:45 (45 segundos)
* **Qué se muestra en pantalla:**
  * *(0:00 - 0:15)* Placa inicial con el título: *QuizLang - Compilador de Exámenes Interactivos*, integrantes del grupo (Albornoz, Díaz, Gallardo, Reinoso), cátedra Compiladores 2026.
  * *(0:15 - 0:45)* Vista panorámica del repositorio en Visual Studio Code: estructura de carpetas (`QuizLang.Core`, `QuizLang.Cli`, `QuizLang.Gui`, `tests`, `docs`).
* **Narración de Voz en Off (Albornoz Joaquín):**
  > *"Hola a todos. Bienvenidos a la presentación técnica del Trabajo Final de Compiladores 2026. Somos el grupo integrado por Joaquín Albornoz, Guadalupe Díaz, Mateo Gallardo y Abel Reinoso.*  
  > *En este proyecto desarrollamos **QuizLang**, un lenguaje de dominio específico orientado a la definición concisa de exámenes y autoevaluaciones técnicas. El compilador procesa archivos fuente con extensión `.quiz` y emite aplicaciones web interactivas autocontenidas en HTML5, CSS y JavaScript que funcionan offline en cualquier navegador.*  
  > *Todo el núcleo fue construido utilizando **Coco/R para C#** bajo el entorno moderno de **.NET 10**."*

---

### Bloque 2: Arquitectura del Compilador y Gramática LL(1) (0:45 – 1:45)

* **Tiempo:** 0:45 – 1:45 (60 segundos)
* **Qué se muestra en pantalla:**
  * *(0:45 - 1:15)* Apertura del archivo `QuizLang.Core\QuizLang.atg`. El cursor resalta las secciones: `CHARACTERS`, `TOKENS`, `COMMENTS` y la regla sintáctica `QuizLang`.
  * *(1:15 - 1:45)* Diagrama arquitectónico en pantalla completa que muestra el flujo: `.atg` $\rightarrow$ `Coco.exe` $\rightarrow$ `Scanner.cs` y `Parser.cs` $\rightarrow$ `SymbolTable` $\rightarrow$ `CodeGen` $\rightarrow$ HTML final.
* **Narración de Voz en Off (Díaz Guadalupe):**
  > *"La base formal de QuizLang radica en su especificación gramatical en Cocol/R dentro del archivo `QuizLang.atg`.*  
  > *Diseñamos una gramática estrictamente **LL(1)** sin conflictos de anticipación, lo que permite que Coco/R genere un analizador sintáctico descendente recursivo determinista y de tiempo lineal.*  
  > *La gramática admite tres modalidades de evaluación: preguntas de opción múltiple con alternativas etiquetadas, preguntas de verdadero o falso y preguntas de respuesta textual abierta.*  
  > *En la sección sintáctica, integramos acciones semánticas directas que instancian las estructuras del Árbol de Sintaxis Abstracta y delegan en nuestra clase `SymbolTable.cs` la validación contextual de reglas complejas antes de pasar a la emisión de código en `CodeGen.cs`."*

---

### Bloque 3: Demostración del Entorno Gráfico (`QuizLang.Gui`) (1:45 – 3:00)

* **Tiempo:** 1:45 – 3:00 (75 segundos)
* **Qué se muestra en pantalla:**
  * *(1:45 - 2:00)* En la terminal de PowerShell se ejecuta: `dotnet run --project QuizLang.Gui\QuizLang.Gui.csproj`. Se abre la ventana de Windows Forms con su interfaz dividida en editor de código a la izquierda, visor integrado a la derecha y tabla de diagnósticos abajo.
  * *(2:00 - 2:30)* En el selector superior de ejemplos se elige *"Compiladores 2026 (Examen Real)"*. Se aprecia el código cargado en el editor con números de línea perfectamente alineados en la barra gris lateral.
  * *(2:30 - 3:00)* El usuario hace clic en el botón **"▶ Compilar"**. La barra de estado se ilumina en color verde mostrando: *"✓ Compilación exitosa: 5 preguntas, 10 puntos en total"*, y el panel derecho de vista previa renderiza de inmediato el examen interactivo.
* **Narración de Voz en Off (Gallardo Mateo):**
  > *"Ahora veamos el compilador en acción mediante nuestra interfaz de desarrollo gráfica `QuizLang.Gui`, construida en Windows Forms.*  
  > *La aplicación cuenta con un editor monoespaciado ergonómico con numeración de líneas sincronizada en tiempo real mediante GDI+. En la barra superior disponemos de un selector con plantillas predefinidas. Seleccionamos el ejemplo 'Compiladores 2026', el cual contiene 5 preguntas teóricas sobre nuestra materia con un puntaje mínimo de aprobación de 7 puntos.*  
  > *Al hacer clic en 'Compilar', la fachada en memoria invoca al escáner y analizador sintáctico de Coco/R. Como no hay errores, se genera la página web en memoria y el control de vista previa integrado nos muestra inmediatamente el examen completamente formateado y listo para responder."*

---

### Bloque 4: Interacción y Corrección en el Navegador Web (3:00 – 4:15)

* **Tiempo:** 3:00 – 4:15 (75 segundos)
* **Qué se muestra en pantalla:**
  * *(3:00 - 3:20)* Se hace clic en el botón superior **"🌐 Ver HTML"**. Se abre Google Chrome o Microsoft Edge en pantalla completa mostrando el archivo HTML autocontenido generado.
  * *(3:20 - 3:50)* El presentador interactúa con el examen en vivo:
    * Pregunta 1 (Opción múltiple): Marca *"Descendente recursivo LL(1)"*.
    * Pregunta 2 (V/F): Marca *"Falso"*.
    * Pregunta 3 (Opción múltiple): Marca deliberadamente una opción incorrecta (*"La gramática debe ser ambigua"*).
    * Pregunta 4 (V/F): Marca *"Verdadero"*.
    * Pregunta 5 (Texto libre): Escribe `"tabla de simbolos"` (con minúsculas para probar la normalización).
  * *(3:50 - 4:15)* Se hace clic en el botón **"Corregir"**. La página evalúa instantáneamente: las preguntas 1, 2, 4 y 5 se pintan de verde con tilde de acierto; la pregunta 3 se torna roja indicando *"✗ Incorrecto (0/2 puntos). Opción correcta: a"*. Al final aparece el panel de veredicto: *"🎉 ¡Felicitaciones! Has APROBADO el examen. Puntaje obtenido: 8 / 10 puntos"*.
* **Narración de Voz en Off (Reinoso Abel):**
  > *"Haciendo clic en 'Ver HTML', abrimos la evaluación generada en el navegador web.*  
  > *Observen que el archivo es 100% autocontenido: no descarga fuentes externas, hojas de estilo remotas ni librerías de terceros, lo que garantiza su funcionamiento en entornos educativos sin internet.*  
  > *Vamos a responder el examen: seleccionamos opciones correctas en las primeras dos preguntas, pero en la tercera marcaremos una respuesta errónea para verificar la corrección. En la pregunta de texto libre escribimos la respuesta en minúsculas y sin acentos.*  
  > *Al presionar 'Corregir', el motor JavaScript evalúa las respuestas en milisegundos. Las tarjetas correctas se iluminan en verde, la pregunta incorrecta destaca su opción en rojo y nos indica la alternativa correcta esperada, y la respuesta textual se valida exitosamente gracias a la normalización insensible a mayúsculas. Con 8 puntos sobre 10, el sistema computa que superamos el mínimo de 7 y nos certifica como Aprobados."*

---

### Bloque 5: Detección de Errores y Navegación Interactiva (4:15 – 5:20)

* **Tiempo:** 4:15 – 5:20 (65 segundos)
* **Qué se muestra en pantalla:**
  * *(4:15 - 4:35)* Regreso a la GUI. Se selecciona en el combo de ejemplos *"Inválido: Error Semántico"*. En el código se visualiza un examen con un puntaje mínimo excesivo (10 puntos para 2 puntos totales) y dos preguntas múltiples que infringen reglas de negocio: una sin ninguna `[CORRECTA]` y otra con letras de opción repetidas (`a)` dos veces).
  * *(4:35 - 4:55)* Se pulsa **"▶ Compilar"**. La barra de estado pasa a color rojo: *"✗ Compilación fallida: se encontraron 3 error(es)"*. La grilla inferior se llena con 3 filas resaltadas en color rojizo detallando tipo, línea, columna y mensaje de error en español.
  * *(4:55 - 5:20)* El usuario hace **doble clic** sobre el error de la fila 2 (*"Letra de opción duplicada 'a'..."*). Inmediatamente, el cursor del editor salta y resalta la línea 139 exacta donde se encuentra el conflicto. El usuario corrige la letra `'a'` por `'b'`, agrega el marcador `[CORRECTA]`, reduce el puntaje mínimo y vuelve a compilar, logrando el éxito inmediato.
* **Narración de Voz en Off (Gallardo Mateo & Reinoso Abel):**
  > *(Gallardo): "Un compilador riguroso debe ser inflexible ante infracciones léxicas, sintácticas o semánticas. Cargamos ahora el ejemplo con errores semánticos y compilamos.*  
  > *Nuestra `SymbolTable` intercepta tres violaciones críticas: la primera pregunta múltiple carece de opción correcta, la segunda repite la letra 'a', y el puntaje mínimo de 10 puntos excede el total del examen. Noten cómo los mensajes están completamente traducidos al español y especifican la coordenada exacta del fallo."*  
  > *(Reinoso): "Además, implementamos una característica de productividad esencial: al hacer doble clic sobre cualquier diagnóstico en la tabla, el editor salta de forma instantánea seleccionando la línea conflictiva. Modificamos la letra duplicada, agregamos la marca de opción correcta, ajustamos el puntaje mínimo y al recompilar obtenemos nuevamente un examen válido y listo para exportar."*

---

### Bloque 6: Verificación Automatizada y Cierre (5:20 – 6:15)

* **Tiempo:** 5:20 – 6:15 (55 segundos)
* **Qué se muestra en pantalla:**
  * *(5:20 - 5:45)* Se abre la consola de PowerShell en la raíz del proyecto y se ejecuta:  
    `powershell -ExecutionPolicy Bypass -File tests\run_tests.ps1`.  
    La consola compila la solución y corre velozmente los 13 casos de prueba, imprimiendo líneas verdes `[PASS]` para cada archivo `.quiz` y concluyendo con:  
    `Resultados: 13/13 pruebas pasaron (PASS).`
  * *(5:45 - 6:15)* Placa final de cierre con las conclusiones del equipo, mención de Coco/R y agradecimiento a la cátedra de Compiladores.
* **Narración de Voz en Off (Albornoz Joaquín & Díaz Guadalupe):**
  > *(Díaz): "Para asegurar que ninguna modificación introduzca regresiones en el compilador, desarrollamos un script de pruebas automatizadas en PowerShell. Al ejecutar `run_tests.ps1`, el sistema compila la solución y somete al ejecutable CLI a 13 pruebas de caja negra: 4 casos válidos y 9 casos con errores sintácticos y semánticos, verificando códigos de salida y mensajes exactos con un resultado de 13 pruebas aprobadas sobre 13."*  
  > *(Albornoz): "En conclusión, QuizLang demuestra cómo la teoría de gramáticas atribuidas LL(1) y herramientas como Coco/R permiten materializar compiladores robustos, confiables y con aplicaciones prácticas directas en el mundo real. Agradecemos a los profesores de la cátedra por su guía a lo largo del cuatrimestre. ¡Muchas gracias!"*

---

## Indicaciones para la Producción del Video

1. **Resolución de Grabación:** 1920x1080 (Full HD). Asegurar zoom del 125% o fuente de 14pt en el editor de código para óptima legibilidad en video.
2. **Edición de Audio:** Normalizar volumen de las voces a -14 LUFS; aplicar filtro de reducción de ruido de fondo.
3. **Puntero del Mouse:** Habilitar efecto de resaltado circular suave en los clics del mouse para guiar la atención del espectador durante la interacción con la GUI y la navegación web.
