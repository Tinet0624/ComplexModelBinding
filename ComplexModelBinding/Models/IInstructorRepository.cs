using ComplexModelBinding.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ComplexModelBinding.Models
{
    public interface IInstructorRepository
    {
        Task SaveInstructor(Instructor instructor);
        Task<IEnumerable<Instructor>> GetAllInstructors();
        Task DeleteInstructor(int instructorId);
        Task UpdateInstructor(Instructor instructor);
        Task<Instructor?> GetInstructor(int instructorId);
    }

    public class InstructorRepository : IInstructorRepository
    {
        private ApplicationDbContext _context;

        public InstructorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteInstructor(int instructorId)
        {
            Instructor? instructor = await GetInstructor(instructorId);

            if (instructor != null)
            {
                // change all related courses to a null instructor
                using DbConnection con = _context.Database.GetDbConnection();
                await con.OpenAsync();
                using DbCommand query = con.CreateCommand();
                query.CommandText = "UPDATE Courses SET TeacherId = null WHERE TeacherId = " + instructor.Id;
                int rowsAffected = await query.ExecuteNonQueryAsync();

                //remove instructor form db
                _context.Instructors.Remove(instructor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Instructor>> GetAllInstructors()
        {
            return await _context.Instructors.OrderBy(Instructor => Instructor.FullName).ToListAsync();
        }

        public async Task<Instructor?> GetInstructor(int instructorId)
        {
            return await _context.Instructors.SingleOrDefaultAsync(i => i.Id == instructorId);
        }

        public async Task SaveInstructor(Instructor instructor)
        {
            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInstructor(Instructor instructor)
        {
            _context.Add(instructor);
            await _context.SaveChangesAsync();
        }


    }
}
