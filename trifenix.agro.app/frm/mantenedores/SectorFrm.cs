using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using trifenix.connect.agro_model;
using trifenix.connect.app.cloud;

namespace trifenix.agro.app.frm.mantenedores
{
    public partial class SectorFrm : Form
    {

        public CurrentFormState State { get; set; }
        public SectorFrm(Sector[] sectors)
        {
            InitializeComponent();
            bsSectors.DataSource = sectors;
            if (sectors.Any())
            {

                gbxSector.Visible = true;
                gbxSector.Enabled = false;

            }
            else {
                pnlButtons.Enabled = true;
                btnEditSector.Enabled = false;
                btnDeleteSector.Enabled = false;
                
            }
           

        }

        public void SetSectors(Sector[] sectors) {
            bsSectors.DataSource = sectors;
            btnSave.BackgroundImage = null;
            btnSave.Text = "Guardar";


        }



        private void Sector_Load(object sender, EventArgs e)
        {
            ValidationForm.SetError(tbxName, null);
        }

        private void tbxName_Validated(object sender, EventArgs e)
        {
            var tbx = ((TextBox)sender);
            ValidationForm.SetError(tbx, String.IsNullOrWhiteSpace(tbx.Text) ? "Descripción es obligatorio" : null);
        }

        private bool Valida() {
            var tbxNameText = tbxName.Text;
            return !string.IsNullOrWhiteSpace(tbxNameText);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!Valida())
            {
                ValidationForm.SetError(tbxName, "Descripción es obligatorio");
                return;
            }

            if (State == CurrentFormState.NEW)
            {
                await Cloud.CreateSector(tbxName.Text);
                btnSave.Text = "";
                btnSave.BackgroundImage = Properties.Resources._09b24e31234507_564a1d23c07b4;
                gbxSector.Enabled = false;
            }
            else if (State == CurrentFormState.EDIT)
            {
                var current = (Sector)bsSectors.Current;
                current.Name = tbxName.Text;
                await Cloud.EditSector(current.Id, current.Name);
                btnSave.Text = "";
                btnSave.BackgroundImage = Properties.Resources._09b24e31234507_564a1d23c07b4;
                gbxSector.Enabled = false;
            }
            else {
                MessageBox.Show("Operación no permitida");
                return;
            }
        }

        public string SectorName { get; set; }

        private void btnAddSector_Click(object sender, EventArgs e)
        {
            gbxSector.Visible = true;
            gbxSector.Enabled = true;
            gbxSector.Text = "Nuevo Sector";
            State = CurrentFormState.NEW;
            pnlButtons.Enabled = false;
            


        }

        private void btnEditSector_Click(object sender, EventArgs e)
        {
            gbxSector.Visible = true;
            gbxSector.Enabled = true;
            gbxSector.Text = "Nuevo Sector";
            State = CurrentFormState.EDIT;
            pnlButtons.Enabled = false;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            gbxSector.Visible = true;
            gbxSector.Enabled = false;
            gbxSector.Text = "";
            State = CurrentFormState.READONLY;
            pnlButtons.Enabled = true;
        }

        private void bsSectors_CurrentChanged(object sender, EventArgs e)
        {
            var current = (Sector)bsSectors.Current;
            tbxCorrelativo.Text = current.ClientId.ToString();
            tbxName.Text = current.Name;
            gbxSector.Text = $"Sector {tbxName.Text}";

        }
    }
}
