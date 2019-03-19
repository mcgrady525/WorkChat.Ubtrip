using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkChat.Ubtrip.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class GetUserInfoResponse: BaseResponse
    {
        public string UserId { get; set; }

        public string DeviceId { get; set; }
    }
}
