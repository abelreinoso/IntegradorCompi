# Guion de la Defensa Oral Final: Compilador QuizLang

**Materia:** Compiladores 2026  
**Fecha de Defensa:** 10 de noviembre de 2026 (tentativa)  
**Integrantes:**
- Albornoz, Joaquín
- Díaz, Guadalupe
- Gallardo, Mateo
- Reinoso, Abel

**Enlaces de la Presentación en Canva (Versión 1 Original):**
- **Enlace de Edición:** [https://www.canva.com/d/WjGJ6HsFEH9oUSF](https://www.canva.com/d/WjGJ6HsFEH9oUSF)
- **Enlace de Visualización:** [https://www.canva.com/d/hU0zAGlc0MPF_Ml](https://www.canva.com/d/hU0zAGlc0MPF_Ml)

**Duración Total Estimada:** 18 a 19 minutos (Presentación teórica: 15 min 30 s + Demostración práctica interactiva: 3 min).

---

## Tabla Resumen de Exposición

| N° | Diapositiva en Canva | Expositor(es) | Duración | Eje Principal |
| :-: | :--- | :--- | :-: | :--- |
| **1** | Portada y Presentación del Proyecto | **Albornoz Joaquín** | 1:00 min | Encuadre académico, equipo y objetivos |
| **2** | Introducción a Coco/R y Filosofía de Diseño | **Díaz Guadalupe** | 1:00 min | Mössenböck, descenso recursivo $LL(1)$ vs $LALR(1)$ |
| **3** | Arquitectura Interna de Coco/R y Caso Taste | **Gallardo Mateo** | 1:15 min | Pipeline de generación, frames y backpatching |
| **4** | Motivación y Dominio de QuizLang | **Reinoso Abel** | 1:00 min | Necesidad docente, DSL y artefacto web cliente |
| **5** | Especificación Léxica y Gramática EBNF | **Albornoz Joaquín** | 1:15 min | Archivo `QuizLang.atg`, alfabeto, tokens y producciones |
| **6** | Demostración de la Propiedad LL(1) | **Díaz Guadalupe** | 1:15 min | Conjuntos First/Follow disjuntos y validación Coco/R |
| **7** | Análisis Semántico y Tabla de Símbolos | **Gallardo Mateo** | 1:15 min | Reglas contextuales en `SymbolTable.cs` y AST |
| **8** | Generación de Código Web Interactivo | **Reinoso Abel** | 1:00 min | `CodeGen.cs`, HTML5/CSS/JS autocontenido offline |
| **9** | Arquitectura del Software e Interfaz Gráfica | **Albornoz Joaquín** | 1:15 min | Modularización en .NET 10 y entorno Windows Forms |
| **10** | Verificación Experimental y Suite de Pruebas | **Díaz Guadalupe** | 1:15 min | Batería de 13 pruebas (13/13 PASS) y `run_tests.ps1` |
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
  > Esta elección ofrece enormes ventajas prácticas en ingeniería de software: el código emitido en C# es completamente transparente y legible, estableciendo una correspondencia directa de uno a uno entre cada símbolo no terminal y un método procedural en `Parser.cs`. Esto facilita una depuración paso a paso sumamente intuitiva y garantiza una complejidad temporal estrictamente lineal, de orden $\mathcal{O}(N)$.  
  > Además, el dialecto Cocol/R unifica en un solo archivo con extensión `.ATG` la especificación de clases de caracteres, tokens léxicos, directivas de comentarios y producciones gramaticales enriquecidas con atributos sintetizados y heredados.  
  > 
  > Ahora le paso la palabra a Mateo para analizar el pipeline interno de Coco/R y las enseñanzas del caso Taste."*

---

### Diapositiva 3: Arquitectura Interna de Coco/R y Caso Taste
* **Título en Canva:** *Del archivo .atg al compilador*
* **Expositor:** **Gallardo Mateo**
* **Tiempo Estimado:** 1:15 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Guada.  
  > El funcionamiento interno de Coco/R se estructura como una cadena de procesamiento elegante: toma nuestra especificación `.ATG` junto a dos plantillas maestras, `Scanner.frame` y `Parser.frame`. El generador valida la corrección formal de la gramática, calcula los conjuntos directores de selección First y Follow, y emite dos artefactos de C#: un autómata finito determinista para el escáner y un conjunto de métodos recursivos para el analizador sintáctico.  
  > 
  > Como base de estudio metodológico, analizamos exhaustivamente el compilador pedagógico **Taste**, provisto por Mössenböck. Taste es un lenguaje imperativo con tipos enteros, booleanos, procedimientos y estructuras de control que genera código intermedio para una máquina virtual basada en pila.  
  > Lo más notable de Taste es su paradigma de compilación en **una sola pasada** (*one-pass*), sin construir un árbol de sintaxis abstracta en memoria. Para resolver los saltos condicionales e incondicionales hacia etiquetas futuras que aún no se conocen, Taste aplica la técnica de **backpatching** mediante la primitiva `Patch(address, value)`.  
  > Estudiar este mecanismo nos permitió entender a fondo la manipulación de código objeto y cómo Coco/R gestiona los flujos de control.  
  > 
  > Le cedo el turno a Abel, quien les explicará la concepción de QuizLang."*

---

### Diapositiva 4: Motivación y Dominio de QuizLang
* **Título en Canva:** *QuizLang reduce fricción*
* **Expositor:** **Reinoso Abel**
* **Tiempo Estimado:** 1:00 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Mateo.  
  > La motivación detrás de QuizLang surge de una problemática pedagógica cotidiana: los docentes a menudo necesitan elaborar cuestionarios y exámenes técnicos interactivos, pero se ven limitados por plataformas de gestión complejas como Moodle o Google Forms, que requieren conectividad constante, autenticación obligatoria y servidores dedicados. La alternativa tradicional es maquetar manualmente código HTML y JavaScript, lo cual demanda horas de trabajo repetitivo y propenso a errores.  
  > 
  > Diseñamos **QuizLang** como un DSL conciso, puramente declarativo y legible tanto para humanos como para máquinas. El lenguaje soporta de forma nativa tres modalidades de evaluación: preguntas de opción múltiple con una única respuesta correcta obligatoria, preguntas de verdadero o falso, y preguntas de texto libre con verificación inteligente.  
  > El artefacto final sintetizado por el compilador es un archivo HTML5 cliente único, estilizado con CSS moderno y potenciado con JavaScript puro, que funciona de manera autónoma y offline en cualquier dispositivo con un navegador web.  
  > 
  > A continuación, Joaquín expondrá la especificación formal y las producciones EBNF de nuestro lenguaje."*

---

### Diapositiva 5: Especificación Léxica y Gramática EBNF
* **Título en Canva:** *Especificar antes de implementar*
* **Expositor:** **Albornoz Joaquín**
* **Tiempo Estimado:** 1:15 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Abel.  
  > Toda la definición formal de QuizLang está concentrada en el archivo `QuizLang.atg`.  
  > En la sección léxica, establecimos una partición rigurosa: en `CHARACTERS` definimos `letter`, `digit`, `printable` y `stringChar`, excluyendo comillas dobles, barras invertidas y saltos de línea. Esto nos permitió declarar en `TOKENS` las cadenas de texto con soporte completo para secuencias de escape como comillas protegidas o saltos de línea, así como tokens para letras identificadoras y números.  
  > Configuramos además el filtrado automático de comentarios unilínea con doble barra, comentarios de bloque delimitados por barra-asterisco, y el descarte de espacios en blanco.  
  > 
  > La sintaxis utiliza la notación EBNF de Wirth. La producción axioma establece:  
  > `QuizLang = "EXAMEN" string [ Config ] { Pregunta } .`  
  > La cláusula `Config` permite definir opcionalmente un `PUNTAJE_MINIMO`. Luego, cada `Pregunta` deriva en `Multiple`, `Texto` o `VerdaderoFalso`. Para las preguntas múltiples, las alternativas se etiquetan con letras y se señala la respuesta acertada mediante el marcador `[CORRECTA]`, mientras que las preguntas de verdadero o falso asignan la constante correspondiente.  
  > 
  > Le devuelvo la palabra a Guadalupe para demostrar la propiedad matemática LL(1)."*

---

### Diapositiva 6: Demostración de la Propiedad LL(1)
* **Título en Canva:** *LL(1), demostrado*
* **Expositor:** **Díaz Guadalupe**
* **Tiempo Estimado:** 1:15 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Joaquín.  
  > Un requisito formal ineludible en Coco/R es que la gramática cumpla estrictamente la condición $LL(1)$. Esto significa que, ante cualquier bifurcación de producciones, el analizador debe decidir la regla correcta mirando únicamente un único símbolo de anticipación (*lookahead*).  
  > 
  > Analizamos matemáticamente los conjuntos de selección:  
  > Primero, en la regla `Pregunta`, los conjuntos First de `Multiple`, `Texto` y `VerdaderoFalso` están encabezados respectivamente por los literales `"PREGUNTA_MULTIPLE"`, `"PREGUNTA_TEXTO"` y `"PREGUNTA_VF"`. Su intersección es estrictamente vacía.  
  > Segundo, en el cuerpo de las alternativas de opción múltiple, el First de `Opcion` es el token `letra`, mientras que el Follow es el delimitador de cierre de llave `}`. No existe ambigüedad de parada ni solapamiento.  
  > Tercero, la cláusula opcional `[ Config ]` distingue unívocamente la palabra clave `"PUNTAJE_MINIMO"` frente a los inicios de pregunta o el fin del archivo.  
  > 
  > Al procesar `QuizLang.atg` con el ejecutable oficial `Coco.exe`, la herramienta reportó formalmente **0 errors detected** y **cero conflictos LL(1)**, confirmando la ausencia total de recursión por izquierda y de factores comunes no factorizados.  
  > 
  > Ahora Mateo detallará la etapa semántica y la tabla de símbolos."*

---

### Diapositiva 7: Análisis Semántico y Tabla de Símbolos
* **Título en Canva:** *La sintaxis no alcanza*
* **Expositor:** **Gallardo Mateo**
* **Tiempo Estimado:** 1:15 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Guadalupe.  
  > Un flujo de tokens puede ser sintácticamente perfecto y, sin embargo, carecer de sentido lógico o violar las reglas del dominio evaluativo. Por eso, desacoplamos las reglas de negocio de la gramática encapsulándolas en la clase `SymbolTable.cs`.  
  > 
  > Durante el recorrido sintáctico, las acciones embebidas en el `.ATG` van poblando un modelo de objetos fuertemente tipado: la clase raíz `Examen`, que almacena una lista polimórfica de objetos `Pregunta`.  
  > Nuestra tabla de símbolos valida cinco reglas contextuales fundamentales:  
  > 1. **Unicidad de enunciados:** Mediante un `HashSet` insensible a mayúsculas y minúsculas, detectamos y rechazamos preguntas repetidas.  
  > 2. **Exactitud de respuestas correctas:** En preguntas múltiples, exigimos que exista exactamente una opción marcada con `[CORRECTA]`, emitiendo un error semántico si hay cero o si hay más de una.  
  > 3. **Consistencia de letras:** Validamos que no existan letras de opción duplicadas; si no son estrictamente consecutivas, emitimos una advertencia (*warning*) sin abortar la compilación.  
  > 4. **Consistencia del puntaje:** Verificamos que el `PUNTAJE_MINIMO` no supere la sumatoria de puntos totales del examen.  
  > 5. **Examen no vacío:** Garantizamos que el examen contenga al menos una pregunta.  
  > Todos los errores semánticos se registran en la clase `Errors` preservando línea y columna exactas.  
  > 
  > Abel explicará a continuación cómo generamos la aplicación web."*

---

### Diapositiva 8: Generación de Código Web Interactivo
* **Título en Canva:** *Del modelo a una experiencia interactiva*
* **Expositor:** **Reinoso Abel**
* **Tiempo Estimado:** 1:00 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Mateo.  
  > Una vez que el análisis semántico certifica la validez del examen, entra en acción el componente `CodeGen.cs`. Su función es sintetizar el modelo en un documento HTML5 enriquecido.  
  > 
  > Nos propusimos una premisa estricta de diseño: **cero dependencias externas**. El HTML producido no requiere conexión a internet, frameworks voluminosos ni hojas de estilo remotas. Además, cada cadena de texto del examen es procesada mediante `WebUtility.HtmlEncode`, eliminando cualquier vulnerabilidad de inyección de código.  
  > 
  > En el navegador, un motor JavaScript integrado realiza la corrección automática en el cliente al presionar 'Corregir':  
  > Pinta en verde las respuestas correctas, resalta en rojo las erróneas mostrando cuál era la alternativa acertada, normaliza respuestas de texto recortando espacios y convirtiendo a minúsculas, y calcula el porcentaje de logro final. Si el estudiante alcanza el puntaje mínimo configurado, el sistema despliega un banner de aprobación.  
  > 
  > Ahora Joaquín les presentará la arquitectura del software y la aplicación gráfica."*

---

### Diapositiva 9: Arquitectura del Software e Interfaz Gráfica
* **Título en Canva:** *Arquitectura modular en .NET 10*
* **Expositor:** **Albornoz Joaquín**
* **Tiempo Estimado:** 1:15 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Abel.  
  > La solución global de software fue organizada bajo una arquitectura modular limpia en .NET 10, compuesta por tres proyectos bien diferenciados:  
  > En primer lugar, `QuizLang.Core`, que contiene el analizador léxico y sintáctico generado, la tabla de símbolos, el modelo del AST, el generador HTML y la fachada unificada `Compiler.cs`.  
  > En segundo lugar, `QuizLang.Cli`, una utilidad de línea de comandos para compilación por lotes en terminales y scripts.  
  > Y en tercer lugar, `QuizLang.Gui`, un entorno de desarrollo de escritorio programado en Windows Forms.  
  > 
  > Diseñamos `QuizLang.Gui` pensando en la ergonomía del usuario: posee un editor de código monoespaciado con números de línea renderizados en tiempo real mediante GDI+, un selector de ejemplos precargados y un panel de vista previa web interactivo.  
  > Una de sus características clave es la **navegación interactiva de errores**: la grilla inferior resalta en rojo las fallas del compilador y, al hacer doble clic sobre cualquier fila, el editor posiciona automáticamente el cursor en la línea y columna exactas del problema.  
  > Toda la solución compila de forma absolutamente limpia, con **0 errores y 0 advertencias**.  
  > 
  > Le paso la palabra a Guadalupe para revisar las pruebas automatizadas."*

---

### Diapositiva 10: Verificación Experimental y Suite de Pruebas
* **Título en Canva:** *Verificar diagnósticos, no solo resultados*
* **Expositor:** **Díaz Guadalupe**
* **Tiempo Estimado:** 1:15 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Joaquín.  
  > En la construcción de compiladores, la verificación empírica es la única garantía de corrección. Implementamos una suite de pruebas automatizadas de caja negra orquestada por el script `run_tests.ps1` en PowerShell.  
  > 
  > Nuestro banco consta de **13 casos de prueba exhaustivos**:  
  > * Cuatro casos válidos que ejercitan preguntas múltiples, verdadero/falso, texto libre y comentarios, verificando la generación del HTML con código de salida 0.  
  > * Nueve casos de error formulados metódicamente: cubren la fase léxica con caracteres inválidos, la fase sintáctica con omisiones de delimitadores o encabezados, y la fase semántica con enunciados repetidos, preguntas múltiples sin opción correcta, opciones con múltiples marcas, letras duplicadas, puntajes mínimos inalcanzables y exámenes vacíos.  
  > 
  > El script automatizado compila previamente la solución con `dotnet build` y evalúa cada prueba contrastando tanto el código de salida de error como la correspondencia exacta del mensaje diagnóstico contra archivos patrón `.expected`.  
  > El resultado certificado es contundente: **13 de 13 pruebas aprobadas con estatus PASS**.  
  > 
  > Mateo nos hablará ahora sobre la metodología de trabajo asistido con Inteligencia Artificial."*

---

### Diapositiva 11: Metodología con IA: Enfoque Agentic y Bitácora
* **Título en Canva:** *IA con roles, evidencia y supervisión*
* **Expositor:** **Gallardo Mateo**
* **Tiempo Estimado:** 1:15 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Guada.  
  > En lugar de recurrir a consultas informales o copiar código desarticulado, estructuramos el desarrollo bajo un entorno *agentic* con la plataforma **Antigravity**, delimitando roles complementarios:  
  > Como agente supervisor empleamos **Claude Opus**, responsable de la planificación arquitectónica de alto nivel, la revisión crítica de la gramática y la auditoría rigurosa de fuentes.  
  > En paralelo, desplegamos subagentes operativos con **Gemini Flash**, orientados a tareas de ejecución rápida: redacción de plantillas de clases del AST, prototipado de la interfaz gráfica y generación de casos de prueba.  
  > 
  > Queremos subrayar que las decisiones sustantivas fueron tomadas por el grupo humano: nosotros seleccionamos el dominio de QuizLang frente a otras 4 ideas generadas en el brainstorming inicial, y resolvimos adoptar C# y .NET 10 por sobre C++ debido a la robustez de su soporte para interfaces gráficas y la estabilidad de las plantillas de Coco/R.  
  > Asimismo, llevamos una **Bitácora de IA** transparente y obligatoria donde documentamos cada interacción, el código propuesto y la validación empírica en consola.  
  > 
  > Abel detallará los desafíos técnicos y errores reales que debimos resolver."*

---

### Diapositiva 12: Depuración Real y Resolución de Conflictos
* **Título en Canva:** *Los errores reales cruzan capas*
* **Expositor:** **Reinoso Abel**
* **Tiempo Estimado:** 1:15 min
* **Speech:**
  > *(Transición recibida)*: *"Gracias, Mateo.  
  > La integración entre una herramienta de metaprogramación como Coco/R y el compilador de C# nos enfrentó a desafíos técnicos concretos donde la asistencia de la IA resultó insuficiente sin nuestro análisis directo.  
  > 
  > Documentamos cuatro casos críticos:  
  > Primero, una restricción léxica de Coco/R: al intentar restar conjuntos de caracteres directamente en la sección `TOKENS`, el generador arrojó `invalid TokenFactor`. Descubrimos que las operaciones de conjuntos sólo son legales en `CHARACTERS`, por lo que aislamos `stringChar` allí antes de referenciarlo en el token.  
  > Segundo, el generador emitió `missing production for grammar name` debido a que el borrador propuesto por la IA nombró a la regla raíz `Quiz` en lugar de coincidir exactamente con el identificador `COMPILER QuizLang`.  
  > Tercero, al inyectar directivas `using` debajo de `COMPILER`, las plantillas de Coco/R las envolvieron dentro de la clase `Parser`, provocando errores de sintaxis en C#; lo solucionamos ubicando los `using` en la primera línea de `QuizLang.atg`.  
  > Y cuarto, modificamos `Parser.frame` para evitar que un símbolo léxico no reconocido se tradujera ingenuamente como `se esperaba ???`.  
  > 
  > Ahora Joaquín y Guadalupe presentarán nuestra reflexión crítica sobre las alucinaciones de la IA."*

---

### Diapositiva 13: Reflexión Crítica: Alucinaciones y Verificación Humana
* **Título en Canva:** *La IA propone; la evidencia decide*
* **Expositores:** **Albornoz Joaquín & Díaz Guadalupe**
* **Tiempo Estimado:** 1:30 min
* **Speech:**
  > *(Transición recibida)*:  
  > **(Albornoz Joaquín):**  
  > *"El aprendizaje más enriquecedor del proyecto fue el análisis de las limitaciones de los modelos generativos.  
  > Durante la elaboración del marco teórico, el subagente de IA introdujo con absoluta naturalidad una cita bibliográfica: un supuesto libro del profesor Hanspeter Mössenböck titulado 'Compilerbau', supuestamente publicado por Hanser Verlag en 2018.  
  > La auditoría de nuestro agente supervisor alertó que dicha obra no existía en el catálogo de Hanser ni en el currículum de Mössenböck: era una alucinación completamente inventada pero verosímil.  
  > Con total honestidad académica, en lugar de borrarla en silencio, decidimos mantenerla señalizada en el informe con la etiqueta explícita `[VERIFICAR]`. Esto deja constancia de que ninguna cita generada por IA puede aceptarse sin corroboración en fuentes primarias."*  
  > 
  > **(Díaz Guadalupe):**  
  > *"Asimismo, enfrentamos el sesgo de la 'falsa certeza': en repetidas oportunidades los subagentes declaraban que una tarea estaba 'verificada al 100%', ignorando advertencias de compilación en tipos anulables de C# o detalles de codificación en consola.  
  > La conclusión del equipo es tajante: la Inteligencia Artificial acelera la codificación mecánica, pero la comprensión conceptual de autómatas y gramáticas, la demostración de la propiedad $LL(1)$ y la responsabilidad ética del software recaen indiscutiblemente en el criterio humano.  
  > 
  > Ahora Mateo y Abel cerrarán la presentación para dar paso a la demostración en vivo."*

---

### Diapositiva 14: Conclusiones y Demostración en Vivo
* **Título en Canva:** *QuizLang compila conocimiento*
* **Expositores:** **Gallardo Mateo & Reinoso Abel**
* **Tiempo Estimado:** 1:00 min
* **Speech:**
  > *(Transición recibida)*:  
  > **(Gallardo Mateo):**  
  > *"A modo de conclusión, hemos alcanzado la totalidad de los objetivos fijados: desarrollamos un compilador formalmente verificado, determinista y robusto, con interfaces de consola y de escritorio, y una salida interactiva de gran valor práctico para la docencia.  
  > Como trabajo a futuro, proyectamos incorporar nuevos tipos de preguntas (como ejercicios numéricos o de emparejamiento), implementar recuperación avanzada de errores sintácticos mediante las directivas `SYNC` y `WEAK` de Coco/R, y generar códigos QR para facilitar la apertura inmediata del examen en teléfonos móviles."*  
  > 
  > **(Reinoso Abel):**  
  > *"Agradecemos profundamente el acompañamiento y las devoluciones del cuerpo docente a lo largo de este cuatrimestre.  
  > A continuación, cerramos las diapositivas y pasamos a la demostración práctica del compilador en tiempo real con nuestra aplicación de escritorio."*

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
3. La barra de estado cambia a verde con el mensaje: *"✓ Compilación exitosa: 5 preguntas, 10 puntos en total"*.
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
