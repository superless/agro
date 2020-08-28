using AutoMapper;
using Microsoft.Azure.Amqp.Framing;
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
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.resources;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.app.cloud;

namespace trifenix.agro.app.frm.mantenedores.doses
{
    public partial class Frm : Form
    {


        MapperConfiguration config = new MapperConfiguration(cfg => {
            cfg.CreateMap<Variety, VarieryExtend>();
        });

        MapperConfiguration configHarvest = new MapperConfiguration(cfg => {
            cfg.CreateMap<WaitingHarvestInput, WaitingHarvestExtend>();
        });

        



        public CurrentFormState State { get; set; }

        public bool Loading { get; set; } = false;

        private Specie[] _species = null;
        private VarieryExtend[] _variesties = null;
        private Variety[] _vrts = null;
        private ApplicationTarget[] _targets = null;
        private CertifiedEntity[] _certifieds = null;


        public Frm(DosesInput dosesInput, Specie[] sps, Variety[] vrts, ApplicationTarget[] targets, CertifiedEntity[] certs)
        {
            InitializeComponent();
            _vrts = vrts;
            _species = sps;
            _targets = targets;
            _certifieds = certs;

            if (dosesInput == null)
            {
                OnAdd();
            }
            else {
                
                OnEdit(dosesInput);
            }
            







            lblDescripcion.Text = Description();
            
        }
        private void SectorFrm_Load_1(object sender, EventArgs e)
        {
            
            SetElements();

            
            
        }
        public void SetElements()
        {  
            Loading = false;
            cbSizeContainer.DataSource = Enum.GetNames(typeof(DosesApplicatedTo)).
                Select(o => new { Text = o, Value = (DosesApplicatedTo)(Enum.Parse(typeof(DosesApplicatedTo), o)) }).ToList();
            cbSizeContainer.DisplayMember = "Text";
            cbSizeContainer.ValueMember = "Value";
        }

        public DosesInput DosesInput => (DosesInput)bsDoses.Current;

        

        private void OnAdd()
        {
            gbxItem.Visible = true;
            gbxItem.Enabled = true;
            gbxItem.Text = $"Nuevo {FriendlyName()}";
            
            State = CurrentFormState.NEW;

            var lst = new List<DosesInput>() { new DosesInput { 
                IdVarieties = Array.Empty<string>(),
                IdSpecies= Array.Empty<string>(),
                IdsApplicationTarget = Array.Empty<string>(),
                WaitingToHarvest = Array.Empty<WaitingHarvestInput>()
            } };
            var bl = new BindingList<DosesInput>(lst);
            bsDoses.DataSource = bl;
            

            bsSpecie.DataSource = GetSpecies();
            bsVariety.DataSource = GetVarieties();
            bsTarget.DataSource = GetTargets();
            bsCertifiedEntity.DataSource = GetCertifiedEntities();

            

        }

        private void OnEdit(DosesInput input)
        {
            gbxItem.Visible = true;
            gbxItem.Enabled = true;
            gbxItem.Text = $"Nuevo {FriendlyName()}";
            State = CurrentFormState.EDIT;
            var lst = new List<DosesInput>() { input };
            var bl = new BindingList<DosesInput>(lst);
            bsDoses.DataSource = bl;
            bsDoses.MoveFirst();
            UpdateSpecies();
            UpdateVarieties();
            UpdateCertified();
            UpdateTargets();
            cbCertifiedEntity.SelectedValue = input.DosesApplicatedTo;



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
            DialogResult = DialogResult.OK;
            Close();
            

        }

       

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        



       


        public bool Valida()
        {
           
            return true;
        }
        public string GetEntityName() => Cloud.GetCosmosEntityName<Dose>();

        public string FriendlyName() => "Dosis";

        

       
       


        

        public string Description() => new MdmDocs().GetInfoFromEntity((int)EntityRelated.DOSES).Description;

        private void gbxItem_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void tbxMinDoses_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Validaciones.ValidaDecimalParaKeyPress(((TextBox)sender).Text, e.KeyChar, 10, 2);
        }

        private void tbxWettingRecommended_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Validaciones.ValidaDecimalParaKeyPress(((TextBox)sender).Text, e.KeyChar, 10, 2);
        }


        
        private Specie[] GetSpecies() {

            
            return _species;


        }

        private Specie[] GetSpeciesFiltered()
        {
            var species = GetSpecies();
            return species.Where(sp => !DosesInput.IdSpecies.Any(di => di.Equals(sp.Id))).ToArray();
        }

        private VarieryExtend[] GetVarieties() {
            if (_variesties == null)
            {
                var mapper = config.CreateMapper();

                _variesties = _vrts.Select(s =>
                {
                    var mp = mapper.Map<VarieryExtend>(s);
                    var specie = _species.First(a => a.Id.Equals(mp.IdSpecie));
                    mp.FullName = $"{specie.Name} - {mp.Name}";
                    return mp;
                }).ToArray();
            }
            return _variesties;
        }

        private VarieryExtend[] GetFilteredVarieties() { 
            var varieties = GetVarieties();
            

            return varieties.Where(sp => !DosesInput.IdSpecies.Any(di => di.Equals(sp.IdSpecie)) && !DosesInput.IdVarieties.Any(di => di.Equals(sp.Id)))
                .ToArray();
        }

        private ApplicationTarget[] GetTargets() {

          
            return _targets;
        }
        private CertifiedEntity[] GetCertifiedEntities() {

            return _certifieds;
        }



        private ApplicationTarget[] GetFilteredTargets() {
            var targets = GetTargets();
            return targets.Where(tg => !DosesInput.IdsApplicationTarget.Any(idtg => tg.Id.Equals(idtg))).ToArray();
        }


        

        private CertifiedEntity[] GetFilteredCertifiedEntities() {
            var certifieds = GetCertifiedEntities();

            return certifieds.Where(c => !DosesInput.WaitingToHarvest.Any(wh => wh.IdCertifiedEntity.Equals(c.Id))).ToArray();
        }


        private void UpdateSpecies() {
            bsSpecie.DataSource = GetSpeciesFiltered();
            bsSpecieCollection.DataSource = GetSpecies().Where(s => DosesInput.IdSpecies.Any(idsp => idsp.Equals(s.Id))).ToList();

            
        }

        private void UpdateVarieties() {
            var filtered = GetFilteredVarieties();
            var species = GetSpecies();
            var mapper = config.CreateMapper();

            bsVariety.DataSource = filtered.Select(s =>
            {
                var mp = mapper.Map<VarieryExtend>(s);
                var specie = species.First(a => a.Id.Equals(mp.IdSpecie));
                mp.FullName = $"{specie.Name} - {mp.Name}";
                return mp;
            }).ToArray();
            var varietiesSelected = DosesInput.IdVarieties.Select(s => GetVarieties().First(a => a.Id.Equals(s))).ToList();
            bsVarietyCollection.DataSource = varietiesSelected;

        }

        private void UpdateTargets() {
            bsTarget.DataSource = GetFilteredTargets();
            bsTargetCollection.DataSource = DosesInput.IdsApplicationTarget.Select(s => GetTargets().First(a => a.Id.Equals(s))).ToList();
        }

        private void UpdateCertified() {
            bsCertifiedEntity.DataSource = GetFilteredCertifiedEntities();

            var mapper = configHarvest.CreateMapper();
            var src = DosesInput.WaitingToHarvest.Select(s =>
            {
                var mp = mapper.Map<WaitingHarvestExtend>(s);

                mp.FullName = GetCertifiedEntities().First(a => a.Id.Equals(mp.IdCertifiedEntity)).Name;
                return mp;
            }).ToList();

            waitingHarvestExtendBindingSource.DataSource = src;
        }

        private void btnAddSpecie_Click(object sender, EventArgs e)
        {
            var specie = (Specie)bsSpecie.Current;
            if (specie != null)
            {
                if (DosesInput.IdSpecies == null)
                {
                    DosesInput.IdSpecies = new string[] { specie.Id };
                }
                else {
                    var lst = DosesInput.IdSpecies.ToList();
                    lst.Add(specie.Id);
                    DosesInput.IdSpecies = lst.ToArray();
                }
                UpdateSpecies();
                UpdateVarieties();
            }
        }

        private void btnRemoveSpecie_Click(object sender, EventArgs e)
        {
            var specie = (Specie)bsSpecieCollection.Current;
            if (specie != null)
            {
                if (DosesInput.IdSpecies == null)
                {
                    MessageBox.Show("No existe especie a eliminar");
                    return;
                }
                else
                {
                    var lst = DosesInput.IdSpecies.ToList();
                    lst.Remove(specie.Id);
                    DosesInput.IdSpecies = lst.ToArray();                    
                }
                UpdateSpecies();
                UpdateVarieties();
            }
        }

        private void btnAddVariety_Click(object sender, EventArgs e)
        {
            var variety = (VarieryExtend)bsVariety.Current;
            if (variety != null)
            {
                if (DosesInput.IdVarieties == null)
                {
                    DosesInput.IdVarieties = new string[] { variety.Id };
                    
                }
                else
                {
                    var lst = DosesInput.IdVarieties.ToList();
                    lst.Add(variety.Id);
                    DosesInput.IdVarieties = lst.ToArray();
                }
                
                UpdateVarieties();
            }
        }

        private void btnRemoveVariety_Click(object sender, EventArgs e)
        {
            var variety = (VarieryExtend)bsVarietyCollection.Current;
            if (variety != null)
            {
                if (DosesInput.IdVarieties == null)
                {
                    MessageBox.Show("No existe variedad a eliminar");
                    return;
                }
                else
                {
                    var lst = DosesInput.IdVarieties.ToList();
                    lst.Remove(variety.Id);
                    DosesInput.IdVarieties = lst.ToArray();
                }

                UpdateVarieties();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var target = (ApplicationTarget)bsTarget.Current;
            if (target != null)
            {
                if (DosesInput.IdsApplicationTarget == null)
                {
                    DosesInput.IdsApplicationTarget = new string[] { target.Id };

                }
                else
                {
                    var lst = DosesInput.IdsApplicationTarget.ToList();
                    lst.Add(target.Id);
                    DosesInput.IdsApplicationTarget = lst.ToArray();
                }

                UpdateTargets();
            }
        }

        private void btnRemoveTarget_Click(object sender, EventArgs e)
        {
            var target = (ApplicationTarget)bsTargetCollection.Current;
            if (target != null)
            {
                if (DosesInput.IdsApplicationTarget == null)
                {
                    MessageBox.Show("No existe variedad a eliminar");
                    return;
                }
                else
                {
                    var lst = DosesInput.IdsApplicationTarget.ToList();
                    lst.Remove(target.Id);
                    DosesInput.IdsApplicationTarget = lst.ToArray();
                }

                UpdateTargets();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxPPm.Text) || string.IsNullOrWhiteSpace(tbxCertDays.Text))
            {
                return;
            }
            var lst = DosesInput.WaitingToHarvest.ToList();


            lst.Add(new WaitingHarvestInput
            {
                IdCertifiedEntity = ((CertifiedEntity)bsCertifiedEntity.Current).Id,
                Ppm = double.Parse(tbxPPm.Text),
                WaitingDays = int.Parse(tbxCertDays.Text)
            });
            DosesInput.WaitingToHarvest = lst.ToArray();
            UpdateCertified();
        }

        private void btnRemoveHarvest_Click(object sender, EventArgs e)
        {
            var harvest = (WaitingHarvestExtend)waitingHarvestExtendBindingSource.Current;
            if (harvest!=null)
            {
                var lst = DosesInput.WaitingToHarvest.ToList();
                var rmv = lst.First(s => s.IdCertifiedEntity.Equals(harvest.IdCertifiedEntity));
                lst.Remove(rmv);
                DosesInput.WaitingToHarvest = lst.ToArray();
                UpdateCertified();
            }
        }
    }
}
