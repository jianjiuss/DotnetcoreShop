using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WxController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> WeChatCheck(string signature, string timestamp, string nonce, string echostr)
        {
            string token = "******";
            List<String> strs = new List<string>();
            strs.Add(token);
            strs.Add(timestamp);
            strs.Add(nonce);
            strs.Sort();//这里进行字典排序
            string GetStr = "";
            strs.ForEach(a => GetStr += a);//得到排序后的字符串


            GetStr = SHA1_Encrypt(GetStr);
            string log = "token=" + token + "||timestamp=" + timestamp + "||nonce=" + nonce + "signature=" + signature + GetStr;
            Console.WriteLine(log);

            if (GetStr == signature)
            {
                return echostr;
            }
            else
            {
                return "返回失败";
            }
        }
        /// <summary>
        /// 对字符串进行SHA1加密
        /// </summary>
        /// <param name="strIN">需要加密的字符串</param>
        /// <returns>密文</returns>
        public string SHA1_Encrypt(string Source_String)
        {
            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }
    }
}
