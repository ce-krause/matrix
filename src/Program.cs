namespace matrix.src;

public static class Program
{
    public static void Truncate()
    {
        Console.Write("\n---\n\n");
    }

    public static double[,] Matrix_Copy(double[,] a, double[,]? b = null, bool transpose = false)
    {
        if (b != null)
        {
            return new double[a.GetLength(0), b.GetLength(1)];
        }

        return new double[a.GetLength(transpose ? 1 : 0), a.GetLength(transpose ? 0 : 1)];
    }

    public static void Matrix_Print(double[,] matrix, string text, bool truncate = false, int spaces = 0)
    {
        Console.Write($"\n{text} = ");

        string spacing = "";

        for (var i = 0; i < 4 + spaces; i++)
        {
            spacing += " ";
        }

        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            if (i > 0) Console.Write(spacing);

            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write($"{matrix[i, j]} ");
            }

            Console.WriteLine();
        }

        if (!truncate)
        {
            Console.WriteLine();

            return;
        }

        Truncate();
    }

    public static double[,] Matrix_Scalar_Multi(double[,] matrix, double scalar)
    {
        double[,] result = Matrix_Copy(matrix);

        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                result[i, j] = matrix[i, j] * scalar;
            }
        }

        return result;
    }

    static double[,]? Matrix_Add(double[,] a, double[,] b)
    {
        var linesA = a.GetLength(0);
        var linesB = b.GetLength(0);

        if (linesA != linesB)
        {
            Console.WriteLine("a and b should have the same amount of lines");

            return null;
        }

        var colsA = a.GetLength(1);
        var colsB = b.GetLength(1);

        if (colsA != colsB)
        {
            Console.WriteLine("a and b should have the same amount of columns");

            return null;
        }

        double[,] result = Matrix_Copy(a);

        for (var i = 0; i < a.GetLength(0); i++)
        {
            for (var j = 0; j < a.GetLength(1); j++)
            {
                result[i, j] = a[i, j] + b[i, j];
            }
        }

        return result;
    }

    public static double[,]? Matrix_Multi(double[,] a, double[,] b)
    {
        if (a.GetLength(1) != b.GetLength(0))
        {
            Console.WriteLine("matrix a should have the same amount of columns as the lines amount of b");

            return null;
        }

        double[,] result = Matrix_Copy(a, b);

        for (var i = 0; i < a.GetLength(0); i++)
        {
            for (var j = 0; j < b.GetLength(1); j++)
            {
                result[i, j] = 0;

                // INFO: k is the shared index
                for (var k = 0; k < a.GetLength(1); k++)
                {
                    result[i, j] += a[i, k] * b[k, j];
                }
            }
        }

        return result;
    }

    public static double? Matrix_Determinant_2x2(double[,] matrix)
    {
        if (!Matrix_IsSymmetric(matrix, 2))
        {
            Console.WriteLine("for Matrix_Determinant_2x2, matrix dimension should be 2x2");

            return null;
        }

        double a = matrix[0, 0] * matrix[1, 1];
        double b = matrix[0, 1] * matrix[1, 0];

        return a - b;
    }


    static double? Matrix_Determinant_3x3(double[,] matrix)

    {
        if (!Matrix_IsSymmetric(matrix, 3))
        {
            Console.WriteLine("for Matrix_Determinant_3x3, matrix dimension should be 3x3");

            return null;
        }

        int size = matrix.GetLength(0) - 1;

        double a = 1;
        double b = 1;
        double c = 1;
        double d = 1;
        double e = 1;
        double f = 1;

        // INFO: using % 3 to reset the index case out of bounds
        for (var i = 0; i <= size; i++)
        {
            a *= matrix[i, i];
            b *= matrix[i, (i + 1) % 3];
            c *= matrix[i, (i + 2) % 3];

            var reverse = size - i;

            d *= matrix[reverse, i];
            e *= matrix[(reverse + 1) % 3, i];
            f *= matrix[(reverse + 2) % 3, i];
        }

        return a + b + c - (d + e + f);
    }

    static double[,] Matrix_Transpose(double[,] matrix)
    {
        var result = Matrix_Copy(matrix, null, true);

        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                result[j, i] = matrix[i, j];
            }
        }

        return result;
    }

    static bool Matrix_IsSymmetric(double[,] matrix, int? n = null)
    {
        var lines = matrix.GetLength(0);
        var cols = matrix.GetLength(1);

        if (n != null)
        {
            if (lines != n || cols != n) return false;

            return true;
        }

        return lines == cols;
    }

    static bool Matrix_IsDiagonal(double[,] matrix)
    {
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                var value = matrix[i, j];

                if (i != j && value != 0) return false;
                if (i == j && value <= 0) return false;
            }
        }

        return true;
    }

    static bool Matrix_IsIdentity(double[,] matrix)
    {
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                var value = matrix[i, j];

                if (i == j && value != 1) return false;
                if (i != j && value != 0) return false;
            }
        }

        return true;
    }

    public static void Main()
    {
        // INFO: só funcionam com matrizes quadradas
        Console.WriteLine("Running matrix functions\n\n---\n");

        double[,] a = { { 1, 2 }, { 3, 4 } };
        double[,] b = { { 4, 3 }, { 2, 1 } };
        double[,] c = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
        double[,] d = { { 9, 8, 7 }, { 6, 5, 4 }, { 3, 2, 1 } };
        double[,] e = { { 10, 11, 12 }, { 13, 14, 15 } };
        double[,] f = { { 21, 20 }, { 19, 18 }, { 17, 16 } };

        {
            Console.WriteLine("Running Matrix_Print");

            Matrix_Print(a, "A");
            Matrix_Print(b, "B");
            Matrix_Print(c, "C");
            Matrix_Print(d, "D", false);
            Matrix_Print(e, "E", false);
            Matrix_Print(f, "F", true);
        }

        {
            Console.WriteLine("Running Matrix_Scalar_Multi");

            var resultA = Matrix_Scalar_Multi(a, 2);
            var resultB = Matrix_Scalar_Multi(b, 8);
            var resultC = Matrix_Scalar_Multi(b, 3);
            var resultD = Matrix_Scalar_Multi(b, 7);
            var resultE = Matrix_Scalar_Multi(e, 4);
            var resultF = Matrix_Scalar_Multi(b, 6);

            Matrix_Print(resultA, "A * 2", false, 4);
            Matrix_Print(resultB, "B * 8", false, 4);
            Matrix_Print(resultC, "C * 3", false, 4);
            Matrix_Print(resultD, "D * 7", false, 4);
            Matrix_Print(resultE, "D * 4", false, 4);
            Matrix_Print(resultF, "D * 6", true, 4);
        }

        {
            Console.WriteLine("Running Matrix_Add");

            var resultA = Matrix_Add(a, b);
            var resultB = Matrix_Add(c, d);

            if (resultA != null) Matrix_Print(resultA, "A + B", false, 4);
            if (resultB != null) Matrix_Print(resultB, "C + D", true, 4);
        }

        {
            Console.WriteLine("Running Matrix_Multi");

            var resultA = Matrix_Multi(a, b);
            var resultB = Matrix_Multi(c, d);
            var resultC = Matrix_Multi(e, f);

            if (resultA != null) Matrix_Print(resultA, "A * B", false, 4);
            if (resultB != null) Matrix_Print(resultB, "C * D", false, 4);
            if (resultC != null) Matrix_Print(resultC, "E * F", true, 4);
        }

        {
            Console.WriteLine("Running Matrix_Determinant_2x2\n");

            var resultA = Matrix_Determinant_2x2(a);
            var resultB = Matrix_Determinant_2x2(b);

            Console.WriteLine($"det(A): {resultA}");
            Console.WriteLine($"det(B): {resultB}");

            Truncate();
        }

        {
            Console.WriteLine("Running Matrix_Determinant_3x3\n");

            var resultC = Matrix_Determinant_3x3(c);
            var resultD = Matrix_Determinant_3x3(d);

            Console.WriteLine($"det(C): {resultC}");
            Console.WriteLine($"det(D): {resultD}");

            Truncate();
        }

        {
            Console.WriteLine("Running Matrix_Transpose");

            var resultA = Matrix_Transpose(a);
            var resultB = Matrix_Transpose(b);
            var resultC = Matrix_Transpose(c);
            var resultD = Matrix_Transpose(d);
            var resultE = Matrix_Transpose(e);
            var resultF = Matrix_Transpose(f);

            Matrix_Print(resultA, "At", false, 1);
            Matrix_Print(resultB, "Bt", false, 1);
            Matrix_Print(resultC, "Ct", false, 1);
            Matrix_Print(resultD, "Dt", false, 1);
            Matrix_Print(resultE, "Et", false, 1);
            Matrix_Print(resultF, "Ft", true, 1);
        }

        {
            Console.WriteLine("Running Matrix_IsSymmetric");

            double[,] x = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
            double[,] y = { { 0, 0, 0 }, { 0, 0, 0 } };

            var resultX = Matrix_IsSymmetric(x);
            var resultY = Matrix_IsSymmetric(y);

            Matrix_Print(x, "X");

            Console.WriteLine($"X is symmetric: {resultX}");

            Matrix_Print(y, "Y");

            Console.WriteLine($"Y is symmetric: {resultY}");

            Truncate();
        }

        {
            Console.WriteLine("Running Matrix_IsDiagonal");

            double[,] x = { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            double[,] y = { { 1, 0, 0 }, { 0, 0, 1 }, { 0, 1, 0 } };

            var resultX = Matrix_IsDiagonal(x);
            var resultY = Matrix_IsDiagonal(y);

            Matrix_Print(x, "X");

            Console.WriteLine($"X is diagonal: {resultX}");

            Matrix_Print(y, "Y");

            Console.WriteLine($"Y is diagonal: {resultY}");

            Truncate();
        }

        {
            Console.WriteLine("Running Matrix_IsIdentity");

            double[,] x = { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            double[,] y = { { 0, 1, 1 }, { 1, 0, 1 }, { 1, 1, 0 } };

            var resultX = Matrix_IsIdentity(x);
            var resultY = Matrix_IsIdentity(y);

            Matrix_Print(x, "X");

            Console.WriteLine($"X is identity: {resultX}");

            Matrix_Print(y, "Y");

            Console.WriteLine($"Y is identity: {resultY}");
        }
    }
}
