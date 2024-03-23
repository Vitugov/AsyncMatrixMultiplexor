using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMatrixMultiplexor
{
    public class Matrix
    {
        public int RowNumber { get; }
        public int ColumnNumber { get; }
        
        private long[,] data;

        public long this[int row, int column]
        {
            get => data[row, column];
            set => data[row, column] = value;
        }

        public Matrix(int rowNumber, int columnNumber)
        {
            RowNumber = rowNumber;
            ColumnNumber = columnNumber;
            data = new long[rowNumber, columnNumber];
        }

        public void FillWithRandom(int maxRandom)
        {
            var random = new Random();
            for (int row = 0; row < RowNumber; row++)
            {
                for (int column = 0; column < ColumnNumber; column++)
                {
                    data[row, column] = random.Next(maxRandom);
                }
            }
            Program.PrintSys("Матрица заполнена рандомными числами.");
        }
    }
}
