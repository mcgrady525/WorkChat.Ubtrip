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
    public class GetUserDetailResponse: BaseResponse
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }
        
    }
}
