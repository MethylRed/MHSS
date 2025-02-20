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
            // MIPソルバーを宣言
            Solver = Solver.CreateSolver("SCIP");
            if (Solver is null) return;

            #region Define Variables
            // 個数変数を定義する。
            // 武器・防具・護石は、同じものを2個以上装備できないのでブール変数。
            // 装飾品は、所持数まで装備できるので、整数変数で上限を所持数にする。

            // 基本的に、武器/防具スキルを持つ装飾品は武器/防具の装飾品スロットにしか装備できない。
            // しかし、その垣根を超えた装備が存在する可能性がある。
            // 武器スキルを装備可能な防具や、武器スキル・防具スキルの区別がない装飾品など。
            // このような装備/装飾品を「両対応装備/装飾品」と呼ぶことにする。
            // ただし、各装備のスロット毎にこれを考えるのは非常にめんどくさいのでやめておく。
            // 「両対応装備/装飾品の個数変数」は、
            // 「武器スキル装飾品を受け入れるもの/武器に装着するもの」と
            // 「防具スキル装飾品を受け入れる/防具に装着するもの」の2種類用意し、
            // 装備はブール変数、装飾品は上限が所持数の整数変数とする。
            
            foreach (var equip in Master.AllEquips)
            {
                // スロットタイプが 2 -> 武器・防具スキル装飾品スロットの区別なし
                if (equip.SlotType == 2)
                {
                    // 装飾品の場合は上限を所持数にする
                    if (equip.EquipKind == EquipKind.Deco)
                    {
                        Variables.Add($"{equip.Name}_Weapon", Solver.MakeIntVar(0.0, ((Deco)equip).HaveCount, $"{equip.Name}_Weapon"));
                        Variables.Add($"{equip.Name}_Armor", Solver.MakeIntVar(0.0, ((Deco)equip).HaveCount, $"{equip.Name}_Armor"));
                    }
                    else
                    {
                        Variables.Add($"{equip.Name}_Weapon", Solver.MakeBoolVar($"{equip.Name}_Weapon"));
                        Variables.Add($"{equip.Name}_Armor", Solver.MakeBoolVar($"{equip.Name}_Armor"));
                    }
                }
                else
                {
                    if (equip.EquipKind == EquipKind.Deco)
                    {
                        Variables.Add(equip.Name, Solver.MakeIntVar(0.0, ((Deco)equip).HaveCount, equip.Name));
                    }
                    else
                    {
                        Variables.Add(equip.Name, Solver.MakeBoolVar(equip.Name));
                    }
                }
            }
            #endregion


            #region Define Constraint
            // 武器・防具・護石は１つまでしか装備できない
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
                Constraints.Add("WeaponSlotCount" + i.ToString(), Solver.MakeConstraint(0.0, double.PositiveInfinity, "WeaponSlotCount" + i.ToString()));
                Constraints.Add("ArmorSlotCount" + i.ToString(), Solver.MakeConstraint(0.0, double.PositiveInfinity, "ArmorSlotCount" + i.ToString()));
            }
            
            // 防御・耐性は指定値以上
            Constraints.Add("Def", Solver.MakeConstraint(condition.Def, double.PositiveInfinity, "Def"));
            Constraints.Add("ResFire", Solver.MakeConstraint(condition.ResFire, double.PositiveInfinity, "ResFire"));
            Constraints.Add("ResWater", Solver.MakeConstraint(condition.ResWater, double.PositiveInfinity, "ResWater"));
            Constraints.Add("ResThunder", Solver.MakeConstraint(condition.ResThunder, double.PositiveInfinity, "ResThunder"));
            Constraints.Add("ResIce", Solver.MakeConstraint(condition.ResIce, double.PositiveInfinity, "ResIce"));
            Constraints.Add("ResDragon", Solver.MakeConstraint(condition.ResDragon, double.PositiveInfinity, "ResDragon"));


            // 両対応装備の個数制約
            // それぞれの「両対応装備/装飾品(武器)」と「両対応装備/装飾品(防具)」の個数変数の和の上限を「1/所持数」に定める。
            foreach (var equip in Master.AllEquips.Where(d => d.SlotType == 2))
            {
                if (equip.EquipKind == EquipKind.Deco)
                {
                    Constraints.Add(equip.Name, Solver.MakeConstraint(0.0, ((Deco)equip).HaveCount, equip.Name));
                }
                else
                {
                    Constraints.Add(equip.Name, Solver.MakeConstraint(0.0, 1.0, equip.Name));
                }
            }



            // 係数の定義
            foreach (var equip in Master.AllEquips)
            {
                if (equip.SlotType == 2)
                {
                    // 武器・防具・護石の制約
                    if (equip.EquipKind != EquipKind.Deco)
                    {
                        Constraints[Kind.EquipKindsToEnString(equip.EquipKind)].SetCoefficient(Variables[$"{equip.Name}_Weapon"], 1);
                        Constraints[Kind.EquipKindsToEnString(equip.EquipKind)].SetCoefficient(Variables[$"{equip.Name}_Armor"], 1);
                    }

                    // スキルの制約
                    foreach (var skill in equip.Skills)
                    {
                        Constraints[skill.Name].SetCoefficient(Variables[$"{equip.Name}_Weapon"], skill.Level);
                        Constraints[skill.Name].SetCoefficient(Variables[$"{equip.Name}_Armor"], skill.Level);
                    }

                    // スロットの制約
                    var slotCount = SlotCount(equip);
                    for (int i = 0; i < 4; i++)
                    {
                        Constraints["WeaponSlotCount" + (i + 1).ToString()].SetCoefficient(Variables[$"{equip.Name}_Weapon"], slotCount[i]);
                        Constraints["ArmorSlotCount" + (i + 1).ToString()].SetCoefficient(Variables[$"{equip.Name}_Armor"], slotCount[i]);
                    }

                    // 防御・耐性の制約
                    Constraints["Def"].SetCoefficient(Variables[$"{equip.Name}_Weapon"], equip.Def);
                    Constraints["Def"].SetCoefficient(Variables[$"{equip.Name}_Armor"], equip.Def);
                    Constraints["ResFire"].SetCoefficient(Variables[$"{equip.Name}_Weapon"], equip.ResFire);
                    Constraints["ResFire"].SetCoefficient(Variables[$"{equip.Name}_Armor"], equip.ResFire);
                    Constraints["ResWater"].SetCoefficient(Variables[$"{equip.Name}_Weapon"], equip.ResWater);
                    Constraints["ResWater"].SetCoefficient(Variables[$"{equip.Name}_Armor"], equip.ResWater);
                    Constraints["ResThunder"].SetCoefficient(Variables[$"{equip.Name}_Weapon"], equip.ResThunder);
                    Constraints["ResThunder"].SetCoefficient(Variables[$"{equip.Name}_Armor"], equip.ResThunder);
                    Constraints["ResIce"].SetCoefficient(Variables[$"{equip.Name}_Weapon"], equip.ResIce);
                    Constraints["ResIce"].SetCoefficient(Variables[$"{equip.Name}_Armor"], equip.ResIce);
                    Constraints["ResDragon"].SetCoefficient(Variables[$"{equip.Name}_Weapon"], equip.ResDragon);
                    Constraints["ResDragon"].SetCoefficient(Variables[$"{equip.Name}_Armor"], equip.ResDragon);

                    // 両対応装備の個数制約
                    Constraints[equip.Name].SetCoefficient(Variables[$"{equip.Name}_Weapon"], 1.0);
                    Constraints[equip.Name].SetCoefficient(Variables[$"{equip.Name}_Armor"], 1.0);

                }
                else
                {
                    // 武器・防具・護石の制約
                    if (equip.EquipKind != EquipKind.Deco)
                    {
                        Constraints[Kind.EquipKindsToEnString(equip.EquipKind)].SetCoefficient(Variables[equip.Name], 1);
                    }

                    // スキルの制約
                    foreach (var skill in equip.Skills)
                    {
                        Constraints[skill.Name].SetCoefficient(Variables[equip.Name], skill.Level);
                    }

                    // スロットの制約
                    var slotCount = SlotCount(equip);
                    if (equip.SlotType == 0)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Constraints["WeaponSlotCount" + (i + 1).ToString()].SetCoefficient(Variables[equip.Name], slotCount[i]);
                        }
                    }
                    else if (equip.SlotType == 1)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Constraints["ArmorSlotCount" + (i + 1).ToString()].SetCoefficient(Variables[equip.Name], slotCount[i]);
                        }
                    }

                    // 防御・耐性の制約
                    Constraints["Def"].SetCoefficient(Variables[equip.Name], equip.Def);
                    Constraints["ResFire"].SetCoefficient(Variables[equip.Name], equip.ResFire);
                    Constraints["ResWater"].SetCoefficient(Variables[equip.Name], equip.ResWater);
                    Constraints["ResThunder"].SetCoefficient(Variables[equip.Name], equip.ResThunder);
                    Constraints["ResIce"].SetCoefficient(Variables[equip.Name], equip.ResIce);
                    Constraints["ResDragon"].SetCoefficient(Variables[equip.Name], equip.ResDragon);
                }
            }
            #endregion

            #region Define Objective
            // 防御を最大化
            Objective obj = Solver.Objective();
            foreach (var equip in Master.AllEquips)
            {
                if (equip.SlotType == 2)
                {
                    obj.SetCoefficient(Variables[$"{equip.Name}_Weapon"], equip.Def);
                    obj.SetCoefficient(Variables[$"{equip.Name}_Armor"], equip.Def);
                }
                else
                {
                    obj.SetCoefficient(Variables[equip.Name], equip.Def);
                }
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
                    var findEquip = Master.AllEquips
                        .Single(x => (variable.Key == x.Name) ||
                        (variable.Key == $"{x.Name}_Weapon") || (variable.Key == $"{x.Name}_Armor"));

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