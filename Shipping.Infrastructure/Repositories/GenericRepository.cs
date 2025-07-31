using Microsoft.EntityFrameworkCore;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;
using Shipping.Infrastructure.Persistence;

namespace Shipping.Infrastructure.Repositories;
internal class GenericRepository<T, Tkey>(ApplicationDbContext context) : IGenericRepository<T, Tkey> where T : class where Tkey : IEquatable<Tkey>
{
    public async Task AddAsync(T entity)
    => await context.Set<T>().AddAsync(entity);

    public void UpdateAsync(T entity)
    => context.Set<T>().Update(entity);

    public async Task<T?> GetByIdAsync(Tkey id)
    => await context.Set<T>().FindAsync(id);

    public async Task<T?> GetByIdAsync(Tkey id, Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        var query = context.Set<T>().AsQueryable();

        if (include != null)
            query = include(query);

        return await query.FirstOrDefaultAsync(e => EF.Property<Tkey>(e, "Id")!.Equals(id));
    }

    public async Task DeleteAsync(Tkey id)
    {
        var entity = await context.Set<T>().FindAsync(id);

        if (entity is not null)
            context.Set<T>().Remove(entity);
    }

    public async Task<IEnumerable<T>> GetAllAsync(PaginationParameters pramter)
    {
        if (pramter.PageSize is not null && pramter.PageNumber is not null)
        {
            return await context.Set<T>()
                .Skip((pramter.PageNumber.Value - 1) * pramter.PageSize.Value)
                .Take(pramter.PageSize.Value)
                .ToListAsync();
        }
        else
            return await context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(PaginationParameters pramter, Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        var query = context.Set<T>().AsQueryable();

        if (include is not null)
            query = include(query);


        if (pramter.PageSize is not null && pramter.PageNumber is not null)
        {
            return await context.Set<T>()
                .Skip((pramter.PageNumber.Value - 1) * pramter.PageSize.Value)
                .Take(pramter.PageSize.Value)
                .ToListAsync();
        }

        return await query.ToListAsync();

    }


}
