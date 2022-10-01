using MongoDB.Driver;

namespace Light.Admin.Database
{
    public interface IMongoDBContext
    {
        IMongoCollection<T> GetCollection<T>(string? name);
    }
}
