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

namespace KoneksiDatabase
{
    public partial class FormProdukDetail : Form
    {
        public FormProdukDetail() => InitializeComponent();

        private void FormProdukDetail_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = Koneksi.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Id, NamaKategori FROM Kategori";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Dictionary<int, string> kategoriDict = new Dictionary<int,
                    string>();
                    while (reader.Read())
                    {
                        kategoriDict.Add((int)reader["Id"],
                        reader["NamaKategori"].ToString());
                    }
                    if (kategoriDict.Count == 0)
                    {
                        MessageBox.Show("Tidak ada kategori ditemukan di database.");
                    }
                    cmbKategori.DataSource = new BindingSource(kategoriDict, null);
                    cmbKategori.DisplayMember = "Value";
                    cmbKategori.ValueMember = "Key";
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memuat kategori: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validasi txtHarga
            if (string.IsNullOrWhiteSpace(txtHarga.Text) || !decimal.TryParse(txtHarga.Text, out _))
            {
                MessageBox.Show("Harga harus diisi dengan angka!");
                txtHarga.Focus();
                return;
            }

            // Validasi txtStok
            if (string.IsNullOrWhiteSpace(txtStok.Text) || !int.TryParse(txtStok.Text, out _))
            {
                MessageBox.Show("Stok harus diisi dengan angka!");
                txtStok.Focus();
                return;
            }

            using (SqlConnection conn = Koneksi.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO Produk (NamaProduk, Harga, Stok, KategoriId, Deskripsi) 
                                     VALUES (@nama, @harga, @stok, @kategori, @deskripsi)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nama", txtNamaProduk.Text);
                    cmd.Parameters.AddWithValue("@harga", Convert.ToDecimal(txtHarga.Text));
                    cmd.Parameters.AddWithValue("@stok", Convert.ToInt32(txtStok.Text));
                    cmd.Parameters.AddWithValue("@deskripsi", txtDeskripsi.Text);
                    if (cmbKategori.SelectedItem != null)
                    {
                        cmd.Parameters.AddWithValue("@kategori", ((KeyValuePair<int, string>)cmbKategori.SelectedItem).Key);
                    }
                    else
                    {
                        MessageBox.Show("Pilih kategori terlebih dahulu.");
                        return;
                    }
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Produk berhasil ditambahkan!");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menambahkan produk: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbKategori_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtHarga_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Hanya angka dan satu titik desimal
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // Hanya satu titik desimal
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtStok_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Hanya angka
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
