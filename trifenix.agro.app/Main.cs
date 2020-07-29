using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using trifenix.agro.app.frm.mantenedores;
using trifenix.connect.app.cloud;

namespace trifenix.agro.app
{
    public partial class Main : Form
    {

        HubConnection connection;

        List<Form> forms = new List<Form>();


        public Main()
        {
            InitializeComponent();
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:7071/api",c=> { c.AccessTokenProvider = () => Task.FromResult("cloud-app"); ;  })
                .WithAutomaticReconnect()
                .Build();
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
            connection.On<string>("Success", (message) =>
            {
                Action<string> action = (m) => lbxEvents.Items.Add(m);
                Action actionFrms = () => forms.ForEach(UpdateForms);
                action.Invoke($"Modificado o creado un {message}");
                actionFrms.Invoke();


            });

            






        }

        private void planFitoSanitarioToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new SectorFrm(Cloud.GetSectors());
            forms.Add(frm);
            frm.ShowDialog();
            forms.Remove(frm);
        }

        private void UpdateForms(Form frm) {
            if (frm.GetType() == typeof(SectorFrm))
            {
                var frmSect = (SectorFrm)frm;
                var sectors = Cloud.GetSectors();
                frmSect.SetSectors(sectors);
            }
        }

        async private void Main_Load(object sender, EventArgs e)
        {
            

        }

        async private void Main_Shown(object sender, EventArgs e)
        {
            

            await connection.StartAsync();
        }
    }
}
