using AutoMapper;
using Light.Admin.Database;
using Light.Admin.Dtos;
using Light.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Light.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        //private readonly IDefaultDbContext db;
        protected IMongoCollection<User> userCollection;
        private readonly IMapper mapper;

        private readonly UserManager<User> userManager;
        public AccountController(IMapper mapper, IMongoCollection<User> userCollection, UserManager<User> userManager)
        {
            //this.db = db;
            this.userCollection = userCollection;
            this.mapper = mapper;
            this.userManager = userManager;
        }
    }
}
