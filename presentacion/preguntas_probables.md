# Preguntas Probables del Tribunal y Respuestas Técnicas

**Asignatura:** Compiladores 2026  
**Evaluación:** Parcial 2 – Avances del Trabajo Final (Parte 2)  
**Proyecto:** QuizLang – Compilador de Exámenes Interactivos  
**Autores:** Albornoz Joaquín, Díaz Guadalupe, Gallardo Mateo, Reinoso Abel

A continuación se presentan 10 preguntas conceptuales y prácticas que la cátedra suele realizar en la defensa de avances de la Parte 2, junto con sus respuestas recomendadas formuladas con precisión técnica y tono seguro.

---

### 1. ¿Por qué eligieron un generador descendente LL(1) como Coco/R en lugar de herramientas ascendentes LALR(1) como Yacc/Bison o frameworks como ANTLR?
**Respuesta:**  
Elegimos Coco/R por tres motivos fundamentales:
1. **Correspondencia directa con la teoría:** Al ser un analizador sintáctico por descenso recursivo, cada no terminal de la gramática genera un método en C# (`Parser.cs`). Esto permite depurar el compilador paso a paso con puntos de interrupción convencionales, a diferencia de las tablas opacas de autómatas de pila de LALR(1).
2. **Especificación integrada en Cocol/R:** En un único archivo `.atg` definimos caracteres, tokens, gramática y acciones semánticas fuertemente tipadas en C#, sin necesidad de herramientas externas de pegado de código.
3. **Eficiencia lineal $\mathcal{O}(N)$ y simplicidad:** Como nuestro DSL no requiere precedencia compleja de operadores aritméticos profundos, una gramática LL(1) ofrece la solución más limpia y rápida sin overhead de bibliotecas pesadas.

---

### 2. ¿Cómo demostraron que la gramática de QuizLang cumple estrictamente la condición LL(1)? ¿Tuvieron que resolver algún conflicto de ambigüedad?
**Respuesta:**  
La demostración se basa en el cálculo de los conjuntos directores ($\text{FIRST}$ y $\text{FOLLOW}$) para cada decisión:
* En `Pregunta = Multiple | Texto | VerdaderoFalso .`, los tres símbolos iniciales son tokens reservados disjuntos (`PREGUNTA_MULTIPLE`, `PREGUNTA_TEXTO`, `PREGUNTA_VF`). Su intersección es vacía.
* En el bloque de opciones `{ Opcion }`, el conjunto $\text{FIRST}(Opcion)$ es $\{\text{letra}\}$ y el conjunto de salida $\text{FOLLOW}$ es el token de cierre $\{\text{"}"}\}$, que también son mutuamente excluyentes.
* La herramienta `Coco.exe` valida automáticamente los autómatas de decisión sobre el archivo `.atg` y emitió **0 conflicts detected**. No necesitamos recurrir a resolutores manuales `IF(...)` porque la sintaxis fue concebida desde el inicio con prefijos determinísticos.

---

### 3. ¿Dónde trazan la frontera entre el análisis sintáctico y el análisis semántico en QuizLang? Den un ejemplo concreto.
**Respuesta:**  
El analizador sintáctico (`Parser.cs`) únicamente valida que la secuencia de tokens cumpla la estructura formal EBNF (por ejemplo, que una pregunta múltiple tenga enunciado entre comillas, llave de apertura, una o más opciones y llave de cierre). No tiene memoria contextual ni capacidad de cómputo sobre la validez lógica del examen.  
El análisis semántico (`SymbolTable.cs`) se encarga de las reglas de negocio contextuales. Por ejemplo:
* Una pregunta múltiple con 4 opciones válidas donde ninguna tiene `[CORRECTA]` es **sintácticamente válida**, pero **semánticamente inválida**.
* Lo mismo ocurre con un examen cuyo `PUNTAJE_MINIMO` es 20 pero sus preguntas suman 10 puntos: la sintaxis no puede calcular la sumatoria de puntos, esa responsabilidad recae exclusivamente en la tabla de símbolos.

---

### 4. ¿Por qué sostienen que QuizLang es un lenguaje declarativo y qué consecuencias tiene esto en la arquitectura del compilador?
**Respuesta:**  
Es declarativo porque el programador o docente describe **qué** compone la evaluación (el título, las preguntas, las opciones y las respuestas correctas), pero no **cómo** se ejecuta un algoritmo de control.  
Consecuencias en la arquitectura:
* No necesitamos modelar estructuras de control de flujo imperativo (bucles `for`/`while`, bifurcaciones condicionales `if/else`, ni instrucciones de salto `goto`).
* No se requiere una máquina virtual con registros o instrucciones de salto hacia adelante con *backpatching* (como sí requería el compilador Taste de Mössenböck).
* En su lugar, el AST se mapea de forma directa y limpia hacia un modelo de objetos en memoria (`Examen`, `Pregunta`, `Opcion`) que luego es serializado e inyectado como un conjunto de datos en el cliente web.

---

### 5. ¿Cómo implementa Coco/R el pasaje de información entre producciones y qué mecanismo usaron para construir el AST?
**Respuesta:**  
Coco/R utiliza **gramáticas atribuidas** y admite parámetros de entrada y salida directamente en las producciones EBNF, utilizando la sintaxis de C#.  
Por ejemplo, en la producción de opciones declaramos:
```cocol
Opcion<out Opcion op> (. op = new Opcion(); .) =
  letra (. op.Letra = t.val[0]; op.Line = t.line; op.Col = t.col; .)
  ")"
  string (. op.Texto = Unquote(t.val); .)
  [ "[CORRECTA]" (. op.EsCorrecta = true; .) ] .
```
El no terminal padre (`Multiple`) invoca `Opcion<out op>` y añade la instancia creada directamente a su lista `pm.Opciones.Add(op)`. De este modo, el árbol sintáctico abstracto (AST) se construye de forma ascendente durante el mismo recorrido del análisis descendente, sin necesidad de recorridos adicionales.

---

### 6. ¿Cómo gestionan el rastreo y la precisión de los errores (línea y columna) en las distintas etapas?
**Respuesta:**  
Coco/R provee en la variable global `t` el token actual, que incluye sus propiedades `t.line` y `t.col`.
* **En el Escáner y Parser:** Coco/R utiliza el objeto `Errors` (`errors.SynErr(line, col, n)`). Interceptamos estos métodos en `Parser.frame` y traducimos los mensajes al español.
* **En la Tabla de Símbolos (`SymbolTable.cs`):** Al construir cada nodo del AST, guardamos explícitamente `Line` y `Col`. Si se detecta una inconsistencia semántica (como un enunciado repetido o una letra de opción duplicada), invocamos `_errors.SemErr(p.Line, p.Col, mensaje)`.
* Esto garantiza que en la terminal o en la grilla de la GUI, el usuario siempre reciba la ubicación exacta del caracter conflictivo.

---

### 7. ¿Por qué decidieron que el target del compilador sea una página HTML/CSS/JS autocontenida y qué precauciones de seguridad tomaron?
**Respuesta:**  
Decidimos emitir una página web autocontenida para maximizar la portabilidad pedagógica: el docente compila su archivo `.quiz` y obtiene un archivo `.html` que puede enviar por correo o subir a cualquier plataforma, funcionando en Windows, Linux, Mac o teléfonos móviles sin requerir ningún runtime ni conexión a Internet.  
Respecto a la seguridad:
* Todo enunciado y texto de opción pasa por `WebUtility.HtmlEncode()` en `CodeGen.cs`. Esto neutraliza cualquier intento de inyección de código HTML malicioso o scripts (prevención estricta contra ataques XSS reflejados en el examen).
* La lógica de validación se encapsula en una estructura JSON inyectada en el documento que es interpretada exclusivamente en el cliente.

---

### 8. En este parcial presentaron la demo únicamente por terminal. ¿En qué estado real se encuentra la interfaz gráfica de Windows Forms?
**Respuesta:**  
La interfaz gráfica en Windows Forms (`QuizLang.Gui`) ya está completamente estructurada y compilando dentro de la solución `QuizLang.sln` bajo .NET 10. Cuenta con su editor de código con numeración de líneas, un combo para cargar exámenes de prueba precargados, botón de compilación e integración del navegador web embebido.  
Decidimos no incluirla en la demo de este parcial porque priorizamos mostrar la solidez del motor central del compilador (CLI, generación con Coco/R, códigos de salida y suite de pruebas automatizadas) y porque actualmente estamos en una etapa de pulido visual para la entrega final: queremos incorporar navegación interactiva avanzada entre los errores y el cursor, y paneles para visualizar la lista de tokens y la tabla de símbolos.

---

### 9. Utilizaron Inteligencia Artificial en el proyecto. ¿Cómo la auditaron y qué nos pueden decir sobre el caso de la cita alucinada que figura en la bitácora?
**Respuesta:**  
Trabajamos bajo un esquema de programación asistida en Antigravity, separando roles entre un Agente Supervisor (Claude Opus) para arquitectura y auditoría, y subagentes (Gemini Flash) para prototipado rápido de código repetitivo y tests, documentando cada interacción en `docs/bitacora_ia.md`.  
El caso de la cita alucinada fue el mejor aprendizaje metodológico del proyecto:
* Uno de los subagentes, al investigar la literatura sobre Coco/R, incluyó una referencia bibliográfica a un supuesto libro de la editorial Hanser del año 2018 sobre compiladores atribuido a autores reconocidos.
* Al contrastar la bibliografía contra los catálogos oficiales de la editorial y bases bibliográficas de ACM/IEEE, descubrimos que el libro no existía: la IA había generado una cita verosímil pero totalmente inventada.
* En lugar de borrarla y esconderla, decidimos mantenerla señalizada con `[VERIFICAR]` en la documentación como evidencia explícita de auditoría crítica. Demostró empíricamente al equipo que nunca se debe delegar la validación conceptual en un modelo generativo sin contrastación primaria.

---

### 10. ¿Cuáles son los próximos pasos prioritarios hacia la entrega final del 10 de noviembre?
**Respuesta:**  
Nuestro cronograma hacia el 10 de noviembre se enfoca en cinco puntos concretos:
1. **Visualizador de fases en la GUI:** Agregar una pestaña o panel lateral que muestre la tabla de tokens generada por el escáner y la representación arbórea del examen.
2. **Demo en vivo multidispositivo vía QR:** Integrar en el compilador o en la GUI la generación de un código QR local para proyectar en el aula y permitir que los evaluadores respondan el examen directamente desde sus teléfonos celulares en tiempo real.
3. **Manejo y recuperación de errores avanzada:** Incorporar las directivas `SYNC` y `WEAK` de Coco/R en `QuizLang.atg`, permitiendo que el parser se recupere de errores de sintaxis y continúe analizando las preguntas siguientes en lugar de abortar en la primera falla.
4. **Extensiones opcionales de sintaxis:** Evaluar la adición de preguntas numéricas con tolerancia o preguntas de emparejamiento.
5. **Consolidación del informe final y grabación del video:** Finalizar la redacción monográfica, completar la revisión cruzada entre los cuatro integrantes y grabar el video explicativo de la defensa.
