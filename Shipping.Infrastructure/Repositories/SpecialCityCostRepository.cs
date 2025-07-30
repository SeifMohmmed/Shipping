using Microsoft.EntityFrameworkCore;
using Shipping.Domain.Entities;
using Shipping.Domain.Repositories;
using Shipping.Infrastructure.Persistence;

namespace Shipping.Infrastructure.Repositories;
internal class SpecialCityCostRepository(ApplicationDbContext context) : GenericRepository<SpecialCityCost, int>(context), ISpecialCityCostRepository
{
    public async Task AddRangeAsync(IEnumerable<SpecialCityCost> entities)
    {
        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task<SpecialCityCost> GetCityCostByMarchantId(string MerchantId, int CityId)
    {
        var CityCostByMarchantId = await context.SpecialCityCosts.FirstOrDefaultAsync
            (c => c.MerchantId == MerchantId && c.CitySettingId == CityId);

        if (CityCostByMarchantId is null)
            return null!;

        return CityCostByMarchantId;

    }
}
