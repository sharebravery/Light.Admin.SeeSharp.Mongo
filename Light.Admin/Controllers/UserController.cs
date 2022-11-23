using Light.Admin.Dtos;
using Light.Admin.IServices;
using Light.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Light.Admin.Controllers
{
    [ApiController]
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;


        public UserController(
            IUserService userService
            )
        {
            this.userService = userService;
        }


        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Create(UserCreateDto dto)
        {
            await userService.CreateAsync(dto);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task Update(ObjectId id, UserCreateDto dto)
        {
            await userService.UpdateAsync(id, dto);
        }

        ///// <summary>
        ///// 批量删除
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        [HttpPut]
        public async Task Delete(ObjectId[] ids)
        {
            await userService.DeleteAsync(ids);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="name"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> Find(string? userName, string? name, string? phoneNumber)
        {
            return Ok(await userService.FindAsync(userName, name, phoneNumber));
        }

        ///// <summary>
        ///// 单个查找
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        [HttpGet]
        public async Task<UserViewModel> FindOne(ObjectId id)
        {
            return await userService.FindOneAsync(id);
        }


    }
}
