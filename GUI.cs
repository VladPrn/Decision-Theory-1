using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP1
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        private void start_Click(object sender, EventArgs e)
        {
            TransportTask task = parse(input.Text);
            MessageBox.Show(generateMessage(task));
        }

        private String generateMessage(TransportTask task)
        {
            StringBuilder str = new StringBuilder("");

            str.AppendLine(generateTask(task));

            Solver sol = new NorthWestCornerSolver(task);
            str.AppendLine(generateSolve(sol.solve(), "Решение методом северо-западного угла:"));

            sol = new MinElementSolver(task);
            str.AppendLine(generateSolve(sol.solve(), "Решение методом минимального элемента:"));

            sol = new VogelApproximationSolver(task);
            str.AppendLine(generateSolve(sol.solve(), "Решение методом аппроксимации Фогеля:"));

            sol = new DifferentialsSolver(task);
            str.AppendLine(generateSolve(sol.solve(), "Решение методом дифференциальных рент"));


            return str.ToString();
        }

        private String generateTask(TransportTask task) {
            StringBuilder str = new StringBuilder("");

            str.AppendLine("Задача:");
            str.Append(formatStr("", 4));
            for (int j = 0; j < task.m; j++)
            {
                str.Append(formatStr(task.vectorM[j].ToString(), 4));
            }
            str.AppendLine();
            for (int i = 0; i < task.n; i++)
            {
                str.Append(formatStr(task.vectorN[i].ToString(), 4));
                for (int j = 0; j < task.m; j++)
                {
                    str.Append(formatStr(task.matrix[i][j].ToString(), 4));
                }
                str.AppendLine();
            }

            return str.ToString();
        }

        private String generateSolve(Solve sv, String handle)
        {
            StringBuilder str = new StringBuilder("");

            str.AppendLine(handle);
            str.AppendLine("Стоимость: " + sv.cost);
            for (int i = 0; i < sv.n; i++)
            {
                for (int j = 0; j < sv.m; j++)
                {
                    str.Append(formatStr(sv.solve[i][j].ToString(), 4));
                }
                str.AppendLine();
            }

            return str.ToString();
        }

        private String formatStr(String str, int count)
        {
            int countSymbols = str.Length;

            for (int i = 0; i < count - countSymbols; i++)
            {
                str += "  ";
            }

            return str;

        }

        private TransportTask parse(String str)
        {
            String[] numbers = str.Split(new char[]{' ', '\n', '\r'});
            int k = 0;
            last(numbers, ref k);
            int n = Int32.Parse(numbers[k]);
            k++;
            last(numbers, ref k);
            int m = Int32.Parse(numbers[k]);
            k++;
            TransportTask task = new TransportTask(n, m);
            for (int j = 0; j < m; j++)
            {
                last(numbers, ref k);
                task.vectorM[j] = Int32.Parse(numbers[k]);
                k++;
            }
            for (int i = 0; i < n; i++)
            {
                task.matrix[i] = new int[m];
                last(numbers, ref k);
                task.vectorN[i] = Int32.Parse(numbers[k]);
                k++;
                for (int j = 0; j < m; j++)
                {
                    last(numbers, ref k);
                    task.matrix[i][j] = Int32.Parse(numbers[k]);
                    k++;
                }
            }
            return task;
        }

        private void last(String[] numbers, ref int k)
        {
            while (numbers[k] == "")
            {
                k++;
            }
        }
    }
}
