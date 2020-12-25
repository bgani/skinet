using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;

        // hashtable stores all of the repositories in use inside of Uow
        private Hashtable _repositories;
        public UnitOfWork(StoreContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            // TEntity is a type of an entity e.g Product

            // checking whether hastable already created
            if(_repositories == null) 
                _repositories = new Hashtable();
            
            var type = typeof(TEntity).Name;

            // checking if repo already contains particular type
            if(!_repositories.ContainsKey(type))
            {
                // getting GenericRepository type
                var repositoryType = typeof(GenericRepository<>);

                // creating instance of repository (e.g Product) and passing context that we get from Uow 
                var repositoryInstance = Activator.CreateInstance(
                    repositoryType.MakeGenericType(typeof(TEntity)), 
                    _context);

                // adding repo to hashtable
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>) _repositories[type];

        }
    }
}