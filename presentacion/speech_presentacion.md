# Speech de la Presentación Oral: Evaluación Parcial 2 (Avances)

**Asignatura:** Compiladores 2026  
**Proyecto:** QuizLang – Compilador de Exámenes Interactivos  
**Integrantes:**
* Albornoz Joaquín
* Díaz Guadalupe
* Gallardo Mateo
* Reinoso Abel  
**Duración Total Estimada:** 10 a 12 minutos (exposición + demo en terminal)  
**Modalidad de la Demo:** Exclusivamente por línea de comandos (CLI + PowerShell + navegador). La interfaz gráfica WinForms se encuentra en etapa de pulido para la presentación final.

---

## Resumen del Reparto de Expositores

| Diapositiva | Título | Expositor | Tiempo Estimado |
| :---: | :--- | :--- | :---: |
| **Slide 1** | Portada e Integrantes | **Albornoz Joaquín** | 1:00 min |
| **Slide 2** | Propuesta del Caso de Estudio y Alcance | **Albornoz Joaquín** | 1:30 min |
| **Slide 3** | Avances en Diseño: Gramática ATG y Parsing LL(1) con Coco/R | **Díaz Guadalupe** | 1:45 min |
| **Slide 4** | Avances en Desarrollo: Semántica y Generación de Código Web | **Díaz Guadalupe** | 1:45 min |
| **Slide 5** | Ejecución, Batería de Pruebas y Demo en Terminal | **Gallardo Mateo** | 3:30 min *(incluye demo)* |
| **Slide 6** | Metodología con IA Generativa y Auditoría Crítica | **Reinoso Abel** | 1:30 min |
| **Slide 7** | Estado de Entregables y Próximos Pasos (Hacia la Final) | **Reinoso Abel** | 1:15 min |
| **Slide 8** | Conclusiones y Espacio de Preguntas | **Reinoso Abel** *(Cierre grupal)* | 0:45 min |

---

## Detalle Diapositiva por Diapositiva

---

### Diapositiva 1: Portada e Integrantes
* **Expositor:** **Albornoz Joaquín**
* **Tiempo estimado:** 1:00 min
* **Apoyo visual en slide:** Título del proyecto "QuizLang", subtítulo "Evaluación Parcial 2: Avances del Trabajo Final (Parte 2)", materia Compiladores 2026, fecha Noviembre 2026, recuadro placeholder para la fotografía grupal, nómina de los 4 integrantes.

#### Speech:
> "Buenas tardes a los profesores y a todos los presentes.  
> Somos el grupo integrado por Guadalupe Díaz, Mateo Gallardo, Abel Reinoso y quien les habla, Joaquín Albornoz.  
> Hoy venimos a presentarles los avances de la Parte 2 de nuestro Trabajo Final de Compiladores: el desarrollo integral de **QuizLang**, un compilador para un lenguaje de dominio específico orientado a la creación de exámenes y evaluaciones interactivas.  
> 
> En esta presentación queremos mostrarles cómo formalizamos la gramática del lenguaje utilizando Coco/R sobre .NET 10 en C#, cómo garantizamos la propiedad LL(1) sin ningún tipo de conflictos, la arquitectura de nuestro verificador semántico y generador de código, y los resultados concretos de nuestra batería de pruebas automatizada, que cuenta con trece casos de prueba con un cien por ciento de éxito.  
> 
> Para este parcial hemos preparado una demostración completa en vivo a través de la terminal, donde van a poder observar la regeneración del compilador con Coco, la emisión de una página web autocontenida y la detección precisa de errores léxicos, sintácticos y semánticos. Nuestra interfaz gráfica en Windows Forms ya se encuentra implementada y está en etapa de pulido visual para la entrega final del diez de noviembre.  
> 
> Ahora vamos a comenzar repasando la motivación del caso de estudio y el alcance definido para el lenguaje."

*(Transición fluida a Slide 2 a cargo del mismo expositor)*

---

### Diapositiva 2: Propuesta del Caso de Estudio y Alcance
* **Expositor:** **Albornoz Joaquín**
* **Tiempo estimado:** 1:30 min
* **Apoyo visual en slide:** Objetivos de la Parte 2, justificación de QuizLang como DSL declarativo, tabla sintética de elementos léxicos y palabras clave del `.atg`, y un fragmento de código QuizLang real extraído de la suite de pruebas.

#### Speech:
> "Cuando nos planteamos el caso de estudio para este trabajo, buscamos resolver un problema real y recurrente en el ámbito educativo: la sobrecarga que significa armar cuestionarios digitales en plataformas pesadas como Moodle o Google Forms, que requieren servidores dedicados, conectividad permanente o configuraciones engorrosas.  
> 
> Así nació **QuizLang**, un DSL pequeño, conciso y estrictamente **declarativo**. Como pueden ver en el ejemplo en pantalla, un examen se define con una sintaxis limpia: arranca con la palabra clave `EXAMEN` seguida de un título entre comillas, una cláusula opcional de `PUNTAJE_MINIMO` y una serie de preguntas de tres tipos: opción múltiple con `PREGUNTA_MULTIPLE`, preguntas de texto libre con `PREGUNTA_TEXTO`, y de verdadero o falso con `PREGUNTA_VF`.  
> 
> Queremos resaltar un aspecto teórico fundamental: **en QuizLang no existen estructuras de control tradicionales**, como bucles `while`, condicionales `if` ni saltos. Esto no es una omisión, sino una decisión consciente de diseño. Al tratarse de un lenguaje declarativo para describir especificaciones de evaluaciones, el flujo no depende de un estado mutable en tiempo de ejecución, sino de una estructura jerárquica bien definida.  
> 
> Respecto al alcance léxico, soportamos cadenas con caracteres de escape, números enteros para puntajes, identificadores de opciones representados por letras individuales, y comentarios tanto de una línea con doble barra como de bloque con barra-asterisco.  
> 
> Para ver en detalle cómo convertimos esta especificación en un compilador funcional, le paso la palabra a mi compañera Guadalupe Díaz."

*(Transición hacia Guadalupe Díaz)*

---

### Diapositiva 3: Avances en Diseño: Gramática ATG y Parsing LL(1) con Coco/R
* **Expositor:** **Díaz Guadalupe**
* **Tiempo estimado:** 1:45 min
* **Apoyo visual en slide:** Diagrama del pipeline de Coco/R (`QuizLang.atg` + Frames $\to$ `Scanner.cs` + `Parser.cs`), fragmento real de producciones EBNF con atributos semánticos (`Opcion<out op>`), y demostración de conjuntos First/Follow disjuntos (0 conflictos LL(1)).

#### Speech:
> "Muchas gracias, Joaquín. Buenas tardes.  
> Entrando en los avances de diseño del compilador, la columna vertebral de nuestro desarrollo es la gramática atribuida especificada en el archivo `QuizLang.atg`, procesada mediante la herramienta **Coco/R para C#**.  
> 
> Como estudiamos en la teoría de la cátedra, Coco/R implementa un analizador sintáctico descendente recursivo guiado por una gramática $LL(1)$. Esto impone una exigencia matemática muy clara: para cada regla de la gramática donde existan alternativas o repeticiones, los conjuntos de selección—es decir, los símbolos directores calculados a partir de los conjuntos *First* y *Follow*—deben ser estrictamente disjuntos con exactamente un token de anticipación ($k=1$).  
> 
> En QuizLang diseñamos la sintaxis cuidadosamente para satisfacer esta condición. Por ejemplo, en la producción `Pregunta`, cada alternativa arranca con un token reservado inequívoco: `PREGUNTA_MULTIPLE`, `PREGUNTA_TEXTO` y `PREGUNTA_VF`. La intersección de sus conjuntos *First* es vacía. Lo mismo ocurre en las opciones de respuesta, donde el token `letra` define el inicio de una opción y la llave de cierre `}` marca el fin del bloque, sin necesidad de recurrir a mecanismos de retroceso ni a directivas complejas de desambiguación.  
> 
> Al ejecutar `Coco.exe` sobre nuestra gramática, la herramienta valida el autómata determinista y emite **cero errores detectados y cero conflictos LL(1)**. Además, utilizamos atributos semánticos en C#, como el parámetro sintetizado `out Opcion op`, que nos permite capturar y propagar los datos del árbol sintáctico directamente durante el análisis en una sola pasada.  
> 
> Ahora pasamos a la siguiente diapositiva para ver qué hacemos con esos datos en la etapa semántica y en la generación de código."

*(Transición fluida a Slide 4 a cargo de la misma expositora)*

---

### Diapositiva 4: Avances en Desarrollo: Semántica y Generación de Código Web
* **Expositor:** **Díaz Guadalupe**
* **Tiempo estimado:** 1:45 min
* **Apoyo visual en slide:** Reglas de validación contextual de `SymbolTable.cs`, diagrama de clases del modelo AST (`Examen`, `Pregunta`, `Opcion`), y pipeline de emisión de `CodeGen.cs` a HTML5 + CSS3 + JS autocontenido con prevención XSS.

#### Speech:
> "Una vez superado el análisis sintáctico, entra en juego la verificación semántica contextual, implementada en nuestra clase `SymbolTable.cs`. Como sabemos, una gramática libre de contexto no puede verificar reglas de negocio o coherencia semántica. Nosotros implementamos cinco validaciones fundamentales:  
> 
> Primero, controlamos que no existan **enunciados de preguntas duplicados** dentro del examen, normalizando el texto con conjuntos hash.  
> Segundo, en las preguntas de opción múltiple, exigimos que exista **exactamente una opción marcada como `[CORRECTA]`**; si no hay ninguna o si se marcan dos, se dispara un error semántico con línea y columna exactas.  
> Tercero, chequeamos que no haya **letras de opciones repetidas** y advertimos si la correlatividad alfabética se rompe.  
> Cuarto, validamos que el examen **contenga al menos una pregunta**.  
> Y quinto, en las reglas globales, verificamos que el **`PUNTAJE_MINIMO` no supere el total de puntos acumulados** de todas las preguntas del examen.  
> 
> Si todas estas comprobaciones pasan sin errores, el control pasa a nuestro generador de código, `CodeGen.cs`. Este componente recorre el modelo AST y traduce la evaluación a una página web única y autocontenida: un archivo `.html` que incluye estilos CSS3 responsivos y la lógica de validación en JavaScript cliente puro. Sanitizamos cada cadena mediante `WebUtility.HtmlEncode` para prevenir cualquier vulnerabilidad de inyección de código. El estudiante o docente puede abrir este archivo en cualquier navegador, sin conexión y sin instalar nada, y obtener corrección instantánea y puntaje acumulado.  
> 
> Para mostrarles cómo responde el compilador en la práctica y ver la demo en vivo, le paso la palabra a Mateo Gallardo."

*(Transición hacia Mateo Gallardo)*

---

### Diapositiva 5: Ejecución, Batería de Pruebas y Demo en Terminal
* **Expositor:** **Gallardo Mateo**
* **Tiempo estimado:** 3:30 min *(exposición teórica de pruebas + demo interactiva)*
* **Apoyo visual en slide:** Tabla con los 13 casos de prueba (4 válidos y 9 con errores), ejemplos textuales de salidas de error (`linea X col Y`), captura del script `run_tests.ps1` con resultado 13/13 PASS, e indicador de 'Demostración en Vivo'.

#### Speech:
> "Buenas tardes a todos. Gracias, Guada.  
> Para garantizar la robustez del compilador construimos una suite integral de trece casos de prueba automatizados, que cubren tanto caminos felices como todas las categorías de falla previstas en el diseño:  
> Contamos con cuatro casos válidos: un examen básico, uno con comentarios, uno exhaustivo y un examen completo sobre la materia Compiladores 2026.  
> Y diseñamos nueve casos con errores provocados intencionalmente: errores léxicos por caracteres no reconocidos, errores sintácticos por falta de llaves o de títulos, y toda la gama de errores semánticos que recién explicó Guadalupe.  
> 
> Cada prueba cuenta con su archivo `.expected`, y nuestro script de automatización `run_tests.ps1` compila la solución en .NET 10 y contrasta las salidas y códigos de retorno, alcanzando un resultado exitoso de **trece de trece pruebas pasadas**.  
> 
> Ahora quiero invitar a los profesores a mirar la terminal para realizar la **demostración en vivo** de estos componentes."

#### [Comienza la Demostración en la Terminal - guiada por Mateo]:
> *(Paso 1: Regeneración con Coco/R)*  
> "En primer lugar, voy a ejecutar nuestro script `tools\generar.ps1`. Fíjense cómo invoca a `Coco.exe` con nuestra gramática `QuizLang.atg`. El proceso se completa en menos de un segundo, indicando '0 errors detected' y regenerando el escáner y el parser en `QuizLang.Core` sin un solo conflicto LL(1)."  
> 
> *(Paso 2: Compilación de caso válido y apertura del HTML)*  
> "A continuación, compilamos un examen válido completo con la CLI de QuizLang apuntando a `tests\compiladores2026.quiz` y especificando como salida `presentacion\demo\demo_valido.html`.  
> La consola nos responde 'Compilación exitosa'. Abrimos el archivo en el navegador y como pueden observar, tenemos el examen renderizado con un diseño limpio y moderno. Voy a responder algunas preguntas a propósito: una correcta y otra incorrecta. Le doy clic a 'Corregir Examen' y automáticamente el motor JavaScript nos pinta en verde los aciertos, en rojo los errores y nos calcula el puntaje final y si alcanzamos el mínimo de aprobación."  
> 
> *(Paso 3, 4 y 5: Pruebas de detección de errores con línea y columna)*  
> "Volvemos a la terminal para verificar la precisión del compilador frente a entradas erróneas:  
> Primero, compilo `tests\error_lexico.quiz`, que tiene un símbolo arroba inválido. La salida es inmediata: `linea 3 col 1: caracter o simbolo lexico no reconocido`.  
> Segundo, probamos `tests\error_sintactico_falta_llave.quiz`. El parser nos indica con exactitud: `linea 6 col 1: se esperaba "}"`.  
> Y tercero, probamos una violación semántica con `tests\sem_multiple_sin_correcta.quiz`: el analizador nos reporta `linea 4 col 19: La pregunta múltiple no tiene ninguna opción marcada como [CORRECTA]`.  
> 
> *(Paso 6: Corrida de la suite automatizada)*  
> "Finalmente, ejecuto el script integral `run_tests.ps1`. Compila la solución completa y ejecuta las trece pruebas una tras otra. En pantalla pueden ver los trece carteles verdes de [PASS] y el resumen final: **Resultados: 13/13 pruebas pasaron**.  
> 
> Con esto demostramos que el núcleo del compilador no solo compila sin advertencias, sino que es plenamente determinista y tolerante a fallos.  
> Ahora le paso la palabra a Abel Reinoso para comentarles cómo trabajamos con la Inteligencia Artificial y cuáles son nuestros próximos pasos."

*(Transición hacia Abel Reinoso)*

---

### Diapositiva 6: Metodología con IA Generativa y Auditoría Crítica
* **Expositor:** **Reinoso Abel**
* **Tiempo estimado:** 1:30 min
* **Apoyo visual en slide:** Arquitectura del entorno agentic Antigravity (Claude Opus supervisor + Gemini Flash subagentes), tabla de tareas asistidas, bitácora de prompts `docs/bitacora_ia.md`, y el caso de estudio de la alucinación bibliográfica detectada.

#### Speech:
> "Muchas gracias, Mateo. Buenas tardes a todos.  
> En esta segunda mitad del proyecto, como pauta pedagógica y ética fundamental, integramos herramientas de Inteligencia Artificial generativa bajo una metodología rigurosa y auditable.  
> 
> No utilizamos la IA mediante consultas aisladas en chats genéricos, sino que configuramos un entorno agentic estructurado en **Antigravity**. En este esquema, tuvimos un **Agente Supervisor (Claude Opus)** que nos ayudó a desglosar las tareas arquitectónicas y auditar la coherencia teórica, y **Subagentes Operativos (Gemini Flash)** que usamos para prototipar rápidamente clases del AST, escribir la plantilla HTML de salida y preparar los casos de prueba iniciales. Todas estas interacciones están documentadas minuciosamente en nuestra bitácora `docs/bitacora_ia.md`.  
> 
> Pero el aspecto más valioso para nuestra formación fue el ejercicio de **verificación y auditoría crítica**. La IA comete errores y puede inducir a una falsa sensación de seguridad. Vivenciamos dos ejemplos muy claros:  
> A nivel técnico, cuando la IA propuso restar caracteres directamente en la sección `TOKENS`, Coco/R falló de inmediato; tuvimos que acudir al manual original de Mössenböck para comprender que las operaciones de conjuntos en Cocol/R pertenecen exclusivamente a la sección `CHARACTERS`.  
> Y a nivel conceptual y bibliográfico, un subagente nos sugirió una cita de un supuesto libro de compiladores de la editorial Hanser del año 2018. Al realizar la búsqueda académica manual comprobamos que el libro no existía en ningún catálogo: **era una referencia alucinada**. Siguiendo nuestro protocolo ético, mantuvimos la señalización de alerta y la descartamos de las fuentes válidas.  
> 
> La conclusión que nos deja esta experiencia es que la IA es un acelerador técnico potente, pero la validez teórica y la responsabilidad final descansan siempre en el criterio humano de los estudiantes."

*(Transición fluida a Slide 7 a cargo del mismo expositor)*

---

### Diapositiva 7: Estado de Entregables y Próximos Pasos (Hacia la Final)
* **Expositor:** **Reinoso Abel**
* **Tiempo estimado:** 1:15 min
* **Apoyo visual en slide:** Matriz de estado de los entregables del parcial (Código Core: Completo, Documento de Diseño: Completo, Manual de Usuario: Completo, GUI: En etapa de pulido, Informe: Borrador en revisión, Video/Slides: Pendiente) y diagrama de hitos hacia la entrega final del 10/11/2026.

#### Speech:
> "Llegando a la situación actual del proyecto, en esta tabla resumimos con total transparencia el estado de avance de cada entregable:  
> El código fuente del compilador y la CLI están al cien por ciento terminados y probados.  
> El documento formal de diseño del compilador y el manual de usuario están completamente redactados.  
> El informe final integral está en fase de borrador consolidado y revisión de estilo.  
> Y nuestra interfaz gráfica de usuario en Windows Forms, como adelantamos, está desarrollada y se encuentra actualmente en **etapa de pulido visual y de integración**, lista para ser exhibida en la entrega final.  
> 
> Nuestro cronograma de trabajo de aquí al diez de noviembre contempla cinco mejoras clave:  
> 1. Enriquecer la interfaz gráfica con un panel interactivo de visualización de fases: inspección de la lista de tokens emitidos por el escáner, visualización del árbol sintáctico y estado de la tabla de símbolos.  
> 2. Implementar una demostración en vivo con código QR proyectado en pantalla, para que los docentes y compañeros puedan escanearlo y responder el examen compilado directamente desde sus teléfonos celulares.  
> 3. Extender opcionalmente el lenguaje con preguntas numéricas y emparejamiento.  
> 4. Incorporar recuperación de errores avanzada mediante las directivas `SYNC` y `WEAK` que provee Coco/R.  
> 5. Y realizar el cierre definitivo de la monografía y la grabación del video institucional."

*(Transición fluida a Slide 8 a cargo del mismo expositor)*

---

### Diapositiva 8: Conclusiones y Espacio de Preguntas
* **Expositor:** **Reinoso Abel** *(con intervención de los 4 integrantes)*
* **Tiempo estimado:** 0:45 min
* **Apoyo visual en slide:** Resumen de logros alcanzados (0 errores, LL(1), 13/13 pruebas, HTML responsivo), enlace/QR al repositorio del proyecto y mensaje de apertura a preguntas del tribunal.

#### Speech:
> "Para concluir, estamos muy conformes con el grado de avance alcanzado en esta Parte 2. Logramos transformar una especificación abstracta en un compilador sólido, eficiente y matemáticamente riguroso, respaldado por la teoría de autómatas y gramáticas que aprendimos en la materia.  
> 
> Agradecemos a los docentes por su atención y quedamos los cuatro a entera disposición para responder a sus preguntas y recibir sus sugerencias y devoluciones. Muchas gracias."

---
