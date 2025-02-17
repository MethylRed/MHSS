using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.LinearSolver;
using MHSS.Models.Data;

namespace MHSS.Models.Utility
{
    public class Solve
    {
        public Solver Solver { get; set; }
        public Dictionary<string, Variable> Variables { get; set; } = new();
        public Dictionary<string, Constraint> Constraints { get; set; } = new();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="condition">検索の条件</param>
        public Solve(Condition condition)
        {
            var allEquips = Master.Heads
                            .Union(Master.Bodies).Union(Master.Arms).Union(Master.Waists)
                            .Union(Master.Legs).Union(Master.Charms).Union(Master.Decos)
                            .Union(Master.Weapons.SelectMany(w => w));


            // MIPソルバーを宣言
            Solver = Solver.CreateSolver("SCIP");
            if (Solver is null) return;

            #region Define Variables
            foreach (var equip in allEquips)
            {
                // 装飾品の場合は所持数を上限にする
                if (equip.EquipKind == EquipKind.Deco)
                {
                    Variables.Add(equip.Name, Solver.MakeIntVar(0.0, ((Deco)equip).HaveCount, equip.Name));
                }
                else
                {
                    Variables.Add(equip.Name, Solver.MakeBoolVar(equip.Name));
                }
            }
            #endregion


            #region Define Constraint
            // 武器・防具・護石は１つまでしか装備できない
            //string[] EquipNames = { "Weapon", "Head", "Body", "Arm", "Waist", "Leg", "Charm" };
            foreach (var name in new[] { "Weapon", "Head", "Body", "Arm", "Waist", "Leg", "Charm" })
            {
                Constraints.Add(name, Solver.MakeConstraint(0.0, 1.0, name));
            }

            // スキルは指定値以上
            foreach (var skill in condition.Skills)
            {
                Constraints.Add(skill.Name, Solver.MakeConstraint(skill.Level, double.PositiveInfinity, skill.Name));
            }

            // スロットは不足してはいけない
            for (int i = 0; i < 4; i++)
            {
                Constraints.Add("ArmorSlotCount" + (i + 1).ToString(), Solver.MakeConstraint(0.0, double.PositiveInfinity, "ArmorSlotCount" + (i + 1).ToString()));
                Constraints.Add("WeaponSlotCount" + (i + 1).ToString(), Solver.MakeConstraint(0.0, double.PositiveInfinity, "WeaponSlotCount" + (i + 1).ToString()));
            }

            // 防御・耐性は指定値以上
            string[] ResAndDef = { "Def", "Fire", "Water", "Thunder", "Ice", "Dragon" };
            foreach (var item in ResAndDef)
            {
                Constraints.Add(item, Solver.MakeConstraint(double.NegativeInfinity, double.PositiveInfinity, item));
            }


            // 係数の定義
            foreach (var equip in allEquips)
            {
                // 武器・防具・護石の制約
                if (equip.EquipKind != EquipKind.Deco)
                {
                    Constraints[Kind.EquipKindsToEnString(equip.EquipKind)]
                        .SetCoefficient(Variables[equip.Name], 1);
                }

                // スキルの制約
                foreach (var skill in equip.Skills)
                {
                    Constraints[skill.Name].SetCoefficient(Variables[equip.Name], skill.Level);
                }

                // スロットの制約
                var slotCount = SlotCount(equip);
                string key = "WeaponSlotCount";
                if (equip.SlotType == 1)
                {
                    key = "ArmorSlotCount";
                }
                for (int i = 0; i < 4; i++)
                {
                    Constraints[key + (i + 1).ToString()].SetCoefficient(Variables[equip.Name], slotCount[i]);
                }

                // 防御・耐性の制約
                Constraints["Def"].SetCoefficient(Variables[equip.Name], equip.Def);
                Constraints["Fire"].SetCoefficient(Variables[equip.Name], equip.ResFire);
                Constraints["Water"].SetCoefficient(Variables[equip.Name], equip.ResWater);
                Constraints["Thunder"].SetCoefficient(Variables[equip.Name], equip.ResThunder);
                Constraints["Ice"].SetCoefficient(Variables[equip.Name], equip.ResIce);
                Constraints["Dragon"].SetCoefficient(Variables[equip.Name], equip.ResDragon);
            }
            #endregion


            #region Define Objective
            // 防御を最大化
            Objective obj = Solver.Objective();
            foreach (var equip in allEquips)
            {
                obj.SetCoefficient(Variables[equip.Name], equip.Def);
            }
            obj.SetMaximization();
            #endregion
        }

        /// <summary>
        /// 検索の実行
        /// </summary>
        /// <param name="searchCount">検索回数</param>
        /// <returns></returns>
        public List<SearchedEquips> Search(int searchCount)
        {
            List<SearchedEquips> searchedEquips = new();

            // 検索回数が指定値以下・解がある・検索回数が最大回数以下のとき検索を実行し続ける
            for (int count = 0; (count < searchCount) && (Solver.Solve() == Solver.ResultStatus.OPTIMAL) && (searchCount < Config.Config.Instance.MaxSearchCount); count++)
            {
                SearchedEquips searchedEquip = new();
                // 個数変数の値が0超のものを列挙
                foreach (var equip in Variables.Where(v => v.Value.SolutionValue() > 0))
                {
                    // 全装備から一致するものを探す
                    var findEquip = Master.AllEquips.Where(x => x.Name == equip.Key).FirstOrDefault();

                    // 無い場合はスキップ
                    if (findEquip == null) continue;

                    // 装備の種類ごとに代入
                    switch (findEquip.EquipKind)
                    {
                        case EquipKind.Weapon: searchedEquip.Weapon = (Weapon)findEquip; break;
                        case EquipKind.Head: searchedEquip.Head = findEquip; break;
                        case EquipKind.Body: searchedEquip.Body = findEquip; break;
                        case EquipKind.Arm: searchedEquip.Arm = findEquip; break;
                        case EquipKind.Waist: searchedEquip.Waist = findEquip; break;
                        case EquipKind.Leg: searchedEquip.Leg = findEquip; break;
                        case EquipKind.Charm: searchedEquip.Charm = findEquip; break;
                        case EquipKind.Deco: searchedEquip.Decos.Add((Deco)findEquip); break;
                    }
                }
                searchedEquips.Add(searchedEquip);
            }
            return searchedEquips;
        }


        //public static Dictionary<Equip, int> SolutionEquips(Solve solve)
        //{


        //    return solve.Variables
        //            .Where(v => v.Value.SolutionValue() > 0)
        //            .ToDictionary(
        //                v => Master.AllEquips.FirstOrDefault(x => x.Name == v.Key),
        //                v => (int)(v.Value.SolutionValue()));
        //}


        // 装備のスロットの計算
        private static int[] SlotCount(Equip equip)
        {
            var slotCount = new int[] { 0, 0, 0, 0 };
            foreach (var s in new[] { equip.Slot1, equip.Slot2, equip.Slot3 })
            {
                for (int i = 0; i < s; i++)
                {
                    if (equip is Deco)
                    {
                        slotCount[i]--;
                    }
                    else
                    {
                        slotCount[i]++;
                    }
                }
            }
            return slotCount;
        }
    }
}