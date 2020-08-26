using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Model
{
    /// <summary>
    /// 位置记录
    /// </summary>
    public class LocationRecords
    {
        [Key]
        public int ID { get; set; }
        public string LastKey { get; set; }
        public string LastIndex { get; set; }
    }
}
