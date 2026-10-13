using System.Drawing;
using System.Windows.Forms;

namespace QuizLang.Gui;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    private ToolStrip toolStrip;
    private ToolStripButton btnAbrir;
    private ToolStripButton btnGuardar;
    private ToolStripButton btnGuardarComo;
    private ToolStripSeparator sep1;
    private ToolStripButton btnCompilar;
    private ToolStripButton btnVerHtml;
    private ToolStripSeparator sep2;
    private ToolStripLabel lblEjemplos;
    private ToolStripComboBox cmbEjemplos;

    private SplitContainer splitMain;
    private SplitContainer splitEditorPreview;
    private Panel panelEditorContainer;
    private PictureBox picLineNumbers;
    private RichTextBox txtEditor;

    private Panel panelPreviewContainer;
    private Panel panelPreviewHeader;
    private Label lblPreviewHeader;
    private Button btnAbrirNavegador;
    private WebBrowser webBrowser;

    private GroupBox grpErrores;
    private DataGridView dgvErrores;
    private DataGridViewTextBoxColumn colTipo;
    private DataGridViewTextBoxColumn colLinea;
    private DataGridViewTextBoxColumn colColumna;
    private DataGridViewTextBoxColumn colMensaje;

    private StatusStrip statusStrip;
    private ToolStripStatusLabel lblEstado;
    private ToolStripStatusLabel lblPosicion;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.toolStrip = new ToolStrip();
        this.btnAbrir = new ToolStripButton();
        this.btnGuardar = new ToolStripButton();
        this.btnGuardarComo = new ToolStripButton();
        this.sep1 = new ToolStripSeparator();
        this.btnCompilar = new ToolStripButton();
        this.btnVerHtml = new ToolStripButton();
        this.sep2 = new ToolStripSeparator();
        this.lblEjemplos = new ToolStripLabel();
        this.cmbEjemplos = new ToolStripComboBox();

        this.splitMain = new SplitContainer();
        this.splitEditorPreview = new SplitContainer();
        this.panelEditorContainer = new Panel();
        this.picLineNumbers = new PictureBox();
        this.txtEditor = new RichTextBox();

        this.panelPreviewContainer = new Panel();
        this.panelPreviewHeader = new Panel();
        this.lblPreviewHeader = new Label();
        this.btnAbrirNavegador = new Button();
        this.webBrowser = new WebBrowser();

        this.grpErrores = new GroupBox();
        this.dgvErrores = new DataGridView();
        this.colTipo = new DataGridViewTextBoxColumn();
        this.colLinea = new DataGridViewTextBoxColumn();
        this.colColumna = new DataGridViewTextBoxColumn();
        this.colMensaje = new DataGridViewTextBoxColumn();

        this.statusStrip = new StatusStrip();
        this.lblEstado = new ToolStripStatusLabel();
        this.lblPosicion = new ToolStripStatusLabel();

        // 
        // toolStrip
        // 
        this.toolStrip.ImageScalingSize = new Size(20, 20);
        this.toolStrip.Items.AddRange(new ToolStripItem[] {
            this.btnAbrir,
            this.btnGuardar,
            this.btnGuardarComo,
            this.sep1,
            this.btnCompilar,
            this.btnVerHtml,
            this.sep2,
            this.lblEjemplos,
            this.cmbEjemplos
        });
        this.toolStrip.Location = new Point(0, 0);
        this.toolStrip.Name = "toolStrip";
        this.toolStrip.Size = new Size(1100, 28);
        this.toolStrip.TabIndex = 0;

        // btnAbrir
        this.btnAbrir.Text = "📂 Abrir...";
        this.btnAbrir.Click += new System.EventHandler(this.BtnAbrir_Click);

        // btnGuardar
        this.btnGuardar.Text = "💾 Guardar";
        this.btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);

        // btnGuardarComo
        this.btnGuardarComo.Text = "💾 Guardar Como...";
        this.btnGuardarComo.Click += new System.EventHandler(this.BtnGuardarComo_Click);

        // btnCompilar
        this.btnCompilar.Font = new Font(this.Font, FontStyle.Bold);
        this.btnCompilar.Text = "▶ Compilar";
        this.btnCompilar.Click += new System.EventHandler(this.BtnCompilar_Click);

        // btnVerHtml
        this.btnVerHtml.Text = "🌐 Ver HTML";
        this.btnVerHtml.Click += new System.EventHandler(this.BtnVerHtml_Click);

        // lblEjemplos
        this.lblEjemplos.Text = "Ejemplos precargados:";

        // cmbEjemplos
        this.cmbEjemplos.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbEjemplos.Size = new Size(200, 28);
        this.cmbEjemplos.SelectedIndexChanged += new System.EventHandler(this.CmbEjemplos_SelectedIndexChanged);

        // 
        // splitMain (Vertical: Editor/Preview arriba, Errores abajo)
        // 
        this.splitMain.Dock = DockStyle.Fill;
        this.splitMain.Orientation = Orientation.Horizontal;
        this.splitMain.SplitterDistance = 450;
        this.splitMain.SplitterWidth = 6;
        this.splitMain.Panel1.Controls.Add(this.splitEditorPreview);
        this.splitMain.Panel2.Controls.Add(this.grpErrores);

        // 
        // splitEditorPreview (Horizontal: Editor izquierda, Vista Previa derecha)
        // 
        this.splitEditorPreview.Dock = DockStyle.Fill;
        this.splitEditorPreview.SplitterDistance = 550;
        this.splitEditorPreview.SplitterWidth = 6;
        this.splitEditorPreview.Panel1.Controls.Add(this.panelEditorContainer);
        this.splitEditorPreview.Panel2.Controls.Add(this.panelPreviewContainer);

        // 
        // panelEditorContainer
        // 
        this.panelEditorContainer.Dock = DockStyle.Fill;
        this.panelEditorContainer.Controls.Add(this.txtEditor);
        this.panelEditorContainer.Controls.Add(this.picLineNumbers);

        // picLineNumbers
        this.picLineNumbers.Dock = DockStyle.Left;
        this.picLineNumbers.Width = 45;
        this.picLineNumbers.BackColor = Color.FromArgb(245, 245, 245);
        this.picLineNumbers.Paint += new PaintEventHandler(this.PicLineNumbers_Paint);

        // txtEditor
        this.txtEditor.Dock = DockStyle.Fill;
        this.txtEditor.Font = new Font("Consolas", 10.5F, FontStyle.Regular);
        this.txtEditor.WordWrap = false;
        this.txtEditor.AcceptsTab = true;
        this.txtEditor.HideSelection = false;
        this.txtEditor.TextChanged += new System.EventHandler(this.TxtEditor_TextChanged);
        this.txtEditor.VScroll += new System.EventHandler(this.TxtEditor_VScroll);
        this.txtEditor.SelectionChanged += new System.EventHandler(this.TxtEditor_SelectionChanged);

        // 
        // panelPreviewContainer
        // 
        this.panelPreviewContainer.Dock = DockStyle.Fill;
        this.panelPreviewContainer.Controls.Add(this.webBrowser);
        this.panelPreviewContainer.Controls.Add(this.panelPreviewHeader);

        // panelPreviewHeader
        this.panelPreviewHeader.Dock = DockStyle.Top;
        this.panelPreviewHeader.Height = 35;
        this.panelPreviewHeader.BackColor = Color.FromArgb(240, 243, 246);
        this.panelPreviewHeader.Controls.Add(this.lblPreviewHeader);
        this.panelPreviewHeader.Controls.Add(this.btnAbrirNavegador);

        // lblPreviewHeader
        this.lblPreviewHeader.Dock = DockStyle.Left;
        this.lblPreviewHeader.Font = new Font(this.Font, FontStyle.Bold);
        this.lblPreviewHeader.Text = "  Vista Previa HTML Autocontenida";
        this.lblPreviewHeader.TextAlign = ContentAlignment.MiddleLeft;
        this.lblPreviewHeader.AutoSize = true;

        // btnAbrirNavegador
        this.btnAbrirNavegador.Dock = DockStyle.Right;
        this.btnAbrirNavegador.Text = "Abrir en Navegador Externo ↗";
        this.btnAbrirNavegador.Width = 200;
        this.btnAbrirNavegador.Click += new System.EventHandler(this.BtnVerHtml_Click);

        // webBrowser
        this.webBrowser.Dock = DockStyle.Fill;
        this.webBrowser.ScriptErrorsSuppressed = true;

        // 
        // grpErrores
        // 
        this.grpErrores.Dock = DockStyle.Fill;
        this.grpErrores.Text = "Panel de Errores y Advertencias (Doble clic en un error para saltar a la línea)";
        this.grpErrores.Controls.Add(this.dgvErrores);

        // dgvErrores
        this.dgvErrores.Dock = DockStyle.Fill;
        this.dgvErrores.AllowUserToAddRows = false;
        this.dgvErrores.AllowUserToDeleteRows = false;
        this.dgvErrores.ReadOnly = true;
        this.dgvErrores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.dgvErrores.MultiSelect = false;
        this.dgvErrores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        this.dgvErrores.RowHeadersVisible = false;
        this.dgvErrores.Columns.AddRange(new DataGridViewColumn[] {
            this.colTipo,
            this.colLinea,
            this.colColumna,
            this.colMensaje
        });
        this.dgvErrores.CellDoubleClick += new DataGridViewCellEventHandler(this.DgvErrores_CellDoubleClick);

        this.colTipo.HeaderText = "Tipo";
        this.colTipo.Width = 90;
        this.colTipo.FillWeight = 15;

        this.colLinea.HeaderText = "Línea";
        this.colLinea.Width = 60;
        this.colLinea.FillWeight = 10;

        this.colColumna.HeaderText = "Columna";
        this.colColumna.Width = 70;
        this.colColumna.FillWeight = 12;

        this.colMensaje.HeaderText = "Mensaje";
        this.colMensaje.FillWeight = 63;

        // 
        // statusStrip
        // 
        this.statusStrip.Items.AddRange(new ToolStripItem[] {
            this.lblEstado,
            this.lblPosicion
        });
        this.statusStrip.Location = new Point(0, 680);
        this.statusStrip.Name = "statusStrip";
        this.statusStrip.Size = new Size(1100, 24);
        this.statusStrip.TabIndex = 2;

        this.lblEstado.Spring = true;
        this.lblEstado.TextAlign = ContentAlignment.MiddleLeft;
        this.lblEstado.Text = "Listo";

        this.lblPosicion.Text = "Línea: 1, Col: 1";

        // 
        // Form1
        // 
        this.AutoScaleDimensions = new SizeF(8F, 20F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(1100, 704);
        this.Controls.Add(this.splitMain);
        this.Controls.Add(this.statusStrip);
        this.Controls.Add(this.toolStrip);
        this.Name = "Form1";
        this.Text = "QuizLang IDE - Compilador de Exámenes";
        this.StartPosition = FormStartPosition.CenterScreen;
    }
}
