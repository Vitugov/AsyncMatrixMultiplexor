using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMatrixMultiplexor
{
    public class Equation
    {
        public Matrix A { get; set; }
        public Matrix B { get; set; }
        public Matrix Product { get; set; }
        public bool IsSolvable => A.ColumnNumber == B.RowNumber;

        public Equation(Matrix a, Matrix b)
        {
            A = a;
            B = b;
            Product = new Matrix(A.RowNumber, B.ColumnNumber);
        }

        public async Task Prepare(int MaxRandom)
        {
            if (!IsSolvable)
            {
                Console.WriteLine("Is not solvable");
            }

            var fillAmatrix = Task.Run(() => A.FillWithRandom(MaxRandom));
            var fillBmatrix = Task.Run(() => B.FillWithRandom(MaxRandom));
            await Task.WhenAll(fillAmatrix, fillBmatrix);
        }

        public async Task<bool> SolveAsync(int MaxThreadCount)
        {
            await Task.Run(() =>
            {
                var options = new ParallelOptions { MaxDegreeOfParallelism = MaxThreadCount };
                Parallel.For(0, A.RowNumber, options, row =>
                {
                    for (int column = 0; column < B.ColumnNumber; column++)
                    {
                        GetCellProduct(row, column);
                    }
                });
            });

            Program.PrintSys("Произведение матриц посчитано.");
            return true;
        }

        public void GetCellProduct(int row, int column)
        {
            long sum = 0;
            
            for (int r = 0; r < A.ColumnNumber; r++)
            {
                sum += A[row, r] * B[r, column];
            }
            
            Product[row, column] = sum;
        }
    }
}
