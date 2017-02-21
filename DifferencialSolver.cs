using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1
{
    class DifferentialsSolver : Solver
    {
        private TransportTask task;
        private TransportTask workedArea;
        private int[][] transport;
        private bool[][] used; 
        private int[] dopVectorN;
        private int[] dopVectorM;
        private int[] saldoVectorN;
        private int[] saldoVectorM;
        private int cost;

        public DifferentialsSolver(TransportTask task)
        {
            this.task = task.clone();
            this.workedArea = this.task.clone();
        }

        public Solve solve()
        {
            while (true) {
                drop();
                setMin();
                if (workedArea.check())
                {
                    break;
                }
                setSaldoN();
                setSaldoM();
                setDopVectorN();
                setDopVectorM();
                int renta = getRenta();
                newInteration(renta);
            }

            return new Solve(transport, task.n, task.m, cost);
        }

        private bool checkLinkToNegative(int y)
        {
            for (int j = 0; j < workedArea.m; j++)
            {
                if (transport[y][j] > 0)
                {
                    for (int i = 0; i < workedArea.n; i++)
                    {
                        if (y != i && transport[i][j] > 0 && dopVectorN[i] == -1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void drop()
        {
            transport = new int[task.n][];
            used = new bool[task.n][];
            for (int k = 0; k < task.n; k++)
            {
                transport[k] = new int[task.m];
                used[k] = new bool[task.m];
            }
            dopVectorN = new int[task.n];
            dopVectorM = new int[task.m];
            saldoVectorN = new int[task.n];
            saldoVectorM = new int[task.m];
        }

        private void newInteration(int renta)
        {
            for (int i = 0; i < workedArea.n; i++)
            {
                for (int j = 0; j < workedArea.m; j++)
                {
                    if (dopVectorN[i] == -1)
                    {
                        workedArea.matrix[i][j] += renta;
                    }
                }
            }
            Buffer.BlockCopy(task.vectorN, 0, workedArea.vectorN, 0, sizeof(int) * workedArea.n);
            Buffer.BlockCopy(task.vectorM, 0, workedArea.vectorM, 0, sizeof(int) * workedArea.m);
        }

        private int getRenta()
        {
            int renta = Int32.MaxValue;
            for (int j = 0; j < workedArea.m; j++)
            {
                if (dopVectorM[j] != 0)
                {
                    renta = Math.Min(renta, dopVectorM[j]);
                }
            }
            return renta;
        }

        private void setDopVectorM()
        {
            for (int j = 0; j < workedArea.m; j++)
            {
                int negativ = -1;
                int min = Int32.MaxValue;
                for (int i = 0; i < workedArea.n; i++)
                {
                    if (dopVectorN[i] == 1)
                    {
                        min = Math.Min(min, workedArea.matrix[i][j]);
                    }
                    else
                    {
                        if (transport[i][j] > 0)
                        {
                            negativ = workedArea.matrix[i][j];
                        }
                    }
                }
                if (negativ != -1)
                {
                    dopVectorM[j] = Math.Abs(negativ - min);
                }
                else
                {
                    dopVectorM[j] = Int32.MaxValue;
                }
            }
        }

        private void setDopVectorN()
        {
            for (int i = 0; i < workedArea.n; i++)
            {
                if (saldoVectorN[i] > 0)
                {
                    dopVectorN[i] = 1;
                }
                else if (saldoVectorN[i] == 0)
                {
                    bool deficit = false;
                    for (int j = 0; j < task.m; j++)
                    {
                        if (saldoVectorM[j] > 0)
                        {
                            deficit = true;
                        }
                    }
                    if (deficit)
                    {
                        dopVectorN[i] = -1;
                    }
                    else
                    {
                        dopVectorN[i] = 0;
                    }
                }
            }

            for (int i = 0; i < workedArea.n; i++)
            {
                if (dopVectorN[i] == 0)
                {
                    if (checkLinkToNegative(i))
                    {
                        dopVectorN[i] = -1;
                    }
                    else
                    {
                        dopVectorN[i] = 1;
                    }
                }
            }
        }

        private void setSaldoN()
        {
            for (int i = 0; i < workedArea.n; i++)
            {
                saldoVectorN[i] = task.vectorN[i];
                for (int j = 0; j < workedArea.m; j++)
                {
                    saldoVectorN[i] -= transport[i][j];
                }
            }
        }

        private void setSaldoM()
        {
            for (int j = 0; j < workedArea.m; j++)
            {
                saldoVectorM[j] = task.vectorM[j];
                for (int i = 0; i < workedArea.n; i++)
                {
                    saldoVectorM[j] -= transport[i][j];
                }
            }
        }

        private void setMin()
        {
            cost = 0;
            int[] minimums = new int[workedArea.m];
            int[] countMinN = new int[workedArea.n];
            int[] countMinM = new int[workedArea.m];
            int count = 0;
            for (int j = 0; j < workedArea.m; j++)
            {
                int min = Int32.MaxValue;
                for (int i = 0; i < workedArea.n; i++)
                {
                    min = Math.Min(min, workedArea.matrix[i][j]);
                }
                minimums[j] = min;
                for (int i = 0; i < workedArea.n; i++)
                {
                    if (min == workedArea.matrix[i][j])
                    {
                        countMinN[i]++;
                        countMinM[j]++;
                        count++;
                    }
                }
            }

            for (int z = 0; z < count; z++)
            {
                int minM, minN, indexM, indexN;
                getMinIndex(countMinM, out minM, out indexM);
                getMinIndex(countMinN, out minN, out indexN);

                if (minM <= minN)
                {
                    for (int i = 0; i < workedArea.n; i++)
                    {
                        if (workedArea.matrix[i][indexM] == minimums[indexM] && !used[i][indexM])
                        {
                            int decr = Math.Min(workedArea.vectorN[i], workedArea.vectorM[indexM]);
                            transport[i][indexM] = decr;
                            used[i][indexM] = true;
                            countMinN[i]--;
                            countMinM[indexM]--;
                            cost += decr * task.matrix[i][indexM];
                            workedArea.vectorN[i] -= decr;
                            workedArea.vectorM[indexM] -= decr;
                            break;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < workedArea.m; j++)
                    {
                        if (workedArea.matrix[indexN][j] == minimums[j] && !used[indexN][j])
                        {
                            int decr = Math.Min(workedArea.vectorN[indexN], workedArea.vectorM[j]);
                            transport[indexN][j] = decr;
                            used[indexN][j] = true;
                            countMinN[indexN]--;
                            countMinM[j]--;
                            cost += decr * task.matrix[indexN][j];
                            workedArea.vectorN[indexN] -= decr;
                            workedArea.vectorM[j] -= decr;
                            break;
                        }
                    }
                }
            }
        }

        private void getMinIndex(int[] arr, out int min, out int index) 
        {
            min = Int32.MaxValue;
            index = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] < min && arr[i] > 0)
                {
                    min = arr[i];
                    index = i;
                }
            }
        }
    }
}