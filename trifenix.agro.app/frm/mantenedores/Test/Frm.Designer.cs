namespace trifenix.agro.app.frm.mantenedores.Test
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm));
            this.tbxCorrelativo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxName = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.gbxItem = new System.Windows.Forms.GroupBox();
            this.tbxAbbreviation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lbxItems = new System.Windows.Forms.ListBox();
            this.bsMain = new System.Windows.Forms.BindingSource(this.components);
            this.ValidationForm = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnDeleteSector = new System.Windows.Forms.Button();
            this.btnEditSector = new System.Windows.Forms.Button();
            this.btnAddSector = new System.Windows.Forms.Button();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.gbxItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ValidationForm)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxCorrelativo
            // 
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
            this.panel1.Size = new System.Drawing.Size(883, 66);
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
            this.gbxItem.Controls.Add(this.tbxAbbreviation);
            this.gbxItem.Controls.Add(this.label3);
            this.gbxItem.Controls.Add(this.tbxName);
            this.gbxItem.Controls.Add(this.tbxCorrelativo);
            this.gbxItem.Controls.Add(this.btnCancel);
            this.gbxItem.Controls.Add(this.label1);
            this.gbxItem.Controls.Add(this.label2);
            this.gbxItem.Controls.Add(this.btnSave);
            this.gbxItem.Controls.Add(this.tbxBrand);
            this.gbxItem.Controls.Add(this.label4);
            this.gbxItem.Enabled = false;
            this.gbxItem.Location = new System.Drawing.Point(284, 71);
            this.gbxItem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbxItem.Name = "gbxItem";
            this.gbxItem.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbxItem.Size = new System.Drawing.Size(583, 416);
            this.gbxItem.TabIndex = 5;
            this.gbxItem.TabStop = false;
            this.gbxItem.Text = "{0}";
            this.gbxItem.Visible = false;
            this.gbxItem.Enter += new System.EventHandler(this.gbxItem_Enter);
            // 
            // tbxAbbreviation
            // 
            this.tbxAbbreviation.Location = new System.Drawing.Point(23, 114);
            this.tbxAbbreviation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxAbbreviation.MaxLength = 10;
            this.tbxAbbreviation.Name = "tbxAbbreviation";
            this.tbxAbbreviation.Size = new System.Drawing.Size(106, 22);
            this.tbxAbbreviation.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 94);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "Abreviación :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(170, 111);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label3";
            this.label4.Size = new System.Drawing.Size(90, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Marca :";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(362, 383);
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
            this.btnSave.Location = new System.Drawing.Point(469, 383);
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
            this.lbxItems.Size = new System.Drawing.Size(257, 372);
            this.lbxItems.TabIndex = 9;
            this.lbxItems.ValueMember = "Id";
            // 
            // bsMain
            // 
            this.bsMain.DataSource = typeof(trifenix.connect.agro_model.Specie);
            this.bsMain.CurrentChanged += new System.EventHandler(this.bsSectors_CurrentChanged);
            // 
            // ValidationForm
            // 
            this.ValidationForm.ContainerControl = this;
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
            this.pnlButtons.Location = new System.Drawing.Point(97, 462);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(178, 30);
            this.pnlButtons.TabIndex = 4;
            // 
            // pb
            // 
            this.pb.Location = new System.Drawing.Point(779, 489);
            this.pb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(89, 18);
            this.pb.TabIndex = 9;
            this.pb.Visible = false;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(728, 491);
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
            this.ClientSize = new System.Drawing.Size(883, 516);
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
            this.Text = "Especie";
            this.Load += new System.EventHandler(this.SectorFrm_Load_1);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbxItem.ResumeLayout(false);
            this.gbxItem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ValidationForm)).EndInit();
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
        private System.Windows.Forms.ErrorProvider ValidationForm;
        private System.Windows.Forms.Button btnDeleteSector;
        private System.Windows.Forms.Button btnEditSector;
        private System.Windows.Forms.Button btnAddSector;
        private System.Windows.Forms.BindingSource bsMain;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.ProgressBar pb;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.TextBox tbxAbbreviation;
        private System.Windows.Forms.TextBox tbxBrand;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}