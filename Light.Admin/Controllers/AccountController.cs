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
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Light.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        //private readonly IDefaultDbContext db;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly JwtSettings jwtSettings;
        private readonly IAccountService accountService;

        public AccountController(
            IHttpContextAccessor httpContextAccessor,
            IAccountService accountService,
           IOptions<JwtSettings> options
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.accountService = accountService;
            this.jwtSettings = options.Value;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<string> GetTest()
        {
            return "Test Authorize";
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<string> SignIn(LoginViewModel model)
        {
            var result = SignInResult.Failed;

            var user = await accountService.ValidateUser(model.Account, model.Password);

            if (user != null)
            {
                var token = await accountService.GetToken(user);
                return token;
            }
            return result.ToString();
        }
    }
}
