using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace SportsStore.Models {
    public class EFOrderRepository : IOrderRepository {
        private ApplicationDbContext context;
        public EFOrderRepository(ApplicationDbContext ctx) {
            context = ctx;
        }
        public IQueryable<Order> Orders => context.Orders
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product);
        public void SaveOrder(Order order) {
            context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderID == 0) {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }
        public Product DeleteProduct(int productID) {
            Product dbEntry = context.Products
            .FirstOrDefault(p => p.ProductID == productID);
            if (dbEntry != null) {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}