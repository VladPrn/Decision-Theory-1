using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1
{
    class TransportTask
    {
        public int n;
        public int m;
        public int[][] matrix;
        public int[] vectorN;
        public int[] vectorM;

        public TransportTask(int n, int m)
        {
            this.n = n;
            this.m = m;
            matrix = new int[n][];
            for (int i = 0; i < n; i++)
            {
                matrix[i] = new int[m];
            }
            vectorN = new int[n];
            vectorM = new int[m];
        }

        public TransportTask clone()
        {
            TransportTask res = new TransportTask(n, m);
            Buffer.BlockCopy(vectorN, 0, res.vectorN, 0, sizeof(int) * n);
            Buffer.BlockCopy(vectorM, 0, res.vectorM, 0, sizeof(int) * m);
            for (int i = 0; i < n; i++)
            {
                Buffer.BlockCopy(matrix[i], 0, res.matrix[i], 0, sizeof(int) * m);
            }
            return res;
        }

        public bool check()
        {
            for (int i = 0; i < n; i++)
            {
                if (vectorN[i] != 0)
                {
                    return false;
                }
            }

            for (int j = 0; j < m; j++)
            {
                if (vectorM[j] != 0)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
