using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryApp
{
    public partial class Form1 : Form
    {
        BookRepository repo = new BookRepository();
        public Form1()
        {
            InitializeComponent();
        }

        private List<Book> FilterBooks(string keyword)
        {
            var allBooks = repo.GetAll();

            if (string.IsNullOrWhiteSpace(keyword))
                return allBooks;

            keyword = keyword.ToLower(); 

            return allBooks
                .Where(b => b.Title.ToLower().Contains(keyword) || b.Author.ToLower().Contains(keyword))
                .ToList();
        }

        private void btnList_Click(object sender, EventArgs e)
        {            
            var books = repo.GetAll()
                 .Select(b => new
                  {
                    b.Id,
                    b.Title,
                    b.Author,
                    b.Year,
                    Durum = b.IsAvailable ? "Mevcut" : "Ödünçte"
                  })
                 .ToList();
            dataGridView1.DataSource = repo.GetAll();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Book book = new Book
            {
                Title = txtTitle.Text,
                Author = txtAuthor.Text,
                Year = int.Parse(txtYear.Text),
                IsAvailable = chkAvailable.Checked
            };
            repo.Add(book);
            MessageBox.Show("Kitap başarıyla eklendi!");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int id = (int)dataGridView1.CurrentRow.Cells["Id"].Value;
                repo.Delete(id);
                MessageBox.Show("Kitap silindi!");
            }
        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int id = (int)dataGridView1.CurrentRow.Cells["Id"].Value;
                repo.UpdateAvailability(id, false); // kitap ödünç alındı
                MessageBox.Show("Kitap ödünç verildi!");
                dataGridView1.DataSource = repo.GetAll(); // listeyi yenile
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int id = (int)dataGridView1.CurrentRow.Cells["Id"].Value;
                repo.UpdateAvailability(id, true); // kitap iade edildi
                MessageBox.Show("Kitap iade edildi!");
                dataGridView1.DataSource = repo.GetAll(); // listeyi yenile
            }
        }

        private void btnExit_MouseEnter(object sender, EventArgs e)
        {
            btnExit.BackColor = Color.Red;
        }

        private void btnExit_MouseLeave(object sender, EventArgs e)
        {
            btnExit.BackColor = Color.DarkRed;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtTitle.Text.Trim();
            var filteredBooks = FilterBooks(keyword);

            // DataGridView’i DataSource ile bağla
            dataGridView1.DataSource = filteredBooks
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    b.Author,
                    b.Year,
                    IsAvailable = b.IsAvailable
                })
                .ToList();
        }
    }
}
