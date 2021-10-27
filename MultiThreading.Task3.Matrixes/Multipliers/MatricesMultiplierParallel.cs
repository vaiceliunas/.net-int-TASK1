using MultiThreading.Task3.MatrixMultiplier.Matrices;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public class MatricesMultiplierParallel : IMatricesMultiplier
    {
        public IMatrix Multiply(IMatrix m1, IMatrix m2)
        {
            // todo: feel free to add your code here
            var resMatrix = new Matrix(m1.RowCount, m2.ColCount);
            Parallel.For(0, m1.RowCount, m1RowNumber =>
            {
                Parallel.For(0, m2.ColCount, m2ColNumber =>
                {
                    long sum = 0;
                    for (byte k = 0; k < m1.ColCount; k++)
                    {
                        sum += m1.GetElement(m1RowNumber, k) * m2.GetElement(k, m2ColNumber);
                    }
                    lock (resMatrix)
                    {
                        resMatrix.SetElement(m1RowNumber, m2ColNumber, sum);
                    }
                });
            });

            return resMatrix;
        }
    }
}
