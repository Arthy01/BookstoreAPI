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
                        Fistname = row["firstname"],
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
        public IActionResult Get(int id)
        {
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
                        Fistname = row["firstname"],
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
        public void Post([FromBody] Customer value)
        {
            using (var db = new DatabaseHelper())
            {

                string query = $"INSERT INTO customers (firstname, lastname, title, street, city, age) VALUES ({value.FirstName}, {value.LastName}, {value.Title}, {value.Street}, {value.City}, {value.Age})";
                db.ExecuteNonQuery(query);

            }
        }

        // PUT api/<CustomersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Customer value)
        {
            using (var db = new DatabaseHelper())
            {

                string query = $"UPDATE customers SET firstname = {value.FirstName}, lastname = {value.LastName}, title = {value.Title}, street = {value.Street}, city = {value.City}, age = {value.Age} WHERE id = {id}";
                db.ExecuteNonQuery(query);

            }
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var db = new DatabaseHelper())
            {
                string query = $"UPDATE customers SET firstname = {null}, lastname = {null}, title = {null}, street = {null}, city = {null}, age = {null} WHERE id = {id}";
                db.ExecuteNonQuery(query);
            }
        }
    }
}
