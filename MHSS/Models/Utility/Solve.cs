using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.LinearSolver;
using MHSS.Models.Repository;

namespace MHSS.Models.Utility
{
    public class Solve
    {
        public Solver Solver { get; set; }

        private List<Equip> Head { get; set; }
        private List<Equip> Body { get; set; }
        private List<Equip> Arm { get; set; }
        private List<Equip> Waist { get; set; }
        private List<Equip> Leg { get; set; }
        private List<Equip> Charm { get; set; }
        private List<Equip> Deco { get; set; }

        private const int DecoCount = 0;

        public List<Dictionary<string, Variable>> EquipVariablesList { get; set; } = new();
        private Dictionary<string, Variable> DecoVariables { get; set; } = new();
        public List<Dictionary<string, Variable>> WeaponVariablesList { get; set; } = new();

        public List<Constraint> EquipConstraintList { get; set; } = new();

        public Solve()
        {
            // MIPソルバーを宣言
            Solver = Solver.CreateSolver("SCIP");
            if (Solver is null) return;

            #region Define Variables
            // 防具と護石の個数変数を定義
            foreach (var equipList in new List<List<Equip>> { Master.Head, Master.Body, Master.Arm, Master.Waist, Master.Leg, Master.Charm})
            {
                EquipVariablesList.Add(equipList.ToDictionary(e => e.Name, e => Solver.MakeBoolVar(e.Name)));
            }

            // 装飾品の個数変数を定義
            foreach (var item in Master.Deco)
            {
                DecoVariables.Add(item.Name,Solver.MakeIntVar(0.0, DecoCount, item.Name));
            }
            EquipVariablesList.Add(DecoVariables);

            // 武器の個数変数を定義
            foreach (var weaponList in Master.Weapons)
            {
                WeaponVariablesList.Add(weaponList.ToDictionary(e => e.Name, e => Solver.MakeBoolVar(e.Name)));
            }
            #endregion


            #region Define Constraint

            // 防具・護石は最大1個まで装備可能
            // => 防具・護石それぞれの個数変数の総和は 0 か 1
            EquipConstraintList.Add(Solver.MakeConstraint(0.0, 1.0, "Head"));

            for (int i = 0; i < EquipVariablesList; i++)
            {

            }

            #endregion
        }
    }
}