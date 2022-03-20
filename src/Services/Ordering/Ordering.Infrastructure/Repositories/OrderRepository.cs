using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }

        public DbSet<Order> CurrentTable => OrderDatabase.Table<Order>();

        public async Task<IEnumerable<Order>> GetOrderByUserName(string userName)
        {
            List<Order> orderList = await CurrentTable.Where(o => o.UserName == userName).ToListAsync();
            return orderList;
        }
    }
}