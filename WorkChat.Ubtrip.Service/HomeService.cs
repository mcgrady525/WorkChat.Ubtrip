using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkChat.Ubtrip.Entity;
using SSharing.Frameworks.Common.Helpers;
using SSharing.Frameworks.Common.Extends;

namespace WorkChat.Ubtrip.Service
{
    public class HomeService
    {
        private readonly string getTokenApiUrl;
        private readonly string corpID;
        private readonly string appSecrect;
        private readonly string appID;
        private readonly string workChatApiHost;
        public HomeService()
        {
            corpID = ConfigHelper.GetAppSetting("CorpID");
            appSecrect = ConfigHelper.GetAppSetting("APP.Ubtrip.Secret");
            appID = ConfigHelper.GetAppSetting("APP.Ubtrip.ID");
            workChatApiHost = ConfigHelper.GetAppSetting("WorkChatApiHost").TrimEnd('/');
            getTokenApiUrl = string.Format("{0}/gettoken?corpid={1}&corpsecret={2}", workChatApiHost, corpID, appSecrect);
        }

        /// <summary>
        /// 获取应用access token
        /// </summary>
        /// <returns></returns>
        public GetAccessTokenResponse GetAccessToken()
        {
            GetAccessTokenResponse result = null;

            //缓存access token
            var cacheAcessTokenKey = string.Format("AccessToken.{0}", appID);
            var cacheValue = CacheHelper.Get(cacheAcessTokenKey);
            if (cacheValue != null)
            {
                result = (GetAccessTokenResponse)cacheValue;
            }
            else
            {
                var rs = HttpHelper.Get(getTokenApiUrl);
                if (!rs.IsNullOrEmpty())
                {
                    result = rs.FromJson<GetAccessTokenResponse>();

                    //存缓存
                    CacheHelper.Set(cacheAcessTokenKey, result, DateTime.Now.AddSeconds(result.ExpiresIn));
                }
            }

            return result;
        }

        /// <summary>
        /// 获取用户身份标识
        /// </summary>
        /// <param name="code"></param>
        public GetUserInfoResponse GetUserInfo(string code)
        {
            GetUserInfoResponse result = null;

            var accessToken = "";
            var getAccessTokenRS = GetAccessToken();
            if (getAccessTokenRS.ErrorCode == 0)
            {
                accessToken = getAccessTokenRS.AccessToken;
            }

            var getUserInfoUrl = string.Format("{0}/user/getuserinfo?access_token={1}&code={2}", workChatApiHost, accessToken, code);

            var rs = HttpHelper.Get(getUserInfoUrl);
            if (!rs.IsNullOrEmpty())
            {
                result = rs.FromJson<GetUserInfoResponse>();
            }

            return result;
        }

        /// <summary>
        /// 获取用户信息详情
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public GetUserDetailResponse GetUserDetail(string userId)
        {
            GetUserDetailResponse result = null;

            var accessToken = "";
            var getAccessTokenRS = GetAccessToken();
            if (getAccessTokenRS.ErrorCode == 0)
            {
                accessToken = getAccessTokenRS.AccessToken;
            }

            var getUserDetailUrl = string.Format("{0}/user/get?access_token={1}&userid={2}", workChatApiHost, accessToken, userId);

            var rs = HttpHelper.Get(getUserDetailUrl);
            if (!rs.IsNullOrEmpty())
            {
                result = rs.FromJson<GetUserDetailResponse>();
            }

            return result;
        }

    }
}
