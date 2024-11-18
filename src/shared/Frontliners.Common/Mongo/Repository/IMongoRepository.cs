using System.Linq.Expressions;
using MongoDB.Driver;

namespace Frontliners.Common.Mongo.Repository;

public interface IMongoRepository<TEntity> where TEntity : IIdentifiable
{
    IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);

    IQueryable<TEntity> GetAll();
    Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    Task<List<TEntity>> FindAsync(CancellationToken cancellationToken);
        

    Task<List<T>> FindAsync<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> selector, CancellationToken cancellationToken);
        
    Task<List<T>> FindAsync<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> selector,int Take, CancellationToken cancellationToken);

    Task<List<T>> FindAsync<T>(Expression<Func<TEntity, bool>> predicate,SortDefinition<TEntity> sort, Expression<Func<TEntity, T>> selector, int Take, CancellationToken cancellationToken);
                
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAllAsync(Expression<Func<TEntity, bool>> predicate, UpdateDefinition<TEntity> update, CancellationToken cancellationToken = default);

        
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteManyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken=default);

    Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
       
    Task CreateIndexAsync(Expression<Func<TEntity, object>> keySelector, CreateIndexOptions? options = null, bool ascending = true);

}