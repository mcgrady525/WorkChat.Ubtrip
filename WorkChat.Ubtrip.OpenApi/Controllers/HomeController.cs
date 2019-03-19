using SSharing.Frameworks.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WorkChat.Ubtrip.Entity;
using WorkChat.Ubtrip.Service;

namespace WorkChat.Ubtrip.OpenApi.Controllers
{
    /// <summary>
    /// Home
    /// </summary>
    [RoutePrefix("api/home")]
    public class HomeController : BaseController
    {
        private static readonly HomeService homeService = new HomeService();

        public HomeController()
        {

        }

        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <returns></returns>
        [Route("gettoken")]
        [HttpGet]
        public RestApiResult<GetAccessTokenResponse> GetAccessToken()
        {
            var rs = homeService.GetAccessToken();
            return new RestApiResult<GetAccessTokenResponse>(0, "ok")
            {
                Data = rs
            };
        }

    }
}
