using DAL.Entities;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repository
{
    public class CategoryRepository : IRepository<Category>
    {

        private readonly Context _context = new Context();

        public IQueryable<Category> GetAll()
        {
            return _context.Categories;
        }


        public Category GetById(int id)
        {
            return _context.Categories.SingleOrDefault(e => e.CategoryID == id);
        }


        public void Create(Category entity)
        {
            _context.Categories.Add(entity);
            _context.SaveChanges();

        }


        public void Update(Category entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();

        }

        public void Delete(int id)
        {
            Category category = GetById(id);
            _context.Categories.Remove(category);
            _context.SaveChanges();

        }




    }
}
