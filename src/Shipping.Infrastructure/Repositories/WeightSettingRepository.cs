using Microsoft.EntityFrameworkCore;
using Shipping.Domain.Entities;
using Shipping.Domain.Repositories;
using Shipping.Infrastructure.Persistence;

namespace Shipping.Infrastructure.Repositories;
internal class WeightSettingRepository(ApplicationDbContext _context) : GenericRepository<WeightSetting, int>(_context), IWeightSettingRepository
{
    public async Task<IEnumerable<WeightSetting>> GetAllWeightSetting()
    => await _context.WeightSettings.ToListAsync();

}
