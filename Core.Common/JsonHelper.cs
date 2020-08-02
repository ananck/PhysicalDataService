using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Core.Common
{
    public static class JsonHelper
    {
        public static string ToJson(this object obj)
        {
            var converters = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };

            return JsonConvert.SerializeObject(obj, converters);
        }
        /// <summary>
        /// 将字符串转换成Jobject对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static JObject ToJObject(this string str)
        {
            try
            {
                return JObject.Parse(str);
            }
            catch (System.Exception)
            {

                return null;
            }

        }
    }
}