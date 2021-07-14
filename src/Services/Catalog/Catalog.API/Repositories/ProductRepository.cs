using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(_catalogContext));
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _catalogContext
                .Products
                .Find(p => true)
                .ToListAsync();
        }

        public async Task<Product> GetProduct(string productId)
        {
            return await _catalogContext
                .Products
                .Find(p => p.Id == productId)
                .FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            FilterDefinition<Product> filters = Builders<Product>
                .Filter
                .ElemMatch(p => p.Name, name);
            return await _catalogContext
                .Products
                .Find(filters)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string categoryName)
        {
            FilterDefinition<Product> filters = Builders<Product>
                .Filter
                .Eq(p => p.Category, categoryName);
            return await _catalogContext
                .Products
                .Find(filters)
                .ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            ReplaceOneResult updateResult = await _catalogContext
                .Products
                .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged
                   && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string productId)
        {
            DeleteResult updateResult = await _catalogContext
                .Products
                .DeleteOneAsync(filter: g => g.Id == productId);
            return updateResult.IsAcknowledged
                   && updateResult.DeletedCount > 0;
        }
    }
}