using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.Models.Data
{
    public class Weapon : Equip
    {
        /// <summary>
        /// 武器の種類
        /// </summary>
        public WeaponKind WeaponKind { get; set; }

        /// <summary>
        /// 攻撃力(本編に準拠して表示攻撃力にする予定)
        /// </summary>
        public int Attack { get; set; }

        /// <summary>
        /// 会心率(%)
        /// </summary>
        public int Affinity { get; set; }

        /// <summary>
        /// 属性の種類1
        /// </summary>
        public Element ElementType1 { get; set; }

        /// <summary>
        /// 属性値1(これも表示値の予定)
        /// </summary>
        public int ElementValue1 { get; set; }

        /// <summary>
        /// 属性の種類2(双属性とか)
        /// </summary>
        public Element ElementType2 { get; set; }

        /// <summary>
        /// 属性値1(これも表示値の予定)
        /// </summary>
        public int ElementValue2 { get; set; }
    }
}
