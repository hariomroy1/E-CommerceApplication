using DataLayer.Data;
using DataLayer.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using Training.Product.Middleware;
using Training.Product.Services;

namespace Training.Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IProductRepository _productRepostiory;
        private readonly ProductContext _context;

        public ProductController(IConfiguration configuration, IProductRepository productRepository)
        {
            _configuration = configuration;
            _productRepostiory = productRepository;
        }

        /// <summary>
        /// Adds a new product based on the provided ProductEntity.
        /// </summary>
        /// <param name="product">The ProductEntity containing product details.</param>
        /// <returns>
        /// An IActionResult representing the result of the product addition operation.
        /// If the product is added successfully, returns Ok with the result.
        /// If the provided ProductEntity is null, throws an Exception.
        /// </returns>
        /// <exception>
        /// Thrown when the provided ProductEntity is null.
        /// </exception>

        [HttpPost("AddProduct")]
       
        public IActionResult AddProduct([FromBody] ProductEntity product)
        {
            if (product == null)
            {
                throw new Exception("Product Not added");
            }
            var result = _productRepostiory.AddProduct(product);
            return Ok(result);

        }

        /// <summary>
        /// Retrieves all products from the repository.
        /// </summary>
        /// <returns>
        /// An IActionResult representing the result of the product retrieval operation.
        /// If products are present, returns Ok with the list of products.
        /// If no products are found, throws an InvalidOperationException.
        /// </returns>
        /// <exception>
        /// Thrown when no products are found in the repository.
        /// </exception>

        [HttpGet("GetProducts")]
       // [Authorize(Roles ="user")]
        public IActionResult GetProducts()
        {
            var products = _productRepostiory.GetAllProducts();
            return Ok(products);

            throw new InvalidOperationException("Product Not present");
        }
        /// <summary>
        /// Finds a product in the repository based on the provided productId.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to find.</param>
        /// <returns>
        /// An IActionResult representing the result of the product retrieval operation.
        /// If the product is found, returns Ok with the product details.
        /// If the product is not found, throws an InvalidOperationException.
        /// </returns>
        /// <param name="productId">The unique identifier of the product to find.</param>
        /// <exception>
        /// Thrown when the product with the provided ID is not found.
        /// </exception>

        [HttpGet("FindProduct/{productId}")]
        //[Authorize(Roles = "user")]
        public async Task<IActionResult> FindProduct(int productId)
        {
            var product = await _productRepostiory.FindProduct(productId);
            /* if (product == null)
             {
                 return NotFound(new { Message = "No such product exist in the database" });
             }*/
            if (product != null)
            {
                return Ok(product);
            }

            throw new InvalidOperationException("Product Not present");
        }

        /// <summary>
        /// Deletes a product from the repository based on the provided productId.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to delete.</param>
        /// <returns>
        /// An IActionResult representing the result of the product deletion operation.
        /// If the product is deleted successfully, returns Ok with a success message.
        /// If the product is not found, throws an InvalidOperationException.
        /// </returns>
        /// <param name="productId">The unique identifier of the product to delete.</param>
        /// <exception>
        /// Thrown when the product with the provided ID is not found.
        /// </exception>

        [HttpDelete("DeleteProduct/{productId}")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var result = await _productRepostiory.DeleteProduct(productId);

            if (!result)
            {
                throw new InvalidOperationException($"User with ID {productId} not found.");
            }

            return Ok(new { Message = "Product Deleted!" });
        }
        /// <summary>
        /// Updates an existing product in the repository based on the provided updatedProduct entity.
        /// </summary>
        /// <param name="updatedProduct">The updated product entity with modified details.</param>
        /// <returns>
        /// An IActionResult representing the result of the product update operation.
        /// If the product is updated successfully, returns Ok with a success message.
        /// If the product is not found, throws an InvalidOperationException.
        /// </returns>
        /// <param name="updatedProduct">The updated product entity with modified details.</param>
        /// <exception>
        /// Thrown when the product with the provided ID is not found.
        /// </exception>

        [HttpPut("UpdateProduct")]
       // [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductEntity updatedProduct)
        {
            var result = await _productRepostiory.UpdateProduct(updatedProduct);

            if (!result)
            {
                throw new InvalidOperationException($"No Such Product present in Database");
            }

            return Ok(new { Message = "Product updated successfully" });
        }

        /// <summary>
        /// Retrieves a list of product categories from the repository.
        /// </summary>
        /// <returns>
        /// An IActionResult representing the result of the product category retrieval operation.
        /// If product categories are present, returns Ok with the list of categories.
        /// If no categories are found, throws an InvalidOperationException.
        /// </returns>
        /// <exception>
        /// Thrown when no product categories are found in the repository.
        /// </exception>

        [HttpGet("GetProductCategories")]
       // [Authorize(Roles = "user")]
        public async Task<IActionResult> GetProductCategories()
        {
            var result = _productRepostiory.GetProductCategories();
            return Ok(result);

            throw new InvalidOperationException($"No Such Product Category in Present");

        }

        /// <summary>
        /// Updates the quantity of items for a specific product in the repository.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="quantityOfItems">The new quantity of items for the product.</param>
        /// <returns>
        /// An IActionResult representing the result of the product quantity update operation.
        /// If the update is successful, returns Ok with a success message.
        /// If the product is not found, throws an InvalidOperationException.
        /// </returns>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="quantityOfItems">The new quantity of items for the product.</param>
        /// <exception>
        /// Thrown when the product with the specified ID is not found in the repository.
        /// </exception>

        [HttpPut("UpdateProductQuantity/{productId}/{quantityOfItmes}")]
       // [Authorize(Roles = "user")]
        public async Task<IActionResult> UpdateProductQuantityAsync(int productId, int quantityOfItmes)
        {

            bool success = await _productRepostiory.UpdateProductQuantityAsync(productId, quantityOfItmes);

            if (success)
            {
                return Ok("Product quantity updated successfully.");
            }
            else
            {
                throw new BadRequestException($"Product is not Present with this id: {productId}");
            }
        }
    }
}
