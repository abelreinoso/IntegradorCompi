# Guion de la Defensa Oral Final (v2 Mejorada): Compilador QuizLang

**Materia:** Compiladores 2026  
**Fecha de Defensa:** 10 de noviembre de 2026 (tentativa)  
**Integrantes:**
- Albornoz, Joaquín
- Díaz, Guadalupe
- Gallardo, Mateo
- Reinoso, Abel

**Enlaces de la Presentación en Canva (Versión 2 Mejorada):**
- **Enlace de Edición:** [https://www.canva.com/d/6DCld9yFdAqYt74](https://www.canva.com/d/6DCld9yFdAqYt74)
- **Enlace de Visualización:** [https://www.canva.com/d/Y3U9CwD6uYhmRES](https://www.canva.com/d/Y3U9CwD6uYhmRES)

**Duración Total Estimada:** 18 a 19 minutos (Presentación teórica: 15 min 30 s + Demostración práctica interactiva: 3 min).

---

## Tabla Resumen de Exposición

| N° | Diapositiva en Canva | Expositor(es) | Duración | Eje Principal |
| :-: | :--- | :--- | :-: | :--- |
| **1** | Portada y Presentación del Proyecto | **Albornoz Joaquín** | 1:00 min | Encuadre académico, equipo y objetivos |
| **2** | Introducción a Coco/R y Filosofía de Diseño | **Díaz Guadalupe** | 1:00 min | Mössenböck, descenso recursivo $LL(1)$ vs $LALR(1)$ |
| **3** | Arquitectura Interna de Coco/R y Caso Taste | **Gallardo Mateo** | 1:15 min | Pipeline de generación, frames y backpatching |
| **4** | Motivación y Dominio de QuizLang | **Reinoso Abel** | 1:00 min | Necesidad docente, DSL y artefacto web cliente |
| **5** | Especificación Léxica y Gramática EBNF | **Albornoz Joaquín** | 1:15 min | Archivo `QuizLang.atg`, alfabeto, tokens y 7 producciones |
| **6** | Demostración de la Propiedad LL(1) | **Díaz Guadalupe** | 1:15 min | Conjuntos First/Follow disjuntos y validación Coco/R |
| **7** | Análisis Semántico y Tabla de Símbolos | **Gallardo Mateo** | 1:15 min | 6 reglas de error semántico + 1 warning en `SymbolTable.cs` |
| **8** | Generación de Código Web Interactivo | **Reinoso Abel** | 1:00 min | `CodeGen.cs`, `HtmlEncode` y evaluación con botón `[ Corregir ]` |
| **9** | Arquitectura del Software e Interfaz Gráfica | **Albornoz Joaquín** | 1:15 min | Modularización en .NET 10 (`Core`, `Cli`, `Gui`) + Coco/R |
| **10** | Verificación Experimental y Suite de Pruebas | **Díaz Guadalupe** | 1:15 min | Desglose de las 13 pruebas (4/3/6, 13/13 PASS) y `run_tests.ps1` |
| **11** | Metodología con IA: Enfoque Agentic y Bitácora | **Gallardo Mateo** | 1:15 min | Supervisor Claude Opus, subagentes Gemini Flash |
| **12** | Depuración Real y Resolución de Conflictos | **Reinoso Abel** | 1:15 min | Lecciones técnicas en Cocol/R y plantillas de C# |
| **13** | Reflexión Crítica: Alucinaciones y Verificación | **Albornoz Joaquín & Díaz Guadalupe** | 1:30 min | Cita alucinada, sesgo de completitud y criterio humano |
| **14** | Conclusiones y Transición a la Demo | **Gallardo Mateo & Reinoso Abel** | 1:00 min | Síntesis de logros y trabajo futuro |
| **Demo** | Demostración en Vivo con la GUI | **Gallardo Mateo & Reinoso Abel** | 3:00 min | Flujo interactivo real con `QuizLang.Gui` y navegador |

---

## Discurso Completo Diapositiva por Diapositiva

### Diapositiva 1: Portada y Presentación del Proyecto
* **Título en Canva:** *DEFENSA FINAL · COMPILADORES 2026 / QuizLang: Un DSL para crear exámenes autocorregibles con Coco/R*
* **Expositor:** **Albornoz Joaquín**
* **Tiempo Estimado:** 1:00 min
* **Speech:**
  > *"Muy buenas tardes a los profesores de la cátedra de Compiladores.  
  > Somos el grupo integrado por Guadalupe Díaz, Mateo Gallardo, Abel Reinoso y quien les habla, Joaquín Albornoz. Hoy nos encontramos aquí para defender nuestro Trabajo Final integrador: **QuizLang**, un compilador de lenguaje de dominio específico desarrollado con la herramienta **Coco/R** sobre la plataforma moderna **.NET 10**.  
  > 
  > El propósito central de nuestra investigación abarcó tres pilares: primero, profundizar en la arquitectura y principios formales del generador de compiladores Coco/R; segundo, diseñar e implementar un lenguaje declarativo orientado a la creación ágil de exámenes técnicos que compilan a páginas web interactivas y autocorregibles; y tercero, evaluar críticamente un flujo de ingeniería asistido por Inteligencia Artificial bajo una arquitectura agentic supervisada.  
  > 
  > A continuación, Guadalupe comenzará exponiendo el marco teórico y la filosofía de diseño detrás de Coco/R."*

---

### Diapositiva 2: Introducción a Coco/R y Filosofía de Diseño
* **Título en Canva:** *Coco/R: gramática que se vuelve código*
* **Expositor:** **Díaz Guadalupe**
* **Tiempo Estimado:** 1:00 min
* **Speech:**
  > *(Transición recibida)*: *"Muchas gracias, Joaquín.  
  > Para comprender las decisiones tomadas en QuizLang, debemos situarnos en la filosofía de **Coco/R**, herramienta desarrollada originalmente por Hanspeter Mössenböck en la ETH Zürich y la Universidad de Linz, y luego difundida globalmente por Pat Terry.  
  > 
  > A diferencia de los generadores ascendentes tradicionales como Lex y Yacc, o Flex y Bison, que sintetizan autómatas por desplazamiento y reducción basados en tablas LALR(1), Coco/R apuesta por el **análisis sintáctico descendente recursivo** sobre gramáticas atribuidas $LL(1)$.  
  > 
  > Como se observa en el diagrama, esta elección ofrece enormes ventajas prácticas: el código emitido en C# es completamente transparente y legible, estableciendo una correspondencia directa de **un símbolo no terminal en el archivo `.atg` a un método C# en `Parser.cs`**. Esto facilita una depuración paso a paso intuitiva y garantiza una complejidad temporal lineal $\mathcal{O}(N)$ con $k=1$ símbolo de anticipación.  
  > Además, el dialecto Cocol/R unifica en un solo archivo el `Scanner DFA` (`CHARACTERS` y `TOKENS`), el `Parser LL(1)` y las `Acciones Semánticas` escritas directamente en C# entre delimitadores `(. .)`.  
  > 
  > Ahora le paso la palabra a Mateo para analizar el pipeline interno de Coco/R y las enseñanzas del caso Taste."*

---

### Diapositiva 3: Arquitectura Interna de Coco/R y Caso Taste
* **Título en Canva:** *Del archivo .atg al compilador*
* **Expositor:** **Gallardo Mateo**
* **Tiempo Estimado:** 1:15 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Guada.  
  > El funcionamiento interno de Coco/R se estructura como una cadena de procesamiento de cinco etapas: toma nuestra especificación `.ATG` junto a dos plantillas maestras, `Scanner.frame` y `Parser.frame`. El generador valida la gramática, calcula los conjuntos directores First y Follow, y emite los archivos `Scanner.cs` (con el autómata finito determinista) y `Parser.cs` (con los métodos de descenso recursivo $LL(1)$).  
  > 
  > Como base de estudio de la Parte 1, analizamos exhaustivamente el compilador pedagógico **Taste**, provisto por Mössenböck. Taste es un lenguaje imperativo que compila en **una sola pasada** (*one-pass*), sin construir un árbol sintáctico en memoria, gestionando alcances y símbolos mediante nodos `Obj` y `Scope` en su tabla de símbolos.  
  > Para emitir instrucciones hacia su máquina virtual de pila (`TasteVM`) y resolver los saltos condicionales e incondicionales (`FJMP` y `JMP`) hacia direcciones futuras que aún no se conocen, Taste aplica la técnica de **backpatching** mediante la función `Patch(adr, val)`.  
  > 
  > Le cedo el turno a Abel, quien les explicará la concepción y dominio de QuizLang."*

---

### Diapositiva 4: Motivación y Dominio de QuizLang
* **Título en Canva:** *QuizLang reduce fricción*
* **Expositor:** **Reinoso Abel**
* **Tiempo Estimado:** 1:00 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Mateo.  
  > La motivación detrás de QuizLang surge de una problemática pedagógica cotidiana: los docentes necesitan elaborar cuestionarios técnicos interactivos, pero suelen depender de plataformas pesadas como Moodle o Google Forms, que exigen conectividad constante y servidores dedicados, o bien de maquetar a mano HTML y JavaScript.  
  > 
  > Diseñamos **QuizLang** como un DSL declarativo y conciso. Como vemos en los cuatro bloques del esquema, el lenguaje contempla:  
  > 1. `PREGUNTA_MULTIPLE`, con opciones identificadas por letras (`a)`, `b)`, `c)`) y la marca `[CORRECTA]`.  
  > 2. `PREGUNTA_VF`, asignando directamente los valores `VERDADERO` o `FALSO`.  
  > 3. `PREGUNTA_TEXTO`, con una `RESPUESTA` libre que se evalúa de forma normalizada.  
  > 4. Configuración de `PUNTAJE_MINIMO` y `PUNTOS` por pregunta, compilando todo a un único archivo HTML5 + JS que funciona 100% offline.  
  > 
  > A continuación, Joaquín expondrá la especificación léxica y las 7 producciones EBNF de nuestro lenguaje."*

---

### Diapositiva 5: Especificación Léxica y Gramática EBNF
* **Título en Canva:** *Especificar antes de implementar*
* **Expositor:** **Albornoz Joaquín**
* **Tiempo Estimado:** 1:15 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Abel.  
  > En la diapositiva vemos las definiciones reales extraídas de nuestro archivo `QuizLang.atg`.  
  > En la capa léxica (`TOKENS`), definimos tres categorías fundamentales a partir de los conjuntos de caracteres: `string`, que reconoce cadenas entre comillas dobles soportando secuencias de escape; `number`, formado por uno o más dígitos; y `letra`, para rotular cada opción. Además, configuramos el descarte de espacios y comentarios de línea (`//`) y de bloque (`/* */`).  
  > 
  > Debajo están las **siete producciones EBNF completas** del lenguaje:  
  > * El axioma `QuizLang` comienza con la palabra clave `"EXAMEN"` y su título, seguido opcionalmente por `Config` (`"PUNTAJE_MINIMO" number`) y una repetición de `{ Pregunta }`.  
  > * `Pregunta` se bifurca en `Multiple`, `Texto` o `VerdaderoFalso`.  
  > * `Multiple` define su enunciado, puntos opcionales y el bloque entre llaves con una o más `Opcion`, donde cada `Opcion` lleva `letra ")" string` y el marcador opcional `"[CORRECTA]"`.  
  > * Finalmente, `Texto` permite declarar la `"RESPUESTA"` esperada y `VerdaderoFalso` exige el signo `"="` seguido de `"VERDADERO"` o `"FALSO"`.  
  > 
  > Le devuelvo la palabra a Guadalupe para demostrar la propiedad matemática LL(1)."*

---

### Diapositiva 6: Demostración de la Propiedad LL(1)
* **Título en Canva:** *LL(1), demostrado*
* **Expositor:** **Díaz Guadalupe**
* **Tiempo Estimado:** 1:15 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Joaquín.  
  > Un requisito formal ineludible en Coco/R es que la gramática cumpla estrictamente la condición $LL(1)$. Esto significa que, ante cualquier alternativa o construcción opcional, el analizador debe decidir el camino correcto mirando únicamente un token de anticipación (*lookahead*).  
  > 
  > En la tabla demostramos cómo se cumplen las condiciones de Wirth:  
  > Primero, en la producción `Pregunta`, los conjuntos `FIRST` de sus tres ramas son los terminales `"PREGUNTA_MULTIPLE"`, `"PREGUNTA_TEXTO"` y `"PREGUNTA_VF"`: su intersección es vacía, por lo que son mutuamente disjuntos.  
  > Segundo, en la cláusula opcional `[ Config ]`, su `FIRST` es `{"PUNTAJE_MINIMO"}`, cuya intersección con `FOLLOW(Config)` (los inicios de pregunta o `EOF`) es vacía.  
  > Tercero, tanto en la repetición de `Opcion` como en el marcador opcional `[CORRECTA]`, el conjunto `FIRST` (`{letra}` y `{"[CORRECTA]"}`) es disjunto del `FOLLOW` (`"}"`).  
  > 
  > Al ejecutar `Coco.exe` sobre `QuizLang.atg`, la herramienta confirmó formalmente **0 errors detected** y cero conflictos $LL(1)$.  
  > 
  > Ahora Mateo detallará el análisis semántico y la tabla de símbolos."*

---

### Diapositiva 7: Análisis Semántico y Tabla de Símbolos
* **Título en Canva:** *La sintaxis no alcanza*
* **Expositor:** **Gallardo Mateo**
* **Tiempo Estimado:** 1:15 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Guadalupe.  
  > Un archivo puede ser sintácticamente válido y aun así ser incoherente como examen. Por eso, las acciones semánticas del `.ATG` construyen un AST tipado (`Examen`, `Pregunta`, `Opcion`) que es validado por `SymbolTable.cs`.  
  > 
  > Nuestra tabla de símbolos verifica exactamente **seis reglas de error semántico** y **una advertencia**:  
  > 1. **Examen no vacío:** exige que el examen tenga al menos una pregunta.  
  > 2. **Enunciado único:** mediante un `HashSet` insensible a mayúsculas y minúsculas, rechaza preguntas duplicadas.  
  > 3. **Sin `[CORRECTA]`:** reporta error si una pregunta múltiple tiene cero opciones marcadas como correctas.  
  > 4. **Más de una `[CORRECTA]`:** reporta error si una pregunta múltiple marca dos o más opciones como correctas.  
  > 5. **Letra repetida:** impide duplicar el identificador de opción dentro de una misma pregunta (por ejemplo, dos opciones `'a'`).  
  > 6. **Puntaje mínimo coherente:** verifica que `PUNTAJE_MINIMO` sea menor o igual al puntaje total del examen.  
  > Además, si las letras de opción no siguen el orden alfabético consecutivo (`a, c`), emite un **Warning** sin abortar la compilación. Todos los diagnósticos informan línea y columna exactas.  
  > 
  > Abel explicará a continuación la generación del código web interactivo."*

---

### Diapositiva 8: Generación de Código Web Interactivo
* **Título en Canva:** *Del modelo a una experiencia interactiva*
* **Expositor:** **Reinoso Abel**
* **Tiempo Estimado:** 1:00 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Mateo.  
  > Una vez validado el modelo semántico, el módulo `CodeGen.cs` sintetiza un único documento HTML5 autocontenido con CSS3 y JavaScript embebidos.  
  > 
  > Aplicamos dos principios clave: **cero dependencias externas**, para que el examen funcione sin servidor ni internet, y **sanitización de seguridad** mediante `WebUtility.HtmlEncode` en cada texto, previniendo inyecciones XSS.  
  > 
  > En el navegador, cuando el alumno responde y presiona el botón **`Corregir`**, el motor JavaScript evalúa las respuestas en el acto: resalta en verde los aciertos, marca en rojo los errores indicando la opción correcta, normaliza las respuestas de texto libre ignorando mayúsculas y espacios extremos, y muestra el puntaje obtenido junto al cartel de Aprobado o Desaprobado.  
  > 
  > Ahora Joaquín presentará la arquitectura modular en .NET 10 y la interfaz gráfica."*

---

### Diapositiva 9: Arquitectura del Software e Interfaz Gráfica
* **Título en Canva:** *Arquitectura modular en .NET 10*
* **Expositor:** **Albornoz Joaquín**
* **Tiempo Estimado:** 1:15 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Abel.  
  > Organizamos la solución `QuizLang.sln` en .NET 10 separando el motor de compilación de las interfaces de usuario en tres proyectos, más Coco/R como generador externo:  
  > 1. **`QuizLang.Core` (`.dll`):** es la biblioteca central que encapsula el `Scanner` y `Parser` generados por Coco/R, el AST, `SymbolTable`, `CodeGen` y la fachada `Compiler.cs`.  
  > 2. **`QuizLang.Cli` (`.exe`):** es la interfaz de consola para compilación por lotes y ejecución de scripts de prueba.  
  > 3. **`QuizLang.Gui` (WinForms):** es el entorno gráfico interactivo de escritorio.  
  > 
  > `QuizLang.Gui` cuenta con un editor con numeración de líneas dibujada con GDI+, un selector con 6 ejemplos precargados, visor web integrado a la derecha y una grilla inferior de errores que, al hacer **doble clic**, salta directamente a la línea y columna del fallo en el editor. Toda la solución compila con **0 errores y 0 advertencias**.  
  > 
  > Le paso la palabra a Guadalupe para ver la verificación experimental."*

---

### Diapositiva 10: Verificación Experimental y Suite de Pruebas
* **Título en Canva:** *Verificar diagnósticos, no solo resultados*
* **Expositor:** **Díaz Guadalupe**
* **Tiempo Estimado:** 1:15 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Joaquín.  
  > Para garantizar que cada fase del compilador responda como corresponde, construimos una suite automatizada ejecutada por el script `run_tests.ps1`, que compila la solución y corre `QuizLang.Cli` sobre **13 casos de prueba**, comparando tanto el código de salida (`0` o `1`) como el texto diagnóstico exacto contra archivos `.expected`.  
  > 
  > Como se detalla en la diapositiva, la batería se divide en tres grupos:  
  > * **4 pruebas válidas (`Exit 0`):** `basico`, `completo`, `comentarios` y `compiladores2026`, que generan el HTML sin errores.  
  > * **3 pruebas léxico/sintácticas (`Exit 1`):** `error_lexico` (carácter `@`), `falta_llave` (omisión de `}`) y `falta_titulo` (omisión del string tras `EXAMEN`).  
  > * **6 pruebas semánticas (`Exit 1`):** una por cada regla de `SymbolTable.cs`: `examen_sin_preguntas`, `enunciado_duplicado`, `multiple_sin_correcta`, `multiple_varias_correctas`, `opcion_letra_repetida` y `puntaje_minimo_excesivo`.  
  > 
  > El resultado es **13 de 13 pruebas en estado PASS**, con 100% de coincidencia en línea, columna y mensaje.  
  > 
  > Mateo comentará ahora nuestra metodología de trabajo con Inteligencia Artificial."*

---

### Diapositiva 11: Metodología con IA: Enfoque Agentic y Bitácora
* **Título en Canva:** *IA con roles, evidencia y supervisión*
* **Expositor:** **Gallardo Mateo**
* **Tiempo Estimado:** 1:15 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Guada.  
  > Siguiendo las pautas de la cátedra, implementamos un flujo de trabajo *agentic* en la plataforma **Antigravity** con separación de roles:  
  > Utilizamos **Claude Opus** como agente supervisor (encargado de planificar, revisar que la gramática sea $LL(1)$ y auditar los resultados) y subagentes **Gemini Flash** para las tareas operativas de investigación, codificación y documentación.  
  > 
  > Las decisiones de diseño fueron siempre del equipo humano: nosotros elegimos la temática de QuizLang entre las 5 opciones del brainstorming inicial, definimos la arquitectura en C# y .NET 10, y auditamos cada entregable. Todo este proceso quedó registrado cronológicamente en nuestra **Bitácora de IA**, siguiendo el ciclo `prompt → artefacto → prueba → decisión humana`.  
  > 
  > Abel presentará los errores técnicos reales que depuramos."*

---

### Diapositiva 12: Depuración Real y Resolución de Conflictos
* **Título en Canva:** *Los errores reales cruzan capas*
* **Expositor:** **Reinoso Abel**
* **Tiempo Estimado:** 1:15 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Mateo.  
  > En la diapositiva resumimos cuatro errores reales de integración entre Coco/R y C# que debimos diagnosticar y corregir:  
  > 1. **`invalid TokenFactor` en `TOKENS`:** el primer borrador intentaba restar conjuntos de caracteres dentro de `TOKENS`; como Coco/R sólo permite diferencias de conjuntos en `CHARACTERS`, definimos allí `stringChar`.  
  > 2. **`missing production` (`Quiz` vs `QuizLang`):** Coco/R exige que el axioma tenga exactamente el mismo nombre que el encabezado `COMPILER QuizLang`.  
  > 3. **Directivas `using` dentro de `Parser`:** al colocar los `using` después de `COMPILER`, `Parser.frame` los insertaba adentro de la clase; los reubicamos en el preámbulo, antes de `COMPILER`.  
  > 4. **Carácter ilegal reportado como `'??? expected'`:** ajustamos `Parser.frame` y el manejo léxico para priorizar el mensaje claro en español: `caracter o simbolo lexico no reconocido`.  
  > 
  > Ahora Joaquín y Guadalupe compartirán nuestra reflexión crítica sobre las alucinaciones de la IA."*

---

### Diapositiva 13: Reflexión Crítica: Alucinaciones y Verificación Humana
* **Título en Canva:** *La IA propone; la evidencia decide*
* **Expositores:** **Albornoz Joaquín & Díaz Guadalupe**
* **Tiempo Estimado:** 1:30 min
* **Speech:**
  > *(Transición recibida)*:  
  > **(Albornoz Joaquín):**  
  > *"Uno de los hallazgos más valiosos del trabajo fue detectar una alucinación bibliográfica técnicamente verosímil.  
  > Al redactar la Parte 1, el subagente incluyó en las referencias un libro atribuido a Hanspeter Mössenböck titulado 'Compilerbau', supuestamente editado por Hanser Verlag en 2018.  
  > Durante la revisión del supervisor comprobamos que ese libro no existe ni en la editorial Hanser ni en las publicaciones del autor en la JKU Linz. Por transparencia académica, decidimos dejarlo documentado con la marca `[VERIFICAR]` y exponerlo como caso de estudio en nuestras reflexiones."*  
  > 
  > **(Díaz Guadalupe):**  
  > *"También observamos el fenómeno de la 'falsa certeza': el subagente afirmó haber 'verificado al 100%' las fuentes pese a haber introducido esa cita sintética.  
  > Esto reafirma que la IA es un excelente acelerador de desarrollo, pero la verificación empírica en consola, el dominio teórico de la gramática $LL(1)$ y la responsabilidad final son indelegablemente humanos.  
  > 
  > Ahora Mateo y Abel darán las conclusiones y pasarán a la demostración en vivo."*

---

### Diapositiva 14: Conclusiones y Demostración en Vivo
* **Título en Canva:** *QuizLang compila conocimiento*
* **Expositores:** **Gallardo Mateo & Reinoso Abel**
* **Tiempo Estimado:** 1:00 min
* **Speech:**
  > *(Transición recibida)*:  
  > **(Gallardo Mateo):**  
  > *"En conclusión, logramos construir un compilador completo con Coco/R en C#, con gramática $LL(1)$ libre de conflictos, 6 validaciones de error semántico en la tabla de símbolos, generación de HTML5 autocontenido, 13 de 13 pruebas automatizadas aprobadas e interfaces tanto de consola como gráfica.  
  > Como líneas de trabajo futuro dejamos planteadas las preguntas de emparejar y numéricas, la exportación con código QR para celulares y la recuperación de errores sintácticos con `SYNC` y `WEAK`."*  
  > 
  > **(Reinoso Abel):**  
  > *"Muchas gracias a los profesores por su tiempo y seguimiento. Pasamos ahora mismo a mostrar el compilador funcionando en vivo desde su interfaz gráfica."*

---

## Guion Breve de la Demostración en Vivo con la GUI

**Duración Estimada:** 3:00 min  
**Operadores:** **Gallardo Mateo & Reinoso Abel**  
**Comando de Inicio:**
```powershell
dotnet run --project QuizLang.Gui
```

### Paso 1: Lanzamiento y Recorrido de la Interfaz (0:00 – 0:45)
1. Mateo ejecuta el comando `dotnet run --project QuizLang.Gui` en la terminal. Se abre la ventana principal de Windows Forms.
2. Explica brevemente la distribución: editor ergonómico con números de línea GDI+ a la izquierda, navegador integrado a la derecha, barra superior de acciones y grilla de diagnósticos en la parte inferior.

### Paso 2: Compilación de Examen Válido y Previsualización (0:45 – 1:30)
1. En el desplegable de ejemplos, selecciona **"Compiladores 2026 (Examen Real)"**.
2. Presiona el botón **"▶ Compilar"**.
3. La barra de estado cambia a verde con el mensaje: *"✓ Compilación exitosa: 5 preguntas, 10 puntos en total."*.
4. El visor integrado de la derecha renderiza instantáneamente el examen HTML generado en memoria.

### Paso 3: Inyección de Error Semántico y Salto Interactivo (1:30 – 2:15)
1. En el selector de ejemplos, carga **"Inválido: Error Semántico"** (o altera deliberadamente una pregunta múltiple quitando la marca `[CORRECTA]` y duplicando la letra `'a'`).
2. Presiona **"▶ Compilar"**.
3. La barra de estado pasa a color rojo alertando de los fallos, y la grilla inferior lista los diagnósticos en español con línea y columna exactas.
4. Abel hace **doble clic** en la fila del diagnóstico: el cursor del editor salta y selecciona automáticamente la línea conflictiva.
5. Explica cómo esta interacción reduce a cero la fricción durante la corrección de errores por parte del usuario.

### Paso 4: Apertura en Navegador y Corrección en Vivo (2:15 – 3:00)
1. Se corrige el examen dejándolo válido y se compila nuevamente con éxito.
2. Se pulsa el botón **"🌐 Ver HTML"**, abriendo la evaluación generada en el navegador web predeterminado.
3. Se responden en vivo varias preguntas (algunas correctas y una incorrecta adrede) y se escribe la respuesta en minúsculas en la pregunta de texto.
4. Se presiona el botón **"Corregir"**: la página muestra inmediatamente la retroalimentación visual en verde y rojo con la solución esperada, computa el puntaje acumulado y emite el dictamen de aprobación final.
5. Se concluye la demo y el equipo queda a completa disposición para la ronda de preguntas del jurado docente.
