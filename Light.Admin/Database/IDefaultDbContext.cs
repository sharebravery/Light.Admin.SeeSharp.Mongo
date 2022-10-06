using MongoDB.Driver;

namespace Light.Admin.Database
{
    public interface IDefaultDbContext
    {
        IMongoCollection<T> GetCollection<T>(string? name);
    }
}
