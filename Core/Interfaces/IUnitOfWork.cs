using System;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity: BaseEntity;
        
        // Returns the number of changes to the database
        Task<int> Complete();
    }
}