using System;
using System.Linq.Expressions;
using ControleReservas.Domain.Interfaces;
using ControleReservas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ControleReservas.Infrastructure.Repositories;

public class Repository<T> : IRespository<T> where T : class
{

    protected readonly ControleReservasDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ControleReservasDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public virtual async Task<T?> GetByIdAsync(Guid id)
        => await _dbSet.FindAsync(id);

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();

    public virtual async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public virtual void UpdateAsync(T entity)
        => _dbSet.Update(entity);
        
    public virtual void Remove(T entity)
        => _dbSet.Remove(entity);

}
