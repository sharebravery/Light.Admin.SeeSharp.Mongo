using AutoMapper;
using Light.Admin.Database;
using Light.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Light.Admin.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IMongoDBContext db;
        protected IMongoCollection<User> userCollection;
        private readonly IMapper mapper;
        public AccountController(IMongoDBContext db, IMapper mapper)
        {
            this.db = db;
            userCollection = db.GetCollection<User>(typeof(User).Name);
            this.mapper = mapper;
        }

    }
}
