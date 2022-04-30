using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EDnevnik
{
    public partial class Upisnice : Form
    {
        DataTable upisnica;
        public Upisnice()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void cmb_godina_populate()
        {
            SqlConnection veza = Konekcija.Povezivanje();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from skolska_godina", veza);
            DataTable godina = new DataTable();
            adapter.Fill(godina);
            cmb_godina.DataSource = godina;
            cmb_godina.DisplayMember = "naziv";
            cmb_godina.ValueMember = "id";
            cmb_godina.SelectedItem = 2;
        }

        private void cmb_odeljenje_populate()
        {
            string godina = cmb_godina.SelectedValue.ToString();
            SqlConnection veza1 = Konekcija.Povezivanje();
            SqlDataAdapter adapter = new SqlDataAdapter("select id, str(razred)+ '/' + str(indeks) as naziv from odeljenje where godina_id="+godina, veza1);
            DataTable odeljenje = new DataTable();
            adapter.Fill(odeljenje);

            cmb_odeljenje.DataSource = odeljenje;
            cmb_odeljenje.DisplayMember = "naziv";
            cmb_odeljenje.ValueMember = "id";
            
        }
        private void Upisnice_Load(object sender, EventArgs e)
        {
            cmb_godina_populate();
            cmb_odeljenje_populate();
            cmb_odeljenje.SelectedIndex = -1;
            cmb_ucenik.Enabled = false;
            txt_upisnica.Enabled = false;
        }

        private void cmb_godina_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmb_godina.IsHandleCreated && cmb_godina.Focused)
            {
                cmb_odeljenje.SelectedIndex = -1;
                cmb_odeljenje_populate();
                cmb_odeljenje.SelectedIndex = -1;
                
                txt_upisnica.Text = "";
                while(dataGridView1.Rows.Count>0)
                {
                    dataGridView1.Rows.Remove(dataGridView1.Rows[0]);
                }
                cmb_ucenik.SelectedIndex = -1;
                cmb_ucenik.Enabled = false;
            }
        }

        private void cmb_ucenik_populate()
        {
            //string godina = cmb_godina.SelectedValue.ToString();
            SqlConnection veza1 = Konekcija.Povezivanje();
            SqlDataAdapter adapter = new SqlDataAdapter("select id, ime+ ' ' + prezime as naziv from osoba where uloga=1", veza1);
            DataTable ucenik = new DataTable();
            adapter.Fill(ucenik);

            cmb_ucenik.DataSource = ucenik;
            cmb_ucenik.DisplayMember = "naziv";
            cmb_ucenik.ValueMember = "id";

        }
        private void cmb_odeljenje_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmb_odeljenje.IsHandleCreated && cmb_odeljenje.Focused)
            {
                cmb_ucenik_populate();
                //cmb_ucenik.SelectedIndex = -1;
                cmb_ucenik.Enabled = true;
                grid_populate();
            }
        }
        private void grid_populate()
        {
            SqlConnection veza1 = Konekcija.Povezivanje();
            SqlDataAdapter adapter = new SqlDataAdapter("select upisnica.id as id, osoba.ime+ ' ' + osoba.prezime as naziv, osoba.id as ucenik from upisnica join osoba on osoba_id=osoba.id where odeljenje_id="+cmb_odeljenje.SelectedValue.ToString(), veza1);
            upisnica = new DataTable();
            adapter.Fill(upisnica);

            dataGridView1.DataSource = upisnica;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Columns["ucenik"].Visible = false;
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int broj_sloga = dataGridView1.CurrentRow.Index;
                if (upisnica.Rows.Count != 0 && broj_sloga >= 0)
                {
                    cmb_ucenik.SelectedValue = dataGridView1.Rows[broj_sloga].Cells["ucenik"].Value.ToString(); 
                    txt_upisnica.Text = dataGridView1.Rows[broj_sloga].Cells["id"].Value.ToString();
                }
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            StringBuilder naredba = new StringBuilder("insert into upisnica(odeljenje_id, osoba_id) values ('");
            naredba.Append(cmb_odeljenje.SelectedValue.ToString() + "','");
            naredba.Append(cmb_ucenik.SelectedValue.ToString() + "')");
            SqlConnection veza1 = Konekcija.Povezivanje();
            SqlCommand komanda = new SqlCommand(naredba.ToString(), veza1);
            try
            {
                veza1.Open();
                komanda.ExecuteNonQuery();
                veza1.Close();
                grid_populate();
            }
            catch (Exception greska)
            {
                MessageBox.Show(greska.Message);
                throw;
            }
            

        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            StringBuilder naredba = new StringBuilder("update upisnica set ");
            naredba.Append(" osoba_id='"+ cmb_ucenik.SelectedValue.ToString() + "' ");
            naredba.Append(" where id= " + txt_upisnica.Text);
            SqlConnection veza1 = Konekcija.Povezivanje();
            SqlCommand komanda = new SqlCommand(naredba.ToString(), veza1);
            try
            {
                veza1.Open();
                komanda.ExecuteNonQuery();
                veza1.Close();
                grid_populate();
            }
            catch (Exception greska)
            {
                MessageBox.Show(greska.Message);
                throw;
            }
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            StringBuilder naredba = new StringBuilder("delete from upisnica ");       
            naredba.Append(" where id= " + txt_upisnica.Text);
            SqlConnection veza1 = Konekcija.Povezivanje();
            SqlCommand komanda = new SqlCommand(naredba.ToString(), veza1);
            try
            {
                veza1.Open();
                komanda.ExecuteNonQuery();
                veza1.Close();
                grid_populate();
            }
            catch (Exception greska)
            {
                MessageBox.Show(greska.Message);
                throw;
            }
        }
    }
}
