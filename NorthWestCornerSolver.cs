using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1
{
    class NorthWestCornerSolver : Solver
    {
        private TransportTask task;

        public NorthWestCornerSolver(TransportTask task)
        {
            this.task = task.clone();
        }

        public Solve solve()
        {
            int cost = 0;
            int[][] transport = new int[task.n][];
            for (int k = 0; k < task.n; k++)
            {
                transport[k] = new int[task.m];
            }

            int i = 0;
            int j = 0;

            while (i < task.n && j < task.m)
            {
                int min = Math.Min(task.vectorN[i], task.vectorM[j]);
                transport[i][j] = min;
                cost += task.matrix[i][j] * transport[i][j];
                task.vectorN[i] -= min;
                task.vectorM[j] -= min;

                if (task.vectorN[i] == 0 && task.vectorM[j] == 0)
                {
                    i++;
                    j++;
                }
                else if (task.vectorN[i] == 0 && task.vectorM[j] > 0)
                {
                    i++;
                }
                else if (task.vectorN[i] > 0 && task.vectorM[j] == 0)
                {
                    j++;
                }
            }

            return new Solve(transport, task.n, task.m, cost);
        }
    }
}
