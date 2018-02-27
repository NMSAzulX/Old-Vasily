using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vasily;

namespace VasilyWebDemo.Entity
{
    [Table("ad_type")]
    public class AdType : IVasily
    {
        [PrimaryKey]
        public int tid { get; set; }

        [Required(ErrorMessage = "名字不能为空！")]
        public string name { get; set; }
        public string description { get; set; }
    }
}
