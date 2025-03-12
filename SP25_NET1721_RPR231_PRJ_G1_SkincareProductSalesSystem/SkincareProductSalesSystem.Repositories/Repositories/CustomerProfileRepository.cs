using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Base;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Repositories.Repositories
{
    public class CustomerProfileRepository : GenericRepository<CustomerProfile>
    {
        public async Task<CustomerProfile> GetProfileByUserId(string userId)
        {
            return await _context.CustomerProfiles.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }
    }
}