using System.Collections.Generic;

namespace Core.Model
{
    public class Response
    {
        public int code { get; set; } = 200;

        public string msg { get; set; } = "操作成功";
        public bool success { get; set; } = true;
        public dynamic data { get; set; }

        public Response()
        {
            code = 200;
            success = true;
            msg = "操作成功";
        }
    }

    public class Response<T>
    {
        public int code { get; set; }

        public string msg { get; set; }
        public bool success { get; set; }

        public List<T> data { get; set; }

        public Response()
        {
            code = 200;
            msg = "操作成功";
        }
    }
}