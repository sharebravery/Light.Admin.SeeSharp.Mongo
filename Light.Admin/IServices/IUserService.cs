using Light.Admin.Dtos;
using Light.Admin.Models;
using Light.Admin.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Light.Admin.IServices
{
    public interface IUserService
    {
        /// <summary>
        /// 创建、注册用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task CreateAsync(UserCreateDto dto);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<UserViewModel> UpdateAsync(ObjectId id, UserCreateDto dto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task DeleteAsync(IEnumerable<ObjectId> ids);

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="name"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<List<UserViewModel>> FindAsync(string? userName, string? name, string? phoneNumber);

        /// <summary>
        /// 根据id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserViewModel> FindOneAsync(ObjectId id);
    }
}
