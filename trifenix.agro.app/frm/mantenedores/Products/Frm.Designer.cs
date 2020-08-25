

namespace trifenix.agro.app.frm.mantenedores.product
{
    partial class Frm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ErrorProvider epValidation;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm));
            this.bsMain = new System.Windows.Forms.BindingSource(this.components);
            this.tbxCorrelativo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxName = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.gbxItem = new System.Windows.Forms.GroupBox();
            this.pnlDoses = new System.Windows.Forms.Panel();
            this.btnEditDoses = new System.Windows.Forms.Button();
            this.btnRemoveDoses = new System.Windows.Forms.Button();
            this.btnAddDoses = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.hoursToReEntryToBarrackDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.applicationDaysIntervalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numberOfSequentialApplicationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wettingRecommendedByHectaresDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dosesQuantityMinDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dosesQuantityMaxDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.waitingDaysLabelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dosesApplicatedToDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dosesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.cbKindMeasure = new System.Windows.Forms.ComboBox();
            this.tbxBrand = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbActiveIngredient = new System.Windows.Forms.ComboBox();
            this.bsIngredient = new System.Windows.Forms.BindingSource(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lbxItems = new System.Windows.Forms.ListBox();
            this.btnDeleteSector = new System.Windows.Forms.Button();
            this.btnEditSector = new System.Windows.Forms.Button();
            this.btnAddSector = new System.Windows.Forms.Button();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            epValidation = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(epValidation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsMain)).BeginInit();
            this.panel1.SuspendLayout();
            this.gbxItem.SuspendLayout();
            this.pnlDoses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dosesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsIngredient)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // epValidation
            // 
            epValidation.ContainerControl = this;
            epValidation.DataSource = this.bsMain;
            // 
            // bsMain
            // 
            this.bsMain.DataSource = typeof(trifenix.connect.agro_model_input.ProductInput);
            this.bsMain.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.bsMain_AddingNew);
            this.bsMain.CurrentChanged += new System.EventHandler(this.bsSectors_CurrentChanged);
            // 
            // tbxCorrelativo
            // 
            this.tbxCorrelativo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsMain, "ClientId", true));
            this.tbxCorrelativo.Enabled = false;
            this.tbxCorrelativo.Location = new System.Drawing.Point(23, 53);
            this.tbxCorrelativo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxCorrelativo.Name = "tbxCorrelativo";
            this.tbxCorrelativo.Size = new System.Drawing.Size(73, 22);
            this.tbxCorrelativo.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Correlativo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(115, 30);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Nombre :";
            // 
            // tbxName
            // 
            this.tbxName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsMain, "Name", true));
            this.tbxName.Location = new System.Drawing.Point(119, 53);
            this.tbxName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxName.MaxLength = 200;
            this.tbxName.Name = "tbxName";
            this.tbxName.Size = new System.Drawing.Size(443, 22);
            this.tbxName.TabIndex = 3;
            this.tbxName.Validated += new System.EventHandler(this.tbxName_Validated);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.lblDescripcion);
            this.panel1.Location = new System.Drawing.Point(0, -2);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1263, 66);
            this.panel1.TabIndex = 4;
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(16, 12);
            this.lblDescripcion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(82, 17);
            this.lblDescripcion.TabIndex = 5;
            this.lblDescripcion.Text = "Descripción";
            // 
            // gbxItem
            // 
            this.gbxItem.Controls.Add(this.pnlDoses);
            this.gbxItem.Controls.Add(this.dataGridView1);
            this.gbxItem.Controls.Add(this.label5);
            this.gbxItem.Controls.Add(this.cbKindMeasure);
            this.gbxItem.Controls.Add(this.tbxBrand);
            this.gbxItem.Controls.Add(this.label4);
            this.gbxItem.Controls.Add(this.label3);
            this.gbxItem.Controls.Add(this.cbActiveIngredient);
            this.gbxItem.Controls.Add(this.tbxName);
            this.gbxItem.Controls.Add(this.tbxCorrelativo);
            this.gbxItem.Controls.Add(this.btnCancel);
            this.gbxItem.Controls.Add(this.label1);
            this.gbxItem.Controls.Add(this.label2);
            this.gbxItem.Controls.Add(this.btnSave);
            this.gbxItem.Enabled = false;
            this.gbxItem.Location = new System.Drawing.Point(284, 71);
            this.gbxItem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbxItem.Name = "gbxItem";
            this.gbxItem.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbxItem.Size = new System.Drawing.Size(962, 610);
            this.gbxItem.TabIndex = 5;
            this.gbxItem.TabStop = false;
            this.gbxItem.Text = "{0}";
            this.gbxItem.Visible = false;
            this.gbxItem.Enter += new System.EventHandler(this.gbxItem_Enter);
            this.gbxItem.Validating += new System.ComponentModel.CancelEventHandler(this.gbxItem_Validating);
            // 
            // pnlDoses
            // 
            this.pnlDoses.Controls.Add(this.btnEditDoses);
            this.pnlDoses.Controls.Add(this.btnRemoveDoses);
            this.pnlDoses.Controls.Add(this.btnAddDoses);
            this.pnlDoses.Location = new System.Drawing.Point(758, 546);
            this.pnlDoses.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlDoses.Name = "pnlDoses";
            this.pnlDoses.Size = new System.Drawing.Size(178, 30);
            this.pnlDoses.TabIndex = 34;
            // 
            // btnEditDoses
            // 
            this.btnEditDoses.BackgroundImage = global::trifenix.agro.app.Properties.Resources.edit_icon;
            this.btnEditDoses.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEditDoses.Location = new System.Drawing.Point(99, 3);
            this.btnEditDoses.Name = "btnEditDoses";
            this.btnEditDoses.Size = new System.Drawing.Size(30, 23);
            this.btnEditDoses.TabIndex = 32;
            this.btnEditDoses.UseVisualStyleBackColor = true;
            this.btnEditDoses.Click += new System.EventHandler(this.btnEditDoses_Click);
            // 
            // btnRemoveDoses
            // 
            this.btnRemoveDoses.BackgroundImage = global::trifenix.agro.app.Properties.Resources.deleteIcon;
            this.btnRemoveDoses.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRemoveDoses.Location = new System.Drawing.Point(134, 2);
            this.btnRemoveDoses.Name = "btnRemoveDoses";
            this.btnRemoveDoses.Size = new System.Drawing.Size(30, 23);
            this.btnRemoveDoses.TabIndex = 33;
            this.btnRemoveDoses.UseVisualStyleBackColor = true;
            // 
            // btnAddDoses
            // 
            this.btnAddDoses.BackgroundImage = global::trifenix.agro.app.Properties.Resources.add_icon;
            this.btnAddDoses.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddDoses.Location = new System.Drawing.Point(62, 2);
            this.btnAddDoses.Name = "btnAddDoses";
            this.btnAddDoses.Size = new System.Drawing.Size(30, 23);
            this.btnAddDoses.TabIndex = 31;
            this.btnAddDoses.UseVisualStyleBackColor = true;
            this.btnAddDoses.Click += new System.EventHandler(this.btnAddDoses_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.hoursToReEntryToBarrackDataGridViewTextBoxColumn,
            this.applicationDaysIntervalDataGridViewTextBoxColumn,
            this.numberOfSequentialApplicationDataGridViewTextBoxColumn,
            this.wettingRecommendedByHectaresDataGridViewTextBoxColumn,
            this.dosesQuantityMinDataGridViewTextBoxColumn,
            this.dosesQuantityMaxDataGridViewTextBoxColumn,
            this.waitingDaysLabelDataGridViewTextBoxColumn,
            this.dosesApplicatedToDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.dosesBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(27, 154);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(910, 387);
            this.dataGridView1.TabIndex = 19;
            // 
            // hoursToReEntryToBarrackDataGridViewTextBoxColumn
            // 
            this.hoursToReEntryToBarrackDataGridViewTextBoxColumn.DataPropertyName = "HoursToReEntryToBarrack";
            this.hoursToReEntryToBarrackDataGridViewTextBoxColumn.HeaderText = "Horas de Reentrada";
            this.hoursToReEntryToBarrackDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.hoursToReEntryToBarrackDataGridViewTextBoxColumn.Name = "hoursToReEntryToBarrackDataGridViewTextBoxColumn";
            this.hoursToReEntryToBarrackDataGridViewTextBoxColumn.ReadOnly = true;
            this.hoursToReEntryToBarrackDataGridViewTextBoxColumn.Width = 150;
            // 
            // applicationDaysIntervalDataGridViewTextBoxColumn
            // 
            this.applicationDaysIntervalDataGridViewTextBoxColumn.DataPropertyName = "ApplicationDaysInterval";
            this.applicationDaysIntervalDataGridViewTextBoxColumn.HeaderText = "Intervalo entre aplicación";
            this.applicationDaysIntervalDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.applicationDaysIntervalDataGridViewTextBoxColumn.Name = "applicationDaysIntervalDataGridViewTextBoxColumn";
            this.applicationDaysIntervalDataGridViewTextBoxColumn.ReadOnly = true;
            this.applicationDaysIntervalDataGridViewTextBoxColumn.Width = 150;
            // 
            // numberOfSequentialApplicationDataGridViewTextBoxColumn
            // 
            this.numberOfSequentialApplicationDataGridViewTextBoxColumn.DataPropertyName = "NumberOfSequentialApplication";
            this.numberOfSequentialApplicationDataGridViewTextBoxColumn.HeaderText = "N° Secuencias";
            this.numberOfSequentialApplicationDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.numberOfSequentialApplicationDataGridViewTextBoxColumn.Name = "numberOfSequentialApplicationDataGridViewTextBoxColumn";
            this.numberOfSequentialApplicationDataGridViewTextBoxColumn.ReadOnly = true;
            this.numberOfSequentialApplicationDataGridViewTextBoxColumn.Width = 150;
            // 
            // wettingRecommendedByHectaresDataGridViewTextBoxColumn
            // 
            this.wettingRecommendedByHectaresDataGridViewTextBoxColumn.DataPropertyName = "WettingRecommendedByHectares";
            this.wettingRecommendedByHectaresDataGridViewTextBoxColumn.HeaderText = "Mojamiento Recomendado";
            this.wettingRecommendedByHectaresDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.wettingRecommendedByHectaresDataGridViewTextBoxColumn.Name = "wettingRecommendedByHectaresDataGridViewTextBoxColumn";
            this.wettingRecommendedByHectaresDataGridViewTextBoxColumn.ReadOnly = true;
            this.wettingRecommendedByHectaresDataGridViewTextBoxColumn.Width = 150;
            // 
            // dosesQuantityMinDataGridViewTextBoxColumn
            // 
            this.dosesQuantityMinDataGridViewTextBoxColumn.DataPropertyName = "DosesQuantityMin";
            this.dosesQuantityMinDataGridViewTextBoxColumn.HeaderText = "Dosis Mínima";
            this.dosesQuantityMinDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.dosesQuantityMinDataGridViewTextBoxColumn.Name = "dosesQuantityMinDataGridViewTextBoxColumn";
            this.dosesQuantityMinDataGridViewTextBoxColumn.ReadOnly = true;
            this.dosesQuantityMinDataGridViewTextBoxColumn.Width = 150;
            // 
            // dosesQuantityMaxDataGridViewTextBoxColumn
            // 
            this.dosesQuantityMaxDataGridViewTextBoxColumn.DataPropertyName = "DosesQuantityMax";
            this.dosesQuantityMaxDataGridViewTextBoxColumn.HeaderText = "Dosis Máxima";
            this.dosesQuantityMaxDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.dosesQuantityMaxDataGridViewTextBoxColumn.Name = "dosesQuantityMaxDataGridViewTextBoxColumn";
            this.dosesQuantityMaxDataGridViewTextBoxColumn.ReadOnly = true;
            this.dosesQuantityMaxDataGridViewTextBoxColumn.Width = 150;
            // 
            // waitingDaysLabelDataGridViewTextBoxColumn
            // 
            this.waitingDaysLabelDataGridViewTextBoxColumn.DataPropertyName = "WaitingDaysLabel";
            this.waitingDaysLabelDataGridViewTextBoxColumn.HeaderText = "Días Previo Cosecha Etiqueta";
            this.waitingDaysLabelDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.waitingDaysLabelDataGridViewTextBoxColumn.Name = "waitingDaysLabelDataGridViewTextBoxColumn";
            this.waitingDaysLabelDataGridViewTextBoxColumn.ReadOnly = true;
            this.waitingDaysLabelDataGridViewTextBoxColumn.Width = 150;
            // 
            // dosesApplicatedToDataGridViewTextBoxColumn
            // 
            this.dosesApplicatedToDataGridViewTextBoxColumn.DataPropertyName = "DosesApplicatedTo";
            this.dosesApplicatedToDataGridViewTextBoxColumn.HeaderText = "Litros en aplicación";
            this.dosesApplicatedToDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.dosesApplicatedToDataGridViewTextBoxColumn.Name = "dosesApplicatedToDataGridViewTextBoxColumn";
            this.dosesApplicatedToDataGridViewTextBoxColumn.ReadOnly = true;
            this.dosesApplicatedToDataGridViewTextBoxColumn.Width = 150;
            // 
            // dosesBindingSource
            // 
            this.dosesBindingSource.DataMember = "Doses";
            this.dosesBindingSource.DataSource = this.bsMain;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(308, 81);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 17);
            this.label5.TabIndex = 14;
            this.label5.Text = "Tipo Medida:";
            // 
            // cbKindMeasure
            // 
            this.cbKindMeasure.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bsMain, "MeasureType", true));
            this.cbKindMeasure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKindMeasure.FormattingEnabled = true;
            this.cbKindMeasure.Items.AddRange(new object[] {
            "KL (Kilo)",
            "LT (Litros)"});
            this.cbKindMeasure.Location = new System.Drawing.Point(311, 103);
            this.cbKindMeasure.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbKindMeasure.Name = "cbKindMeasure";
            this.cbKindMeasure.Size = new System.Drawing.Size(199, 24);
            this.cbKindMeasure.TabIndex = 13;
            // 
            // tbxBrand
            // 
            this.tbxBrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsMain, "Brand", true));
            this.tbxBrand.Location = new System.Drawing.Point(589, 53);
            this.tbxBrand.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxBrand.MaxLength = 200;
            this.tbxBrand.Name = "tbxBrand";
            this.tbxBrand.Size = new System.Drawing.Size(348, 22);
            this.tbxBrand.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(586, 30);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Marca :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 81);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Ingrediente Activo:";
            // 
            // cbActiveIngredient
            // 
            this.cbActiveIngredient.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bsMain, "IdActiveIngredient", true));
            this.cbActiveIngredient.DataSource = this.bsIngredient;
            this.cbActiveIngredient.DisplayMember = "Name";
            this.cbActiveIngredient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbActiveIngredient.FormattingEnabled = true;
            this.cbActiveIngredient.Location = new System.Drawing.Point(27, 107);
            this.cbActiveIngredient.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbActiveIngredient.Name = "cbActiveIngredient";
            this.cbActiveIngredient.Size = new System.Drawing.Size(260, 24);
            this.cbActiveIngredient.TabIndex = 9;
            this.cbActiveIngredient.ValueMember = "Id";
            // 
            // bsIngredient
            // 
            this.bsIngredient.DataSource = typeof(trifenix.connect.agro_model.Ingredient);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(731, 574);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.Location = new System.Drawing.Point(838, 574);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(99, 28);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Guardar";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lbxItems
            // 
            this.lbxItems.DataSource = this.bsMain;
            this.lbxItems.DisplayMember = "Name";
            this.lbxItems.FormattingEnabled = true;
            this.lbxItems.ItemHeight = 16;
            this.lbxItems.Location = new System.Drawing.Point(18, 84);
            this.lbxItems.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbxItems.Name = "lbxItems";
            this.lbxItems.Size = new System.Drawing.Size(257, 564);
            this.lbxItems.TabIndex = 9;
            this.lbxItems.ValueMember = "Id";
            // 
            // btnDeleteSector
            // 
            this.btnDeleteSector.BackgroundImage = global::trifenix.agro.app.Properties.Resources.deleteIcon;
            this.btnDeleteSector.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDeleteSector.Location = new System.Drawing.Point(134, 2);
            this.btnDeleteSector.Name = "btnDeleteSector";
            this.btnDeleteSector.Size = new System.Drawing.Size(30, 23);
            this.btnDeleteSector.TabIndex = 33;
            this.btnDeleteSector.UseVisualStyleBackColor = true;
            // 
            // btnEditSector
            // 
            this.btnEditSector.BackgroundImage = global::trifenix.agro.app.Properties.Resources.edit_icon;
            this.btnEditSector.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEditSector.Location = new System.Drawing.Point(99, 3);
            this.btnEditSector.Name = "btnEditSector";
            this.btnEditSector.Size = new System.Drawing.Size(30, 23);
            this.btnEditSector.TabIndex = 32;
            this.btnEditSector.UseVisualStyleBackColor = true;
            this.btnEditSector.Click += new System.EventHandler(this.btnEditSector_Click);
            // 
            // btnAddSector
            // 
            this.btnAddSector.BackgroundImage = global::trifenix.agro.app.Properties.Resources.add_icon;
            this.btnAddSector.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddSector.Location = new System.Drawing.Point(62, 2);
            this.btnAddSector.Name = "btnAddSector";
            this.btnAddSector.Size = new System.Drawing.Size(30, 23);
            this.btnAddSector.TabIndex = 31;
            this.btnAddSector.UseVisualStyleBackColor = true;
            this.btnAddSector.Click += new System.EventHandler(this.btnAddSector_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnEditSector);
            this.pnlButtons.Controls.Add(this.btnDeleteSector);
            this.pnlButtons.Controls.Add(this.btnAddSector);
            this.pnlButtons.Enabled = false;
            this.pnlButtons.Location = new System.Drawing.Point(100, 651);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(178, 30);
            this.pnlButtons.TabIndex = 4;
            // 
            // pb
            // 
            this.pb.Location = new System.Drawing.Point(1140, 688);
            this.pb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(89, 18);
            this.pb.TabIndex = 9;
            this.pb.Visible = false;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(1081, 688);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblProgress.Size = new System.Drawing.Size(46, 17);
            this.lblProgress.TabIndex = 10;
            this.lblProgress.Text = "label3";
            this.lblProgress.Visible = false;
            // 
            // Frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1258, 709);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.pb);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.lbxItems);
            this.Controls.Add(this.gbxItem);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Productos";
            this.Load += new System.EventHandler(this.SectorFrm_Load_1);
            ((System.ComponentModel.ISupportInitialize)(epValidation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsMain)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbxItem.ResumeLayout(false);
            this.gbxItem.PerformLayout();
            this.pnlDoses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dosesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsIngredient)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxCorrelativo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.GroupBox gbxItem;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox lbxItems;
        private System.Windows.Forms.Button btnDeleteSector;
        private System.Windows.Forms.Button btnEditSector;
        private System.Windows.Forms.Button btnAddSector;
        private System.Windows.Forms.BindingSource bsMain;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.ProgressBar pb;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbActiveIngredient;
        private System.Windows.Forms.BindingSource bsIngredient;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbKindMeasure;
        private System.Windows.Forms.TextBox tbxBrand;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlDoses;
        private System.Windows.Forms.Button btnEditDoses;
        private System.Windows.Forms.Button btnRemoveDoses;
        private System.Windows.Forms.Button btnAddDoses;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn hoursToReEntryToBarrackDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn applicationDaysIntervalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfSequentialApplicationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn wettingRecommendedByHectaresDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dosesQuantityMinDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dosesQuantityMaxDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn waitingDaysLabelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dosesApplicatedToDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource dosesBindingSource;
    }
}