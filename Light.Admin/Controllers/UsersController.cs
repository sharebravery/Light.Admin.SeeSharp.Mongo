using AutoMapper;
using Light.Admin.CSharp.Dtos;
using Light.Admin.CSharp.Models;
using Light.Admin.Database;
using Light.Admin.IServices;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Light.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {

        private readonly IUsersService userService;

        public UsersController(IUsersService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="username"></param>
        /// <param name="name"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Find(string username, string name, string phoneNumber)
        {
            return Ok(await userService.Find(username, name, phoneNumber));
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Create(UserDto dto)
        {
            return await userService.Create(dto);
        }
    }
}
