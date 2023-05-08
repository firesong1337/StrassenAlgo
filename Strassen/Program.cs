using System;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        int n = 100; // размер матрицы
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

        // перемножение матриц с использованием 1 потока
        DateTime start = DateTime.Now;
        C = Strassen(A, B, 0, 0, 0, 0, n);
        TimeSpan time1 = DateTime.Now - start;
        Console.WriteLine("1 поток: " + time1.TotalMilliseconds + " мс");

        // перемножение матриц с использованием 4 потоков
        start = DateTime.Now;
        Parallel.Invoke(
            () => { C = Strassen(A, B, 0, 0, 0, 0, n / 2); },
            () => { C = Strassen(A, B, 0, n / 2, n / 2, 0, n / 2); },
            () => { C = Strassen(A, B, n / 2, 0, 0, n / 2, n / 2); },
            () => { C = Strassen(A, B, n / 2, n / 2, n / 2, n / 2, n / 2); }
        );
        TimeSpan time4 = DateTime.Now - start;
        Console.WriteLine("4 потока: " + time4.TotalMilliseconds + " мс");
    }

    static int[,] Strassen(int[,] A, int[,] B, int rowA, int colA, int rowB, int colB, int size)
    {
        int[,] C = new int[size, size];

        if (size == 1)
        {
            C[0, 0] = A[rowA, colA] * B[rowB, colB];
        }
        else
        {
            int newSize = size / 2;

            // вычисление матриц P1-P7
            int[,] P1 = Strassen(A, B, rowA, colA, rowB, colB + newSize, newSize);
            int[,] P2 = Strassen(A, B, rowA, colA + newSize, rowB + newSize, colB + newSize, newSize);
            int[,] P3 = Strassen(A, B, rowA + newSize, colA, rowB, colB, newSize);
            int[,] P4 = Strassen(A, B, rowA + newSize, colA + newSize, rowB + newSize, colB, newSize);
            int[,] P5 = Strassen(A, B, rowA, colA, rowB, colB, newSize);
            int[,] P6 = Strassen(A, B, rowA + newSize, colA, rowB, colB + newSize, newSize);
            int[,] P7 = Strassen(A, B, rowA, colA + newSize, rowB + newSize, colB, newSize);

            // вычисление матриц C11-C22
            int[,] C11 = Add(P1, P2, newSize);
            int[,] C12 = Add(P3, P4, newSize);
            int[,] C21 = Add(P5, P6, newSize);
            int[,] C22 = Add(Subtract(Add(P1, P4, newSize), Add(P2, P3, newSize), newSize), P7, newSize);

            // объединение матриц C11-C22 в матрицу C
            for (int i = 0; i < newSize; i++)
            {
                for (int j = 0; j < newSize; j++)
                {
                    C[i, j] = C11[i, j];
                    C[i, j + newSize] = C12[i, j];
                    C[i + newSize, j] = C21[i, j];
                    C[i + newSize, j + newSize] = C22[i, j];
                }
            }
        }

        return C;
    }

    static int[,] Add(int[,] A, int[,] B, int size)
    {
        int[,] C = new int[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                C[i, j] = A[i, j] + B[i, j];
            }
        }

        return C;
    }

    static int[,] Subtract(int[,] A, int[,] B, int size)
    {
        int[,] C = new int[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                C[i, j] = A[i, j] - B[i, j];
            }
        }

        return C;
    }
}


