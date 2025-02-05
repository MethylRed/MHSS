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


        public Dictionary<string, Variable> EquipVariables { get; set; }
        public Dictionary<string, Variable> DecoVariables { get; set; }
        private Dictionary<string, Variable> WeaponVariables { get; set; }

        public Solve()
        {
            // MIPソルバーを宣言
            Solver = Solver.CreateSolver("SCIP");
            if (Solver is null) return;

            EquipVariables = new();
            DecoVariables = new();

            #region Define Variables
            // 防具と護石のデータを読み込んで結合
            Head = Master.Head;
            Body = Master.Body;
            Arm = Master.Arm;
            Waist = Master.Waist;
            Leg = Master.Leg;
            Charm = Master.Charm;
            var x = Head.Union(Body).Union(Arm).Union(Waist).Union(Leg).Union(Charm);

            // 防具・護石の個数変数を定義
            foreach (var item in x)
            {
                EquipVariables.Add(item.Name, Solver.MakeBoolVar(item.Name));
            }

            // 装飾品の個数変数を定義
            Deco = Master.Deco;
            foreach (var item in Deco)
            {
                DecoVariables.Add(item.Name,Solver.MakeIntVar(0.0, DecoCount, item.Name));
            }
            #endregion

        }
    }
}