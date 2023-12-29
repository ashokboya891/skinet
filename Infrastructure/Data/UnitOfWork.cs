using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entites;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _storeContext;
        private  Hashtable _repositories;

        public UnitOfWork(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        public async Task<int> Complete()
        {
            return await _storeContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _storeContext.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_repositories==null) _repositories=new Hashtable();
            var type=typeof(TEntity).Name;
            if(!_repositories.ContainsKey(type))
            {
                var _repositoryType=typeof(GenericRepository<>);
                var _repositoryinstance=Activator.CreateInstance(_repositoryType.MakeGenericType
                (typeof(TEntity)),_storeContext);
                _repositories.Add(type,_repositoryinstance);

            }
            return (IGenericRepository<TEntity>) _repositories[type];
        }
    }
}