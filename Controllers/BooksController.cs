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
        public IActionResult Get()
        {
            using (var db = new DatabaseHelper())
            {
                string query = "SELECT * FROM books limit 10";
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
                return Ok(books);
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
