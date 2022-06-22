using System.Linq.Expressions;
using AuthDemo.Mongo.Config;
using AuthDemo.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace AuthDemo.Mongo.Repositories;

public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : class, IDocument
{
    private readonly IMongoCollection<TDocument> _collection;

    public MongoRepository(MongoSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var db = client.GetDatabase(settings.DatabaseName);
        _collection = db.GetCollection<TDocument>(typeof(TDocument).Name);
    }

    public async Task<IEnumerable<TDocument>> GetAsync()
    {
        var documents = await _collection.FindAsync(_ => true);
        return await documents.ToListAsync();
    }

    public async Task<TDocument> FirstOrDefaultAsync(Expression<Func<TDocument,bool>> filter)
    {
        var document = await _collection.FindAsync(filter);

        return await document.FirstOrDefaultAsync();
    }

    public async Task<TDocument> AddAsync(TDocument document)
    {
        await _collection.InsertOneAsync(document);

        return document;
    }

    public async Task DeleteAsync(Expression<Func<TDocument, bool>> filter)
    {
        await _collection.DeleteManyAsync(filter);
    }

    public IMongoQueryable<TDocument> GetQuery()
    {
        return _collection.AsQueryable();
    }
}