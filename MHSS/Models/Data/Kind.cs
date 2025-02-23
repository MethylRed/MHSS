using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.Models.Data
{
    /// <summary>
    /// 装備の種類
    /// </summary>
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

    /// <summary>
    /// 武器の種類
    /// </summary>
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

    /// <summary>
    /// 属性の種類
    /// </summary>
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
        /// <summary>
        /// スキルのカテゴリ
        /// </summary>
        public static List<string> SkillCategory = new()
        {
            "クエスト", "アイテム", "戦闘(生存)", "特殊攻撃耐性", "パラメータ変化",
            "戦闘(属性・異常)", "戦闘(攻撃)", "グループスキル", "シリーズスキル"
        };

        /// <summary>
        /// 属性種類の辞書
        /// </summary>
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

        /// <summary>
        /// 装備の種類：文字列(日本語)に変換
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 装備の種類：文字列(英語)に変換
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 武器の種類：文字列に変換
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
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
        static Kind() { }
    }
}
