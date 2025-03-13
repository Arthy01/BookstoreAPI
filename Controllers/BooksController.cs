using Microsoft.AspNetCore.Mvc;
using System.Data;
using System;
using System.Collections.Generic;
using BookstoreAPI.Models;

namespace BookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // GET: api/books?page=1&pageSize=500
        [HttpGet]
        public IActionResult Get(int page = 1, int pageSize = 500)
        {
            if (pageSize < 1 || pageSize > 1000)
                pageSize = 500;

            if (page < 1)
                page = 1;

            int offset = (page - 1) * pageSize;

            using (var db = new DatabaseHelper())
            {
                string query = $"SELECT * FROM books LIMIT {pageSize} OFFSET {offset}";
                DataTable result = db.ExecuteQuery(query);

                List<Book> books = new List<Book>();
                foreach (DataRow row in result.Rows)
                {
                    books.Add(new Book
                    {
                        Title = row["title"].ToString(),
                        Creator = row["creator"] as string,
                        Issued = Convert.ToDateTime(row["issued"]),
                        Downloads = Convert.ToUInt64(row["downloads"]),
                        Url = row["url"].ToString(),
                        Language = row["language"].ToString(),
                        SubjectId = row["subject_id"] != DBNull.Value ? Convert.ToUInt64(row["subject_id"]) : null
                    });
                }

                return Ok(new
                {
                    Page = page,
                    PageSize = pageSize,
                    Books = books
                });
            }
        }

        [HttpGet("search")]
        public IActionResult SearchByTitle(string title, int page = 1, int pageSize = 500)
        {
            if (string.IsNullOrWhiteSpace(title))
                return BadRequest("Der Titel darf nicht leer sein.");

            if (pageSize < 1 || pageSize > 500)
                pageSize = 500;

            if (page < 1)
                page = 1;

            int offset = (page - 1) * pageSize;

            using (var db = new DatabaseHelper())
            {
                string query = $"SELECT * FROM books WHERE title LIKE @title LIMIT {pageSize + 1} OFFSET {offset}";
                var parameters = new Dictionary<string, object> { { "@title", $"%{title}%" } };

                DataTable result = db.ExecuteQuery(query, parameters);

                if (result.Rows.Count > 500)
                    return BadRequest("Zu viele Ergebnisse. Bitte den Titel genauer spezifizieren.");

                List<Book> books = new List<Book>();
                foreach (DataRow row in result.Rows)
                {
                    books.Add(new Book
                    {
                        Title = row["title"].ToString(),
                        Creator = row["creator"] as string,
                        Issued = Convert.ToDateTime(row["issued"]),
                        Downloads = Convert.ToUInt64(row["downloads"]),
                        Url = row["url"].ToString(),
                        Language = row["language"].ToString(),
                        SubjectId = row["subject_id"] != DBNull.Value ? Convert.ToUInt64(row["subject_id"]) : null
                    });
                }

                return Ok(new
                {
                    Page = page,
                    PageSize = pageSize,
                    Books = books
                });
            }
        }


        // GET api/books/5
        [HttpGet("{id}")]
        public IActionResult Get(ulong id)
        {
            if (id < 1)
            {
                return BadRequest("Ungültige ID. Sie muss größer als 0 sein.");
            }

            using (var db = new DatabaseHelper())
            {
                string query = $"SELECT * FROM books WHERE id = {id} LIMIT 1";
                DataTable result = db.ExecuteQuery(query);

                if (result.Rows.Count == 0)
                {
                    return NotFound("Buch nicht gefunden");
                }

                DataRow row = result.Rows[0];

                Book book = new Book
                {
                    Title = row["title"].ToString(),
                    Creator = row["creator"] as string,
                    Issued = Convert.ToDateTime(row["issued"]),
                    Downloads = Convert.ToUInt64(row["downloads"]),
                    Url = row["url"].ToString(),
                    Language = row["language"].ToString(),
                    SubjectId = row["subject_id"] != DBNull.Value ? Convert.ToUInt64(row["subject_id"]) : null
                };

                return Ok(book);
            }
        }

        // POST api/books
        [HttpPost]
        public IActionResult Post([FromBody] Book newBook)
        {
            if (newBook == null || string.IsNullOrWhiteSpace(newBook.Title) || string.IsNullOrWhiteSpace(newBook.Url))
            {
                return BadRequest("Titel und URL sind erforderlich.");
            }

            using (var db = new DatabaseHelper())
            {
                string subjectIdValue = newBook.SubjectId.HasValue ? newBook.SubjectId.ToString() : "NULL";
                string query = $"INSERT INTO books (title, creator, issued, downloads, url, language, subject_id) " +
                               $"VALUES ('{newBook.Title}', '{newBook.Creator}', '{newBook.Issued:yyyy-MM-dd}', {newBook.Downloads}, '{newBook.Url}', '{newBook.Language}', {subjectIdValue})";

                int rowsAffected = db.ExecuteNonQuery(query);

                if (rowsAffected > 0)
                    return Ok("Buch erfolgreich hinzugefügt.");
                else
                    return StatusCode(500, "Fehler beim Einfügen des Buches.");
            }
        }

        // PUT api/books/5
        [HttpPut("{id}")]
        public IActionResult Put(ulong id, [FromBody] Book updatedBook)
        {
            if (id < 1 || updatedBook == null)
            {
                return BadRequest("Ungültige Daten.");
            }

            using (var db = new DatabaseHelper())
            {
                string query = $"UPDATE books SET " +
                               $"title = '{updatedBook.Title}', " +
                               $"creator = '{updatedBook.Creator}', " +
                               $"issued = '{updatedBook.Issued:yyyy-MM-dd}', " +
                               $"downloads = {updatedBook.Downloads}, " +
                               $"url = '{updatedBook.Url}', " +
                               $"language = '{updatedBook.Language}', " +
                               $"subject_id = {updatedBook.SubjectId} " +
                               $"WHERE id = {id}";

                int rowsAffected = db.ExecuteNonQuery(query);

                if (rowsAffected > 0)
                    return Ok("Buch erfolgreich aktualisiert.");
                else
                    return NotFound("Buch nicht gefunden.");
            }
        }

        // DELETE api/books/5
        [HttpDelete("{id}")]
        public IActionResult Delete(ulong id)
        {
            if (id < 1)
            {
                return BadRequest("Ungültige ID.");
            }

            using (var db = new DatabaseHelper())
            {
                string query = $"DELETE FROM books WHERE id = {id}";
                int rowsAffected = db.ExecuteNonQuery(query);

                if (rowsAffected > 0)
                    return Ok("Buch erfolgreich gelöscht.");
                else
                    return NotFound("Buch nicht gefunden.");
            }
        }
    }
}
