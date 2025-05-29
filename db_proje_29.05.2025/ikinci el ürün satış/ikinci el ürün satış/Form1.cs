using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace ikinci_el_ürün_satış
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void ListeleUrunler()
        {
            string connectionString = @"Server=DESKTOP-R0OJKTE\SQLEXPRESS;Database=second_hand_db; Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT u.urunID, u.urunAdi, u.urunYasi, u.hasarDurumu, u.urunFiyati,
                         e.esyaAdi, k.kullaniciAdi
                         FROM urunBilgi u
                         JOIN esya e ON u.esyaID = e.esyaID
                         JOIN kullaniciBilgileri k ON u.kullaniciID = k.kullaniciID";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }


        private void btnEkle_Click(object sender, EventArgs e)
        {
            string connectionString = @"Server=DESKTOP-R0OJKTE\SQLEXPRESS;Database=second_hand_db; Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(connectionString);
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string esyaAdi = txtEsyaAdi.Text.Trim();
            int kullaniciID = -1;
            int esyaID = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kullanıcı varsa al yoksa ekle
                    SqlCommand cmdKullanici = new SqlCommand("SELECT kullaniciID FROM kullaniciBilgileri WHERE kullaniciAdi = @adi", conn);
                    cmdKullanici.Parameters.AddWithValue("@adi", kullaniciAdi);
                    var resultKullanici = cmdKullanici.ExecuteScalar();

                    if (resultKullanici != null)
                    {
                        kullaniciID = Convert.ToInt32(resultKullanici);
                    }
                    else
                    {
                        SqlCommand ekleKullanici = new SqlCommand("INSERT INTO kullaniciBilgileri (kullaniciAdi) OUTPUT INSERTED.kullaniciID VALUES (@adi)", conn);
                        ekleKullanici.Parameters.AddWithValue("@adi", kullaniciAdi);
                        kullaniciID = (int)ekleKullanici.ExecuteScalar();
                    }

                    // Eşya varsa al yoksa ekle
                    SqlCommand cmdEsya = new SqlCommand("SELECT esyaID FROM esya WHERE esyaAdi = @adi", conn);
                    cmdEsya.Parameters.AddWithValue("@adi", esyaAdi);
                    var resultEsya = cmdEsya.ExecuteScalar();

                    if (resultEsya != null)
                    {
                        esyaID = Convert.ToInt32(resultEsya);
                    }
                    else
                    {
                        SqlCommand ekleEsya = new SqlCommand("INSERT INTO esya (esyaAdi) OUTPUT INSERTED.esyaID VALUES (@adi)", conn);
                        ekleEsya.Parameters.AddWithValue("@adi", esyaAdi);
                        esyaID = (int)ekleEsya.ExecuteScalar();
                    }

                    // Ürünü ekle
                    SqlCommand insertUrun = new SqlCommand(@"INSERT INTO urunBilgi 
                (urunAdi, urunYasi, hasarDurumu, urunFiyati, esyaID, kullaniciID) 
                VALUES (@urunAdi, @urunYasi, @hasarDurumu, @urunFiyati, @esyaID, @kullaniciID)", conn);

                    insertUrun.Parameters.AddWithValue("@urunAdi", txtUrunAdi.Text.Trim());
                    insertUrun.Parameters.AddWithValue("@urunYasi", int.Parse(txtUrunYasi.Text));
                    insertUrun.Parameters.AddWithValue("@hasarDurumu", txtHasarDurumu.Text.Trim());
                    insertUrun.Parameters.AddWithValue("@urunFiyati", decimal.Parse(txtUrunFiyati.Text));
                    insertUrun.Parameters.AddWithValue("@esyaID", esyaID);
                    insertUrun.Parameters.AddWithValue("@kullaniciID", kullaniciID);

                    insertUrun.ExecuteNonQuery();

                    MessageBox.Show("Ürün başarıyla eklendi.");

                    // İstersen ürün listesini yenilemek için buraya listeleme kodu da ekleyebilirsin
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }

        }

        private void btnListele_Click(object sender, EventArgs e)
        {
            ListeleUrunler();

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            string connectionString = @"Server=DESKTOP-R0OJKTE\SQLEXPRESS;Database=second_hand_db; Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(connectionString);
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int urunID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["urunID"].Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM urunBilgi WHERE urunID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", urunID);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Ürün silindi.");
                ListeleUrunler();
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir satır seçin.");
            }
        }
    }
}
