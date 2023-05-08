using System;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        int n = 800; // размер матрицы
        int[,] A = new int[n, n];
        int[,] B = new int[n, n];
        int[,] C = new int[n, n];

        // заполнение матриц случайными числами
        Random rand = new Random();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                A[i, j] = rand.Next(10);
                B[i, j] = rand.Next(10);
            }
        }

        // умножение матриц с использованием 1 потока
        DateTime start = DateTime.Now;
        MultiplyMatrix(A, B, C);
        TimeSpan time1 = DateTime.Now - start;
        Console.WriteLine("1 поток: " + time1.TotalMilliseconds + " мс");

        // умножение матриц с использованием 2 потоков
        start = DateTime.Now;
        Parallel.Invoke(
            () => MultiplyMatrix(A, B, C, 0, n / 2),
            () => MultiplyMatrix(A, B, C, n / 2, n)
        );
        TimeSpan time2 = DateTime.Now - start;
        Console.WriteLine("2 потока: " + time2.TotalMilliseconds + " мс");

        // умножение матриц с использованием 4 потоков
        start = DateTime.Now;
        Parallel.Invoke(
            () => MultiplyMatrix(A, B, C, 0, n / 4),
            () => MultiplyMatrix(A, B, C, n / 4, n / 2),
            () => MultiplyMatrix(A, B, C, n / 2, n * 3 / 4),
            () => MultiplyMatrix(A, B, C, n * 3 / 4, n)
        );
        TimeSpan time4 = DateTime.Now - start;
        Console.WriteLine("4 потока: " + time4.TotalMilliseconds + " мс");
    }

    static void MultiplyMatrix(int[,] A, int[,] B, int[,] C, int startRow, int endRow)
    {
        int n = A.GetLength(0);
        int m = A.GetLength(1);
        int p = B.GetLength(1);

        for (int i = startRow; i < endRow; i++)
        {
            for (int j = 0; j < p; j++)
            {
                int sum = 0;
                for (int k = 0; k < m; k++)
                {
                    sum += A[i, k] * B[k, j];
                }
                C[i, j] = sum;
            }
        }
    }

    static void MultiplyMatrix(int[,] A, int[,] B, int[,] C)
    {
        int n = A.GetLength(0);
        int m = A.GetLength(1);
        int p = B.GetLength(1);

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < p; j++)
            {
                int sum = 0;
                for (int k = 0; k < m; k++)
                {
                    sum += A[i, k] * B[k, j];
                }
                C[i, j] = sum;
            }
        }
    }
}

