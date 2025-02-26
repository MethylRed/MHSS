using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.Models.Data
{
    public class Equip
    {
        /// <summary>
        /// 装備の種類
        /// </summary>
        public EquipKind EquipKind { get; set; }

        /// <summary>
        /// 装備の名前
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 装備のシリーズ名
        /// </summary>
        public string SeriesName { get; set; } = string.Empty;


        /// <summary>
        /// 武器スキル装飾品か防具スキル装飾品か
        /// 0: 武器スキル 1: 防具スキル 2: 区別なし
        /// </summary>
        public int SlotType { get; set; } = 0;

        /// <summary>
        /// スロット1つ目
        /// </summary>
        public int Slot1 { get; set; } = 0;

        /// <summary>
        /// スロット2つ目
        /// </summary>
        public int Slot2 { get; set; } = 0;

        /// <summary>
        /// スロット3つ目
        /// </summary>
        public int Slot3 { get; set; } = 0;

        /// <summary>
        /// 防御力
        /// </summary>
        public int Def { get; set; } = 0;

        /// <summary>
        /// 火耐性
        /// </summary>
        public int ResFire { get; set; } = 0;

        /// <summary>
        /// 水耐性
        /// </summary>
        public int ResWater { get; set; } = 0;

        /// <summary>
        /// 雷耐性
        /// </summary>
        public int ResThunder { get; set; } = 0;

        /// <summary>
        /// 氷耐性
        /// </summary>
        public int ResIce { get; set; } = 0;

        /// <summary>
        /// 龍耐性
        /// </summary>
        public int ResDragon { get; set; } = 0;

        /// <summary>
        /// スキル
        /// </summary>
        public List<Skill> Skills { get; set; } = new();


        public Equip() { }

        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="other"></param>
        public Equip(Equip other)
        {
            EquipKind = other.EquipKind;
            Name = other.Name;
            SlotType = other.SlotType;
            Slot1 = other.Slot1;
            Slot2 = other.Slot2;
            Slot3 = other.Slot3;
            Def = other.Def;
            ResFire = other.ResFire;
            ResWater = other.ResWater;
            ResThunder = other.ResThunder;
            ResIce = other.ResIce;
            ResDragon = other.ResDragon;
            Skills = new List<Skill>();
            foreach (Skill skill in other.Skills)
            {
                Skills.Add(skill);
            }
        }
    }
}
