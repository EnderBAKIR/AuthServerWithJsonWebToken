using Auth.CoreLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.RepositoryLayer.UnitOfWorks
{
    public class UnitOFWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public UnitOFWork(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
