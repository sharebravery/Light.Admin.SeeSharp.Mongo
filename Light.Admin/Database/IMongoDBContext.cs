using MongoDB.Driver;

namespace Light.Admin.Database
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string? name);

    }
}
