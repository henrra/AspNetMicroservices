using System.Threading.Tasks;
using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Coupon> GetDiscountAsync(string productName)
        {
            await using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM public.coupon WHERE product_name = @ProductName", new { ProductName = productName });
            return coupon
                ?? new Coupon
                {
                    ProductName = "No Discount",
                    Amount = 0,
                    Description = "No Discount Desc"
                };
        }

        public async Task<bool> CreateDiscountAsync(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            int affected = await connection.ExecuteAsync
            ("INSERT INTO public.coupon (product_name, description, amount) values(@ProductName, @Description, @Amount)",
                new { coupon.ProductName, coupon.Description, coupon.Amount });

            return affected > 0;
        }

        public async Task<bool> UpdateDiscountAsync(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            int affected = await connection.ExecuteAsync
            ("UPDATE public.coupon SET product_name=@ProductName,  description=@Description, amount=@Amount)",
                new { coupon.ProductName, coupon.Description, coupon.Amount });
            return affected > 0;
        }

        public async Task<bool> DeleteDiscountAsync(string productName)
        {
            await using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            int affected = await connection.ExecuteAsync
                ("DELETE FROM public.coupon WHERE product_name=@ProductName", new { ProductName = productName });
            return affected > 0;
        }
    }
}