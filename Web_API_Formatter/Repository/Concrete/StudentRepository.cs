using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Web_API_Formatter.Data;
using Web_API_Formatter.Entities;
using Web_API_Formatter.Repository.Abstract;

namespace Web_API_Formatter.Repository.Concrete
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolDbContext _context;

        public StudentRepository(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Student entity)
        {
            await _context.Students.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Student entity)
        {
            await Task.Run(() =>
            {
                _context.Students.Remove(entity);
            });

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await Task.Run(() =>
            {
                return _context.Students;
            });
        }

        public async Task<Student> GetAsync(Expression<Func<Student, bool>> expression)
        {
            var item = await _context.Students.FirstOrDefaultAsync(expression);
            return item;
        }

        public async Task UpdateAsync(Student entity)
        {
            await Task.Run(() =>
            {
                _context.Students.Update(entity);
            });
            await _context.SaveChangesAsync();
        }
    }
}
