using Furion.DynamicApiController;
using Furion.RemoteRequest.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Light.Admin.Mongo.Spider.Controllers
{
    public class CrawController : IDynamicApiController
    {
        private readonly string BASE_URL = "https://www.gkzenti.cn/api/json";

        public async Task<IActionResult> GetCrawAsync()
        {
            var response = await BASE_URL.SetQueries(new { cls = "行测", province = "浙江" }).GetAsStringAsync();

            return new ContentResult()
            {
                Content = response,
                ContentType = "application/json",
                StatusCode = 200
            };
        }
    }
}