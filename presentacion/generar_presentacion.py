#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
generar_presentacion.py
Genera la presentación 'Avance_Parcial2_QuizLang.pptx' para la Evaluación Parcial 2
de Compiladores 2026 utilizando python-pptx.
"""

import os
from pptx import Presentation
from pptx.util import Inches, Pt
from pptx.dml.color import RGBColor
from pptx.enum.text import PP_ALIGN
from pptx.enum.shapes import MSO_SHAPE

# ==========================================
# PALETA DE COLORES Y CONSTANTES DE DISEÑO
# ==========================================
BG_COLOR = RGBColor(248, 250, 252)        # Slate 50 (#F8FAFC)
DARK_NAVY = RGBColor(15, 23, 42)          # Slate 900 (#0F172A)
PRIMARY_INDIGO = RGBColor(79, 70, 229)    # Indigo 600 (#4F46E5)
PRIMARY_DARK = RGBColor(55, 48, 163)      # Indigo 800 (#3730A3)
PRIMARY_LIGHT = RGBColor(224, 231, 255)   # Indigo 100 (#E0E7FF)
TEAL_ACCENT = RGBColor(13, 148, 136)      # Teal 600 (#0D9488)
SUCCESS_GREEN = RGBColor(22, 163, 74)     # Green 600 (#16A34A)
SUCCESS_BG = RGBColor(220, 252, 231)      # Green 100 (#DCFCE7)
ERROR_RED = RGBColor(220, 38, 38)         # Red 600 (#DC2626)
CARD_BG = RGBColor(255, 255, 255)         # Blanco (#FFFFFF)
CARD_BORDER = RGBColor(226, 232, 240)     # Slate 200 (#E2E8F0)
TEXT_MAIN = RGBColor(30, 41, 59)          # Slate 800 (#1E293B)
TEXT_MUTED = RGBColor(100, 116, 139)      # Slate 500 (#64748B)
CODE_BG = RGBColor(15, 23, 42)            # Fondo bloque código oscuro (#0F172A)
CODE_TEXT = RGBColor(241, 245, 249)       # Texto bloque código claro (#F1F5F9)
CODE_KEYWORD = RGBColor(129, 140, 248)    # Indigo claro (#818CF8)
PLACEHOLDER_BG = RGBColor(241, 245, 249)  # Slate 100

FONT_HEADING = "Segoe UI"
FONT_BODY = "Segoe UI"
FONT_CODE = "Consolas"

TOTAL_SLIDES = 8

def create_slide(prs):
    """Crea una diapositiva en blanco 16:9 con fondo limpio."""
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    bg = slide.shapes.add_shape(MSO_SHAPE.RECTANGLE, 0, 0, prs.slide_width, prs.slide_height)
    bg.fill.solid()
    bg.fill.fore_color.rgb = BG_COLOR
    bg.line.fill.background()
    return slide

def add_header(slide, title, category=None):
    """Agrega un encabezado moderno consistente a la diapositiva."""
    header_box = slide.shapes.add_textbox(Inches(0.8), Inches(0.4), Inches(11.733), Inches(0.95))
    tf = header_box.text_frame
    tf.word_wrap = True
    tf.margin_left = tf.margin_top = tf.margin_right = tf.margin_bottom = 0

    if category:
        p_cat = tf.paragraphs[0]
        p_cat.text = category.upper()
        p_cat.font.name = FONT_HEADING
        p_cat.font.size = Pt(9.5)
        p_cat.font.bold = True
        p_cat.font.color.rgb = PRIMARY_INDIGO
        p_cat.space_after = Pt(2)
        p_title = tf.add_paragraph()
    else:
        p_title = tf.paragraphs[0]

    p_title.text = title
    p_title.font.name = FONT_HEADING
    p_title.font.size = Pt(22)
    p_title.font.bold = True
    p_title.font.color.rgb = DARK_NAVY

def add_footer(slide, current_slide, speaker_name=None):
    """Agrega pie de página consistente con número de diapositiva y orador."""
    footer_box = slide.shapes.add_textbox(Inches(0.8), Inches(7.0), Inches(11.733), Inches(0.35))
    tf = footer_box.text_frame
    tf.word_wrap = True
    tf.margin_left = tf.margin_top = tf.margin_right = tf.margin_bottom = 0
    p = tf.paragraphs[0]
    
    text = f"Compiladores 2026 – QuizLang | Evaluación Parcial 2"
    if speaker_name:
        text += f"   •   Expositor: {speaker_name}"
    text += f"                                                               Diapositiva {current_slide} de {TOTAL_SLIDES}"
    
    p.text = text
    p.font.name = FONT_BODY
    p.font.size = Pt(9)
    p.font.color.rgb = TEXT_MUTED

def add_card(slide, left, top, width, height, bg_color=CARD_BG, border_color=CARD_BORDER):
    """Crea un contenedor tipo tarjeta moderna con borde sutil."""
    card = slide.shapes.add_shape(MSO_SHAPE.ROUNDED_RECTANGLE, left, top, width, height)
    card.fill.solid()
    card.fill.fore_color.rgb = bg_color
    if border_color:
        card.line.color.rgb = border_color
        card.line.width = Pt(1)
    else:
        card.line.fill.background()
    return card

def add_code_block(slide, left, top, width, height, code_text, font_size=Pt(10)):
    """Crea un recuadro oscuro para código fuente en tipografía Consolas."""
    card = add_card(slide, left, top, width, height, bg_color=CODE_BG, border_color=DARK_NAVY)
    
    tb = slide.shapes.add_textbox(left + Inches(0.18), top + Inches(0.15), width - Inches(0.36), height - Inches(0.3))
    tf = tb.text_frame
    tf.word_wrap = True
    tf.margin_left = tf.margin_top = tf.margin_right = tf.margin_bottom = 0
    
    lines = code_text.strip().split("\n")
    for i, line in enumerate(lines):
        p = tf.paragraphs[0] if i == 0 else tf.add_paragraph()
        p.text = line
        p.font.name = FONT_CODE
        p.font.size = font_size
        p.font.color.rgb = CODE_TEXT
        p.space_after = Pt(2)
    return card

def set_slide_notes(slide, notes_text):
    """Asigna el speech completo del orador a las notas de la diapositiva."""
    notes_slide = slide.notes_slide
    tf = notes_slide.notes_text_frame
    tf.text = notes_text.strip()

# ==========================================
# CONSTRUCCIÓN DE LAS DIAPOSITIVAS
# ==========================================

def build_presentation(output_path):
    prs = Presentation()
    prs.slide_width = Inches(13.333)
    prs.slide_height = Inches(7.5)

    # -------------------------------------------------------------------------
    # DIAPOSITIVA 1: PORTADA E INTEGRANTES
    # -------------------------------------------------------------------------
    s1 = create_slide(prs)
    
    # Tarjeta izquierda: Título, materia, fecha y datos del proyecto
    add_card(s1, Inches(0.8), Inches(0.8), Inches(6.8), Inches(5.8))
    
    tb_title = s1.shapes.add_textbox(Inches(1.2), Inches(1.2), Inches(6.0), Inches(4.8))
    tf1 = tb_title.text_frame
    tf1.word_wrap = True
    tf1.margin_left = tf1.margin_top = tf1.margin_right = tf1.margin_bottom = 0
    
    p = tf1.paragraphs[0]
    p.text = "EVALUACIÓN PARCIAL 2  •  AVANCES DEL TRABAJO FINAL (PARTE 2)"
    p.font.name = FONT_HEADING
    p.font.size = Pt(10)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    p.space_after = Pt(12)
    
    p = tf1.add_paragraph()
    p.text = "QuizLang"
    p.font.name = FONT_HEADING
    p.font.size = Pt(36)
    p.font.bold = True
    p.font.color.rgb = DARK_NAVY
    p.space_after = Pt(4)
    
    p = tf1.add_paragraph()
    p.text = "Compilador de Exámenes Interactivos Autocontenidos"
    p.font.name = FONT_HEADING
    p.font.size = Pt(16)
    p.font.color.rgb = TEAL_ACCENT
    p.font.bold = True
    p.space_after = Pt(24)
    
    p = tf1.add_paragraph()
    p.text = "Asignatura: Compiladores  •  Ciclo Lectivo 2026"
    p.font.name = FONT_BODY
    p.font.size = Pt(13)
    p.font.bold = True
    p.font.color.rgb = TEXT_MAIN
    p.space_after = Pt(6)
    
    p = tf1.add_paragraph()
    p.text = "Fecha de Presentación: Noviembre 2026"
    p.font.name = FONT_BODY
    p.font.size = Pt(12)
    p.font.color.rgb = TEXT_MUTED
    p.space_after = Pt(20)
    
    p = tf1.add_paragraph()
    p.text = "Integrantes del Grupo:"
    p.font.name = FONT_BODY
    p.font.size = Pt(12)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    p.space_after = Pt(6)
    
    integrantes = [
        "• Albornoz Joaquín",
        "• Díaz Guadalupe",
        "• Gallardo Mateo",
        "• Reinoso Abel"
    ]
    for integ in integrantes:
        p = tf1.add_paragraph()
        p.text = integ
        p.font.name = FONT_BODY
        p.font.size = Pt(12)
        p.font.color.rgb = TEXT_MAIN
        p.space_after = Pt(3)

    # Tarjeta derecha: Placeholder bien visible para la foto del grupo
    add_card(s1, Inches(7.8), Inches(0.8), Inches(4.733), Inches(5.8), bg_color=CARD_BG, border_color=PRIMARY_INDIGO)
    
    # Recuadro interior para foto
    ph_box = s1.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(8.1), Inches(1.2), Inches(4.133), Inches(4.3))
    ph_box.fill.solid()
    ph_box.fill.fore_color.rgb = PLACEHOLDER_BG
    ph_box.line.color.rgb = CARD_BORDER
    ph_box.line.width = Pt(1.5)
    
    tb_ph = s1.shapes.add_textbox(Inches(8.2), Inches(2.6), Inches(3.933), Inches(1.5))
    tf_ph = tb_ph.text_frame
    tf_ph.word_wrap = True
    p_ph = tf_ph.paragraphs[0]
    p_ph.text = "[ FOTO DEL GRUPO ]"
    p_ph.alignment = PP_ALIGN.CENTER
    p_ph.font.name = FONT_HEADING
    p_ph.font.size = Pt(16)
    p_ph.font.bold = True
    p_ph.font.color.rgb = PRIMARY_INDIGO
    p_ph.space_after = Pt(6)
    
    p_subph = tf_ph.add_paragraph()
    p_subph.text = "Albornoz • Díaz • Gallardo • Reinoso\n(Insertar fotografía del equipo de trabajo)"
    p_subph.alignment = PP_ALIGN.CENTER
    p_subph.font.name = FONT_BODY
    p_subph.font.size = Pt(11)
    p_subph.font.color.rgb = TEXT_MUTED

    add_footer(s1, 1, speaker_name="Albornoz Joaquín")
    set_slide_notes(s1, (
        "Buenas tardes a los profesores y a todos los presentes. "
        "Somos el grupo integrado por Guadalupe Díaz, Mateo Gallardo, Abel Reinoso y quien les habla, Joaquín Albornoz. "
        "Hoy venimos a presentarles los avances de la Parte 2 de nuestro Trabajo Final de Compiladores: el desarrollo integral de QuizLang, "
        "un compilador para un lenguaje de dominio específico orientado a la creación de exámenes y evaluaciones interactivas. "
        "En esta presentación queremos mostrarles cómo formalizamos la gramática del lenguaje utilizando Coco/R sobre .NET 10 en C#, "
        "cómo garantizamos la propiedad LL(1) sin ningún tipo de conflictos, la arquitectura de nuestro verificador semántico y generador de código, "
        "y los resultados concretos de nuestra batería de pruebas automatizada, que cuenta con trece casos de prueba con un cien por ciento de éxito. "
        "Para este parcial hemos preparado una demostración completa en vivo a través de la terminal, donde van a poder observar la regeneración del "
        "compilador con Coco, la emisión de una página web autocontenida y la detección precisa de errores léxicos, sintácticos y semánticos. "
        "Nuestra interfaz gráfica en Windows Forms ya se encuentra implementada y está en etapa de pulido visual para la entrega final del diez de noviembre. "
        "Ahora vamos a comenzar repasando la motivación del caso de estudio y el alcance definido para el lenguaje."
    ))

    # -------------------------------------------------------------------------
    # DIAPOSITIVA 2: PROPUESTA DEL CASO DE ESTUDIO Y ALCANCE
    # -------------------------------------------------------------------------
    s2 = create_slide(prs)
    add_header(s2, "Propuesta del Caso de Estudio y Alcance del Lenguaje", "Caso de Estudio")
    
    # Columna izquierda: Objetivo, Características y Alcance
    add_card(s2, Inches(0.8), Inches(1.5), Inches(5.8), Inches(5.2))
    tb2_left = s2.shapes.add_textbox(Inches(1.0), Inches(1.7), Inches(5.4), Inches(4.8))
    tf2_l = tb2_left.text_frame
    tf2_l.word_wrap = True
    tf2_l.margin_left = tf2_l.margin_top = tf2_l.margin_right = tf2_l.margin_bottom = 0
    
    p = tf2_l.paragraphs[0]
    p.text = "Objetivo del Compilador (Parte 2)"
    p.font.name = FONT_HEADING
    p.font.size = Pt(13)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    p.space_after = Pt(4)
    
    p = tf2_l.add_paragraph()
    p.text = "• Desarrollo de un compilador de extremo a extremo para un DSL propio propuesto por el grupo, traduciendo especificaciones .quiz a exámenes HTML5 autocontenidos e interactivos."
    p.font.name = FONT_BODY
    p.font.size = Pt(11)
    p.font.color.rgb = TEXT_MAIN
    p.space_after = Pt(10)
    
    p = tf2_l.add_paragraph()
    p.text = "Naturaleza Estrictamente Declarativa"
    p.font.name = FONT_HEADING
    p.font.size = Pt(13)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    p.space_after = Pt(4)
    
    p = tf2_l.add_paragraph()
    p.text = "• No posee estructuras de control imperativas (sin bucles while, bifurcaciones if ni saltos goto)."
    p.font.name = FONT_BODY
    p.font.size = Pt(11)
    p.font.color.rgb = TEXT_MAIN
    p.space_after = Pt(3)
    
    p = tf2_l.add_paragraph()
    p.text = "• Modela datos jerárquicos: título, puntaje mínimo global y conjunto de preguntas independientes."
    p.font.name = FONT_BODY
    p.font.size = Pt(11)
    p.font.color.rgb = TEXT_MAIN
    p.space_after = Pt(10)

    p = tf2_l.add_paragraph()
    p.text = "Elementos Léxicos y Alcance Formal"
    p.font.name = FONT_HEADING
    p.font.size = Pt(13)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    p.space_after = Pt(4)

    p = tf2_l.add_paragraph()
    p.text = "• Tipos de datos: cadenas delimitadas con escapes (\"), números enteros y letras de opción (a..z)."
    p.font.name = FONT_BODY
    p.font.size = Pt(11)
    p.font.color.rgb = TEXT_MAIN
    p.space_after = Pt(3)

    p = tf2_l.add_paragraph()
    p.text = "• Palabras clave según ATG: EXAMEN, PUNTAJE_MINIMO, PREGUNTA_MULTIPLE, PUNTOS, [CORRECTA], PREGUNTA_TEXTO, RESPUESTA, PREGUNTA_VF, VERDADERO, FALSO."
    p.font.name = FONT_BODY
    p.font.size = Pt(11)
    p.font.color.rgb = TEXT_MAIN
    p.space_after = Pt(3)

    p = tf2_l.add_paragraph()
    p.text = "• Comentarios: de línea (//) y de bloque (/* ... */) ignorados automáticamente por el escáner."
    p.font.name = FONT_BODY
    p.font.size = Pt(11)
    p.font.color.rgb = TEXT_MAIN

    # Columna derecha: Ejemplo real de código QuizLang
    add_card(s2, Inches(6.8), Inches(1.5), Inches(5.733), Inches(5.2))
    
    tb2_rt = s2.shapes.add_textbox(Inches(7.0), Inches(1.7), Inches(5.333), Inches(0.4))
    tf2_rt = tb2_rt.text_frame
    p_ex = tf2_rt.paragraphs[0]
    p_ex.text = "Ejemplo Real de Código QuizLang (tests/basico.quiz)"
    p_ex.font.name = FONT_HEADING
    p_ex.font.size = Pt(12)
    p_ex.font.bold = True
    p_ex.font.color.rgb = DARK_NAVY
    
    code_example = """// Examen representativo de evaluación
EXAMEN "Evaluación de Algoritmos"
PUNTAJE_MINIMO 2

PREGUNTA_MULTIPLE "¿Cuál es la complejidad del QuickSort en el caso promedio?" PUNTOS 1 {
  a) "O(N^2)"
  b) "O(N log N)" [CORRECTA]
  c) "O(log N)"
}

PREGUNTA_VF "¿Un árbol binario completo es siempre balanceado?" PUNTOS 1 = FALSO

PREGUNTA_TEXTO "¿Cómo se llama la fase que agrupa caracteres en unidades léxicas?" PUNTOS 1
  RESPUESTA "Scanner"
"""
    add_code_block(s2, Inches(7.0), Inches(2.2), Inches(5.333), Inches(4.3), code_example, font_size=Pt(9.5))

    add_footer(s2, 2, speaker_name="Albornoz Joaquín")
    set_slide_notes(s2, (
        "Cuando nos planteamos el caso de estudio para este trabajo, buscamos resolver un problema real y recurrente en el ámbito educativo: "
        "la sobrecarga que significa armar cuestionarios digitales en plataformas pesadas como Moodle o Google Forms, que requieren servidores dedicados, "
        "conectividad permanente o configuraciones engorrosas. "
        "Así nació QuizLang, un DSL pequeño, conciso y estrictamente declarativo. Como pueden ver en el ejemplo en pantalla, un examen se define con una "
        "sintaxis limpia: arranca con la palabra clave EXAMEN seguida de un título entre comillas, una cláusula opcional de PUNTAJE_MINIMO y una serie de "
        "preguntas de tres tipos: opción múltiple con PREGUNTA_MULTIPLE, preguntas de texto libre con PREGUNTA_TEXTO, y de verdadero o falso con PREGUNTA_VF. "
        "Queremos resaltar un aspecto teórico fundamental: en QuizLang no existen estructuras de control tradicionales, como bucles while, condicionales if "
        "ni saltos. Esto no es una omisión, sino una decisión consciente de diseño. Al tratarse de un lenguaje declarativo para describir especificaciones de "
        "evaluaciones, el flujo no depende de un estado mutable en tiempo de ejecución, sino de una estructura jerárquica bien definida. "
        "Respecto al alcance léxico, soportamos cadenas con caracteres de escape, números enteros para puntajes, identificadores de opciones representados por "
        "letras individuales, y comentarios tanto de una línea con doble barra como de bloque con barra-asterisco. "
        "Para ver en detalle cómo convertimos esta especificación en un compilador funcional, le paso la palabra a mi compañera Guadalupe Díaz."
    ))

    # -------------------------------------------------------------------------
    # DIAPOSITIVA 3: AVANCES EN DISEÑO: GRAMÁTICA ATG Y PARSING LL(1)
    # -------------------------------------------------------------------------
    s3 = create_slide(prs)
    add_header(s3, "Avances en Diseño: Gramática Atribuida y Parsing LL(1) con Coco/R", "Diseño del Compilador")
    
    # 3 Columnas: Pipeline Coco/R, Gramática ATG real, Demostración LL(1)
    w_col = Inches(3.75)
    
    # Col 1: Pipeline y Motor Coco/R
    add_card(s3, Inches(0.8), Inches(1.5), w_col, Inches(5.2))
    tb3_1 = s3.shapes.add_textbox(Inches(1.0), Inches(1.7), w_col - Inches(0.4), Inches(4.8))
    tf3_1 = tb3_1.text_frame
    tf3_1.word_wrap = True
    tf3_1.margin_left = tf3_1.margin_top = tf3_1.margin_right = tf3_1.margin_bottom = 0
    
    p = tf3_1.paragraphs[0]
    p.text = "Pipeline de Coco/R (.NET 10)"
    p.font.name = FONT_HEADING
    p.font.size = Pt(13)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    p.space_after = Pt(8)
    
    p = tf3_1.add_paragraph()
    p.text = "• Entrada: QuizLang.atg unifica alfabeto, tokens, comentarios y producciones con acciones en C#."
    p.font.name = FONT_BODY
    p.font.size = Pt(11)
    p.font.color.rgb = TEXT_MAIN
    p.space_after = Pt(6)

    p = tf3_1.add_paragraph()
    p.text = "• Plantillas (Frames): Scanner.frame y Parser.frame oficiales de JKU Linz adaptados a C# moderno."
    p.font.name = FONT_BODY
    p.font.size = Pt(11)
    p.font.color.rgb = TEXT_MAIN
    p.space_after = Pt(6)

    p = tf3_1.add_paragraph()
    p.text = "• Salida generada: Scanner.cs (DFA eficiente) y Parser.cs (analizador descendente recursivo puro)."
    p.font.name = FONT_BODY
    p.font.size = Pt(11)
    p.font.color.rgb = TEXT_MAIN
    p.space_after = Pt(6)

    p = tf3_1.add_paragraph()
    p.text = "• Atributos sintetizados: uso de parámetros out C# (ej: Opcion<out op>) para construir el AST en 1 pasada."
    p.font.name = FONT_BODY
    p.font.size = Pt(11)
    p.font.color.rgb = TEXT_MAIN

    # Col 2: Fragmento real del ATG
    add_card(s3, Inches(4.75), Inches(1.5), w_col, Inches(5.2))
    tb3_2 = s3.shapes.add_textbox(Inches(4.95), Inches(1.7), w_col - Inches(0.4), Inches(0.4))
    tf3_2 = tb3_2.text_frame
    p = tf3_2.paragraphs[0]
    p.text = "Producciones Reales (.atg)"
    p.font.name = FONT_HEADING
    p.font.size = Pt(13)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    
    atg_code = """QuizLang =
  (. symbolTable = new SymbolTable(errors); .)
  "EXAMEN" string
  (. symbolTable.SetExamen(Unquote(t.val), t.line, t.col); .)
  [ Config ]
  { Pregunta }
  (. symbolTable.ValidarExamen(); .) .

Pregunta =
  Multiple | Texto | VerdaderoFalso .

Opcion<out Opcion op> (. op = new Opcion(); .) =
  letra (. op.Letra = t.val[0]; op.Line = t.line; .)
  ")"
  string (. op.Texto = Unquote(t.val); .)
  [ "[CORRECTA]" (. op.EsCorrecta = true; .) ] .
"""
    add_code_block(s3, Inches(4.95), Inches(2.2), w_col - Inches(0.4), Inches(4.3), atg_code, font_size=Pt(9))

    # Col 3: Demostración LL(1) y Conjuntos First/Follow
    add_card(s3, Inches(8.7), Inches(1.5), w_col + Inches(0.133), Inches(5.2))
    tb3_3 = s3.shapes.add_textbox(Inches(8.9), Inches(1.7), w_col - Inches(0.2), Inches(4.8))
    tf3_3 = tb3_3.text_frame
    tf3_3.word_wrap = True
    tf3_3.margin_left = tf3_3.margin_top = tf3_3.margin_right = tf3_3.margin_bottom = 0

    p = tf3_3.paragraphs[0]
    p.text = "Demostración de Propiedad LL(1)"
    p.font.name = FONT_HEADING
    p.font.size = Pt(13)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    p.space_after = Pt(8)

    p = tf3_3.add_paragraph()
    p.text = "• Decisión k=1 determinista: los símbolos directores en cada alternativa son mutuamente disjuntos."
    p.font.name = FONT_BODY
    p.font.size = Pt(11)
    p.font.color.rgb = TEXT_MAIN
    p.space_after = Pt(6)

    p = tf3_3.add_paragraph()
    p.text = "• FIRST(Pregunta):\n  {\"PREGUNTA_MULTIPLE\"} ∩\n  {\"PREGUNTA_TEXTO\"} ∩\n  {\"PREGUNTA_VF\"} = ∅"
    p.font.name = FONT_CODE
    p.font.size = Pt(10)
    p.font.color.rgb = PRIMARY_DARK
    p.space_after = Pt(6)

    p = tf3_3.add_paragraph()
    p.text = "• Cierre de Opciones:\n  FIRST(Opcion) = {letra}\n  FOLLOW({Opcion}) = {\"}\"}\n  Intersección estrictamente vacía."
    p.font.name = FONT_CODE
    p.font.size = Pt(10)
    p.font.color.rgb = PRIMARY_DARK
    p.space_after = Pt(8)

    p = tf3_3.add_paragraph()
    p.text = "Resultado Coco/R: '0 errors detected' y '0 LL(1) conflicts'."
    p.font.name = FONT_BODY
    p.font.size = Pt(11)
    p.font.bold = True
    p.font.color.rgb = SUCCESS_GREEN

    add_footer(s3, 3, speaker_name="Díaz Guadalupe")
    set_slide_notes(s3, (
        "Muchas gracias, Joaquín. Buenas tardes. "
        "Entrando en los avances de diseño del compilador, la columna vertebral de nuestro desarrollo es la gramática atribuida especificada en el archivo "
        "QuizLang.atg, procesada mediante la herramienta Coco/R para C#. "
        "Como estudiamos en la teoría de la cátedra, Coco/R implementa un analizador sintáctico descendente recursivo guiado por una gramática LL(1). "
        "Esto impone una exigencia matemática muy clara: para cada regla de la gramática donde existan alternativas o repeticiones, los conjuntos de "
        "selección—es decir, los símbolos directores calculados a partir de los conjuntos First y Follow—deben ser estrictamente disjuntos con exactamente "
        "un token de anticipación (k=1). "
        "En QuizLang diseñamos la sintaxis cuidadosamente para satisfacer esta condición. Por ejemplo, en la producción Pregunta, cada alternativa arranca con "
        "un token reservado inequívoco: PREGUNTA_MULTIPLE, PREGUNTA_TEXTO y PREGUNTA_VF. La intersección de sus conjuntos First es vacía. "
        "Lo mismo ocurre en las opciones de respuesta, donde el token letra define el inicio de una opción y la llave de cierre } marca el fin del bloque, "
        "sin necesidad de recurrir a mecanismos de retroceso ni a directivas complejas de desambiguación. "
        "Al ejecutar Coco.exe sobre nuestra gramática, la herramienta valida el autómata determinista y emite cero errores detectados y cero conflictos LL(1). "
        "Además, utilizamos atributos semánticos en C#, como el parámetro sintetizado out Opcion op, que nos permite capturar y propagar los datos del árbol "
        "sintáctico directamente durante el análisis en una sola pasada. "
        "Ahora pasamos a la siguiente diapositiva para ver qué hacemos con esos datos en la etapa semántica y en la generación de código."
    ))

    # -------------------------------------------------------------------------
    # DIAPOSITIVA 4: AVANCES EN DESARROLLO: SEMÁNTICA Y GENERACIÓN WEB
    # -------------------------------------------------------------------------
    s4 = create_slide(prs)
    add_header(s4, "Avances en Desarrollo: Semántica y Generación de Código Web", "Desarrollo del Compilador")
    
    # Columna izquierda: Verificación Semántica (SymbolTable.cs)
    add_card(s4, Inches(0.8), Inches(1.5), Inches(5.8), Inches(5.2))
    tb4_l = s4.shapes.add_textbox(Inches(1.0), Inches(1.7), Inches(5.4), Inches(4.8))
    tf4_l = tb4_l.text_frame
    tf4_l.word_wrap = True
    tf4_l.margin_left = tf4_l.margin_top = tf4_l.margin_right = tf4_l.margin_bottom = 0
    
    p = tf4_l.paragraphs[0]
    p.text = "Verificación Semántica (SymbolTable.cs)"
    p.font.name = FONT_HEADING
    p.font.size = Pt(13)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    p.space_after = Pt(8)
    
    checks = [
        ("1. Enunciados Duplicados", "HashSet<string> normalizado case-insensitive para impedir preguntas idénticas."),
        ("2. Opciones [CORRECTA]", "Exige exactamente una opción correcta por pregunta múltiple (falla si hay 0 o >1)."),
        ("3. Letras de Opciones", "Detecta letras duplicadas en una misma pregunta y advierte si no son correlativas."),
        ("4. Examen sin Preguntas", "Rechaza exámenes vacíos sin al menos una pregunta evaluable."),
        ("5. Coherencia de Puntaje", "Valida que PUNTAJE_MINIMO no sea mayor a la suma de puntos reales de las preguntas.")
    ]
    for title_chk, desc_chk in checks:
        p = tf4_l.add_paragraph()
        p.text = f"• {title_chk}: {desc_chk}"
        p.font.name = FONT_BODY
        p.font.size = Pt(11)
        p.font.color.rgb = TEXT_MAIN
        p.space_after = Pt(6)

    p = tf4_l.add_paragraph()
    p.text = "Reporte exacto: invoca Errors.SemErr(line, col) permitiendo posicionamiento exacto del cursor."
    p.font.name = FONT_BODY
    p.font.size = Pt(11)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_DARK

    # Columna derecha: Generación de Código (CodeGen.cs)
    add_card(s4, Inches(6.8), Inches(1.5), Inches(5.733), Inches(5.2))
    tb4_r = s4.shapes.add_textbox(Inches(7.0), Inches(1.7), Inches(5.333), Inches(4.8))
    tf4_r = tb4_r.text_frame
    tf4_r.word_wrap = True
    tf4_r.margin_left = tf4_r.margin_top = tf4_r.margin_right = tf4_r.margin_bottom = 0

    p = tf4_r.paragraphs[0]
    p.text = "Generación de Código Web (CodeGen.cs)"
    p.font.name = FONT_HEADING
    p.font.size = Pt(13)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    p.space_after = Pt(8)

    codegen_items = [
        ("Página Autocontenida", "Emisión en un único archivo .html que integra estructura HTML5, diseño CSS3 moderno responsivo y motor de corrección JS cliente puro."),
        ("Evaluación Offline Instantánea", "No requiere servidores backend, node ni base de datos; funciona en cualquier navegador (Edge, Chrome, Firefox) incluso sin conexión."),
        ("Cálculo y Retroalimentación Visual", "Muestra puntaje acumulado, porcentaje alcanzado, estado APROBADO / NO APROBADO, tarjetas verdes con tildes y rojas con respuestas esperadas."),
        ("Seguridad Anti-XSS Estricta", "Todos los enunciados y alternativas se escapan con WebUtility.HtmlEncode para evitar inyección de scripts maliciosos.")
    ]
    for title_cg, desc_cg in codegen_items:
        p = tf4_r.add_paragraph()
        p.text = f"• {title_cg}: {desc_cg}"
        p.font.name = FONT_BODY
        p.font.size = Pt(11)
        p.font.color.rgb = TEXT_MAIN
        p.space_after = Pt(6)

    # Mini badge estético de tecnología
    p = tf4_r.add_paragraph()
    p.text = "Target Web: HTML5  •  CSS3 Variables  •  JavaScript ES6 (Client-Side)"
    p.font.name = FONT_BODY
    p.font.size = Pt(10.5)
    p.font.bold = True
    p.font.color.rgb = TEAL_ACCENT

    add_footer(s4, 4, speaker_name="Díaz Guadalupe")
    set_slide_notes(s4, (
        "Una vez superado el análisis sintáctico, entra en juego la verificación semántica contextual, implementada en nuestra clase SymbolTable.cs. "
        "Como sabemos, una gramática libre de contexto no puede verificar reglas de negocio o coherencia semántica. Nosotros implementamos cinco "
        "validaciones fundamentales: "
        "Primero, controlamos que no existan enunciados de preguntas duplicados dentro del examen, normalizando el texto con conjuntos hash. "
        "Segundo, en las preguntas de opción múltiple, exigimos que exista exactamente una opción marcada como [CORRECTA]; si no hay ninguna o si se "
        "marcan dos, se dispara un error semántico con línea y columna exactas. "
        "Tercero, chequeamos que no haya letras de opciones repetidas y advertimos si la correlatividad alfabética se rompe. "
        "Cuarto, validamos que el examen contenga al menos una pregunta. "
        "Y quinto, en las reglas globales, verificamos que el PUNTAJE_MINIMO no supere el total de puntos acumulados de todas las preguntas del examen. "
        "Si todas estas comprobaciones pasan sin errores, el control pasa a nuestro generador de código, CodeGen.cs. Este componente recorre el modelo "
        "AST y traduce la evaluación a una página web única y autocontenida: un archivo .html que incluye estilos CSS3 responsivos y la lógica de validación "
        "en JavaScript cliente puro. Sanitizamos cada cadena mediante WebUtility.HtmlEncode para prevenir cualquier vulnerabilidad de inyección de código. "
        "El estudiante o docente puede abrir este archivo en cualquier navegador, sin conexión y sin instalar nada, y obtener corrección instantánea y "
        "puntaje acumulado. "
        "Para mostrarles cómo responde el compilador en la práctica y ver la demo en vivo, le paso la palabra a Mateo Gallardo."
    ))

    # -------------------------------------------------------------------------
    # DIAPOSITIVA 5: EJECUCIÓN, BATERÍA DE PRUEBAS Y DEMO EN TERMINAL
    # -------------------------------------------------------------------------
    s5 = create_slide(prs)
    add_header(s5, "Ejecución, Batería de Pruebas y Demostración en Terminal", "Pruebas y Verificación")
    
    # Columna izquierda: Tabla de casos de prueba y resumen
    add_card(s5, Inches(0.8), Inches(1.5), Inches(5.8), Inches(5.2))
    tb5_l = s5.shapes.add_textbox(Inches(1.0), Inches(1.7), Inches(5.4), Inches(4.8))
    tf5_l = tb5_l.text_frame
    tf5_l.word_wrap = True
    tf5_l.margin_left = tf5_l.margin_top = tf5_l.margin_right = tf5_l.margin_bottom = 0

    p = tf5_l.paragraphs[0]
    p.text = "Suite de Pruebas Automatizadas (run_tests.ps1)"
    p.font.name = FONT_HEADING
    p.font.size = Pt(13)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    p.space_after = Pt(6)

    p = tf5_l.add_paragraph()
    p.text = "• 13 casos de prueba con verificación de salida exacta (.expected):"
    p.font.name = FONT_BODY
    p.font.size = Pt(11)
    p.font.color.rgb = TEXT_MAIN
    p.space_after = Pt(4)

    test_summary = [
        ("4 Casos Válidos (ExitCode 0)", "basico, comentarios, compiladores2026, completo (generan HTML funcional)."),
        ("1 Caso Error Léxico (ExitCode 1)", "error_lexico (carácter arroba @ no reconocido)."),
        ("2 Casos Error Sintáctico (ExitCode 1)", "falta_llave (bloque sin cerrar), falta_titulo (sintaxis EXAMEN rota)."),
        ("6 Casos Error Semántico (ExitCode 1)", "enunciado_duplicado, sin_preguntas, sin_correcta, varias_correctas, letra_repetida, puntaje_minimo_excesivo.")
    ]
    for cat, desc in test_summary:
        p = tf5_l.add_paragraph()
        p.text = f"  - {cat}: {desc}"
        p.font.name = FONT_BODY
        p.font.size = Pt(10.5)
        p.font.color.rgb = TEXT_MAIN
        p.space_after = Pt(3)

    p = tf5_l.add_paragraph()
    p.text = "\nResultado de la Suite de Automatización:"
    p.font.name = FONT_BODY
    p.font.size = Pt(11)
    p.font.bold = True
    p.font.color.rgb = DARK_NAVY
    p.space_after = Pt(2)

    p = tf5_l.add_paragraph()
    p.text = "Resultados: 13/13 pruebas pasaron (PASS). [100% Éxito]"
    p.font.name = FONT_CODE
    p.font.size = Pt(11.5)
    p.font.bold = True
    p.font.color.rgb = SUCCESS_GREEN

    # Columna derecha: Salidas reales de error y Pase a la Demo
    add_card(s5, Inches(6.8), Inches(1.5), Inches(5.733), Inches(5.2))
    tb5_r = s5.shapes.add_textbox(Inches(7.0), Inches(1.7), Inches(5.333), Inches(4.8))
    tf5_r = tb5_r.text_frame
    tf5_r.word_wrap = True
    tf5_r.margin_left = tf5_r.margin_top = tf5_r.margin_right = tf5_r.margin_bottom = 0

    p = tf5_r.paragraphs[0]
    p.text = "Salidas Reales de Detección de Errores"
    p.font.name = FONT_HEADING
    p.font.size = Pt(13)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    p.space_after = Pt(6)

    err_samples = """LÉXICO: linea 3 col 1: caracter o simbolo lexico no reconocido
SINTÁCTICO: linea 6 col 1: se esperaba "}"
SEMÁNTICO: linea 4 col 19: La pregunta múltiple "¿Cuál es la respuesta correcta?" no tiene ninguna opción marcada como [CORRECTA].
GLOBAL: linea 2 col 16: El puntaje mínimo (10) es mayor al total de puntos del examen (1).
"""
    add_code_block(s5, Inches(7.0), Inches(2.2), Inches(5.333), Inches(2.2), err_samples, font_size=Pt(8.5))

    # Banner destacado de Demostración en Vivo
    demo_banner = add_card(s5, Inches(7.0), Inches(4.7), Inches(5.333), Inches(1.8), bg_color=PRIMARY_LIGHT, border_color=PRIMARY_INDIGO)
    tb_db = s5.shapes.add_textbox(Inches(7.2), Inches(4.85), Inches(4.933), Inches(1.5))
    tf_db = tb_db.text_frame
    tf_db.word_wrap = True
    p = tf_db.paragraphs[0]
    p.text = "DEMOSTRACIÓN EN VIVO (POR TERMINAL)"
    p.font.name = FONT_HEADING
    p.font.size = Pt(12)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_DARK
    p.space_after = Pt(4)

    steps = (
        "1. Regeneración del parser con Coco/R (tools/generar.ps1)\n"
        "2. Compilación de examen válido y apertura en navegador\n"
        "3. Verificación de errores en vivo (léxico, sintáctico, semántico)\n"
        "4. Ejecución del runner de pruebas de regresión (run_tests.ps1)"
    )
    p = tf_db.add_paragraph()
    p.text = steps
    p.font.name = FONT_BODY
    p.font.size = Pt(10)
    p.font.color.rgb = TEXT_MAIN

    add_footer(s5, 5, speaker_name="Gallardo Mateo")
    set_slide_notes(s5, (
        "Buenas tardes a todos. Gracias, Guada. "
        "Para garantizar la robustez del compilador construimos una suite integral de trece casos de prueba automatizados, que cubren tanto caminos "
        "felices como todas las categorías de falla previstas en el diseño: "
        "Contamos con cuatro casos válidos: un examen básico, uno con comentarios, uno exhaustivo y un examen completo sobre la materia Compiladores 2026. "
        "Y diseñamos nueve casos con errores provocados intencionalmente: errores léxicos por caracteres no reconocidos, errores sintácticos por falta de "
        "llaves o de títulos, y toda la gama de errores semánticos que recién explicó Guadalupe. "
        "Cada prueba cuenta con su archivo .expected, y nuestro script de automatización run_tests.ps1 compila la solución en .NET 10 y contrasta las salidas "
        "y códigos de retorno, alcanzando un resultado exitoso de trece de trece pruebas pasadas. "
        "Ahora quiero invitar a los profesores a mirar la terminal para realizar la demostración en vivo de estos componentes: "
        "[DEMO EN TERMINAL]: "
        "1) Ejecuto tools/generar.ps1: Coco/R verifica la gramática y emite 0 errors detected. "
        "2) Compilo compiladores2026.quiz y abro el HTML generado: muestro interactividad, corrección en cliente y cálculo de puntaje. "
        "3) Compilo error_lexico.quiz (muestra linea 3 col 1: caracter o simbolo lexico no reconocido). "
        "4) Compilo error_sintactico_falta_llave.quiz (muestra linea 6 col 1: se esperaba '}'). "
        "5) Compilo sem_multiple_sin_correcta.quiz (muestra regla de [CORRECTA]). "
        "6) Corro run_tests.ps1 mostrando la suite completa en verde. "
        "Con esto demostramos que el núcleo del compilador no solo compila sin advertencias, sino que es plenamente determinista y tolerante a fallos. "
        "Ahora le paso la palabra a Abel Reinoso para comentarles cómo trabajamos con la Inteligencia Artificial y cuáles son nuestros próximos pasos."
    ))

    # -------------------------------------------------------------------------
    # DIAPOSITIVA 6: METODOLOGÍA CON IA GENERATIVA Y AUDITORÍA CRÍTICA
    # -------------------------------------------------------------------------
    s6 = create_slide(prs)
    add_header(s6, "Metodología con IA Generativa y Auditoría Crítica", "Inteligencia Artificial")
    
    # Columna izquierda: Arquitectura de IA y Bitácora
    add_card(s6, Inches(0.8), Inches(1.5), Inches(5.8), Inches(5.2))
    tb6_l = s6.shapes.add_textbox(Inches(1.0), Inches(1.7), Inches(5.4), Inches(4.8))
    tf6_l = tb6_l.text_frame
    tf6_l.word_wrap = True
    tf6_l.margin_left = tf6_l.margin_top = tf6_l.margin_right = tf6_l.margin_bottom = 0

    p = tf6_l.paragraphs[0]
    p.text = "Entorno Agentic Antigravity y Roles"
    p.font.name = FONT_HEADING
    p.font.size = Pt(13)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    p.space_after = Pt(8)

    roles = [
        ("Agente Supervisor (Claude Opus)", "Planificación arquitectónica, partición de tareas, auditoría crítica de código y contraste conceptual con bibliografía canónica."),
        ("Subagentes Operativos (Gemini Flash)", "Búsqueda preliminar, generación de código repetitivo (clases AST, plantillas CSS/HTML), scripts PowerShell y casos de prueba iniciales."),
        ("Equipo de Estudiantes (Reinoso, Albornoz, Díaz, Gallardo)", "Toma de decisiones de diseño, revisión línea por línea, validación experimental empírica y asimilación conceptual para la defensa."),
        ("Bitácora Transparente (docs/bitacora_ia.md)", "Registro cronológico estricto de prompts, respuestas generadas y contrastación humana realizada por el equipo.")
    ]
    for r_title, r_desc in roles:
        p = tf6_l.add_paragraph()
        p.text = f"• {r_title}: {r_desc}"
        p.font.name = FONT_BODY
        p.font.size = Pt(10.5)
        p.font.color.rgb = TEXT_MAIN
        p.space_after = Pt(6)

    # Columna derecha: Verificación Crítica y Alucinación Detectada
    add_card(s6, Inches(6.8), Inches(1.5), Inches(5.733), Inches(5.2))
    tb6_r = s6.shapes.add_textbox(Inches(7.0), Inches(1.7), Inches(5.333), Inches(4.8))
    tf6_r = tb6_r.text_frame
    tf6_r.word_wrap = True
    tf6_r.margin_left = tf6_r.margin_top = tf6_r.margin_right = tf6_r.margin_bottom = 0

    p = tf6_r.paragraphs[0]
    p.text = "Verificación Crítica: Casos de Desvío de la IA"
    p.font.name = FONT_HEADING
    p.font.size = Pt(13)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    p.space_after = Pt(8)

    p = tf6_r.add_paragraph()
    p.text = "1. Detección de Cita Bibliográfica Alucinada"
    p.font.name = FONT_BODY
    p.font.size = Pt(11.5)
    p.font.bold = True
    p.font.color.rgb = ERROR_RED
    p.space_after = Pt(3)

    p = tf6_r.add_paragraph()
    p.text = "• Un subagente sugirió una referencia a un libro ('Compilerbau: Grundlagen...', Hanser 2018). El equipo auditó catálogos oficiales de la editorial y bases académicas: el libro no existía. Se documentó y mantuvo con etiqueta [VERIFICAR] como evidencia ética de verificación crítica humana."
    p.font.name = FONT_BODY
    p.font.size = Pt(10.5)
    p.font.color.rgb = TEXT_MAIN
    p.space_after = Pt(8)

    p = tf6_r.add_paragraph()
    p.text = "2. Corrección de Reglas de Conjunto en Coco/R"
    p.font.name = FONT_BODY
    p.font.size = Pt(11.5)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_DARK
    p.space_after = Pt(3)

    p = tf6_r.add_paragraph()
    p.text = "• La IA intentó restar conjuntos en TOKENS (ANY - '\"'). Coco/R falló; el equipo estudió el manual de Mössenböck y reestructuró la gramática declarando stringChar en CHARACTERS."
    p.font.name = FONT_BODY
    p.font.size = Pt(10.5)
    p.font.color.rgb = TEXT_MAIN
    p.space_after = Pt(8)

    p = tf6_r.add_paragraph()
    p.text = "3. Falsa Sensación de Seguridad de los Modelos"
    p.font.name = FONT_BODY
    p.font.size = Pt(11.5)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_DARK
    p.space_after = Pt(3)

    p = tf6_r.add_paragraph()
    p.text = "• El modelo declaró el proyecto '100% verificado'; el supervisor y el equipo demostraron que sólo se habían probado los casos nominales, exigiendo validación de bordes e independencia de pruebas."
    p.font.name = FONT_BODY
    p.font.size = Pt(10.5)
    p.font.color.rgb = TEXT_MAIN

    add_footer(s6, 6, speaker_name="Reinoso Abel")
    set_slide_notes(s6, (
        "Muchas gracias, Mateo. Buenas tardes a todos. "
        "En esta segunda mitad del proyecto, como pauta pedagógica y ética fundamental, integramos herramientas de Inteligencia Artificial generativa "
        "bajo una metodología rigurosa y auditable. "
        "No utilizamos la IA mediante consultas aisladas en chats genéricos, sino que configuramos un entorno agentic estructurado en Antigravity. "
        "En este esquema, tuvimos un Agente Supervisor (Claude Opus) que nos ayudó a desglosar las tareas arquitectónicas y auditar la coherencia teórica, "
        "y Subagentes Operativos (Gemini Flash) que usamos para prototipar rápidamente clases del AST, escribir la plantilla HTML de salida y preparar "
        "los casos de prueba iniciales. Todas estas interacciones están documentadas minuciosamente en nuestra bitácora docs/bitacora_ia.md. "
        "Pero el aspecto más valioso para nuestra formación fue el ejercicio de verificación y auditoría crítica. La IA comete errores y puede inducir a una "
        "falsa sensación de seguridad. Vivenciamos dos ejemplos muy claros: "
        "A nivel técnico, cuando la IA propuso restar caracteres directamente en la sección TOKENS, Coco/R falló de inmediato; tuvimos que acudir al manual "
        "original de Mössenböck para comprender que las operaciones de conjuntos en Cocol/R pertenecen exclusivamente a la sección CHARACTERS. "
        "Y a nivel conceptual y bibliográfico, un subagente nos sugirió una cita de un supuesto libro de compiladores de la editorial Hanser del año 2018. "
        "Al realizar la búsqueda académica manual comprobamos que el libro no existía en ningún catálogo: era una referencia alucinada. Siguiendo nuestro "
        "protocolo ético, mantuvimos la señalización de alerta y la descartamos de las fuentes válidas. "
        "La conclusión que nos deja esta experiencia es que la IA es un acelerador técnico potente, pero la validez teórica y la responsabilidad final "
        "descansan siempre en el criterio humano de los estudiantes."
    ))

    # -------------------------------------------------------------------------
    # DIAPOSITIVA 7: ESTADO DE ENTREGABLES Y PRÓXIMOS PASOS (HACIA LA FINAL)
    # -------------------------------------------------------------------------
    s7 = create_slide(prs)
    add_header(s7, "Estado de Entregables y Próximos Pasos (Hacia la Final)", "Planificación y Cronograma")
    
    # Columna izquierda: Matriz de Estado de Entregables
    add_card(s7, Inches(0.8), Inches(1.5), Inches(5.8), Inches(5.2))
    tb7_l = s7.shapes.add_textbox(Inches(1.0), Inches(1.7), Inches(5.4), Inches(4.8))
    tf7_l = tb7_l.text_frame
    tf7_l.word_wrap = True
    tf7_l.margin_left = tf7_l.margin_top = tf7_l.margin_right = tf7_l.margin_bottom = 0

    p = tf7_l.paragraphs[0]
    p.text = "Estado de Entregables al Parcial 2"
    p.font.name = FONT_HEADING
    p.font.size = Pt(13)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    p.space_after = Pt(8)

    items_status = [
        ("Código Fuente del Compilador (Core + CLI)", "COMPLETO (0 errores, 0 warnings, 13/13 tests)", SUCCESS_GREEN),
        ("Documento de Diseño del Compilador", "COMPLETO (EBNF, LL(1), Semántica, Diagramas)", SUCCESS_GREEN),
        ("Manual de Usuario (Sintaxis y CLI)", "COMPLETO (Guía de sintaxis y compilación)", SUCCESS_GREEN),
        ("Interfaz Gráfica (GUI WinForms)", "EN ETAPA DE PULIDO (se presenta en la final)", PRIMARY_INDIGO),
        ("Informe Final Integrado", "BORRADOR EN REVISIÓN (consolidación de secciones)", TEAL_ACCENT),
        ("Video Demostrativo y Diapositivas Finales", "PENDIENTE (programado post-parcial)", TEXT_MUTED)
    ]
    for ent, est, col in items_status:
        p = tf7_l.add_paragraph()
        p.text = f"• {ent}:"
        p.font.name = FONT_BODY
        p.font.size = Pt(10.5)
        p.font.bold = True
        p.font.color.rgb = DARK_NAVY
        p.space_after = Pt(1)

        p_sub = tf7_l.add_paragraph()
        p_sub.text = f"   Estado: {est}"
        p_sub.font.name = FONT_BODY
        p_sub.font.size = Pt(10)
        p_sub.font.color.rgb = col
        p_sub.space_after = Pt(5)

    # Columna derecha: Cronograma y Mejoras Planificadas hacia la Final
    add_card(s7, Inches(6.8), Inches(1.5), Inches(5.733), Inches(5.2))
    tb7_r = s7.shapes.add_textbox(Inches(7.0), Inches(1.7), Inches(5.333), Inches(4.8))
    tf7_r = tb7_r.text_frame
    tf7_r.word_wrap = True
    tf7_r.margin_left = tf7_r.margin_top = tf7_r.margin_right = tf7_r.margin_bottom = 0

    p = tf7_r.paragraphs[0]
    p.text = "Mejoras Planificadas hacia la Final (10/11/2026)"
    p.font.name = FONT_HEADING
    p.font.size = Pt(13)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    p.space_after = Pt(8)

    roadmap = [
        ("Visualizador de Fases en GUI", "Panel interactivo en Windows Forms para inspeccionar la tabla de tokens del escáner, el árbol sintáctico (AST) y la tabla de símbolos."),
        ("Demo Multidispositivo con QR", "Generación de código QR local para proyectar y permitir que docentes y alumnos respondan el examen generado desde el celular."),
        ("Recuperación de Errores con SYNC/WEAK", "Incorporar directivas SYNC y WEAK de Coco/R para que el parser no aborte en la primera falla y continúe analizando las preguntas siguientes."),
        ("Extensiones Opcionales del Lenguaje", "Soporte de preguntas de emparejamiento, preguntas numéricas con margen de tolerancia y orden aleatorio opcional."),
        ("Cierre Documental y Video", "Revisión final cruzada del informe escrito y grabación del video explicativo formal para la defensa final.")
    ]
    for r_tit, r_txt in roadmap:
        p = tf7_r.add_paragraph()
        p.text = f"• {r_tit}: {r_txt}"
        p.font.name = FONT_BODY
        p.font.size = Pt(10.5)
        p.font.color.rgb = TEXT_MAIN
        p.space_after = Pt(6)

    add_footer(s7, 7, speaker_name="Reinoso Abel")
    set_slide_notes(s7, (
        "Llegando a la situación actual del proyecto, en esta tabla resumimos con total transparencia el estado de avance de cada entregable: "
        "El código fuente del compilador y la CLI están al cien por ciento terminados y probados. "
        "El documento formal de diseño del compilador y el manual de usuario están completamente redactados. "
        "El informe final integral está en fase de borrador consolidado y revisión de estilo. "
        "Y nuestra interfaz gráfica de usuario en Windows Forms, como adelantamos, está desarrollada y se encuentra actualmente en etapa de pulido visual "
        "y de integración, lista para ser exhibida en la entrega final. "
        "Nuestro cronograma de trabajo de aquí al diez de noviembre contempla cinco mejoras clave: "
        "1. Enriquecer la interfaz gráfica con un panel interactivo de visualización de fases: inspección de la lista de tokens emitidos por el escáner, "
        "visualización del árbol sintáctico y estado de la tabla de símbolos. "
        "2. Implementar una demostración en vivo con código QR proyectado en pantalla, para que los docentes y compañeros puedan escanearlo y responder "
        "el examen compilado directamente desde sus teléfonos celulares. "
        "3. Extender opcionalmente el lenguaje con preguntas numéricas y emparejamiento. "
        "4. Incorporar recuperación de errores avanzada mediante las directivas SYNC y WEAK que provee Coco/R. "
        "5. Y realizar el cierre definitivo de la monografía y la grabación del video institucional."
    ))

    # -------------------------------------------------------------------------
    # DIAPOSITIVA 8: CONCLUSIONES Y ESPACIO DE PREGUNTAS
    # -------------------------------------------------------------------------
    s8 = create_slide(prs)
    add_header(s8, "Conclusiones del Avance y Espacio de Preguntas", "Cierre y Devoluciones")
    
    # Tarjeta central ancha: Síntesis de logros
    add_card(s8, Inches(0.8), Inches(1.5), Inches(11.733), Inches(5.2))
    tb8 = s8.shapes.add_textbox(Inches(1.2), Inches(1.8), Inches(10.933), Inches(4.5))
    tf8 = tb8.text_frame
    tf8.word_wrap = True
    tf8.margin_left = tf8.margin_top = tf8.margin_right = tf8.margin_bottom = 0

    p = tf8.paragraphs[0]
    p.text = "Hitos Clave Consolidados en la Parte 2"
    p.font.name = FONT_HEADING
    p.font.size = Pt(15)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_INDIGO
    p.space_after = Pt(12)

    hitos = [
        "✓ Lenguaje de Dominio Específico Funcional: DSL declarativo con sintaxis limpia para exámenes interactivos.",
        "✓ Gramática Atribuida LL(1) Rigurosa: validada con Coco/R sin conflictos ni ambigüedades (0 errors detected).",
        "✓ Arquitectura Robusta en .NET 10: separación limpia entre Core (librería), Cli (consola) y Gui (Windows Forms).",
        "✓ Verificación Semántica y Seguridad: 5 reglas contextuales y prevención estricta contra inyección XSS.",
        "✓ Suite Automatizada 100% Exitosa: 13/13 pruebas PASS con detección milimétrica de línea y columna.",
        "✓ Metodología Ética con IA: bitácora exhaustiva y auditoría humana crítica frente a citas alucinadas."
    ]
    for h in hitos:
        p = tf8.add_paragraph()
        p.text = h
        p.font.name = FONT_BODY
        p.font.size = Pt(12)
        p.font.color.rgb = TEXT_MAIN
        p.space_after = Pt(8)

    p_cierre = tf8.add_paragraph()
    p_cierre.text = "\n¿Preguntas, sugerencias o comentarios de la cátedra?"
    p_cierre.font.name = FONT_HEADING
    p_cierre.font.size = Pt(16)
    p_cierre.font.bold = True
    p_cierre.font.color.rgb = DARK_NAVY
    p_cierre.space_after = Pt(4)

    p_subcierre = tf8.add_paragraph()
    p_subcierre.text = "¡Muchas gracias por su atención! – Albornoz, Díaz, Gallardo y Reinoso"
    p_subcierre.font.name = FONT_BODY
    p_subcierre.font.size = Pt(12.5)
    p_subcierre.font.bold = True
    p_subcierre.font.color.rgb = TEAL_ACCENT

    add_footer(s8, 8, speaker_name="Equipo QuizLang (Cierre)")
    set_slide_notes(s8, (
        "Para concluir, estamos muy conformes con el grado de avance alcanzado en esta Parte 2. "
        "Logramos transformar una especificación abstracta en un compilador sólido, eficiente y matemáticamente riguroso, "
        "respaldado por la teoría de autómatas y gramáticas que aprendimos en la materia. "
        "Agradecemos a los docentes por su atención y quedamos los cuatro a entera disposición para responder a sus preguntas y recibir sus sugerencias "
        "y devoluciones. Muchas gracias."
    ))

    # Guardar presentación
    prs.save(output_path)
    print(f"Presentación generada exitosamente en: {output_path}")

if __name__ == "__main__":
    script_dir = os.path.dirname(os.path.abspath(__file__))
    out_file = os.path.join(script_dir, "Avance_Parcial2_QuizLang.pptx")
    build_presentation(out_file)
