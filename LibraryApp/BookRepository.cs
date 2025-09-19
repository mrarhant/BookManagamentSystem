using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp
{
    public class BookRepository
    {
        public string connectionString = "connectionString";

        public List<Book> GetAll()
        {
            List<Book> books = new List<Book>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Books", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        Id = (int)reader["Id"],
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        Year = (int)reader["Year"],
                        IsAvailable = (bool)reader["IsAvailable"]
                    });
                }
            }
            return books;
        }
        public void Add(Book book)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Books (Title, Author, Year, IsAvailable) VALUES (@t, @a, @y, @i)", conn);
                cmd.Parameters.AddWithValue("@t", book.Title);
                cmd.Parameters.AddWithValue("@a", book.Author);
                cmd.Parameters.AddWithValue("@y", book.Year);
                cmd.Parameters.AddWithValue("@i", book.IsAvailable);
                cmd.ExecuteNonQuery();
            }
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Books WHERE Id=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateAvailability(int id, bool isAvailable)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Books SET IsAvailable=@isAvailable WHERE Id=@id", conn);
                cmd.Parameters.AddWithValue("@isAvailable", isAvailable);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
