using Microsoft.AspNetCore.Mvc;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // GET: api/<BooksController>
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

                List<object> books = new List<object>();
                foreach (DataRow row in result.Rows)
                {
                    books.Add(new
                    {
                        Id = row["id"],
                        Title = row["title"],
                        Author = row["creator"]
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

        // GET api/<BooksController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BooksController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BooksController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
