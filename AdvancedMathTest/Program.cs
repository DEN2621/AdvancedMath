using System;

using AdvancedMath;

int a = 74;
Complex b = new Complex(37, 120, ComplexForm.Algebraic).InForm(ComplexForm.Exponential), c = a % b;
Console.WriteLine(c);
