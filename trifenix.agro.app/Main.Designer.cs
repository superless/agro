namespace trifenix.agro.app
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.procesosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.planFitoSanitarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mantenedoresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.superficieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sectorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.parcelaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cuartelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configuraciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lbxEvents = new System.Windows.Forms.ListBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.procesosToolStripMenuItem,
            this.mantenedoresToolStripMenuItem,
            this.configuraciónToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1671, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // procesosToolStripMenuItem
            // 
            this.procesosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.planFitoSanitarioToolStripMenuItem});
            this.procesosToolStripMenuItem.Name = "procesosToolStripMenuItem";
            this.procesosToolStripMenuItem.Size = new System.Drawing.Size(99, 29);
            this.procesosToolStripMenuItem.Text = "Procesos";
            // 
            // planFitoSanitarioToolStripMenuItem
            // 
            this.planFitoSanitarioToolStripMenuItem.Name = "planFitoSanitarioToolStripMenuItem";
            this.planFitoSanitarioToolStripMenuItem.Size = new System.Drawing.Size(251, 34);
            this.planFitoSanitarioToolStripMenuItem.Text = "Plan FitoSanitario";
            this.planFitoSanitarioToolStripMenuItem.Click += new System.EventHandler(this.planFitoSanitarioToolStripMenuItem_Click);
            // 
            // mantenedoresToolStripMenuItem
            // 
            this.mantenedoresToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.superficieToolStripMenuItem});
            this.mantenedoresToolStripMenuItem.Name = "mantenedoresToolStripMenuItem";
            this.mantenedoresToolStripMenuItem.Size = new System.Drawing.Size(142, 29);
            this.mantenedoresToolStripMenuItem.Text = "Mantenedores";
            // 
            // superficieToolStripMenuItem
            // 
            this.superficieToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sectorToolStripMenuItem,
            this.parcelaToolStripMenuItem,
            this.cuartelToolStripMenuItem});
            this.superficieToolStripMenuItem.Name = "superficieToolStripMenuItem";
            this.superficieToolStripMenuItem.Size = new System.Drawing.Size(191, 34);
            this.superficieToolStripMenuItem.Text = "Superficie";
            // 
            // sectorToolStripMenuItem
            // 
            this.sectorToolStripMenuItem.Name = "sectorToolStripMenuItem";
            this.sectorToolStripMenuItem.Size = new System.Drawing.Size(169, 34);
            this.sectorToolStripMenuItem.Text = "Sector";
            this.sectorToolStripMenuItem.Click += new System.EventHandler(this.sectorToolStripMenuItem_Click);
            // 
            // parcelaToolStripMenuItem
            // 
            this.parcelaToolStripMenuItem.Name = "parcelaToolStripMenuItem";
            this.parcelaToolStripMenuItem.Size = new System.Drawing.Size(169, 34);
            this.parcelaToolStripMenuItem.Text = "Parcela";
            // 
            // cuartelToolStripMenuItem
            // 
            this.cuartelToolStripMenuItem.Name = "cuartelToolStripMenuItem";
            this.cuartelToolStripMenuItem.Size = new System.Drawing.Size(169, 34);
            this.cuartelToolStripMenuItem.Text = "Cuartel";
            // 
            // configuraciónToolStripMenuItem
            // 
            this.configuraciónToolStripMenuItem.Name = "configuraciónToolStripMenuItem";
            this.configuraciónToolStripMenuItem.Size = new System.Drawing.Size(139, 29);
            this.configuraciónToolStripMenuItem.Text = "Configuración";
            // 
            // lbxEvents
            // 
            this.lbxEvents.FormattingEnabled = true;
            this.lbxEvents.ItemHeight = 20;
            this.lbxEvents.Location = new System.Drawing.Point(12, 49);
            this.lbxEvents.Name = "lbxEvents";
            this.lbxEvents.Size = new System.Drawing.Size(1620, 864);
            this.lbxEvents.TabIndex = 1;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1671, 952);
            this.Controls.Add(this.lbxEvents);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "Aresa App";
            this.Load += new System.EventHandler(this.Main_Load);
            this.Shown += new System.EventHandler(this.Main_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem procesosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem planFitoSanitarioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mantenedoresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configuraciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem superficieToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sectorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem parcelaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cuartelToolStripMenuItem;
        private System.Windows.Forms.ListBox lbxEvents;
    }
}

