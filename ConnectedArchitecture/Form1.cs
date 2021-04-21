using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectedArchitecture
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=DOGUSAKKAS;Initial Catalog=Northwind;Integrated Security=True");

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Select * From Categories",baglanti);
            baglanti.Open();
            SqlDataReader okuyucu = komut.ExecuteReader();
            comboBox1.DisplayMember = "CategoryName";
            comboBox1.ValueMember = "CategoryID";

            List<Category> liste = new List<Category>();

            while (okuyucu.Read())
            {
                Category dogus = new Category();
                dogus.CategoryID = (int)okuyucu["CategoryID"];
                dogus.CategoryName = (string)okuyucu["CategoryName"].ToString();
                liste.Add(dogus);
            }
            baglanti.Close();
            comboBox1.DataSource = liste;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            var secilenID = comboBox1.SelectedValue;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select * From Products Where CategoryID="+secilenID,baglanti);
            SqlDataReader okuyucu = komut.ExecuteReader();

            while (okuyucu.Read())
            {
                Button btn = new Button();
                btn.Name = "b" + okuyucu["ProductID"];
                btn.Height = 50;
                btn.AutoSize = true;
                btn.Text = okuyucu["ProductName"].ToString();
                flowLayoutPanel1.Controls.Add(btn);

                btn.Click += UrunDetayGetir;
            }
            baglanti.Close();
        }

        private void UrunDetayGetir(object tiklanilan,EventArgs tikdetay)
        {
            Button tiklanilanButton = (Button)tiklanilan;
            var IDtxt = tiklanilanButton.Name.Remove(0, 1);

            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select * From Products Where ProductID="+IDtxt,baglanti);
            SqlDataReader okuyucu = komut.ExecuteReader();
            okuyucu.Read();
            UrunDetay detay = new UrunDetay();

            detay.lbl_UrunAdi.Text = okuyucu["ProductName"].ToString();

            detay.lbl_qpu.Text = okuyucu["QuantityPerUnit"].ToString();

            detay.Show();
            baglanti.Close();

        }

    }
}
