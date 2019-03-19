using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSharing.Frameworks.Common.Helpers;
using SSharing.Frameworks.Common.Extends;
using WorkChat.Ubtrip.Service;
using SSharing.Frameworks.Common.Consts;

namespace WorkChat.Ubtrip.Site.Controllers
{
    public class HomeController : BaseController
    {
        private static readonly HomeService homeService = new HomeService();
        private readonly string corpID;
        private readonly string redirectSiteHost;
        private readonly string workChatOAuthHost;
        private readonly string corpKey;

        public HomeController()
        {
            corpID = ConfigHelper.GetAppSetting("CorpID");
            corpKey = ConfigHelper.GetAppSetting("CorpKey");
            redirectSiteHost = ConfigHelper.GetAppSetting("RedirectSiteHost").TrimEnd('/');
            workChatOAuthHost = ConfigHelper.GetAppSetting("WorkChatOAuthHost").TrimEnd('/');
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var redirectUrl = HttpUtility.UrlEncode(string.Format("{0}/home/login", redirectSiteHost));
            var oauth2Url = string.Format("{0}/authorize?appid={1}&redirect_uri={2}&response_type=code&scope=snsapi_base&state=#wechat_redirect", workChatOAuthHost, corpID, redirectUrl);
            ViewBag.OAuth2Url = oauth2Url;

            LoggerHelper.Info(()=> { return string.Format("网页授权链接地址为：{0}", oauth2Url); });

            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public RedirectResult Login(string code)
        {
            //依据code获取userid
            //依据userid获取员工信息，包括手机和邮箱
            //最终跳转Url

            var goToUrl = string.Empty;
            try
            {
                LoggerHelper.Info(() => { return string.Format("开始登录，code:{0}", code); });

                var userId = string.Empty;
                var email = string.Empty;
                var mobile = string.Empty;
                var corpCode = ConfigHelper.GetAppSetting("CorpCode");
                var ubtripSiteHost = ConfigHelper.GetAppSetting("UbtripSiteHost").TrimEnd('/');

                var getUserInfoRS = homeService.GetUserInfo(code);
                if (getUserInfoRS != null)
                {
                    userId = getUserInfoRS.UserId;
                }

                var getUserDetailRS = homeService.GetUserDetail(userId);
                if (getUserDetailRS != null)
                {
                    email = getUserDetailRS.Email;
                    mobile = getUserDetailRS.Mobile;
                }

                var data = string.Format("Mobile={0}&ExpireTime={1}", mobile, DateTime.Now.AddDays(1).ToString(DateTimeTypeConst.DATETIME)).ToDES(corpKey, corpKey, System.Security.Cryptography.CipherMode.ECB);
                data = data.Replace('+', '*').Replace('/', '-').Replace('=', '_').Replace("\n", string.Empty);
                goToUrl = string.Format("{0}/Application/Ubt/RedirectPage.aspx?CorpCode={1}&Data={2}&RedirectPage=UserDefault", ubtripSiteHost, corpCode, data);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(() => { return string.Format("登录发生异常，message:{0}，detail:{1}", ex.Message, ex.ToString()); });
            }

            return Redirect(goToUrl);
        }
    }
}