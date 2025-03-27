using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using ProductApi.Models;
using MinimalX.Data;
using Azure.Storage.Blobs;

namespace MinimalX.ProductsControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {   
        // connection to database
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public ProductsController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // retrieve all products
        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts() => await _context.Products.ToListAsync();

        // retrieve single product
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product == null ? NotFound() : product;
        }

        // create a product
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // update a product
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest("ID mismatch");

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // delete a product
         [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{id}/upload-image")]
        [Authorize(Roles = "administrator, vendor")]
        public async Task<ActionResult> UploadImage(int id, IFormFile file)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            var blobServiceClient = new BlobServiceClient(_configuration.GetConnectionString("AzureBlobStorage"));

            var containerClient = blobServiceClient.GetBlobContainerClient("product-images");
            await containerClient.CreateIfNotExistsAsync();

            
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = containerClient.GetBlobClient(fileName);
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream);
            }

            
            product.ImageUrl = blobClient.Uri.ToString();
            product.ImageFileName = fileName;
            await _context.SaveChangesAsync();

            return Ok(new { product.ImageUrl, product.ImageFileName });
        }

        [HttpGet("test-blob")]
        public async Task<IActionResult> TestBlobConnectivity()
        {
        try
 
            {
 
                var blobServiceClient = new BlobServiceClient(_configuration.GetConnectionString("AzureBlobStorage"));
 
                var containerClient = blobServiceClient.GetBlobContainerClient("product-images");
 
                await containerClient.CreateIfNotExistsAsync();
       
                var blobs = new List<string>();
                await foreach (var blobItem in containerClient.GetBlobsAsync())
                {
                    blobs.Add(blobItem.Name);
                }
                return Ok(new { Message = "Connected to blob storage successfully", BlobList = blobs });
 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error connecting to blob storage", Error = ex.Message });
            }
 
        }

    }
}