using AutoMapper;
using Light.Admin.Database;
using Light.Admin.Dtos;
using Light.Admin.IServices;
using Light.Admin.Mongo;
using Light.Admin.Mongo.Utils;
using Light.Admin.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Runtime.Intrinsics.Arm;

namespace Light.Admin.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper mapper;
        readonly IMongoCollection<User> userCollection;
        private readonly IMongoDbContext db;


        public UserService(IMapper mapper, IMongoDbContext db)
        {
            this.userCollection = db.GetCollection<User>(nameof(User));
            this.mapper = mapper;
            this.db = db;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task CreateAsync(UserCreateDto dto)
        {
            var user = mapper.Map<User>(dto);

            user.PasswordHash = Hash.Sha256(dto.Password);

            await userCollection.InsertOneAsync(user);
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
