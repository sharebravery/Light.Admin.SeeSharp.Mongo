using AutoMapper;
using Light.Admin.Database;
using Light.Admin.Dtos;
using Light.Admin.IServices;
using Light.Admin.Models;
using Light.Admin.Services;
using Light.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Light.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor accessor;


        public UserController(
            IUserService userService,
            UserManager<User> userManager,
            IHttpContextAccessor accessor
            )
        {
            this.userService = userService;
            this.userManager = userManager;
            this.accessor = accessor;
        }


        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IdentityResult> Create(UserCreateDto dto)
        {
            var user = new User
            {
                UserName = dto.UserName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                //IP = ip
            };

            ModelState.ToString();

            IdentityResult? result;

            if (string.IsNullOrWhiteSpace(dto.Password))
                result = await userManager.CreateAsync(user,
                    new string(user.PhoneNumber.TakeLast(Math.Min(user.PhoneNumber.Length, 6)).ToArray()));
            else
                result = await userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded == false)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }


            return result;
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
