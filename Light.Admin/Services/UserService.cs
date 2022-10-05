using AutoMapper;
using Light.Admin.Database;
using Light.Admin.Dtos;
using Light.Admin.IServices;
using Light.Admin.Models;
using Light.Admin.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading;
using System.Transactions;

namespace Light.Admin.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoDBContext db;
        protected IMongoCollection<User> userCollection;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public UserService(IMongoDBContext db, IMapper mapper)
        {
            this.db = db;
            userCollection = db.GetCollection<User>(typeof(User).Name);
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task CreateAsync(UserCreateDto dto)
        {
            var user = mapper.Map<User>(dto);
            await userCollection.InsertOneAsync(user);

            //var user = new User
            //{
            //    UserName = dto.UserName,
            //    PhoneNumber = dto.PhoneNumber,
            //    Email = dto.Email
            //};
            //if (string.IsNullOrWhiteSpace(dto.Password))
            //    await userManager.CreateAsync(user,
            //        new string(user.PhoneNumber.TakeLast(Math.Min(user.PhoneNumber.Length, 6)).ToArray()));
            //else
            //    await userManager.CreateAsync(user, dto.Password);

        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UserViewModel> UpdateAsync(ObjectId id, UserCreateDto dto)
        {
            var user = await userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (user is null)
            {
                //return NotFound();
            }

            user.Id = id;
            user.PhoneNumber = dto.PhoneNumber;
            user.UserName = dto.UserName;
            user.Email = dto.Email;

            await userCollection.ReplaceOneAsync(x => x.Id == id, user);

            return new UserViewModel(user);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="name"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<List<UserViewModel>> FindAsync(string? userName, string? name, string? phoneNumber)
        {
            var fb = Builders<User>.Filter;
            var result = await userCollection.FindAsync(
                fb.Where(p => p.UserName.Contains(userName!)).If(userName) &
                fb.Where(P => P.NormalizedUserName.Contains(name!)).If(name) &
                fb.Where(p => p.PhoneNumber.Contains(phoneNumber!)).If(phoneNumber));

            return result.ToList().Select(t => new UserViewModel(t)).ToList();

            var all = await userCollection.FindAsync(Builders<User>.Filter.Empty);

            return all.ToList().Select(t => new UserViewModel(t)).ToList();
        }

        /// <summary>
        /// 单个查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserViewModel> FindOneAsync(ObjectId id)
        {
            var user = await userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (user == null) return null;

            return new UserViewModel(user);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task DeleteAsync(IEnumerable<ObjectId> ids)
        {
            //var update = Builders<User>.Update.Set(p => p.DeletedAt, true);
            //await userCollection.UpdateManyAsync(p => ids.Contains(p.Id), update);

            var res = await userCollection.DeleteManyAsync(p => ids.Contains(p.Id));

            if (res.DeletedCount == 0) throw new ArgumentException("删除失败");
        }
    }
}
