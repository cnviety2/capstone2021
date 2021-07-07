using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Capstone2021.Test.Controller
{
    [RoutePrefix("test/Orders")]
    public class OdersController : ApiController
    {
        private List<Order> listOrder = Order.CreateOrders();

        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(listOrder);
        }

        [Authorize]
        [Route("{id}")]
        public IHttpActionResult GetAnOrder(int id)
        {
            Order order = null;
            listOrder.ForEach(o =>
            {
                if (o.OrderID == id) { order = o; }
            });
            if (order == null) return Ok("Not exist");
            return Ok(order);
        }

    }


    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string ShipperCity { get; set; }
        public Boolean IsShipped { get; set; }

        public static List<Order> CreateOrders()
        {
            List<Order> OrderList = new List<Order>
            {
                new Order {OrderID = 10248, CustomerName = "Taiseer Joudeh", ShipperCity = "Amman", IsShipped = true },
                new Order {OrderID = 10249, CustomerName = "Ahmad Hasan", ShipperCity = "Dubai", IsShipped = false},
                new Order {OrderID = 10250,CustomerName = "Tamer Yaser", ShipperCity = "Jeddah", IsShipped = false },
                new Order {OrderID = 10251,CustomerName = "Lina Majed", ShipperCity = "Abu Dhabi", IsShipped = false},
                new Order {OrderID = 10252,CustomerName = "Yasmeen Rami", ShipperCity = "Kuwait", IsShipped = true}
            };

            return OrderList;
        }
    }
}