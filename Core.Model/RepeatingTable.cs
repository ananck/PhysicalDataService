using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Model
{

    /// <summary>
    /// 重复记录
    /// </summary>
    public class RepeatingTable
    {
        [Key]
        [MaxLength(100)]
        public string DetailID { get; set; }
        public string MakerFullName { get; set; }
        public string ItemInfo { get; set; }
        public string Detail { get; set; }
    }
}
