using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.Models.Data
{
    public class Deco : Equip
    {
        /// <summary>
        /// 装飾品の所持数
        /// </summary>
        public int HaveCount { get; set; } = 0;
    }
}
