using DentalBooking.Contract.Repository.IUOW;
using DentalBooking.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly DatabaseContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(DatabaseContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    // Triển khai thuộc tính Entities
    public IQueryable<T> Entities => _dbSet;

    // Lấy tất cả bản ghi
    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }

    // Lấy tất cả bản ghi (async)
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    // Lấy bản ghi theo Id (object)
    public T GetById(object id)
    {
        return _dbSet.Find(id);
    }

    // Lấy bản ghi theo Id (async)
    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    // Tìm kiếm với điều kiện (predicate)
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    // Thêm bản ghi (async)
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    // Thêm bản ghi (non-async)
    public void Insert(T obj)
    {
        _dbSet.Add(obj);
    }

    // Thêm nhiều bản ghi (non-async)
    public void InsertRange(IList<T> obj)
    {
        _dbSet.AddRange(obj);
    }

    // Thêm bản ghi (async)
    public async Task InsertAsync(T obj)
    {
        await _dbSet.AddAsync(obj);
    }

    // Cập nhật bản ghi
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    // Xóa bản ghi (non-async)
    public void Delete(object id)
    {
        var entity = _dbSet.Find(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    // Xóa bản ghi (async)
    public async Task DeleteAsync(object id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    // Xóa bản ghi (non-async)
    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    // Xóa bản ghi (async) theo đối tượng kiểu T
    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    // Lưu các thay đổi (non-async)
    public void Save()
    {
        _context.SaveChanges();
    }

    // Lưu các thay đổi (async)
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
