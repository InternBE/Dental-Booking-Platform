using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.Contract.Repository.IUOW
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Entities { get; }

        // non async
        IEnumerable<T> GetAll();
        T? GetById(object id);
        void Insert(T obj);
        void InsertRange(IList<T> obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task InsertAsync(T obj);
        void Delete(T entity);
    }
}
