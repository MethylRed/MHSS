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
        public Dictionary<string, Constraint> ExtraConstraints { get; set; } = new();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="condition">検索の条件</param>
        public Solve(Condition condition)
        {
            //var allEquips = Master.Heads
            //                .Union(Master.Bodies).Union(Master.Arms).Union(Master.Waists)
            //                .Union(Master.Legs).Union(Master.Charms).Union(Master.Decos)
            //                .Union(Master.Weapons.SelectMany(w => w));


            // MIPソルバーを宣言
            Solver = Solver.CreateSolver("SCIP");
            if (Solver is null) return;

            #region Define Variables
            foreach (var equip in Master.AllEquips)
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

            // スキルの制約
            foreach (var skill in condition.Skills)
            {
                // 固定する場合、上限=下限
                if (skill.IsFixed)
                {
                    Constraints.Add(skill.Name, Solver.MakeConstraint(skill.Level, skill.Level, skill.Name));
                }
                // 固定しない場合、指定レベル以上
                else
                {
                    Constraints.Add(skill.Name, Solver.MakeConstraint(skill.Level, double.PositiveInfinity, skill.Name));
                }
            }

            // スロットは不足してはいけない
            for (int i = 1; i <= 4; i++)
            {
                Constraints.Add("ArmorSlotCount" + i.ToString(), Solver.MakeConstraint(0.0, double.PositiveInfinity, "ArmorSlotCount" + i.ToString()));
                Constraints.Add("WeaponSlotCount" + i.ToString(), Solver.MakeConstraint(0.0, double.PositiveInfinity, "WeaponSlotCount" + i.ToString()));
            }

            // 防御・耐性は指定値以上
            Constraints.Add("Def", Solver.MakeConstraint(condition.Def, double.PositiveInfinity, "Def"));
            Constraints.Add("ResFire", Solver.MakeConstraint(condition.ResFire, double.PositiveInfinity, "ResFire"));
            Constraints.Add("ResWater", Solver.MakeConstraint(condition.ResWater, double.PositiveInfinity, "ResWater"));
            Constraints.Add("ResThunder", Solver.MakeConstraint(condition.ResThunder, double.PositiveInfinity, "ResThunder"));
            Constraints.Add("ResIce", Solver.MakeConstraint(condition.ResIce, double.PositiveInfinity, "ResIce"));
            Constraints.Add("ResDragon", Solver.MakeConstraint(condition.ResDragon, double.PositiveInfinity, "ResDragon"));


            // 係数の定義
            foreach (var equip in Master.AllEquips)
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
                Constraints["ResFire"].SetCoefficient(Variables[equip.Name], equip.ResFire);
                Constraints["ResWater"].SetCoefficient(Variables[equip.Name], equip.ResWater);
                Constraints["ResThunder"].SetCoefficient(Variables[equip.Name], equip.ResThunder);
                Constraints["ResIce"].SetCoefficient(Variables[equip.Name], equip.ResIce);
                Constraints["ResDragon"].SetCoefficient(Variables[equip.Name], equip.ResDragon);
            }
            #endregion


            #region Define Objective
            // 防御を最大化
            Objective obj = Solver.Objective();
            foreach (var equip in Master.AllEquips)
            {
                obj.SetCoefficient(Variables[equip.Name], equip.Def);
            }
            obj.SetMaximization();
            #endregion
        }


        /// <summary>
        /// 1回検索する
        /// </summary>
        /// <param name="count">検索済み回数</param>
        /// <returns></returns>
        public SearchedEquips SearchSingle(int count)
        {
            // 検索結果のインスタンスを生成
            SearchedEquips searchedEquip = new();

            // 検索された、装飾品以外の個数変数
            List<Variable> searchedVariables = new();

            // 検索を実行
            if (Solver.Solve() == Solver.ResultStatus.OPTIMAL)
            {
                // 個数変数の値が0超のものを列挙
                foreach (var variable in Variables.Where(v => v.Value.SolutionValue() > 0))
                {
                    // 全装備から一致するものを探す
                    var findEquip = Master.AllEquips.Single(x => x.Name == variable.Key);

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

                        // 装飾品はダブりで数を数える
                        case EquipKind.Deco:
                            for (int i = 0; i < variable.Value.SolutionValue(); i++)
                            {
                                searchedEquip.Decos.Add((Deco)findEquip);
                            }
                            break;
                    }

                    // 複数検索条件のための個数変数をコレクション
                    if (findEquip.EquipKind != EquipKind.Deco)
                    {
                        searchedVariables.Add(variable.Value);
                    }
                }

                //// 複数検索の場合、武器・防具・護石は同じ組み合わせであってはならない
                ExtraConstraints.Add("Extra" + count.ToString(), Solver.MakeConstraint(0.0, searchedVariables.Count - 1.0, "Extra" + count.ToString()));
                foreach (var searchedVariable in searchedVariables)
                {
                    ExtraConstraints["Extra" + count.ToString()].SetCoefficient(searchedVariable, 1.0);
                }
            }
            else
            {
                searchedEquip = null;
            }
            return searchedEquip;
        }


        // 装備のスロットの計算
        private static int[] SlotCount(Equip equip)
        {
            // Lv.n以上のスロット数の配列
            var slotCount = new int[] { 0, 0, 0, 0 };
            foreach (var s in new[] { equip.Slot1, equip.Slot2, equip.Slot3 })
            {
                // Lvn以上スロット数 = Lvn~4のスロット数の和
                for (int i = 0; i < s; i++)
                {
                    // 装飾品の場合は負
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