using System.Threading.Tasks;
using Discount.Grpc.Protos;

namespace Basket.API.Services
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountServiceClient;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountServiceClient)
        {
            _discountServiceClient = discountServiceClient;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            CouponModel discount = await _discountServiceClient.GetDiscountAsync(new GetDiscountRequest { ProductName = productName});
            return discount;
        }
    }
}