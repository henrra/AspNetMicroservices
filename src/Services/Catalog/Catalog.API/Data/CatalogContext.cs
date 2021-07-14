using Catalog.API.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IOptions<DatabaseSetting> dbSettingOption)
        {
            DatabaseSetting dbSetting = dbSettingOption.Value;
            var client = new MongoClient(dbSetting.ConnectionString);
            IMongoDatabase database = client.GetDatabase(dbSetting.DatabaseName);
            Products = database.GetCollection<Product>(dbSetting.CollectionName);
            
            // Seed
            CatalogContextSeed.SeedData(Products);
        }
        
        public IMongoCollection<Product> Products { get; }
    }
}