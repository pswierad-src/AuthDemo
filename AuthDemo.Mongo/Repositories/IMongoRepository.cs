using System.Linq.Expressions;
using AuthDemo.Mongo.Documents;

namespace AuthDemo.Mongo.Repositories;

public interface IMongoRepository<TDocument> where TDocument : class, IDocument
{
    Task<IEnumerable<TDocument>> GetAsync();
    Task<TDocument> FirstOrDefaultAsync(Expression<Func<TDocument, bool>> filter);
    Task<TDocument> AddAsync(TDocument document);
    Task DeleteAsync(Expression<Func<TDocument, bool>> filter);
}