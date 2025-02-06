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
        weapon,
        head,
        body,
        arm,
        waist,
        leg,
        deco,
        charm,
    }

    public enum WeaponKind
    {

    }

    public enum Element
    {
        fire,
        water,
        thunder,
        ice,
        dragon,
        poison,
        blast,
        sleep,
        paralysis
    }

    public static class EquipKinds
    {
        public static string EquipKindsToString(this EquipKind kind)
        {
            return kind switch
            {
                EquipKind.weapon => "武器",
                EquipKind.head => "頭",
                EquipKind.body => "胴",
                EquipKind.arm => "腕",
                EquipKind.waist => "腰",
                EquipKind.leg => "足",
                EquipKind.charm => "護石",
                EquipKind.deco => "装飾品",
                _ => string.Empty,
            };
        }
    }
}
