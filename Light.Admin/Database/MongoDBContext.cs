using MongoDB.Bson;
using MongoDB.Driver;

namespace Light.Admin.Database
{
    public class MongoDbContext : MongoDatabaseBase
    {
        public override IMongoClient Client => throw new NotImplementedException();

        public override DatabaseNamespace DatabaseNamespace => throw new NotImplementedException();

        public override MongoDatabaseSettings Settings => throw new NotImplementedException();

        public override Task CreateCollectionAsync(string name, CreateCollectionOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task DropCollectionAsync(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override IMongoCollection<TDocument> GetCollection<TDocument>(string name, MongoCollectionSettings settings = null)
        {
            throw new NotImplementedException();
        }

        public override Task<IAsyncCursor<BsonDocument>> ListCollectionsAsync(ListCollectionsOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task RenameCollectionAsync(string oldName, string newName, RenameCollectionOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<TResult> RunCommandAsync<TResult>(Command<TResult> command, ReadPreference readPreference = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
