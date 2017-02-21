using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1
{
    class VogelApproximationSolver : Solver
    {
        private TransportTask task;
        private bool[][] used;
        private int[] dopVectorN;
        private int[] dopVectorM;

        public VogelApproximationSolver(TransportTask task)
        {
            this.task = task.clone();
        }

        public Solve solve()
        {
            int cost = 0;
            int[][] transport = new int[task.n][];
            used = new bool[task.n][];
            for (int k = 0; k < task.n; k++)
            {
                transport[k] = new int[task.m];
                used[k] = new bool[task.m];
            }

            while (!task.check())
            {
                int i, j;
                getCell(out i, out j);
                used[i][j] = true;

                int min = Math.Min(task.vectorN[i], task.vectorM[j]);
                transport[i][j] = min;
                cost += task.matrix[i][j] * transport[i][j];
                task.vectorN[i] -= min;
                task.vectorM[j] -= min;
            }

            return new Solve(transport, task.n, task.m, cost); 
        }

        private void getCell(out int y, out int x)
        {
            getVector(out y, out x);
            getMinInVector(ref y, ref x);
        }

        private void getMinInVector(ref int y, ref int x)
        {
            int minCost = Int32.MaxValue;
            if (x == -1)
            {
                for (int j = 0; j < task.m; j++)
                {
                    if (task.vectorM[j] <= 0)
                    {
                        continue;
                    }
                    if (task.matrix[y][j] < minCost || (task.matrix[y][j] == minCost && dopVectorM[j] > dopVectorM[x]))
                    {
                        minCost = task.matrix[y][j];
                        x = j;
                    }
                }
            }
            else
            {
                for (int i = 0; i < task.n; i++)
                {
                    if (task.vectorN[i] <= 0)
                    {
                        continue;
                    }
                    if (task.matrix[i][x] < minCost || (task.matrix[i][x] == minCost && dopVectorN[i] > dopVectorN[y]))
                    {
                        minCost = task.matrix[i][x];
                        y = i;
                    }
                }
            }
        }

        private void getVector(out int y, out int x)
        {
            y = 0;
            x = 0;
            int delt = 0;

            dopVectorN = new int[task.n];
            for (int i = 0; i < task.n; i++)
            {
                int min1 = Int32.MaxValue;
                int min2 = Int32.MaxValue;
                for (int j = 0; j < task.m; j++)
                {
                    if (!used[i][j])
                    {
                        if (task.matrix[i][j] < min1)
                        {
                            min1 = task.matrix[i][j];
                        }
                        else if (task.matrix[i][j] < min2)
                        {
                            min2 = task.matrix[i][j];
                        }
                    }
                }
                dopVectorN[i] = Math.Abs(min2 - min1);
                if (dopVectorN[i] > delt && task.vectorN[i] > 0)
                {
                    delt = dopVectorN[i];
                    y = i;
                    x = -1;
                }
            }

            dopVectorM = new int[task.m];
            for (int j = 0; j < task.m; j++)
            {
                int min1 = Int32.MaxValue;
                int min2 = Int32.MaxValue;
                for (int i = 0; i < task.n; i++)
                {
                    if (used[i][j])
                    {
                        if (task.matrix[i][j] < min1)
                        {
                            min1 = task.matrix[i][j];
                        }
                        else if (task.matrix[i][j] < min2)
                        {
                            min2 = task.matrix[i][j];
                        }
                    }
                }
                dopVectorM[j] = Math.Abs(min2 - min1);
                if (dopVectorM[j] > delt && task.vectorM[j] > 0)
                {
                    delt = dopVectorM[j];
                    y = -1;
                    x = j;
                }
            }
        }
    }
}