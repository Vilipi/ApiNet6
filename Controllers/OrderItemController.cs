using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiNet6.Controllers;

[ApiController]
[Route("[controller]")]


public class OrderItemController : ControllerBase
{
//    Comprobación en ESTÁTICO

//    private static List<OrderItem> _orders = new List<OrderItem>
//    {
//     new OrderItem { Id = 1, OrderId = 1, ProductId = 2, Quantity = 2 },
//     new OrderItem { Id = 2, OrderId = 1, ProductId = 1, Quantity = 1 },
//     new OrderItem { Id = 3, OrderId = 1, ProductId = 4, Quantity = 3 },

//     };

//     private readonly ILogger<OrderItemController> _logger;

//     public OrderItemController(ILogger<OrderItemController> logger)
//     {
//         _logger = logger;
//     }

    private readonly DataContext _context; // referencia al conexto de del la db

    public OrderItemController(DataContext context) {
        _context = context;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "get all orders", Description = "Get a list of orders from our database")]
    [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(List<OrderItem>))]
    [SwaggerResponse(404, "Not Found")]
    public ActionResult<List<OrderItem>> Get() 
    {
        var orders = _context.Orders;
        if (orders == null)
            return NotFound();
        else
            return Ok(_context.Orders);
    }

    [HttpGet]
    [Route("{OrderId}")] // Obtenemos un una un array segun su numero de orden
    [SwaggerOperation(Summary = "Listing a order by its OrderID", Description = "Get a single order from our database")]
    [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(OrderItem))]
    [SwaggerResponse(404, "Not Found")]
    public ActionResult<OrderItem> Get(int OrderId)
    {
        // List <OrderItem> order = _orders.FindAll(x => x.OrderId == OrderId);
        // return order == null? NotFound() : Ok(order);

        // var orderItem = _context.Orders?.Find(OrderId);

        var orderItems = from o in _context.Orders
                                      where o.OrderId == OrderId
                                      select o;

        return orderItems == null? NotFound() : Ok(orderItems);

         

    }   
/// <remarks>
/// Sample request:
///
///     POST 
///     [
///     {
///        "productId": 1,
///        "quantity": 2
///     },
///     {
///        "productId": 2,
///        "quantity": 2
///     },
///     ]
///
/// </remarks>
    [HttpPost] // Create
    [SwaggerOperation(Summary = "Creating a order by its ID", Description = "Createan order to our database")]
    [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(OrderItem))]
    public ActionResult Post(List<OrderItem> orderItem) {
      
        int? lastOrder = _context.Orders == null? 1 : _context.Orders.Max(x => x.OrderId) + 1; // devuelve el siguiente id de la tabla
    
        foreach (OrderItem item in orderItem){
            item.OrderId = (int?)lastOrder;
            _context.Orders?.Add(item);
        }          
        _context.SaveChanges();
        return Ok(orderItem); // Status 200
        
    }
}