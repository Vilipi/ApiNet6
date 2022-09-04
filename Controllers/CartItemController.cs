using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiNet6.Controllers;

[ApiController]
[Route("[controller]")]

public class CartItemController : ControllerBase
{
    //    Comprobación en ESTÁTICO
    
    //  private static List<CartItem> _cartItem = new List<CartItem> {
    // new CartItem { Id = 1, ProductId = 1, Quantity = 1 },
    // new CartItem { Id = 2, ProductId = 3, Quantity = 2 },
    // new CartItem { Id = 3, ProductId = 5, Quantity = 4 }
    // };

    // private readonly ILogger<CartItemController> _logger;

    //  public CartItemController(ILogger<CartItemController> logger) {
    //     _logger = logger;
    // }

    private readonly DataContext _context; // referencia al conexto de del la db

    public CartItemController(DataContext context) {
        _context = context;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all cartItems", Description = "Get all cartItems from our database")]
    [SwaggerResponse(StatusCodes.Status200OK, "Ok", typeof(CartItem))]
    [SwaggerResponse(404, "Not found")]
    public ActionResult<CartItem> Get() {
        var cart = _context.CartItems;

        return Ok(_context.CartItems); // status 200
    }


/// <remarks>
/// Sample request:
///
///     POST 
///     {
///        "productId": 1,
///        "quantity": 1
///     }
///
/// </remarks>
    [HttpPost] // Create
    [SwaggerOperation(Summary = "Create a cartItem", Description = "Creating a cartItem to our database")]
    [SwaggerResponse(StatusCodes.Status201Created, "Created", typeof(CartItem))]
    [SwaggerResponse(409, "Conflict")]
    public ActionResult Post(CartItem cartItem) {
        // var existingCartItem = _cartItem.Find(x => x.ProductId == cartItem.ProductId);
        var existingCartItem = _context.CartItems?.Find(cartItem.ProductId);
        if (existingCartItem != null) {
            return Conflict("There is an existing cartItem with this ProductId"); // status 409
        } else {
            var newCartItem = new CartItem { ProductId = cartItem.ProductId, Quantity = cartItem.Quantity };
            _context.CartItems?.Add(cartItem);
            _context.SaveChanges();
            var resourceUrl = Request.Path.ToString() + "/" + cartItem.Id;
            return Created(resourceUrl, cartItem); // Status 201
        }
    }

    [HttpDelete] // Delete
    [Route("{ProductId}")]
    [SwaggerOperation(Summary = "Remove a cartItem", Description = "Removing a cartItem from our database")]
    [SwaggerResponse(StatusCodes.Status200OK, "Ok", typeof(CartItem))]
    [SwaggerResponse(404, "Not found")]

    public ActionResult Delete(int ProductId) {
        var productToRemove = _context.CartItems?.Where(x => x.ProductId == ProductId).FirstOrDefault(); // buscando el CartItem por el ProductId
        if (productToRemove == null) {
            return NotFound("There is not a cartItem saved for this product"); // status 404
        } else {
            _context.CartItems?.Remove(productToRemove);
            _context.SaveChanges();
            return Ok(); // status 200
        }
    }

/// <remarks>
/// Sample request:
///
///     PUT 
///     {
///        "productId": 1,
///        "quantity": 2
///     }
///
/// </remarks>
    [HttpPut] // Update
    [Route("{cartItemProductId}")]
    [SwaggerOperation(Summary = "Updating a cartItem", Description = "Updating a cartItem from our database")]
    [SwaggerResponse(StatusCodes.Status200OK, "Ok", typeof(CartItem))]
    [SwaggerResponse(404, "Not found")]
    public ActionResult Put(CartItem cartItem) {
        var cartItemProductID = cartItem.ProductId; // valor del route
        var existingCartItem = _context.CartItems?.Where(x => x.ProductId == cartItem.ProductId).FirstOrDefault(); // buscando el CartItem por el ProductId
        if (existingCartItem == null || cartItemProductID != cartItem.ProductId) {
            return NotFound("There is not any cartItem with this ProductId"); // Status 409
        } else {
            existingCartItem.Id = cartItem.Id;
            existingCartItem.ProductId = cartItemProductID;
            existingCartItem.Quantity = cartItem.Quantity;
            _context.SaveChanges();
            return Ok(); //200
        }
    }
}
