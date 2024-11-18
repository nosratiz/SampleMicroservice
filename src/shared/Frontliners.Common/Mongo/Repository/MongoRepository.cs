using System.Linq.Expressions;
using MongoDB.Driver;

namespace Frontliners.Common.Mongo.Repository;

public class MongoRepository<TEntity>(IMongoDatabase database, string collectionName) : IMongoRepository<TEntity>
    where TEntity : IIdentifiable
{
    private IMongoCollection<TEntity> Collection { get; } = database.GetCollection<TEntity>(collectionName);

    public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        => Collection.AsQueryable().Where(predicate);

    public IQueryable<TEntity> GetAll() => Collection.AsQueryable().Where(e=>e.IsDeleted==false);



    public async Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken)
        => await GetAsync(e => e.Id == id && e.IsDeleted==false, cancellationToken);

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken) => await Collection.Find(predicate)
        .SingleOrDefaultAsync(cancellationToken);

    public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken) => await Collection.Find(predicate).ToListAsync(cancellationToken);

    public async Task<List<TEntity>> FindAsync(CancellationToken cancellationToken) =>
        await Collection.Find(FilterDefinition<TEntity>.Empty).ToListAsync(cancellationToken);

    public Task<List<T>> FindAsync<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> selector, 
        int take, CancellationToken cancellationToken)
        => Collection.Find(predicate).Project(selector).Limit(take).ToListAsync(cancellationToken);
        
        
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken) 
        => await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
      
    public async Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken)
        => await Collection.InsertManyAsync(entities, cancellationToken: cancellationToken);

        
    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        => await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, cancellationToken: cancellationToken);
        
        
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        => await Collection.DeleteOneAsync(e => e.Id == id, cancellationToken);

        
    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        => await Collection.Find(predicate).AnyAsync(cancellationToken);

      
    public async Task CreateIndexAsync(Expression<Func<TEntity, object>> keySelector, CreateIndexOptions? options = null, bool ascending = true)
    {            var indexKeysDefinition = ascending ? Builders<TEntity>.IndexKeys.Ascending(keySelector)
            : Builders<TEntity>.IndexKeys.Descending(keySelector);
          
        var indexModel = new CreateIndexModel<TEntity>(indexKeysDefinition, options);
        await Collection.Indexes.CreateOneAsync(indexModel);
    }

    public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        await Collection.DeleteManyAsync(predicate, cancellationToken);
    }

    public async Task<List<T>> FindAsync<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> selector, CancellationToken cancellationToken)
        => await Collection.Find(predicate).Project(selector)
            .ToListAsync(cancellationToken);
        

    public async Task UpdateAllAsync(Expression<Func<TEntity, bool>> predicate, UpdateDefinition<TEntity> update, CancellationToken cancellationToken = default)
    {
        await Collection.UpdateManyAsync(predicate, update, cancellationToken: cancellationToken);
    }

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await Collection.CountDocumentsAsync(predicate, cancellationToken: cancellationToken);
        

        
    public async Task<List<T>> FindAsync<T>(Expression<Func<TEntity, bool>> predicate,
        SortDefinition<TEntity> sort,
        Expression<Func<TEntity, T>> selector,
        int take, CancellationToken cancellationToken)
        => await Collection.Find(predicate)
            .Sort(sort)
            .Project(selector)
            .Limit(take)
            .ToListAsync(cancellationToken);
}