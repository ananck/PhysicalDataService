using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.SaveModel
{
    public class Commodity
    {
        public long id { get; set; }
        public long maufactureId { get; set; }
        public string tradeName { get; set; }
        public string kind { get; set; }
        public string mark { get; set; }
        public string materialIdentity { get; set; }
    }
}
