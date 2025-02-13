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


        public Solve()
        {
            var allEquips = Master.Head.Union(Master.Body).Union(Master.Arm).Union(Master.Waist)
                            .Union(Master.Leg).Union(Master.Charm).Union(Master.Deco)
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
            // 制約式の定義
            // 武器・防具・護石は１つまでしか装備できない
            string[] EquipNames = { "Weapon", "Head", "Body", "Arm", "Waist", "Leg", "Charm" };
            foreach (var name in EquipNames)
            {
                Constraints.Add(name, Solver.MakeConstraint(0.0, 1.0, name));
            }

            // スキルは指定値以上
            foreach (var skill in Master.Skills)
            {
                Constraints.Add(skill.Name, Solver.MakeConstraint(0.0, double.PositiveInfinity, skill.Name));
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
                foreach (var skill in equip.Skill)
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