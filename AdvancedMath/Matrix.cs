// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace AdvancedMath
{
    public enum CR
    {
        Column,
        Row
    }
    public class Matrix
    {
        public Matrix()
        {
            M = 1;
            N = 1;
            Data = new Complex[1, 1] { { 1 } };
        }
        public Matrix(Complex[,] data)
        {
            M = data.GetLength(0);
            N = data.GetLength(1);
            Data = data;
        }
        public Matrix(int m, int n)
        {
            M = m;
            N = n;
            Data = new Complex[m, n];
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    this[i, j] = 0;
                }
            }
        }

        public int M { get; }
        public int N { get; }

        private Complex[,] Data { get; set; }
        
        public Complex this[int i, int j]
        {
            get
            {
                return Data[i, j];
            }
            set
            {
                Data[i, j] = value;
            }
        }
        
        public Complex Determinant
        {
            get
            {
                if (IsSquare)
                {
                    Complex result = 1;
                    ToUpperTriangular();
                    for (int i = 0; i < M; ++i)
                    {
                        result *= this[i, i];
                    }
                    return result;
                }
                else
                {
                    throw new();
                }
            }
        }
        
        public bool IsSquare => M == N;
        public bool IsVector => M == 1 || N == 1;

        public Matrix Transposed
        {
            get
            {
                Matrix res = new(N, M);
                for (int i = 0; i < M; ++i)
                {
                    for (int j = 0; j < N; ++j)
                    {
                        res[j, i] = this[i, j];
                    }
                }
                return res;
            }
        }

        public static implicit operator Matrix(Complex value) => new(new Complex[1, 1] { { value } });
        public static implicit operator Matrix(Complex[,] value)
        {
            Matrix res = new(value.GetLength(0), value.GetLength(1));
            for (int i = 0; i < value.GetLength(0); ++i) //-V3080
            {
                for (int j = 0; j < value.GetLength(1); ++j)
                {
                    res[i, j] = value[i, j];
                }
            }
            return res;
        }

        public static explicit operator Complex(Matrix value) => value.Determinant;
        public static explicit operator Complex[,](Matrix value)
        {
            Complex[,] res = new Complex[value.M, value.N];
            for (int i = 0; i < value.M; ++i)
            {
                for (int j = 0; j < value.N; ++j)
                {
                    res[i, j] = value[i, j];
                }
            }
            return res;
        }

        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (left.M == right.N && left.N == right.M)
            {
                Matrix res = new(left.M, left.N);
                for (int i = 0; i < res.M; ++i)
                {
                    for (int j = 0; j < res.N; ++j)
                    {
                        res[i, j] = left[i, j] + right[i, j];
                    }
                }
                return res;
            }
            else
            {
                throw new();
            }
        }
        public static Matrix operator -(Matrix left, Matrix right)
        {
            if (left.M == right.N && left.N == right.M)
            {
                Matrix res = new(left.M, left.N);
                for (int i = 0; i < res.M; ++i)
                {
                    for (int j = 0; j < res.N; ++j)
                    {
                        res[i, j] = left[i, j] - right[i, j];
                    }
                }
                return res;
            }
            else
            {
                throw new();
            }
        }

        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left.N == right.M)
            {
                Matrix res = new(left.M, right.N);
                for (int i = 0; i < left.M; ++i)
                {
                    for (int j = 0; j < right.N; ++j)
                    {
                        for (int e = 0; e < right.M; ++e)
                        {
                            res[i, j] += left[i, e] * right[e, j];
                        }
                    }
                }
                return res;
            }
            else
            {
                throw new();
            }
        }
        public static Matrix operator *(Matrix left, Complex right)
        {
            Matrix res = new(left.M, left.N);
            for (int i = 0; i < res.M; ++i)
            {
                for (int j = 0; j < res.N; ++j)
                {
                    res[i, j] = left[i, j] * right;
                }
            }
            return res;
        }
        public static Matrix operator *(Complex left, Matrix right)
        {
            Matrix res = new(right.M, right.N);
            for (int i = 0; i < res.M; ++i)
            {
                for (int j = 0; j < res.N; ++j)
                {
                    res[i, j] = left * right[i, j];
                }
            }
            return res;
        }

        public static Matrix operator /(Matrix left, Complex right)
        {
            Matrix res = new(left.M, left.N);
            for (int i = 0; i < res.M; ++i)
            {
                for (int j = 0; j < res.N; ++j)
                {
                    res[i, j] = left[i, j] / right;
                }
            }
            return res;
        }/*
        public static Matrix operator /(Complex left, Matrix right)
        {
            
        }*/

        public Matrix LinearTransform(CR cr, int cr1, Complex k, int cr2)
        {
            switch (cr)
            {
                case CR.Column:
                {
                    for (int i = 0; i < M; ++i)
                    {
                        this[i, cr1 - 1] += k * this[i, cr2 - 1];
                    }
                    return this;
                }
                case CR.Row:
                {
                    for (int i = 0; i < N; ++i)
                    {
                        this[cr1 - 1, i] += k * this[cr2 - 1, i];
                    }
                    return this;
                }
                default: throw new();
            }
        }
        public Matrix Pow(int n)
        {
            if (M == N)
            {
                if (n == 1)
                {
                    return this;
                }
                else
                {
                    Matrix res = new(M, N);
                    for (int i = 0; i < M; ++i)
                    {
                        for (int j = 0; j < N; ++j)
                        {
                            for (int e = 0; e < M; ++e)
                            {
                                res[i, j] += this[i, e] * Pow(n - 1)[e, j];
                            }
                        }
                    }
                    return res;
                }
            }
            else
            {
                throw new();
            }
        }
        public Matrix Remove(CR cr, int cr0)
        {
            switch (cr)
            {
                case CR.Column:
                {
                    Matrix temp = new(M, N - 1);
                    for (int i = 0; i < M; ++i)
                    {
                        for (int j = 0; j < N - 1; ++j)
                        {
                            temp[i, j] = j < cr0 - 1 ? this[i, j] : this[i, j + 1];
                        }
                    }
                    return temp;
                }
                case CR.Row:
                {
                    Matrix temp = new(M - 1, N);
                    for (int i = 0; i < M - 1; ++i)
                    {
                        for (int j = 0; j < N; ++j)
                        {
                            temp[i, j] = i < cr0 - 1 ? this[i, j] : this[i + 1, j];
                        }
                    }
                    return temp;
                }
                default: throw new();
            }
        }
        public Matrix Remove(int row, int column) => Remove(CR.Row, row).Remove(CR.Column, column);
        public Matrix Swap(CR cr, int cr1, int cr2)
        {
            switch (cr)
            {
                case CR.Column:
                {
                    for (int i = 0; i < M; ++i)
                    {
                        (this[i, cr1 - 1], this[i, cr2 - 1]) = (this[i, cr2 - 1], this[i, cr1 - 1]);
                    }
                    return this;
                }
                case CR.Row:
                {
                    for (int i = 0; i < N; ++i)
                    {
                        (this[cr1 - 1, i], this[cr2 - 1, i]) = (this[cr2 - 1, i], this[cr1 - 1, i]);
                    }
                    return this;
                }
                default: throw new();
            }
        }
        public Matrix ToUpperTriangular()
        {
            if (IsSquare)
            {
                if (M == 1)
                {
                    return this;
                }
                for (int i = 2; i <= M; ++i)
                {
                    LinearTransform(CR.Row, i, -this[i - 1, 0] / this[0, 0], 1);
                }
                Matrix temp = Remove(1, 1).ToUpperTriangular();
                for (int i = 1; i < M; ++i)
                {
                    this[i, 0] = 0;
                    for (int j = 1; j < N; ++j)
                    {
                        this[i, j] = temp[i - 1, j - 1];
                    }
                }
                return this;
            }
            else
            {
                throw new();
            }
        }

        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < M; ++i)
            {
                for (int j = 0; j < N; ++j)
                {
                    res += $"{this[i, j]}{(j != N - 1 ? "\t" : i != M - 1 ? "\n" : "")}";
                }
            }
            return res;
        }
    }
}
