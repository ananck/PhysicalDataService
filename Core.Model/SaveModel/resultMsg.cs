using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.SaveModel
{
   public  class resultMsg
    {
        public int code { get; set; }
        public bool success { get; set; }
        public object data { get; set; }
        public string msg { get; set; }
    }
}
