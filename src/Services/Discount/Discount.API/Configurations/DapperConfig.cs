using Discount.API.Entities;
using Discount.API.Mappers;

namespace Discount.API.Configurations
{
    public class DapperConfig
    {
        public static void Configure()
        {
            ColumnMapper<Coupon>.CreateColumnMap()
                .SetMap(x => x.Id, "id")
                .SetMap(x => x.Description, "description")
                .SetMap(x => x.ProductName, "product_name")
                .SetMap(x => x.Amount, "amount")
                .Map();
        }
    }
}