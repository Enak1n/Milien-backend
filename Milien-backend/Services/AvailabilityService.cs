using Microsoft.EntityFrameworkCore;
using Milien_backend.DataBase;
using Milien_backend.Services.Interfaces;

namespace Milien_backend.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly Context _context;

        public AvailabilityService(Context context)
        {
            _context = context;
        }

        public async Task<bool> CheckAcceptEmail(string login)
        {
            var user = await _context.Customers.FirstOrDefaultAsync(x => x.Login == login);

            if(user == null)
            {
                return false;
            }

            return user.ComfimedEmail;
        }

        public async Task<bool> CheckCode(string code, string email)
        {
            var user = await _context.Customers.FirstOrDefaultAsync(x => x.Email == email);

            if(user.ConfirmedCode == code)
            {
                user.ConfirmedCode = null;
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> CheckEmail(string email)
        {
            var emailForCheck = _context.Customers.FirstOrDefault(x => x.Email == email);

            return emailForCheck == null ? true : false;
        }

        public async Task<bool> CheckLogin(string login)
        {
            var loginForCheck = _context.Customers.FirstOrDefault(p => p.Login == login);

            return loginForCheck == null ? true : false;
        }

        public async Task<bool> CheckPhoneNumber(string phoneNumber)
        {
            var user = _context.Customers.FirstOrDefault(p => p.PhoneNumber == phoneNumber);

            return user == null ? true : false;
        }
    }
}
