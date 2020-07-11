using DAL.Entities;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repository
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly Context _context = new Context();

        public IQueryable<Product> GetAll()
        {
            return _context.Products;
        }


        public Product GetById(int id)
        {
            return _context.Products.SingleOrDefault(e => e.ProductID == id);
        }


        public void Create(Product entity)
        {
            _context.Products.Add(entity);
            _context.SaveChanges();

        }


        public void Update(Product entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();

        }

        public void Delete(int id)
        {
            Product product = GetById(id);
            _context.Products.Remove(product);
            _context.SaveChanges();

        }
    }
}
