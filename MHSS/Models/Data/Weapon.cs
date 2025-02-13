using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.Models.Data
{
    public class Weapon : Equip
    {
    //    /// <summary>
    //    /// 装備の種類(頭とか)
    //    /// </summary>
    //    public EquipKind EquipKind { get; set; } = EquipKind.Weapon;

        /// <summary>
        /// 武器の種類
        /// </summary>
        public WeaponKind WeaponKind { get; set; }

        ///// <summary>
        ///// 武器の名前
        ///// </summary>
        //public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 攻撃力(本編に準拠して表示攻撃力にする予定)
        /// </summary>
        public int Attack { get; set; }

        /// <summary>
        /// 会心率(%)
        /// </summary>
        public int Affinity { get; set; }

        /// <summary>
        /// 属性の種類
        /// </summary>
        public Element ElementType { get; set; }

        /// <summary>
        /// 属性値(これも表示値の予定)
        /// </summary>
        public int ElementValue { get; set; }

        ///// <summary>
        ///// スロット1つ目
        ///// </summary>
        //public int Slot1 { get; set; }

        ///// <summary>
        ///// スロット2つ目
        ///// </summary>
        //public int Slot2 { get; set; }

        ///// <summary>
        ///// スロット3つ目
        ///// </summary>
        //public int Slot3 { get; set; }

        ///// <summary>
        ///// スキル
        ///// </summary>
        //public List<Skill> Skill { get; set; } = new();
    }
}
