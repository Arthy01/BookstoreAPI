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

                List<Order> orders = new List<Order>();
                foreach (DataRow row in result.Rows)
                {
                    orders.Add(new Order
                    {
                        // Id = row["id"],
                        Timestamp = Convert.ToDateTime(row["timestamp"]),
                        Amount = Convert.ToSingle(row["amount"]),
                        CustomerID = Convert.ToUInt64(row["customer_id"])
                    });
                }
                return Ok(orders);
            }
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public IActionResult Get(ulong id)
        {
            using (var db = new DatabaseHelper())
            {
                string query = $"SELECT * FROM orders WHERE orders.id = {id}";
                DataTable result = db.ExecuteQuery(query);

                List<Order> orders = new List<Order>();
                foreach (DataRow row in result.Rows)
                {
                    orders.Add(new Order
                    {
                        // Id = row["id"],
                        Timestamp = Convert.ToDateTime(row["timestamp"]),
                        Amount = Convert.ToSingle(row["amount"]),
                        CustomerID = Convert.ToUInt64(row["customer_id"])
                    });
                }
                return Ok(orders);
            }
        }

        // POST api/<OrdersController>
        [HttpPost]
        public IActionResult Post([FromBody] Order value)
        {
            using (var db = new DatabaseHelper())
            {
                if (value.CustomerID <= 0)
                    return BadRequest("Ungültige Customer number");

                string query = $"INSERT INTO orders (timestamp, amount, customer_id) VALUES ('{value.Timestamp: yyyy-MM-dd}', '{value.Amount}', '{value.CustomerID}')";
                db.ExecuteNonQuery(query);
                return Ok("Order hinzugefügt");
            }
        }
    }
}
