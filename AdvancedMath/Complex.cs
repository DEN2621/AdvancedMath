// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace AdvancedMath
{
    /// <summary>
    /// Перечисление возможных форм комплексного числа
    /// </summary>
    public enum ComplexForm
    {
        /// <summary>
        /// Алгебраическая форма комплексного числа
        /// </summary>
        Algebraic,
        /// <summary>
        /// Экспонециальная форма комплексного числа
        /// </summary>
        Exponential,
        /// <summary>
        /// Матричная форма комплексного числа
        /// </summary>
        Matrix,
        /// <summary>
        /// Тригонометрическая форма комплексного числа
        /// </summary>
        Trigonometric
    }
    /// <summary>
    /// Класс, предназначенный для работы с комплексными числами
    /// </summary>
    public class Complex
    {
        private double re, im;

        /// <summary>
        /// Действительная часть комплексного числа
        /// </summary>
        public double Re { get => Math.Round(re, 13); private set => re = value; }
        /// <summary>
        /// Мнимая часть комплексного числа
        /// </summary>
        public double Im { get => Math.Round(im, 13); private set => im = value; }
        /// <summary>
        /// Модуль комплексного числа
        /// </summary>
        public double R { get; private set; }
        /// <summary>
        /// Аргумент комплексного числа
        /// </summary>
        public double A { get; private set; }

        /// <summary>
        /// Текущая форма комплексного числа
        /// </summary>
        private ComplexForm Form { get; set; }

        /// <summary>
        /// Определяет, является ли указанное значение действительным числом
        /// </summary>
        public bool IsReal => Im == 0;
        /// <summary>
        /// Определяет, является ли указанное значение чисто мнимым числом
        /// </summary>
        public bool IsImagine => (Re == 0) && (Im != 0);

        /// <summary>
        /// Возвращает число, сопряжённое указанному значению
        /// </summary>
        public Complex Conjugate => new Complex(Re, -Im, ComplexForm.Algebraic).ToForm(Form);

        /// <summary>
        /// Инициализирует новый экземпляр класса Complex, представляющий мнимую единицу (i)
        /// </summary>
        public Complex()
        {
            Form = ComplexForm.Algebraic;
            Re = 0;
            Im = 1;
            R = 1;
            A = Math.PI / 2;
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса Complex на основе двух заданных действитльных чисел в заданной форме
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="form">Форма комплексного числа</param>
        public Complex(double a, double b, ComplexForm form)
        {
            Form = form;
            switch (Form)
            {
                case ComplexForm.Algebraic or ComplexForm.Matrix:
                {
                    Re = a;
                    Im = b;
                    R = Math.Sqrt(Math.Pow(Re, 2) + Math.Pow(Im, 2));
                    A = Math.Atan2(Im, Re);
                    break;
                }
                case ComplexForm.Exponential or ComplexForm.Trigonometric:
                {
                    R = a;
                    A = b % (2 * Math.PI);
                    Re = R * Math.Cos(A);
                    Im = R * Math.Sin(A);
                    break;
                }
                default: break;
            }
        }

        public static implicit operator Complex(double value) => new(value, 0, ComplexForm.Algebraic);

        public static explicit operator double(Complex value) => value.R;

        public static Complex operator -(Complex value) => new Complex(-value.Re, -value.Im, ComplexForm.Algebraic).ToForm(value.Form);

        public static Complex operator +(Complex left, Complex right) => new Complex(left.Re + right.Re, left.Im + right.Im, ComplexForm.Algebraic).ToForm(left.Form);
        public static Complex operator +(Complex left, double right) => new Complex(left.Re + right, left.Im, ComplexForm.Algebraic).ToForm(left.Form);
        public static Complex operator +(double left, Complex right) => new Complex(left + right.Re, right.Im, ComplexForm.Algebraic).ToForm(right.Form);

        public static Complex operator -(Complex left, Complex right) => new Complex(left.Re - right.Re, left.Im - right.Im, ComplexForm.Algebraic).ToForm(left.Form);
        public static Complex operator -(Complex left, double right) => new Complex(left.Re - right, left.Im, ComplexForm.Algebraic).ToForm(left.Form);
        public static Complex operator -(double left, Complex right) => new Complex(left - right.Re, -right.Im, ComplexForm.Algebraic).ToForm(right.Form);

        public static Complex operator *(Complex left, Complex right) => new Complex(left.R * right.R, (left.A + right.A) % (2 * Math.PI), ComplexForm.Exponential).ToForm(left.Form);
        public static Complex operator *(Complex left, double right) => new Complex(left.R * right, left.A, ComplexForm.Exponential).ToForm(left.Form);
        public static Complex operator *(double left, Complex right) => new Complex(right.R, left * right.A, ComplexForm.Exponential).ToForm(right.Form);

        public static Complex operator /(Complex left, Complex right) => new Complex(left.R / right.R, (left.A - right.A) % (2 * Math.PI), ComplexForm.Exponential).ToForm(left.Form);
        public static Complex operator /(Complex left, double right) => new Complex(left.R / right, left.A, ComplexForm.Exponential).ToForm(left.Form);
        public static Complex operator /(double left, Complex right) => new Complex(left / right.R, -right.A, ComplexForm.Exponential).ToForm(right.Form);

        public static Complex operator %(Complex left, Complex right) => left - new Complex((int)Math.Round((left / right).Re), (int)Math.Round((left / right).Im), ComplexForm.Algebraic).ToForm(left.Form) * right;
        public static Complex operator %(Complex left, double right) => new Complex(left.Re % right, left.Im % right, ComplexForm.Algebraic).ToForm(left.Form);
        public static Complex operator %(double left, Complex right) => left - new Complex((int)Math.Round((left / right).Re), (int)Math.Round((left / right).Im), ComplexForm.Algebraic).ToForm(right.Form) * right;

        public static Complex operator ++(Complex value)
        {
            ++value.Re;
            return value;
        }
        public static Complex operator --(Complex value)
        {
            --value.Re;
            return value;
        }
        public static bool operator >(Complex left, Complex right) => left.R > right.R;
        public static bool operator <(Complex left, Complex right) => left.R < right.R;

        /// <summary>
        /// Представляет комплексное число в требуемой форме, но не изменяет его форму
        /// </summary>
        /// <param name="form">Требуемая форма комплексного числа</param>
        /// <returns>Новый экземпляр класса Complex в требуемой форме</returns>
        public Complex InForm(ComplexForm form) => new Complex(Re, Im, ComplexForm.Algebraic).ToForm(form);
        /// <summary>
        /// Переводит комплексное число в требуемую форму
        /// </summary>
        /// <param name="form">Требуемая форма комплексного числа</param>
        /// <returns>Текущее комплексное число в требуемой форме</returns>
        public Complex ToForm(ComplexForm form)
        {
            if (Form != form)
            {
                Form = form;
            }
            return this;
        }

        public override string ToString()
        {
            switch (Form)
            {
                case ComplexForm.Algebraic:
                {
                    string res = Re != 0 ? $"{Re}" : (IsReal ? "0" : "");
                    switch (Im)
                    {
                        case 1: res += Re != 0 ? "+i" : "i"; return res;
                        case 0: return res;
                        case -1: res += "-i"; return res;
                        default:
                        {
                            res += Im > 0 ? (IsImagine ? $"{Im}i" : $"+{Im}i") : $"{Im}i";
                            return res;
                        }
                    }
                }
                case ComplexForm.Exponential:
                {
                    string res = $"{R}";
                    switch (A)
                    {
                        case 0: return res;
                        case Math.PI: return $"-{res}";
                        default:
                        {
                            res += $"e^{(A < 0 ? "(-" : "")}i{Math.Abs(A)}{(A < 0 ? ")" : "")}";
                            return res;
                        }
                    }
                }
                case ComplexForm.Matrix:
                {
                    return new Matrix(new Complex[2, 2] { { Re, Im }, { -Im, Re } }).ToString();
                }
                case ComplexForm.Trigonometric:
                {
                    return $"{R}({$"cos({A})+i*sin({A}))"}";
                }
                default: throw new Exception();
            }
        }
    }
}
