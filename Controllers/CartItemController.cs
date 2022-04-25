using Microsoft.AspNetCore.Mvc;

namespace ApiNet6.Controllers;

[ApiController]
[Route("[controller]")]

public class CartItemController : ControllerBase
{
     private static List<CartItem> _cartItem = new List<CartItem> {
    new CartItem { Id = 1, ProductId = 1, Quantity = 1 },
    new CartItem { Id = 2, ProductId = 3, Quantity = 2 },
    new CartItem { Id = 3, ProductId = 5, Quantity = 4 }
    };

    // private readonly ILogger<CartItemController> _logger;

    //  public CartItemController(ILogger<CartItemController> logger) {
    //     _logger = logger;
    // }

    private readonly DataContext _context; // referencia al conexto de del la db

    public CartItemController(DataContext context) {
        _context = context;
    }

    [HttpGet]
    public ActionResult<Product> Get() {
        return Ok(_context.CartItems); // status 200
    }

    [HttpPost] // Create
    public ActionResult Post(CartItem cartItem) {
        var existingCartItem = _cartItem.Find(x => x.ProductId == cartItem.ProductId);
        if (existingCartItem != null) {
            return Conflict("There is an existing cartItem with this ProductId"); // status 409
        } else {
            _cartItem.Add(cartItem);
            var resourceUrl = Request.Path.ToString() + "/" + cartItem.Id;
            return Created(resourceUrl, cartItem); // Status 201
        }
    }

    [HttpDelete] // Delete
    [Route("{ProductId}")]
    public ActionResult<CartItem> Delete(int ProductId) {
        var productToRemove = _cartItem.Find(x => x.ProductId == ProductId);
        if (productToRemove == null) {
            return NotFound("There is not a cartItem saved for this product"); //404
        } else {
            _cartItem.Remove(productToRemove);
            return NoContent(); //204
        }
    }

    [HttpPut] // Update
    [Route("{cartItemId}")]
    public ActionResult Put(CartItem cartItem) {
        var cartItemId = cartItem.Id; 
        var existingcartItem = _cartItem.Find(x => x.Id == cartItem.Id);
        if (existingcartItem == null) {
            return Conflict("There is not any cartItem with this Id"); // Status 409
        } else {
            existingcartItem.Id = cartItem.Id;
            existingcartItem.ProductId = cartItem.ProductId;
            existingcartItem.Quantity = cartItem.Quantity;
            return Ok(); //200
        }
    }
}
