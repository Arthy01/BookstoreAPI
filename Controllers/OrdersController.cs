using BookstoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        // GET: api/<OrdersController>
        [HttpGet]
        public IActionResult Get()
        {
            using (var db = new DatabaseHelper())
            {
                string query = "SELECT * FROM orders";
                DataTable result = db.ExecuteQuery(query);

                List<object> orders = new List<object>();
                foreach (DataRow row in result.Rows)
                {
                    orders.Add(new
                    {
                        Id = row["id"],
                        Timestamp = row["timestamp"],
                        Amount = row["amount"],
                        CustomerID = row["customer_id"]
                    });
                }
                return Ok(orders);
            }
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using (var db = new DatabaseHelper())
            {
                string query = $"SELECT * FROM orders WHERE orders.id = {id}";
                DataTable result = db.ExecuteQuery(query);

                List<object> orders = new List<object>();
                foreach (DataRow row in result.Rows)
                {
                    orders.Add(new
                    {
                        Id = row["id"],
                        Timestamp = row["timestamp"],
                        Amount = row["amount"],
                        CustomerID = row["customer_id"]
                    });
                }
                return Ok(orders);
            }
        }

        // POST api/<OrdersController>
        [HttpPost]
        public void Post([FromBody] Order value)
        {
            using (var db = new DatabaseHelper())
            {
                string query = $"INSERT INTO orders (timestamp, amount, customer_id) VALUES ('{value.Timestamp}', '{value.Amount}', '{value.CustomerID}')";
                db.ExecuteNonQuery(query);
            }
        }
    }
}
