using CoraetionTask.Data;
using CoraetionTask.Data.Base;
using CoraetionTask.Models;

namespace CoraetionTask.Services
{
    public class ProductRepository : EntityBaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext db) : base(db)
        {
        }
    }
}
