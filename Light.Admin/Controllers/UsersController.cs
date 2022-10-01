using Light.Admin.CSharp.Models;
using Light.Admin.Database;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Light.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {

        private readonly IMongoDBContext db;
        protected IMongoCollection<User> userCollection;
        public UsersController(IMongoDBContext db)
        {
            this.db = db;
            userCollection = db.GetCollection<User>(typeof(User).Name);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var all = await userCollection.FindAsync(Builders<User>.Filter.Empty);
            return Ok(all.ToList());
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        [HttpPost]
        public async void Post(User user)
        {
             await userCollection.InsertOneAsync(user);
        }
    }
}
