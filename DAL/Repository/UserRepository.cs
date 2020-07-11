using DAL.Entities;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repository
{
    public class UserRepository : IRepository<User>
    {
        private readonly Context _context = new Context();

        public IQueryable<User> GetAll()
        {
            return _context.Users;
        }


        public User GetById(int id)
        {
            return _context.Users.SingleOrDefault(e => e.UserID == id);
        }


        public void Create(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();

        }


        public void Update(User entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();

        }

        public void Delete(int id)
        {
            User user = GetById(id);
            _context.Users.Remove(user);
            _context.SaveChanges();

        }
    }
}
