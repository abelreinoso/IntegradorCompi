# QuizLang - Compilador de Exámenes Interactivos

**Proyecto Final - Compiladores 2026**  
**Integrantes:**
* Albornoz Joaquín
* Díaz Guadalupe
* Gallardo Mateo
* Reinoso Abel

---

## 1. Descripción del Proyecto

**QuizLang** es un lenguaje de dominio específico (DSL) diseñado para definir exámenes y evaluaciones de forma clara y concisa. El compilador traduce archivos `.quiz` a páginas web interactivas autocontenidas (`HTML5 + CSS3 + JavaScript`), permitiendo responder el examen en cualquier navegador y obtener corrección instantánea, cálculo de puntaje acumulado y verificación de aprobación.

El compilador fue construido utilizando **Coco/R para C#**, implementando un analizador descendente recursivo basado en una gramática atribuida **LL(1)** sin conflictos.

---

## 2. Requisitos del Sistema

* **Sistema Operativo:** Windows 10 / 11
* **Entorno de ejecución:** PowerShell 5.1 o superior / PowerShell 7
* **SDK:** .NET SDK 10.0 (o superior)

---

## 3. Estructura del Proyecto

```text
TrabajoFinal/
├── QuizLang.sln              # Solución completa en .NET 10
├── README.md                 # Este documento de bienvenida e instrucciones
├── run_tests.ps1             # Script para compilar y ejecutar toda la suite de pruebas
├── QuizLang.Core/            # Biblioteca de clases del compilador
│   ├── QuizLang.atg          # Especificación formal de la gramática en Coco/R
│   ├── Scanner.cs            # Analizador léxico generado por Coco/R
│   ├── Parser.cs             # Analizador sintáctico generado por Coco/R
│   ├── SymbolTable.cs        # Verificador y tabla de símbolos semántica
│   ├── CodeGen.cs            # Generador de páginas HTML/CSS/JS autocontenidas
│   ├── Compiler.cs           # Fachada de compilación de alto nivel
│   └── Modelo/               # Clases del AST (Examen, Pregunta, Opcion, etc.)
├── QuizLang.Cli/             # Compilador de consola (línea de comandos)
│   └── Program.cs
├── QuizLang.Gui/             # Entorno de desarrollo gráfico en Windows Forms
│   ├── Form1.cs
│   └── Form1.Designer.cs
├── tools/
│   ├── generar.ps1           # Script PowerShell para regenerar Scanner y Parser con Coco/R
│   └── coco/                 # Binario Coco.exe y plantillas Scanner.frame y Parser.frame
├── tests/                    # Casos de prueba válidos e inválidos con sus .expected
│   ├── basico.quiz / .expected
│   ├── completo.quiz / .expected
│   ├── compiladores2026.quiz / .expected
│   ├── comentarios.quiz / .expected
│   └── sem_*.quiz / error_*.quiz / .expected
└── docs/                     # Documentación técnica y bitácora
    ├── bitacora/
    │   └── bitacora_desarrollo.md
    ├── diseno_compilador.md
    └── manual_usuario.md
```

---

## 4. Instrucciones de Compilación y Uso

### 4.1. Regenerar Scanner y Parser desde la Gramática
Si se modifica `QuizLang.atg`, se puede regenerar el código C# ejecutando:

```powershell
powershell -ExecutionPolicy Bypass -File tools\generar.ps1
```

### 4.2. Compilar la Solución Completa
Para compilar los tres proyectos (`QuizLang.Core`, `QuizLang.Cli`, `QuizLang.Gui`):

```powershell
dotnet build QuizLang.sln
```

### 4.3. Ejecutar la Suite de Pruebas Automatizadas
El script `run_tests.ps1` compila la solución y corre el compilador sobre todos los casos de prueba (4 válidos y 9 inválidos), verificando códigos de salida y mensajes de error exactos:

```powershell
powershell -ExecutionPolicy Bypass -File run_tests.ps1
```

### 4.4. Uso del Compilador de Consola (`QuizLang.Cli`)
```powershell
dotnet run --project QuizLang.Cli\QuizLang.Cli.csproj -- <archivo_entrada.quiz> [-o <archivo_salida.html>]
```
**Ejemplo:**
```powershell
dotnet run --project QuizLang.Cli\QuizLang.Cli.csproj -- tests\compiladores2026.quiz -o examen.html
```

### 4.5. Uso del Entorno Gráfico (`QuizLang.Gui`)
```powershell
dotnet run --project QuizLang.Gui\QuizLang.Gui.csproj
```
El entorno incluye editor de código con números de línea, selector de ejemplos de examen precargados, botón de compilación, visor interactivo integrado y panel inferior de errores navegable por doble clic.

---

## 5. Documentación Detallada

* **[Documento de Diseño del Compilador](docs/diseno_compilador.md):** Especificación léxica, sintaxis EBNF y demostración LL(1), análisis semántico, arquitectura y diagramas Mermaid.
* **[Manual de Usuario](docs/manual_usuario.md):** Guía práctica de sintaxis de QuizLang, uso de la CLI y de la GUI.
* **[Bitácora de Desarrollo](docs/bitacora/bitacora_desarrollo.md):** Registro de pasos, prompts, depuración y solución de errores encontrados.
