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

namespace trifenix.agro.app.frm.mantenedores.barrack
{
    public partial class Frm : Form, IFenixForm
    {
        BackgroundWorker bworker;

        private string entityName;
        private readonly Specie currentSpecie;
        private readonly Season currentSeason;

        public CurrentFormState State { get; set; }

        public bool Loading { get; set; } = false;

        public Frm(Specie currentSpecie, Season currentSeason)
        {
            InitializeComponent();
            entityName = GetEntityName();
            pb.Maximum = 100;
            pb.Step = 1;
            bworker = new BackgroundWorker();

            bworker.WorkerReportsProgress = true;

            lblDescripcion.Text = Description();
            this.currentSpecie = currentSpecie;
            this.currentSeason = currentSeason;
        }
        private async void SectorFrm_Load_1(object sender, EventArgs e)
        {

            SetElements();



        }
        public void SetElements()
        {
            pb.Visible = true;
            lblProgress.Text = "40";
            pb.Value = 40;
            
           

            // bindingSources
            var varieties = Cloud.GetElements<Variety>($"index eq {(int)EntityRelated.VARIETY} and rel/any(element:element/id eq '{currentSpecie.Id}' and element/index eq {(int)EntityRelated.SPECIE })");
            var pollinatores = new List<Variety>() { new Variety { Id = "", Name = "Seleccione Polinizador" } };
            var rootStocks = new List<Rootstock>() { new Rootstock { Id = "", Name = "Seleccione Porta-injerto" } };

            pollinatores.AddRange(varieties);
            rootStocks.AddRange(Cloud.GetElements<Rootstock>(EntityRelated.ROOTSTOCK));

            var sectors = Cloud.GetElements<Sector>(EntityRelated.SECTOR);

            bsPolinator.DataSource = pollinatores;
            bsVariety.DataSource = varieties;
            bsSectors.DataSource = sectors;
            bsRootStock.DataSource = rootStocks;

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
            lblProgress.Text = "100";
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
            ValidationForm.SetError(tbx, string.IsNullOrWhiteSpace(tbx.Text) ? "campo obligatorio" : null);
        }






        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Valida())
            {
                ValidationForm.SetError(tbxName, "Descripción es obligatoria");
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
            lblProgress.Text = $"{e.ProgressPercentage}";
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
            if (bsSectors.Current == null)
            {
                ValidationForm.SetError(cbSector, "el sector es obligatorio");
                return false;
            }
            if (bsPlotland.Current == null)
            {
                ValidationForm.SetError(cbPlotland, "la parcela es obligatoria");
                return false;
            }
            if (bsVariety.Current == null)
            {
                ValidationForm.SetError(cbVariety, "Debe seleccionar una variedad");
                return false;
            }
            if (string.IsNullOrWhiteSpace(tbxAnioPlant.Text))
            {
                ValidationForm.SetError(tbxAnioPlant, "Indique el año de plantación");
                return false;
            }
            if (string.IsNullOrWhiteSpace(tbxHectares.Text))
            {
                ValidationForm.SetError(tbxAnioPlant, "Indique las hectareas del cuartel");
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbxNroPlants.Text))
            {
                ValidationForm.SetError(tbxNroPlants, "Indique el número de plantas");
                return false;
            }
            var anioPlant = int.Parse(tbxAnioPlant.Text);
            var currentYear = DateTime.Now.Year;

            if (anioPlant < 1950 || anioPlant > currentYear)
            {
                ValidationForm.SetError(tbxAnioPlant, "el año debe estar entre 1950 y el año actual");
                return false;
            }


            return true;
        }
        public string GetEntityName() => Cloud.GetCosmosEntityName<Barrack>();

        public string FriendlyName() => "Cuartel";



        public void Edit(object obj)
        {
            var current = (Barrack)obj;

            var currentVariety = (Variety)bsVariety.Current;
            var currentPollinator = (Variety)bsPolinator.Current;
            var currentRootstock = (Rootstock)bsRootStock.Current;
            var currentPlotland = (PlotLand)bsPlotland.Current;

            var plants = int.Parse(tbxNroPlants.Text);
            var anio = int.Parse(tbxAnioPlant.Text);
            var hectares = double.Parse(tbxHectares.Text);


            Cloud.PushElement(new BarrackInput { Name = tbxName.Text, Id = current.Id, Hectares = hectares, IdPlotLand = currentPlotland.Id, IdPollinator = currentPollinator.Id, IdRootstock = currentRootstock.Id, IdVariety = currentVariety.Id, NumberOfPlants = plants, SeasonId = currentSeason.Id, PlantingYear = anio }, entityName).Wait();

        }

        public void New()
        {

            var currentVariety = (Variety)bsVariety.Current;
            var currentPollinator = (Variety)bsPolinator.Current;
            var currentRootstock = (Rootstock)bsRootStock.Current;
            var currentPlotland = (PlotLand)bsPlotland.Current;

            var plants = int.Parse(tbxNroPlants.Text);
            var anio = int.Parse(tbxAnioPlant.Text);
            var hectares = double.Parse(tbxHectares.Text);


            Cloud.PushElement(new BarrackInput { Name = tbxName.Text, Hectares = hectares, IdPlotLand = currentPlotland.Id, IdPollinator = currentPollinator.Id, IdRootstock = currentRootstock.Id, IdVariety = currentVariety.Id, NumberOfPlants = plants, SeasonId = currentSeason.Id, PlantingYear = anio }, entityName).Wait();

        }

        public void ChangedList(object obj) {
            if (obj != null)
            {
                var current = (Barrack)obj;
                tbxCorrelativo.Text = current.ClientId.ToString();
                tbxName.Text = current.Name;
                gbxItem.Text = $"{FriendlyName()} {tbxName.Text}";

                bsPolinator.SelectItem(current.IdPollinator);

                var plotland = Cloud.GetElement<PlotLand>(EntityRelated.PLOTLAND, current.IdPlotLand);

                bsSectors.SelectItem(plotland.IdSector);
                bsPlotland.SelectItem(current.IdPlotLand);
                bsVariety.SelectItem(current.IdVariety);
                bsRootStock.SelectItem(current.IdRootstock);
                tbxAnioPlant.Text = current.PlantingYear.ToString();
                tbxHectares.Text = current.Hectares.ToString();
                tbxNroPlants.Text = current.NumberOfPlants.ToString();





            }
        }

        public object GetList() {

            var queryVariety = $"index eq {(int)EntityRelated.VARIETY} and rel/any(elem:elem/id eq '{currentSpecie.Id}' and elem/index eq {(int)EntityRelated.SPECIE})";
            var varieties = Cloud.GetElements<Variety>(queryVariety);

            if (!varieties.Any()) return new List<Barrack>();


            var join = string.Join(",", varieties.Select(s => s.Id));


            var query = $"index eq {(int)EntityRelated.BARRACK} and rel/any(elem:elem/id eq '{currentSeason.Id}' and elem/index eq {(int)EntityRelated.SEASON}) and rel/any(element:search.in(element/id,'{join}') and element/index eq {(int)EntityRelated.VARIETY})";
            return Cloud.GetElements<Barrack>(query);
        }

        private void gbxItem_Enter(object sender, EventArgs e)
        {

        }

        public string Description() => new MdmDocs().GetInfoFromEntity((int)EntityRelated.BARRACK).Description;

        private void tbxHectares_KeyPress(object sender, KeyPressEventArgs e)
        { 
            
            e.Handled = Validaciones.ValidaDecimalParaKeyPress(((TextBox)sender).Text, e.KeyChar, 4, 2);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Validaciones.ValidaEnteroParaKeyPress(e.KeyChar);
        }

        private void tbxNroPlants_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validaciones.ValidaEnteroParaKeyPress(e.KeyChar);
        }

        private void bsSectors_CurrentChanged_1(object sender, EventArgs e)
        {
            var currentSector = (Sector)bsSectors.Current;
            if (currentSector!=null)
            {
                var plotlands = Cloud.GetElements<PlotLand>($"index eq {(int)EntityRelated.PLOTLAND} and rel/any(element:element/index eq {(int)EntityRelated.SECTOR} and element/id eq '{currentSector.Id}')");
                bsPlotland.DataSource = plotlands;
                bsPlotland.ResetBindings(true);
            }
            
        }

        
    }
}
