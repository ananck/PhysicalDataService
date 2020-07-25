using System.Collections.Generic;

namespace Core.Model
{
    public class Response
    {
        public int Code { get; set; } = 200;

        public string Msg { get; set; } = "操作成功";
        public bool Success { get; set; } = true;
        public dynamic Data { get; set; }

        public Response()
        {
            Code = 200;
            Success = true;
            Msg = "操作成功";
        }
    }

    public class Response<T>
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public List<T> Data { get; set; }

        public Response()
        {
            Code = 200;
            Message = "操作成功";
        }
    }
}