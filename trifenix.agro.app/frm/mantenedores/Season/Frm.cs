using AutoMapper;
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
using trifenix.agro.app.model_extend;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.resources;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.app.cloud;

namespace trifenix.agro.app.frm.mantenedores.season
{
    public partial class Frm : Form, IFenixForm
    {
        MapperConfiguration config = new MapperConfiguration(cfg => {
            cfg.CreateMap<Season, SeasonExtend>();
        });

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
        private void SectorFrm_Load_1(object sender, EventArgs e)
        {
            
            SetElements();

            
            
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
            if (trifenix.connect.util.Mdm.Reflection.IsEnumerable(bsMain.Current) && !((IEnumerable<Object>)bsMain.Current).Any())
            {
                return;
            }
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

        public void SetElements()
        {
            var costcenters = Cloud.GetElements<CostCenter>(EntityRelated.COSTCENTER);

            if (!costcenters.Any())
            {
                MessageBox.Show("Debe existir al menos un centro de costo para crear una temporada");
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }
            bsCostCenter.DataSource = costcenters;

            pb.Visible = true;
            lblProgress.Text = "40%";
            pb.Value = 40;
            
            //bindingSources
            var seasons = Cloud.GetElements<Season>(EntityRelated.SEASON);
            

            var mapper = config.CreateMapper();


            bsMain.DataSource = seasons.Select(s => {
                var eseason = mapper.Map<SeasonExtend>(s);
                var localCostCenter = costcenters.FirstOrDefault(a => a.Id.Equals(eseason.IdCostCenter));
                eseason.CostCenterName = $"{localCostCenter.Name} [desde: {eseason.StartDate:dd-MM-yyyy}, hasta : {eseason.EndDate:dd-MM-yyyy}]";
                return eseason;

            });

            
            
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
            State = CurrentFormState.NEW;
            pnlButtons.Enabled = false;
            DefaultFormatDates();
        }

        private void OnEdit()
        {
            gbxItem.Visible = true;
            gbxItem.Enabled = true;
            gbxItem.Text = $"Nuevo {FriendlyName()}";
            State = CurrentFormState.EDIT;
            cbCostCenter.Enabled = false;
            
            pnlButtons.Enabled = false;
        }

        private void OnCurrentChange()
        {
            gbxItem.Visible = true;
            gbxItem.Enabled = false;
            pnlButtons.Enabled = !Loading;
        }

        public bool Valida()
        {
            
            if (State == CurrentFormState.NEW)
            {
                var currentSeasons = ((IEnumerable<Season>)bsMain.DataSource).ToList();
                var currentCenterCost = (CostCenter)bsCostCenter.Current;
                if (currentSeasons.Any(s=>s.IdCostCenter.Equals(currentCenterCost.Id)))
                {
                    ValidationForm.SetError(dtEndSeason, "ya existe una temporada para este centro, solo puede editar");
                    return false;
                }

            }
            var initDate = dtStartSeason.Value;
            var endDate = dtEndSeason.Value;

            if (initDate.Year != DateTime.Now.Year)
            {
                ValidationForm.SetError(dtEndSeason, "Por el momento, las temporadas solo se pueden crear para el año actual");
                return false;
            }

            if (endDate<initDate)
            {
                ValidationForm.SetError(dtEndSeason, "la fecha de inicio no puede ser mayor a la final");
                return false;
            }
            var diff = (endDate - initDate).TotalDays;

            if (diff > 365)
            {
                ValidationForm.SetError(dtEndSeason, "No puede existir más de un año de diferencia entre las fechas");
                return false;
            }

            if (diff < 60)
            {
                ValidationForm.SetError(dtEndSeason, "Deben existir al menos 2 meses de diferencia");
                return false;
            }




            return true;
        }
        public string GetEntityName() => Cloud.GetCosmosEntityName<Season>();

        public string FriendlyName() => "Temporada";

        

        public void Edit(object obj)
        {
            var current = (SeasonExtend)obj;
            var currentCostCenter = (CostCenter)bsCostCenter.Current;

            
            Cloud.PushElement(new SeasonInput { Id = current.Id, Current = true, StartDate = dtStartSeason.Value, EndDate = dtEndSeason.Value, IdCostCenter = currentCostCenter.Id }, entityName).Wait();
            
        }

        public void New()
        {
            var currentCostCenter = (CostCenter)bsCostCenter.Current;


            Cloud.PushElement(new SeasonInput { Current = true, StartDate = dtStartSeason.Value.ToUniversalTime(), EndDate = dtEndSeason.Value.ToUniversalTime(), IdCostCenter = currentCostCenter.Id }, entityName).Wait();

        }

        public void ChangedList(object obj) {
            if (obj!=null)
            {
                var current = (SeasonExtend)obj;
                dtStartSeason.Value = current.StartDate;
                dtEndSeason.Value = current.EndDate;
                bsCostCenter.SelectItem(current.IdCostCenter);

                
              
                gbxItem.Text = $"Temporada {current.CostCenterName}";
            }
        }

        public object GetList() => Cloud.GetElements<Season>(EntityRelated.SEASON);

        public string Description() => new MdmDocs().GetInfoFromEntity((int)EntityRelated.SEASON).Description;

        public void DefaultFormatDates() {
            var currentYear = DateTime.Now.Year;
            var nextYear = currentYear + 1;
            dtStartSeason.Value = new DateTime(currentYear, 3, 1);
            dtEndSeason.Value = new DateTime(nextYear, 2, DateTime.DaysInMonth(nextYear, 2));
        }
    }
}
