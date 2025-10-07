using System;
using FractionLib;
using Xunit;

namespace FractionTests
{
    public class FractionTests
    {
        // ===== Конструктор =====
        [Fact]
        public void Constructor_ShouldSimplifyFraction()
        {
            var f = new Fraction(2, 4);
            Assert.Equal(1, f.Numerator);
            Assert.Equal(2, f.Denominator);
        }

        [Fact]
        public void Constructor_ShouldNormalizeSign()
        {
            var f = new Fraction(1, -2);
            Assert.Equal(-1, f.Numerator);
            Assert.Equal(2, f.Denominator);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenDenominatorIsZero()
        {
            Assert.Throws<DivideByZeroException>(() => new Fraction(1, 0));
        }

        // ===== Арифметические операции =====
        [Theory]
        [InlineData(1, 2, 1, 3, 5, 6)]
        [InlineData(-1, 2, 1, 3, -1, 6)]
        [InlineData(1, 4, 3, 4, 1, 1)]
        public void Addition_ShouldReturnCorrectResult(
            int n1, int d1, int n2, int d2, int expN, int expD)
        {
            var a = new Fraction(n1, d1);
            var b = new Fraction(n2, d2);

            var result = a + b;

            Assert.Equal(expN, result.Numerator);
            Assert.Equal(expD, result.Denominator);
        }

        [Theory]
        [InlineData(1, 2, 1, 3, 1, 6)]
        [InlineData(1, 3, 1, 3, 0, 1)]
        [InlineData(3, 4, 1, 2, 1, 4)]
        public void Subtraction_ShouldReturnCorrectResult(
            int n1, int d1, int n2, int d2, int expN, int expD)
        {
            var a = new Fraction(n1, d1);
            var b = new Fraction(n2, d2);

            var result = a - b;

            Assert.Equal(expN, result.Numerator);
            Assert.Equal(expD, result.Denominator);
        }

        [Theory]
        [InlineData(1, 2, 2, 3, 1, 3)]
        [InlineData(-1, 2, 2, 3, -1, 3)]
        [InlineData(3, 5, 5, 3, 1, 1)]
        public void Multiplication_ShouldReturnCorrectResult(
            int n1, int d1, int n2, int d2, int expN, int expD)
        {
            var a = new Fraction(n1, d1);
            var b = new Fraction(n2, d2);

            var result = a * b;

            Assert.Equal(expN, result.Numerator);
            Assert.Equal(expD, result.Denominator);
        }

        [Theory]
        [InlineData(1, 2, 2, 3, 3, 4)]
        [InlineData(-1, 2, 1, 3, -3, 2)]
        [InlineData(3, 4, 3, 4, 1, 1)]
        public void Division_ShouldReturnCorrectResult(
            int n1, int d1, int n2, int d2, int expN, int expD)
        {
            var a = new Fraction(n1, d1);
            var b = new Fraction(n2, d2);

            var result = a / b;

            Assert.Equal(expN, result.Numerator);
            Assert.Equal(expD, result.Denominator);
        }

        [Fact]
        public void Division_ByZeroNumerator_ShouldThrow()
        {
            var a = new Fraction(1, 2);
            var b = new Fraction(0, 3);

            Assert.Throws<DivideByZeroException>(() => _ = a / b);
        }

        // ===== Остальные методы =====
        [Fact]
        public void ToDouble_ShouldReturnCorrectValue()
        {
            var f = new Fraction(1, 4);
            Assert.Equal(0.25, f.ToDouble(), 5);
        }

        [Fact]
        public void ToString_ShouldReturnExpectedFormat()
        {
            var f = new Fraction(3, 5);
            Assert.Equal("3/5", f.ToString());
        }

        [Fact]
        public void Equals_ShouldReturnTrueForEquivalentFractions()
        {
            var a = new Fraction(2, 4);
            var b = new Fraction(1, 2);
            Assert.True(a.Equals(b));
        }

        [Fact]
        public void Equals_ShouldReturnFalseForDifferentFractions()
        {
            var a = new Fraction(1, 2);
            var b = new Fraction(2, 3);
            Assert.False(a.Equals(b));
        }

        [Fact]
        public void GetHashCode_ShouldMatchForEquivalentFractions()
        {
            var a = new Fraction(2, 4);
            var b = new Fraction(1, 2);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }
    }
}