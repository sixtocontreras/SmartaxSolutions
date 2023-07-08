using Newtonsoft.Json.Linq;
using System.Configuration;
using System.IO;
using System.Net;

namespace Smartax.Web.Application.Clases.Seguridad
{
    public class RecaptchaValidator
    {
        public static bool IsReCaptchaValid(string captchaResponse)
        {
            var result = false;
            var secretKey = FixedData.GoogleRecaptchaSecretKey;
            var apiUrl = FixedData.GoogleRecaptchaApiUrl;
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>(FixedData.GoogleRecaptchaSuccess);
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
        }

    }
}
