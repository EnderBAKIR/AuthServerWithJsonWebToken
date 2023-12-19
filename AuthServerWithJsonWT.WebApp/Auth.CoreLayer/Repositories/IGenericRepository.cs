using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Auth.CoreLayer.Repositories
{
    public interface IGenericRepository<Tentity> where Tentity : class
    {
        Task<Tentity> GetByIdAsync(int id);
        Task<IEnumerable<Tentity>> GetAllAsync();
        IQueryable<Tentity> Where(Expression<Func<Tentity,bool>>predicate);
        Task AddAsync(Tentity entity);
        void Remove(Tentity entity);
        Tentity Update(Tentity entity); 
    }
}
