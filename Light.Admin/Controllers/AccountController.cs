using AutoMapper;
using Light.Admin.Database;
using Light.Admin.Dtos;
using Light.Admin.Mongo;
using Light.Admin.Mongo.Basics;
using Light.Admin.Mongo.IServices;
using Light.Admin.Mongo.Services;
using Light.Admin.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.AspNetCore.Http.Extensions;


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
        public async Task SignIn(LoginViewModel model)
        {

            var user = await accountService.ValidateUser(model.Username, model.Password);

            if (user != null)
            {
                var token = await accountService.GetToken(user);

                // 响应头携带Token
               HttpContext.Response.Headers.Add("x-access-token", token);
            }

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
