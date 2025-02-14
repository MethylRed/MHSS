﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.Models.Data
{
    public enum EquipKind
    {
        Weapon,
        Head,
        Body,
        Arm,
        Waist,
        Leg,
        Charm,
        Deco
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
        None,
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



    public static class Kind
    {
        public static List<string> SkillCategory = new()
        {
            "クエスト", "アイテム", "戦闘(生存)", "特殊攻撃耐性", "パラメータ変化",
            "戦闘(属性・異常)", "戦闘(攻撃)", "グループスキル", "シリーズスキル"
        };

        public const string a = "クエスト";


        public static Dictionary<string, int> ElementType = new()
        {
            {"無", 0},
            {"火", 1},
            {"水", 2},
            {"雷", 3},
            {"氷", 4},
            {"龍", 5},
            {"毒", 6},
            {"爆破", 7},
            {"睡眠", 8},
            {"麻痺", 9}
        };

        public static string EquipKindsToJpString(this EquipKind kind)
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

        public static string EquipKindsToEnString(this EquipKind kind)
        {
            return kind switch
            {
                EquipKind.Weapon => "Weapon",
                EquipKind.Head => "Head",
                EquipKind.Body => "Body",
                EquipKind.Arm => "Arm",
                EquipKind.Waist => "Waist",
                EquipKind.Leg => "Leg",
                EquipKind.Charm => "Charm",
                EquipKind.Deco => "Deco",
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

        static Kind()
        {

        }
    }
}
