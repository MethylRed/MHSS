using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
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
            switch (kind)
            {
                case EquipKind.head:
                    return "頭";
                case EquipKind.body:
                    return "胴";
                case EquipKind.arm:
                    return "腕";
                case EquipKind.waist:
                    return "腰";
                case EquipKind.leg:
                    return "足";
                case EquipKind.deco:
                    return "装飾品";
                case EquipKind.charm:
                    return "護石";
                default:
                    return string.Empty;
            }
        }
    }
}
