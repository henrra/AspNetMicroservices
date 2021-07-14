using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync()
        {
            IEnumerable<Product> products = await _productRepository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductAsync(string id)
        {
            Product product = await _productRepository.GetProduct(id);
            if (product != null) return Ok(product);
            _logger.LogError("Product with id : {ProductId} is not found", id);
            return NotFound();
        }

        [HttpGet]
        [Route("[action]/{category}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategoryAsync(string category)
        {
            IEnumerable<Product> products = await _productRepository.GetProductsByName(category);
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProductAsync([FromBody] Product product)
        {
            await _productRepository.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new {id = product.Id}, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int) HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateProductAsync([FromBody] Product product)
        {
            return Ok(await _productRepository.UpdateProduct(product));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Product), (int) HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteProductAsync(string id)
        {
            return Ok(await  _productRepository.DeleteProduct(id));
        }
    }
}