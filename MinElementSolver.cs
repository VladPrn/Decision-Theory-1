using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1
{
    class MinElementSolver : Solver
    {
        private TransportTask task;
        private bool[][] used;

        public MinElementSolver(TransportTask task)
        {
            this.task = task.clone();
        }

        public Solve solve() {
            int cost = 0;
            int[][] transport = new int[task.n][];
            used = new bool[task.n][];
            for (int k = 0; k < task.n; k++)
            {
                transport[k] = new int[task.m];
                used[k] = new bool[task.m];
            }

            while (!task.check()) {
                int i, j;
                getMin(out i, out j);
                used[i][j] = true;

                int min = Math.Min(task.vectorN[i], task.vectorM[j]);
                transport[i][j] = min;
                cost += task.matrix[i][j] * transport[i][j];
                task.vectorN[i] -= min;
                task.vectorM[j] -= min;
            }

            return new Solve(transport, task.n, task.m, cost); 
        }

        private void getMin(out int y, out int x)
        {
            x = 0;
            y = 0;
            int min = Int32.MaxValue;
            for (int i = 0; i < task.n; i++)
            {
                for (int j = 0; j < task.m; j++)
                {
                    if (!used[i][j] && task.matrix[i][j] < min)
                    {
                        min = task.matrix[i][j];
                        y = i;
                        x = j;
                    }
                }
            }
        }
    }
}
