using BookstoreAPI.Models;
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
                string query = "SELECT * FROM customers";
                DataTable result = db.ExecuteQuery(query);

                List<object> customers = new List<object>();
                foreach (DataRow row in result.Rows)
                {
                    customers.Add(new
                    {
                        Id = row["id"],
                        Firstname = row["firstname"],
                        Lastname = row["lastname"],
                        Title = row["title"],
                        Street = row["street"],
                        City = row["city"],
                        Age = row["age"]
                    });
                }
                return Ok(customers);
            }
        }

        // GET api/<CustomersController>/5
        [HttpGet("{id}")]
        public IActionResult Get(ulong id)
        {
            if (id < 1) {
                return BadRequest("Ungültige ID.");
            }

            using (var db = new DatabaseHelper())
            {
                string query = $"SELECT * FROM customers WHERE customers.id = {id}";
                DataTable result = db.ExecuteQuery(query);

                List<object> customers = new List<object>();
                foreach (DataRow row in result.Rows)
                {
                    customers.Add(new
                    {
                        Id = row["id"],
                        Firstname = row["firstname"],
                        Lastname = row["lastname"],
                        Title = row["title"],
                        Street = row["street"],
                        City = row["city"],
                        Age = row["age"]
                    });
                }
                return Ok(customers);
            }
        }

        // POST api/<CustomersController>
        [HttpPost]
        public IActionResult Post([FromBody] Customer value)
        {
            using (var db = new DatabaseHelper())
            {
                if (value == null || string.IsNullOrWhiteSpace(value.FirstName) || string.IsNullOrWhiteSpace(value.LastName) || string.IsNullOrWhiteSpace(value.Street) || string.IsNullOrWhiteSpace(value.City))
                {
                    return BadRequest("Volständige Angaben sind erforderlich.");
                }

                string query = $"INSERT INTO customers (firstname, lastname, title, street, city, age) VALUES ('{value.FirstName}', '{value.LastName}', '{value.Title}', '{value.Street}', '{value.City}', '{value.Age}')";
                int rowsAffected = db.ExecuteNonQuery(query);

                if (rowsAffected > 0)
                    return Ok("Kunde erfolgreich hinzugefügt.");
                else
                    return BadRequest("Kunde konnte nicht hinzugefügt werden.");
            }
        }

        // PUT api/<CustomersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(ulong id, [FromBody] Customer value)
        {
            if (id < 1 || value == null)
            {
                return BadRequest("Ungültige Daten.");
            }

            using (var db = new DatabaseHelper())
            {

                string query = $"UPDATE customers SET firstname = '{value.FirstName}', lastname = '{value.LastName}', title = '{value.Title}', street = '{value.Street}', city = '{value.City}', age = '{value.Age}' WHERE id = {id}";
                int rowsAffected = db.ExecuteNonQuery(query);

                if (rowsAffected > 0)
                    return Ok("Kunde erfolgreich aktualisiert.");
                else
                    return NotFound("Kunde nicht gefunden.");

            }
        }

        // DELETE api/CustomersController/5
        [HttpDelete("{id}")]
        public IActionResult Delete(ulong id)
        {
            if (id < 1)
            {
                return BadRequest("Ungültige ID.");
            }

            using (var db = new DatabaseHelper())
            {
                string query = $"DELETE FROM orders WHERE customer_id = {id}";
                int rowsAffected = db.ExecuteNonQuery(query);

                query = $"DELETE FROM customers WHERE id = {id}";
                rowsAffected += db.ExecuteNonQuery(query);

                if (rowsAffected > 0)
                    return Ok("Kunde erfolgreich gelöscht.");
                else
                    return NotFound("Kunde nicht gefunden.");
            }
        }
    }
}
