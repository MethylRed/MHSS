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
        Weapon,
        Head,
        Body,
        Arm,
        Waist,
        Leg,
        Deco,
        Charm,
    }

    public enum WeaponKind
    {
        GreatSword,
        LondSword,
        SwordAndShield,
        DualBlades,
        Hammer,
        HuntingHorn,
        Lance,
        Gunlance,
        SwitchAxe,
        ChargeBlade,
        InsectGlaive,
        LightBowgun,
        HeavyBowgun,
        Bow
    }

    public enum Element
    {
        Fire,
        Water,
        Thunder,
        Ice,
        Dragon,
        Poison,
        Blast,
        Sleep,
        Paralysis
    }

    public static class EquipKinds
    {
        public static string EquipKindsToString(this EquipKind kind)
        {
            return kind switch
            {
                EquipKind.Weapon => "武器",
                EquipKind.Head => "頭",
                EquipKind.Body => "胴",
                EquipKind.Arm => "腕",
                EquipKind.Waist => "腰",
                EquipKind.Leg => "足",
                EquipKind.Charm => "護石",
                EquipKind.Deco => "装飾品",
                _ => string.Empty
            };
        }


        public static string WeaponKindsToString(this WeaponKind kind)
        {
            return kind switch
            {
                WeaponKind.GreatSword => "大剣",
                WeaponKind.LondSword => "太刀",
                WeaponKind.SwordAndShield => "片手剣",
                WeaponKind.DualBlades => "双剣",
                WeaponKind.Hammer => "ハンマー",
                WeaponKind.HuntingHorn => "狩猟笛",
                WeaponKind.Lance => "ランス",
                WeaponKind.Gunlance => "ガンランス",
                WeaponKind.SwitchAxe => "スラッシュアックス",
                WeaponKind.ChargeBlade => "チャージアックス",
                WeaponKind.InsectGlaive => "操虫棍",
                WeaponKind.LightBowgun => "ライトボウガン",
                WeaponKind.HeavyBowgun => "ヘビィボウガン",
                WeaponKind.Bow => "弓",
                _ => string.Empty
            };
        }
    }
}
