using Light.Admin.CSharp.Dtos;
using Light.Admin.CSharp.Models;
using Light.Admin.Models;

namespace Light.Admin.IServices
{
    public interface IUsersService
    {
        /// <summary>
        /// 创建、注册用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public Task<string> Create(UserDto userDto);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public void Update(UserDto userDto);

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <returns></returns>
        Task<List<User>> Find(string username, string name, string phoneNumber);

        /// <summary>
        /// 根据id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> FindOne(string id);
    }
}
