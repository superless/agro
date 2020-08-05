using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using trifenix.agro.app.helper;
using trifenix.agro.app.interfaces;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.resources;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.app.cloud;

namespace trifenix.agro.app.frm.mantenedores.ingredient
{
    public partial class Frm : Form, IFenixForm
    {
        BackgroundWorker bworker;

        private string entityName;

        public CurrentFormState State { get; set; }

        public bool Loading { get; set; } = false;

        public Frm()
        {
            InitializeComponent();
            entityName = GetEntityName();
            pb.Maximum = 100;
            pb.Step = 1;
            bworker = new BackgroundWorker();

            bworker.WorkerReportsProgress = true;

            lblDescripcion.Text = Description();


        }
        private async void SectorFrm_Load_1(object sender, EventArgs e)
        {
            
       
            
            

            SetElements();

            
            
        }
        public void SetElements()
        {
            bsIngredientCategory.DataSource = Cloud.GetElements<IngredientCategory>(EntityRelated.CATEGORY_INGREDIENT);

            pb.Visible = true;
            lblProgress.Text = "40%";
            pb.Value = 40;
            bsMain.DataSource = GetList();
            if (bsMain.Count != 0)
            {
                gbxItem.Visible = true;
                gbxItem.Enabled = false;
                pnlButtons.Enabled = true;
                btnEditSector.Enabled = true;
                btnDeleteSector.Enabled = true;
            }
            else
            {
                pnlButtons.Enabled = true;
                btnEditSector.Enabled = false;
                btnDeleteSector.Enabled = false;
            }
            pb.Value = 100;
            lblProgress.Text = "100%";
            pb.Visible = false;
            lblProgress.Visible = false;
            Loading = false;

        }


        

        private void OnAdd()
        {
            gbxItem.Visible = true;
            gbxItem.Enabled = true;
            gbxItem.Text = $"Nuevo {FriendlyName()}";
            tbxName.Text = "";
            State = CurrentFormState.NEW;
            pnlButtons.Enabled = false;
        }

        private void OnEdit()
        {
            gbxItem.Visible = true;
            gbxItem.Enabled = true;
            gbxItem.Text = $"Nuevo {FriendlyName()}";
            State = CurrentFormState.EDIT;
            pnlButtons.Enabled = false;
        }

        private void OnCurrentChange() {
            gbxItem.Visible = true;
            gbxItem.Enabled = false;
            pnlButtons.Enabled = !Loading;
        }

        
        

        






        private void tbxName_Validated(object sender, EventArgs e)
        {
            var tbx = ((TextBox)sender);
            ValidationForm.SetError(tbx, String.IsNullOrWhiteSpace(tbx.Text) ? "campo obligatorio" : null);
        }

        


        

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Valida())
            {
                ValidationForm.SetError(tbxName, "Descripción es obligatorio");
                return;
            }
            LoadProgress(DoWork);

            

        }

        private void LoadProgress(Action action) {
            bworker.ProgressChanged += bworker_ProgressChanged;

            bworker.DoWork += Bworker_DoWork;

            bworker.RunWorkerAsync(action);

            bworker.WorkerReportsProgress = true;

            Task.Run(() =>
            {
                bworker.ReportProgress(0);
            }).Wait();
            bworker.RunWorkerCompleted += Bworker_RunWorkerCompleted;
        }

        private void Bworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pb.Value = 100;
            lblProgress.Text = "";
            lblProgress.Visible = false;
            pb.Visible = false;

            bworker = new BackgroundWorker();
            bworker.WorkerReportsProgress = true;
        }

        private void Bworker_DoWork(object sender, DoWorkEventArgs e)
        {
            bworker.ReportProgress(10);
            System.Threading.Thread.Sleep(300);
            bworker.ReportProgress(30);
            System.Threading.Thread.Sleep(300);
            bworker.ReportProgress(60);
            Action fobj = (Action)e.Argument;
            fobj.Invoke();
            bworker.ReportProgress(100);
        }

        

        

        private void btnAddSector_Click(object sender, EventArgs e)
        {
            OnAdd();
            


        }

        private void btnEditSector_Click(object sender, EventArgs e)
        {
            OnEdit();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            gbxItem.Visible = true;
            gbxItem.Enabled = false;
            gbxItem.Text = "";
            State = CurrentFormState.READONLY;
            pnlButtons.Enabled = true;
        }

        

        private void bworker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            btnSave.Enabled = e.ProgressPercentage == 100;
            btnCancel.Enabled = e.ProgressPercentage == 100;
            pnlButtons.Enabled = e.ProgressPercentage == 100;
            pb.Visible = e.ProgressPercentage != 100;
            lblProgress.Visible = true;
            lblProgress.Text = $"{e.ProgressPercentage}%";
            pb.Value = e.ProgressPercentage;
        }


        private void bsSectors_CurrentChanged(object sender, EventArgs e)
        {
            ChangedList(bsMain.Current);
            OnCurrentChange();
        }

        public void DoWork()
        {
            Loading = true;
            if (State == CurrentFormState.NEW)
            {
                New();
                while (Loading)
                {
                    Thread.Sleep(300);
                }

            }
            else if (State == CurrentFormState.EDIT)
            {

                Edit(bsMain.Current);
                
                while (Loading)
                {
                    Thread.Sleep(300);
                }
            }
            else
            {
                MessageBox.Show("Operación no permitida");
                return;
            }
            Loading = false;
        }


        public bool Valida()
        {
            if (string.IsNullOrWhiteSpace(tbxName.Text))
            {
                ValidationForm.SetError(tbxName, "El nombre es obligatorio");
                return false;
            };
            return true;
        }
        public string GetEntityName() => Cloud.GetCosmosEntityName<Ingredient>();

        public string FriendlyName() => "Ingrediente";

        

        public void Edit(object obj)
        {
            var current = (CostCenter)obj;
            var currentBusinessName = (BusinessName)bsIngredientCategory.Current;

            var currentCategory = (Ingredient)bsIngredientCategory.Current;
            Cloud.PushElement(new IngredientInput { Id = current.Id, Name = tbxName.Text, idCategory = currentCategory.Id }, entityName).Wait();

        }

        public void New()
        {
            var currentCategory = (Ingredient)bsIngredientCategory.Current;
            Cloud.PushElement(new IngredientInput { Name = tbxName.Text, idCategory = currentCategory.Id  }, entityName).Wait();
         
        }

        public void ChangedList(object obj) {
            if (obj!=null)
            {
                var current = (Ingredient)obj;
                tbxCorrelativo.Text = current.ClientId.ToString();
                tbxName.Text = current.Name;
                gbxItem.Text = $"{FriendlyName()} {tbxName.Text}";
                //business names
                bsIngredientCategory.SelectItem(current.idCategory);
                
            }
        }

        public object GetList() => Cloud.GetElements<Ingredient>(EntityRelated.INGREDIENT);
        public string Description() => new MdmDocs().GetInfoFromEntity((int)EntityRelated.INGREDIENT).Description;
        private void gbxItem_Enter(object sender, EventArgs e)
        {

        }
    }
}
