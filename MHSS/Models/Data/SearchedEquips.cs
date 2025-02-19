using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.LinearSolver;

namespace MHSS.Models.Data
{
    public class SearchedEquips
    {
        /// <summary>
        /// 検索結果ステータス
        /// </summary>
        public Solver.ResultStatus ResultStatus { get; set; }

        /// <summary>
        /// 検索結果：武器
        /// </summary>
        public Weapon Weapon { get; set; } = new();

        /// <summary>
        /// 検索結果：頭
        /// </summary>
        public Equip Head { get; set; } = new();

        /// <summary>
        /// 検索結果：胴
        /// </summary>
        public Equip Body { get; set; } = new();

        /// <summary>
        /// 検索結果：腕
        /// </summary>
        public Equip Arm { get; set; } = new();

        /// <summary>
        /// 検索結果：腰
        /// </summary>
        public Equip Waist { get; set; } = new();

        /// <summary>
        /// 検索結果：脚
        /// </summary>
        public Equip Leg { get; set; } = new();

        /// <summary>
        /// 検索結果：護石
        /// </summary>
        public Equip Charm { get; set; } = new();

        /// <summary>
        /// 検索結果：装飾品
        /// </summary>
        public List<Deco> Decos { get; set; } = new();


        public List<int> Slots => new[]
        {
            Weapon.Slot1, Weapon.Slot2, Weapon.Slot3,
            Head.Slot1, Head.Slot2, Head.Slot3,
            Body.Slot1, Body.Slot2, Body.Slot3,
            Arm.Slot1, Arm.Slot2, Arm.Slot3,
            Waist.Slot1, Waist.Slot2, Waist.Slot3,
            Leg.Slot1, Leg.Slot2, Leg.Slot3,
            Charm.Slot1, Charm.Slot2, Charm.Slot3
        }.ToList();


        /// <summary>
        /// 検索結果：全装備
        /// </summary>
        public List<Equip> Equips => new[] { Weapon, Head, Body, Arm, Waist, Leg, Charm }.Concat(Decos).ToList();


        /// <summary>
        /// 検索結果：防御力
        /// </summary>
        public int Def => Equips.Sum(equip => equip.Def);

        /// <summary>
        /// 検索結果：火耐性
        /// </summary>
        public int ResFire => Equips.Sum(equip => equip.ResFire);

        /// <summary>
        /// 検索結果：水耐性
        /// </summary>
        public int ResWater => Equips.Sum(equip => equip.ResWater);

        /// <summary>
        /// 検索結果：雷耐性
        /// </summary>
        public int ResThunder => Equips.Sum(equip => equip.ResThunder);


        /// <summary>
        /// 検索結果：氷耐性
        /// </summary>
        public int ResIce => Equips.Sum(equip => equip.ResIce);

        /// <summary>
        /// 検索結果：龍耐性
        /// </summary>
        public int ResDragon => Equips.Sum(equip => equip.ResDragon);


        /// <summary>
        /// 検索結果：発動スキル
        /// </summary>
        public List<Skill> Skills => Equips
                                    .SelectMany(e => e.Skills)
                                    .GroupBy(s => s.Name)
                                    .Select(g => new Skill
                                    {
                                        Name = g.Key,
                                        Level = g.Sum(s => s.Level)
                                    })
                                    .OrderByDescending(s => s.Level)
                                    .ToList();
    }
}
