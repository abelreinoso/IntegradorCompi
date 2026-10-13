# Bitácora de Uso de Inteligencia Artificial

## Proyecto Final de Compiladores 2026
**Integrantes:** Albornoz Joaquín, Díaz Guadalupe, Gallardo Mateo, Reinoso Abel  
**Institución:** Cátedra de Compiladores 2026  
**Documento:** Registro de Interacciones y Asistencia por IA en Fase de Investigación  

---

| Fecha | Herramienta | Prompt / Tarea Resumida | Resumen de la Respuesta | Verificación Realizada (Fuentes consultadas) |
| :--- | :--- | :--- | :--- | :--- |
| **2026-10-13** | Antigravity – subagente modelo flash | Investigación bibliográfica exhaustiva de Coco/R (puntos 1.1 y 1.2), análisis técnico minucioso del proyecto pedagógico TASTE para la defensa oral del 19/10, y redacción de la bitácora de IA. | Generación de dos informes académicos completos (`investigacion_cocor.md` y `taste_explicacion.md`) que cubren historia, arquitectura interna, autómatas DFA, descenso recursivo LL(1)/LL(k), resolución de conflictos mediante lookahead y semántica, tabla de símbolos, generación de código para máquina virtual de pila, recorrido detallado de la gramática y componentes de Taste en C#, y banco de 15 preguntas típicas de examen oral con respuestas técnicas. | Verificado contra: <br>1. *The Compiler Generator Coco/R User Manual* (Hanspeter Mössenböck, JKU Linz, 2011/2018). <br>2. Tutorial oficial y diapositivas de JMLC'06 (*slides.zip*). <br>3. Código fuente de la distribución de referencia C# de TASTE (`Taste.ATG`, `SymTab.cs`, `CodeGen.cs`, `Taste.cs`). <br>4. Sitio web oficial de Coco/R (SSW JKU Linz). |

---

### Criterios de Validación Metodológica
1. **Contraste de Código Fuente:** Todas las citas de gramática EBNF, signaturas de métodos en C# (`Scan()`, `Peek()`, `ResetPeek()`, `NewObj()`, `Find()`, `OpenScope()`, `CloseScope()`, `Emit()`, `Patch()`, `Interpret()`) fueron contrastadas directamente con el código fuente oficial de la distribución de Coco/R para C#.
2. **Fidelidad Teórica:** Los conceptos teóricos de autómatas finitos deterministas, gramáticas libres de contexto, cálculo de First y Follow, conjuntos de sincronización (`SYNC`) y tokens débiles (`WEAK`) fueron validados con el manual de usuario oficial de Mössenböck y la bibliografía clásica de Aho, Lam, Sethi y Ullman (*Dragon Book*) y Pat Terry (*Compiling with C# and Java*).
3. **Señalización de Incertidumbres:** En caso de no contarse con verificación documental primaria para algún dato específico, se adopta la convención de marcarlo explícitamente con la etiqueta `[VERIFICAR]`.
