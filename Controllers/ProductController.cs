using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiNet6.Controllers;

[ApiController]
[Route("[controller]")]

public class ProductController : ControllerBase
{
//    Comprobación en ESTÁTICO

//    private static List<Product> _products = new List<Product>
//    {
//     new Product { Id = 1, Name = "Product 1", Category = "Category 1", Brief = "Brief 1", Description = "Description 1", Price = 100, MainImage = "MainImage 1", Images = "Image 1,Image 2" },
//     new Product { Id = 2, Name = "Product 2", Category = "Category 2", Brief = "Brief 2", Description = "Description 2", Price = 200, MainImage = "MainImage 2", Images = "Images 1,Images 2,Images 3" }, 
//     new Product { Id = 3, Name = "Product 3", Category = "Category 3", Brief = "Brief 3", Description = "Description 3", Price = 300, MainImage = "MainImage 3", Images = "Images 1,Images 2,Images 3" }
//     };

    // private readonly ILogger<ProductController> _logger;

    // public ProductController(ILogger<ProductController> logger)
    // {
    //     _logger = logger;
    // }

    private readonly DataContext _context; // referencia al conexto de del la db

    public ProductController(DataContext context) {
        _context = context;
    }

  
    [HttpGet]
    [SwaggerOperation(Summary = "Listing all products", Description = "Get a list of all the products")]
    [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(List<Product>))]
    [SwaggerResponse(404, "Not Found")]
    public ActionResult<Product> Get()
    {
        var products = _context.Products;
        if (products == null)
            return NotFound();
        else
            return Ok(_context.Products);
    }

    
    [HttpGet]
    [Route("{Id}")]
    [SwaggerOperation(Summary = "Listing a product by its ID", Description = "Get a single product from our database")]
    [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(Product))]
    [SwaggerResponse(404, "Not Found")]

    public ActionResult<Product> Get(int Id)
    {
        var product = _context.Products?.Find(Id); // AÑADIENDO INTERROGANTE PARA PERMITIR NULL
        return product == null? NotFound("Producto no encontrado") : Ok(product);
    }
    
} 

