using AutoMapper;
using Light.Admin.Database;
using Light.Admin.Dtos;
using Light.Admin.Models;
using Light.Admin.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Light.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        //private readonly IDefaultDbContext db;
        protected IMongoCollection<User> userCollection;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor accessor;
        private readonly SignInManager<User> signInManager;
        private readonly IHttpContextAccessor httpContextAccessor;


        private readonly UserManager<User> userManager;
        public AccountController(
            IMapper mapper,
            IMongoCollection<User> userCollection,
            UserManager<User> userManager,
            IHttpContextAccessor accessor,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor
            )
        {
            //this.db = db;
            this.userCollection = userCollection;
            this.mapper = mapper;
            this.userManager = userManager;
            this.accessor = accessor;
            this.signInManager = signInManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<SignInResult> SignIn(LoginViewModel model)
        {
            var result = SignInResult.Failed;
            var Ip = accessor.HttpContext!.Connection.RemoteIpAddress?.ToString();

            var user = await signInManager.UserManager.FindByNameAsync(model.Account);
            if (user == null) return result;
            //result = await signInManager.CheckPasswordSignInAsync(user, model.Password, true);
            //if (!result.Succeeded) return result;
            result = await signInManager.PasswordSignInAsync(
                    model.Account, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "登录失败，请重试");

            }
            return result;
        }
    }
}
