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

namespace EntityDbfirst
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DbSinavOgrenciEntities db = new DbSinavOgrenciEntities();   //modelime ulaşmak için kullandığım sınıf dbsinavogrencientities

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BtnDersListesi_Click(object sender, EventArgs e)
        {
            //ENTİTY FRAMEWORKTE DBFİRST KULLANMADAN YAPILAN LİSTELEME
            SqlConnection baglanti = new SqlConnection(@"Data Source=LEYLA\SQLEXPRESS;Initial Catalog=DbSinavOgrenci;Integrated Security=True");
            SqlCommand komut = new SqlCommand("Select*From tbldersler", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        private void BtnOgrenciListele_Click(object sender, EventArgs e)
        {
            //ENTİTY FRAMEWORK KULLANARAK OGRENCİ LİSTELEME

            dataGridView1.DataSource = db.TBLOGRENCİ.ToList();
            dataGridView1.Columns[3].Visible = false; //öğrencilisteleye basınca 3.kolon yani fotoğrafları gösterme.
            dataGridView1.Columns[4].Visible = false;//öğrencilisteleye basınca 4.kolon yani tblnotları gösterme.
        }

        private void BtnNotListesi_Click(object sender, EventArgs e)
        {
            //bir entity framework linq sorgusu oluştur.

            var query = from item in db.TBLNOTLAR
                        select new { item.NOTID, item.OGR, item.DERS, item.SINAV1, item.SINAV2, item.SINAV3, item.ORTALAMA, item.DURUM };  //from ile tblnotlar secildi ve select new ile süslü parantezler içinde belirtilen kolonlar seçildi.
            dataGridView1.DataSource = query.ToList();
            //dataGridView1.DataSource=db.TBLNOTLAR.ToList(); //not listesinin tüm verileri listelendi....
        }

        private void BtnKaydet_Click(object sender, EventArgs e) //yeni öğrenci ekleme
        {
            TBLOGRENCİ t = new TBLOGRENCİ();  //öğrenciler sinifindan yeni bir nesne türettim.
            t.AD = TxtAd.Text; //ad adlı propa txtad dan gelen değeri atayacak.
            t.SOYAD = TxtSoyad.Text;

            db.TBLOGRENCİ.Add(t);//oğrenci tablosuna ekle.
            db.SaveChanges();
            MessageBox.Show("Öğrenci listeye başarıyla eklenmiştir.");
        }

        private void BtnSil_Click(object sender, EventArgs e) //öğrenci silme
        {
            int id = Convert.ToInt32(TxtOgrenciID.Text); //OGRENCİİD TEXTBOXUNA GİRİLEN DEĞERİ İD'YE ATA.
            var x = db.TBLOGRENCİ.Find(id); //İD DEĞİŞKENİNİ DE X DEĞİŞKENİNE ATA.X SİLMEK İSTEDİĞİMİZ DEĞERİ TUTUYOR.
            db.TBLOGRENCİ.Remove(x); //x i sil.
            db.SaveChanges();
            MessageBox.Show("Öğrenci başarıyla silinmiştir.");
        }

        private void BtnGuncelle_Click(object sender, EventArgs e) //öğrenci güncelleme
        {
            int id = Convert.ToInt32(TxtOgrenciID.Text);
            var x = db.TBLOGRENCİ.Find(id);
            x.AD = TxtAd.Text;
            x.SOYAD = TxtSoyad.Text;
            x.FOTOGRAF = TxtFoto.Text;
            db.SaveChanges();
            MessageBox.Show("Öğrenci başarıyla güncellenmiştir.");
        }
        //VERİTABANI ÜZERİNDEKİ DEĞİŞİKLİĞİN MODELE YANSITILMASI YANİ EĞER VERİTABANINDA YENİ TABLO EKLERSEK  BU DEĞİŞİKLİĞİ DİYAGRAMA EKLEMEK İÇİN UPDATE FROM MODEL  DATABASE KULLANILIR.
    }
}
