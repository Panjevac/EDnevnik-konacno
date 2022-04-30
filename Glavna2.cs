using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDnevnik
{
    public partial class Glavna2 : Form
    {
        public Glavna2()
        {
            InitializeComponent();
        }
        private void Glavna2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void raspodelaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Raspodela frm_raspodela = new Raspodela();
            frm_raspodela.Show();
        }

        private void oceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ocena frm_Ocena = new Ocena();
            frm_Ocena.Show();
        }

        private void sifarniciToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void osobeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Osoba frm_osoba = new Osoba();
            frm_osoba.Show();

        }

        private void smeroviToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sifarnik frm_smer = new Sifarnik("Smer");
            frm_smer.Show();
        }

        private void skolskeGodineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sifarnik frm_skolska = new Sifarnik("skolska_godina");
            frm_skolska.Show();
        }

        private void predmetiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sifarnik frm_predmet = new Sifarnik("predmet");
            frm_predmet.Show();
        }

        private void upisniciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Upisnice frm_upisnice = new Upisnice();
            frm_upisnice.Show();
        }
    }
}
