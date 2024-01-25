using DataLayer.Data;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.User.Services;

namespace Training.User.Respositories
{
    public class RegisterRepository : IRegisterRepository
    {

        public readonly UserContext _context;
        public RegisterRepository(UserContext context)
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

        public async Task<RegisterEntity> FindCurrentUser(string email)
        {
            return await _context.Registers.FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task<RegisterEntity> FindCurrentUserById(int userId)
        {
            return await _context.Registers.FirstOrDefaultAsync(x => x.RegisterId == userId);
        }
        public RegisterEntity Login(LoginEntity user)
        {
            if(user.Email == null || user.Password== null)
            {
                throw new Exception("Login Id or Password is not present.");
            }
            return _context.Registers.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
        }
    }
}
