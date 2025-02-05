using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.LinearSolver;
using Models.Repository;

namespace MHSS.Models.Utility
{
    public class Solve
    {
        public Solver Solver { get; set; }

        private List<Equip> Head { get; set; }
        private List<Equip> Body { get; set; }


        public void SolveMain()
        {
            Solver = Solver.CreateSolver("SCIP");
            if (Solver is null) return;

            Head = Master.Head;
            Body = Master.Body;

            var equip = Head.Union(Body);

        }
        
    }
}
