using DataLayer.Data;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Training.User.Services;

namespace Training.User.Respositories
{
    public class RegisterRepository : IRegisterRepository
    {

        public readonly DatabaseContext _context;
        public RegisterRepository(DatabaseContext context)
        {
            _context = context;
        }
        public string Create(RegisterEntity user)
        {
            if (_context.Registers.Where(u => u.Email == user.Email).FirstOrDefault() != null)
            {
                return "Already Exist";
            }

            user.MemberSince = DateTime.Now;
            _context.Registers.Add(user);
            _context.SaveChanges();
            return "Success";
        }
        /// <summary>
        /// Finds and retrieves the user entity from the database with the specified email address.
        /// </summary>
        /// <param name="email">The email address of the user to find.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the <see cref="RegisterEntity"/>
        /// entity corresponding to the specified email address, if found; otherwise, null.</returns>
        public async Task<RegisterEntity> FindCurrentUser(string email)
        {
            return await _context.Registers.FirstOrDefaultAsync(x => x.Email == email);
        }

        /// <summary>
        /// Finds and retrieves the user entity from the database with the specified user ID.
        /// </summary>
        /// <param name="userId">The ID of the user to find.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the <see cref="RegisterEntity"/>
        /// entity corresponding to the specified user ID, if found; otherwise, null.</returns>
        public async Task<RegisterEntity> FindCurrentUserById(int userId)
        {
            return await _context.Registers.FirstOrDefaultAsync(x => x.RegisterId == userId);
        }

        /// <summary>
        /// Validates the user credentials provided in the <paramref name="user"/> object and returns the corresponding user entity if the credentials are valid.
        /// </summary>
        /// <param name="user">The login credentials provided by the user.</param>
        /// <returns>The <see cref="RegisterEntity"/> entity representing the user 
        /// if the login credentials are valid; otherwise, returns null.</returns>
        public RegisterEntity Login(LoginEntity user)
        {

            return _context.Registers.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
        }
    }
}
