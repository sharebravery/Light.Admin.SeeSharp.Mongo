using AutoMapper;
using Light.Admin.Database;
using Light.Admin.Mongo;
using Light.Admin.Mongo.IServices;
using Light.Admin.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Light.Admin.Controllers

{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly IMongoDbContext db;

        public AccountController(
            IAccountService accountService,
            IMongoDbContext db
            )
        {
            this.accountService = accountService;
            this.db = db;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<string> SignIn(LoginViewModel model)
        {

            var user = await accountService.ValidateUser(model.Username, model.Password);

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            var token = await accountService.GetToken(user);

            // 响应头携带Token
            HttpContext.Response.Headers.Add("x-access-token", token);

            return token;
        }

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <param name="mapper"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<UserViewModel>  Me(
          [FromServices] IMapper mapper
      )
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId").Value;

            var user = await db.GetCollection<User>(nameof(User)).Find(x => x.Id ==new ObjectId(userId)).FirstOrDefaultAsync();

            var result = mapper.Map<UserViewModel>(user);

            return result;
        }

        /// <summary>
        /// 登出
        /// </summary>
        [HttpPost]
        [Authorize]
        public async void SingOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
