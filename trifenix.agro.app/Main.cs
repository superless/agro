using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using mantenedores = trifenix.agro.app.frm.mantenedores;

using trifenix.agro.app.interfaces;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro_model;

using trifenix.connect.app.cloud;
using trifenix.agro.app.model_extend;
using AutoMapper;

namespace trifenix.agro.app
{
    public partial class Main : Form
    {

        HubConnection connection;

        List<IFenixForm> forms = new List<IFenixForm>();

        MapperConfiguration config = new MapperConfiguration(cfg => {
            cfg.CreateMap<Season, SeasonExtend>();
        });



        public Main()
        {
            InitializeComponent();
            connection = new HubConnectionBuilder()
                
                .WithUrl("http://localhost:7071/api",c=> { c.AccessTokenProvider = () => Task.FromResult("cloud-app"); ;  })
                .WithAutomaticReconnect()
                .Build();

            connection.ServerTimeout = TimeSpan.FromSeconds(120);
            connection.HandshakeTimeout = TimeSpan.FromSeconds(120);

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
            connection.On<string>("Success", (message) =>
            {
                Action<string> action = (m) => lbxEvents.Items.Add(m);
                Action actionFrms = () => forms.ForEach(m=>m.SetElements());
                action.Invoke($"[{DateTime.Now:dd-MM-yyy HH:mm}] Modificado o creado un {message}");
                actionFrms.Invoke();


            });

            SetSpecies();
            SetSeason();

        }

        private Specie GetSpecie() => (Specie)tsCbSpecie.SelectedItem;


        private SeasonExtend GetSeason() => (SeasonExtend)tsCbCentroCostos.SelectedItem;

        private void SetSpecies() {

            tsCbSpecie.ComboBox.DataSource = Cloud.GetElements<Specie>(EntityRelated.SPECIE);
            tsCbSpecie.ComboBox.DisplayMember = "Name";

        }

        private void SetSeason() {
            var costcenters = Cloud.GetElements<CostCenter>(EntityRelated.COSTCENTER);

            var seasons = Cloud.GetElements<Season>(EntityRelated.SEASON);
            var mapper = config.CreateMapper();

            tsCbCentroCostos.ComboBox.DataSource = seasons.Select(s =>
            {
                var eseason = mapper.Map<SeasonExtend>(s);
                var localCostCenter = costcenters.FirstOrDefault(a => a.Id.Equals(eseason.IdCostCenter));
                eseason.CostCenterName = $"{localCostCenter.Name}";
                return eseason;

            }).ToArray();

            tsCbCentroCostos.ComboBox.DisplayMember = nameof(SeasonExtend.CostCenterName);


        }

        private void planFitoSanitarioToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        
        

        private void Main_Load(object sender, EventArgs e)
        {
            
        }

        async private void Main_Shown(object sender, EventArgs e)
        {
            
            await connection.StartAsync();
        }

        private void sectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.sector.Frm();
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }

        private void TestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.Test.Frm();
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }


        private void parcelaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.plotland.Frm();
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }

        private void razonesSocialesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.businessname.Frm();
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }

        private void objetivoDeAplicaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.applicationtarget.Frm();
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }

        private void entidadCertificadoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.certifiedentity.Frm();
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }

        private void centroDeCostosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.costcenter.Frm();
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }

        private void cargoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.jobs.Frm();
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }

        private void especieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.specie.Frm();
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }

        private void variedadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.variety.Frm();
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }

        private void portaInjertoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.rootstoock.Frm();
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);

        }

        private void cuartelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.barrack.Frm(GetSpecie(), GetSeason());
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }

        private void temporadasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.season.Frm();
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }

        private void categoríaDeIngredienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.ingredient_category.Frm();
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }

        private void ingredienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.ingredient.Frm();
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }

        private void productosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.product.Frm();
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }

        private void marcaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.brand.Frm();
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }
    }
}
