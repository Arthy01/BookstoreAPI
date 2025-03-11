using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        // GET: api/<CustomersController>
        [HttpGet]
        public IActionResult Get()
        {
            using (var db = new DatabaseHelper())
            {
                string query = "SELECT * FROM customers limit 10";
                DataTable result = db.ExecuteQuery(query);

                List<object> customers = new List<object>();
                foreach (DataRow row in result.Rows)
                {
                    // TODO: Atrribute bearbeiten
                    customers.Add(new
                    {
                        Id = row["id"],
                        Name = row["name"]
                    });
                }
                return Ok(customers);
            }
        }

        // GET api/<CustomersController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using (var db = new DatabaseHelper())
            {
                string query = $"SELECT * FROM customers WHERE customers.id = {id} limit 10";
                DataTable result = db.ExecuteQuery(query);

                List<object> customers = new List<object>();
                foreach (DataRow row in result.Rows)
                {
                    // TODO: Atrribute bearbeiten
                    customers.Add(new
                    {
                        Id = row["id"],
                        Name = row["name"]
                    });
                }
                return Ok(customers);
            }
        }

        // POST api/<CustomersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            using (var db = new DatabaseHelper())
            {
                // TODO: Konvertierung und Standardwerte hinzufügen
                value = "Hier wird verabeitet";

                string query = $"INSERT INTO customers (name) VALUES ('{value}')";
                db.ExecuteNonQuery(query);

            }
        }

        // PUT api/<CustomersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            using (var db = new DatabaseHelper())
            {
                // TODO: Konvertierung und Standardwerte hinzufügen
                value = "Hier wird verabeitet";

                string query = $"UPDATE customers SET name = '{value}' WHERE id = {id}";
                db.ExecuteNonQuery(query);

            }
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var db = new DatabaseHelper())
            {
                string query = $"DELETE FROM customers WHERE id = {id}";
                db.ExecuteNonQuery(query);

            }
        }
    }
}
