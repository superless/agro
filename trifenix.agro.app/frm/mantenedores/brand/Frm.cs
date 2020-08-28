using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using trifenix.agro.app.interfaces;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.resources;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.app.cloud;

namespace trifenix.agro.app.frm.mantenedores.brand
{
    public partial class Frm : Form, IFenixForm
    {
        BackgroundWorker bworker;
        private bool _addinNew = false;
        private string entityName;

        MapperConfiguration config = new MapperConfiguration(cfg => {
            cfg.CreateMap<Brand, BrandInput>();
        });


        public CurrentFormState State { get; set; }

        public bool Loading { get; set; } = false;

        public Frm()
        {
            InitializeComponent();
            entityName = GetEntityName();
            pb.Maximum = 100;
            pb.Step = 1;
            bworker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };

            lblDescripcion.Text = Description();
        }

        private async void SectorFrm_Load_1(object sender, EventArgs e)
        {            
            SetElements();
        }

        public void SetElements()
        {
            pb.Visible = true;
            lblProgress.Text = "40%";
            pb.Value = 40;
            
            pb.Value = 100;
            lblProgress.Text = "100%";
            pb.Visible = false;
            lblProgress.Visible = false;
            Loading = false;
            var lst = new List<Brand>();
            lst.AddRange(GetList() as IEnumerable<Brand>);
            var bl = new BindingList<Brand>(lst);
            bl.AllowNew = true;
            bl.AllowEdit = true;
            bl.AllowRemove = true;

            bsMain.DataSource = bl;


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

        }


        

        private void OnAdd()
        {
            gbxItem.Visible = true;
            gbxItem.Enabled = true;
            gbxItem.Text = $"Nuevo {FriendlyName()}";
            tbxName.Text = "";
            State = CurrentFormState.NEW;
            pnlButtons.Enabled = false;
            bsMain.AddNew();

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
            bsMain.CancelEdit();
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

            if (!_addinNew)
            {
                OnCurrentChange();
            }
            _addinNew = false;

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
        public string GetEntityName() => Cloud.GetCosmosEntityName<Brand>();

        public string FriendlyName() => "Marca";

        

        public void Edit(object obj)
        {
            var current = (Brand)obj;
            var mapper = config.CreateMapper();

            var input = mapper.Map<BrandInput>(current);

            Cloud.PushElement(input, entityName).Wait();
            
        }

        public void New()
        {
            var current = (Brand)bsMain.Current;

            var mapper = config.CreateMapper();
            var input = mapper.Map<BrandInput>(current);

            Cloud.PushElement(input, entityName).Wait();
         
        }

        public void ChangedList(object obj) {
            if (obj!=null)
            {
                var current = (Sector)obj;
                tbxCorrelativo.Text = current.ClientId.ToString();
                tbxName.Text = current.Name;
                gbxItem.Text = $"Marca {tbxName.Text}";
            }
        }

        public object GetList() => Cloud.GetElements<Brand>(EntityRelated.BRAND);

        public string Description() => new MdmDocs().GetInfoFromEntity((int)EntityRelated.BRAND).Description;

        private void gbxItem_Enter(object sender, EventArgs e)
        {

        }

        private void bsMain_AddingNew(object sender, AddingNewEventArgs e)
        {
            _addinNew = true;
        }
    }
}
