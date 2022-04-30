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
    
    public partial class Ocena : Form
    {
        DataTable grid;
        public Ocena()
        {
            InitializeComponent();
        }

        private void cmb_godina_populate()
        {
            SqlConnection veza1 = Konekcija.Povezivanje();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from skolska_godina", veza1);
            DataTable godina = new DataTable();
            adapter.Fill(godina);

            cmb_godina.DataSource = godina;
            cmb_godina.DisplayMember = "naziv";
            cmb_godina.ValueMember = "id";
            cmb_godina.SelectedValue = 2;

        }

        private void cmb_profesor_populate()
        {
            SqlConnection veza1 = Konekcija.Povezivanje();
            StringBuilder naredba = new StringBuilder("select osoba.id as id, ime+prezime as naziv from osoba ");
            naredba.Append("join raspodela on osoba.id = nastavnik_id");
            naredba.Append(" where raspodela.godina_id = " + cmb_godina.SelectedValue.ToString()) ;
            SqlDataAdapter adapter = new SqlDataAdapter(naredba.ToString(), veza1);
            DataTable profesor = new DataTable();
            adapter.Fill(profesor);

            cmb_prof.Enabled = true;
            cmb_prof.DataSource = profesor;
            cmb_prof.DisplayMember = "naziv";
            cmb_prof.ValueMember = "id";
            cmb_prof.SelectedIndex = -1;


        }
        private void Ocena_Load(object sender, EventArgs e)
        {
            cmb_godina_populate();
            cmb_predmet.Enabled = false;
            cmb_prof.Enabled = false;
            cmb_odeljenje.Enabled = false;
            cmb_ucenik.Enabled = false;
            cmb_ocena.Items.Add(1);
            cmb_ocena.Items.Add(2);
            cmb_ocena.Items.Add(3);
            cmb_ocena.Items.Add(4);
            cmb_ocena.Items.Add(5);
        }

        private void cmb_godina_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cmb_godina.IsHandleCreated && cmb_godina.Focused)
            {
                cmb_profesor_populate();
                
                //cmb_prof.SelectedIndex = -1;
            }
            
        }
        private void cmb_predmet_populate()
        {
            SqlConnection veza1 = Konekcija.Povezivanje();
            StringBuilder naredba = new StringBuilder("select distinct predmet.id as id, naziv from predmet ");
            naredba.Append(" join raspodela on predmet.id = predmet_id");
            naredba.Append(" where godina_id = " + cmb_godina.SelectedValue.ToString());
            naredba.Append(" and nastavnik_id= " + cmb_prof.SelectedValue.ToString());
            SqlDataAdapter adapter = new SqlDataAdapter(naredba.ToString(), veza1);
            DataTable predmet = new DataTable();
            adapter.Fill(predmet);

            cmb_predmet.Enabled = true;
            cmb_predmet.DataSource = predmet;
            cmb_predmet.DisplayMember = "naziv";
            cmb_predmet.ValueMember = "id";
            cmb_predmet.SelectedIndex = -1;


        }

        private void cmb_prof_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmb_prof.IsHandleCreated && cmb_prof.Focused)
            {
                cmb_predmet_populate();
                //cmb_prof.SelectedIndex = -1;
            }
        }

        private void cmb_predmet_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmb_predmet.IsHandleCreated && cmb_predmet.Focused)
            {
                cmb_odeljenje_populate();
                //cmb_prof.SelectedIndex = -1;
            }
        }

        private void cmb_odeljenje_populate()
        {
            SqlConnection veza1 = Konekcija.Povezivanje();
            StringBuilder naredba = new StringBuilder("select distinct odeljenje.id as id, str(razred)+'/'+str(indeks) as naziv from odeljenje ");
            naredba.Append(" join raspodela on odeljenje.id = odeljenje_id");
            naredba.Append(" where raspodela.godina_id = " + cmb_godina.SelectedValue.ToString());
            naredba.Append(" and razredni_id= " + cmb_prof.SelectedValue.ToString());
            naredba.Append(" and predmet_id= " + cmb_predmet.SelectedValue.ToString());
            SqlDataAdapter adapter = new SqlDataAdapter(naredba.ToString(), veza1);
            DataTable odeljenje = new DataTable();
            adapter.Fill(odeljenje);

            cmb_odeljenje.Enabled = true;
            cmb_odeljenje.DataSource = odeljenje;
            cmb_odeljenje.DisplayMember = "naziv";
            cmb_odeljenje.ValueMember = "id";
            cmb_odeljenje.SelectedIndex = -1;


        }

        private void cmb_odeljenje_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmb_odeljenje.IsHandleCreated && cmb_odeljenje.Focused)
            {
                cmb_ucenik_populate();
                grid_populate();
                ucenik_ocena_id(0);
                //cmb_prof.SelectedIndex = -1;
            }
        }
        private void cmb_ucenik_populate()
        {
            SqlConnection veza1 = Konekcija.Povezivanje();
            StringBuilder naredba = new StringBuilder("select distinct osoba.id as id, ime+prezime as naziv from osoba ");
            naredba.Append(" join upisnica on osoba.id = osoba_id");
            naredba.Append(" where upisnica.odeljenje_id = " + cmb_odeljenje.SelectedValue.ToString());
     
            SqlDataAdapter adapter = new SqlDataAdapter(naredba.ToString(), veza1);
            DataTable ucenik = new DataTable();
            adapter.Fill(ucenik);

            cmb_ucenik.Enabled = true;
            cmb_ucenik.DataSource = ucenik;
            cmb_ucenik.DisplayMember = "naziv";
            cmb_ucenik.ValueMember = "id";
            cmb_ucenik.SelectedIndex = -1;


        }
        private void grid_populate()
        {
            SqlConnection veza1 = Konekcija.Povezivanje();
            StringBuilder naredba = new StringBuilder("select ocena.id as id, ime+prezime as naziv,ocena, ucenik_id, datum from osoba ");
            naredba.Append(" join ocena on osoba.id = ucenik_id");
            naredba.Append(" join raspodela on raspodela_id = raspodela.id");
            naredba.Append(" where raspodela_id = ");
            naredba.Append("(select id from raspodela where godina_id= "+ cmb_godina.SelectedValue.ToString());
            naredba.Append(" and nastavnik_id= " + cmb_prof.SelectedValue.ToString());
            naredba.Append(" and predmet_id= " + cmb_predmet.SelectedValue.ToString());
            naredba.Append(" and odeljenje_id= " + cmb_odeljenje.SelectedValue.ToString()+")");
            //txt_test.Text = naredba.ToString();
            SqlDataAdapter adapter = new SqlDataAdapter(naredba.ToString(), veza1);
            grid = new DataTable();
            adapter.Fill(grid);

            data_grid.DataSource = grid;
            data_grid.AllowUserToAddRows = false;


        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            StringBuilder naredba = new StringBuilder("select id from raspodela");
            naredba.Append(" where godina_id= " + cmb_godina.SelectedValue.ToString());
            naredba.Append(" and nastavnik_id= " + cmb_prof.SelectedValue.ToString());
            naredba.Append(" and predmet_id= " + cmb_predmet.SelectedValue.ToString());
            naredba.Append(" and odeljenje_id= " + cmb_odeljenje.SelectedValue.ToString());
            SqlConnection veza1 = Konekcija.Povezivanje();
            SqlCommand komanda = new SqlCommand(naredba.ToString(), veza1);
            int id_raspodele = 0;
            try
            {
                veza1.Open();
                id_raspodele=(int)komanda.ExecuteScalar();
                veza1.Close();
            }
            catch (Exception greska)
            {
                MessageBox.Show(greska.Message);
                throw;
            }
            if(id_raspodele>0)
            {
                naredba = new StringBuilder("insert into ocena(datum, raspodela_id, ucenik_id, ocena) values('");
                DateTime datum = date_picker.Value;
                naredba.Append(datum.ToString("yyyy-MM-dd")+"','");
                naredba.Append(id_raspodele.ToString() + "','");
                naredba.Append(cmb_ucenik.SelectedValue.ToString() + "','");
                naredba.Append(cmb_ocena.SelectedItem.ToString() + "')");

                komanda = new SqlCommand(naredba.ToString(), veza1);
                try
                {
                    veza1.Open();
                    id_raspodele = (int)komanda.ExecuteNonQuery();
                    veza1.Close();
                }
                catch (Exception greska)
                {
                    MessageBox.Show(greska.Message);
                    throw;
                }
                grid_populate();
            }
        }

        
        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txt_id.Text) > 0)
            {
                DateTime datum = date_picker.Value;
                StringBuilder naredba = new StringBuilder("update ocena set");
                naredba.Append(" ucenik_id = '" + cmb_ucenik.SelectedValue.ToString()+"', ");
                naredba.Append(" ocena = '" + cmb_ocena.SelectedItem.ToString() + "', ");
                naredba.Append(" datum = '" + datum.ToString("yyyy-MM-dd") + "' ");
                naredba.Append(" where id = " + txt_id.Text);
                SqlConnection veza1 = Konekcija.Povezivanje();
                SqlCommand komanda = new SqlCommand(naredba.ToString(), veza1);
                
                try
                {
                    veza1.Open();
                    komanda.ExecuteNonQuery();
                    veza1.Close();
                }
                catch (Exception greska)
                {
                    MessageBox.Show(greska.Message);
                    throw;
                }
                
                    grid_populate();
                }
            }

        private void data_grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex>=0)
            {
                ucenik_ocena_id(e.RowIndex);
            }
        }

        private void ucenik_ocena_id(int br_sloga)
        {
            cmb_ucenik.SelectedValue = grid.Rows[br_sloga]["ucenik_id"];
            cmb_ocena.SelectedItem = grid.Rows[br_sloga]["ocena"];
            txt_id.Text = grid.Rows[br_sloga]["id"].ToString();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txt_id.Text) > 0)
            {
                DateTime datum = date_picker.Value;
                StringBuilder naredba = new StringBuilder("delete from ocena where id= "+ txt_id.Text);
                
                SqlConnection veza1 = Konekcija.Povezivanje();
                SqlCommand komanda = new SqlCommand(naredba.ToString(), veza1);

                try
                {
                    veza1.Open();
                    komanda.ExecuteNonQuery();
                    veza1.Close();
                }
                catch (Exception greska)
                {
                    MessageBox.Show(greska.Message);
                    throw;
                }

                grid_populate();
            }
        }
    }
    }

