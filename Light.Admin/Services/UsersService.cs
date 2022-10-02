using AutoMapper;
using Light.Admin.CSharp.Dtos;
using Light.Admin.CSharp.Models;
using Light.Admin.Database;
using Light.Admin.IServices;
using MongoDB.Driver;

namespace Light.Admin.Services
{
    public class UsersService : IUsersService
    {
        private readonly IMongoDBContext db;
        protected IMongoCollection<User> userCollection;
        private readonly IMapper mapper;
        public UsersService(IMongoDBContext db, IMapper mapper)
        {
            this.db = db;
            userCollection = db.GetCollection<User>(typeof(User).Name);
            this.mapper = mapper;
        }

        public Task<string> Create(UserDto userDto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> Find(string username, string name, string phoneNumber)
        {
            var all = await userCollection.FindAsync(Builders<User>.Filter.Empty);
            return all.ToList();
        }

        public Task<User> FindOne(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(UserDto userDto)
        {
            throw new NotImplementedException();
        }
    }
}
