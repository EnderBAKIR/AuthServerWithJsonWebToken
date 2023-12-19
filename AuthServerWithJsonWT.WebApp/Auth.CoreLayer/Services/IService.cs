using Auth.SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Auth.CoreLayer.Services
{
    public interface IService<Tentity , TDto>where Tentity : class where TDto : class
    {
        Task<Response<TDto>> GetByIdAsync(int id);

        Task<Response<IEnumerable<TDto>>> GetAllAsync();

        Task<Response< IEnumerable<TDto>>> Where(Expression<Func<Tentity, bool>> predicate);

        Task<Response<TDto>> AddAsync(Tentity entity);

        Task<Response<NoDataDto>> Remove(Tentity entity);

        Task<Response<NoDataDto>> Update(Tentity entity);
    }
}
