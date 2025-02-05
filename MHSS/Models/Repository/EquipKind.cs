using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.Models.Repository
{
    public enum EquipKind
    {
        head,
        body,
        arm,
        waist,
        leg,
        deco,
        charm,
    }

    public static class EquipKinds
    {
        public static string EquipKindsToString(this EquipKind kind)
        {
            return kind switch
            {
                EquipKind.head => "頭",
                EquipKind.body => "胴",
                EquipKind.arm => "腕",
                EquipKind.waist => "腰",
                EquipKind.leg => "足",
                EquipKind.deco => "装飾品",
                EquipKind.charm => "護石",
                _ => string.Empty,
            };
        }
    }
}
