using System;
using FractionLib;
using FluentAssertions;
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
            f.Numerator.Should().Be(1);
            f.Denominator.Should().Be(2);
        }

        [Fact]
        public void Constructor_ShouldNormalizeSign()
        {
            var f = new Fraction(1, -2);
            f.Numerator.Should().Be(-1);
            f.Denominator.Should().Be(2);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenDenominatorIsZero()
        {
            Action act = () => new Fraction(1, 0);
            act.Should().Throw<DivideByZeroException>()
                .WithMessage("Denominator cannot be zero.");
        }

        // ===== Операции =====
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

            result.Should().BeEquivalentTo(new Fraction(expN, expD));
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

            result.Should().BeEquivalentTo(new Fraction(expN, expD));
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

            result.Should().BeEquivalentTo(new Fraction(expN, expD));
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

            result.Should().BeEquivalentTo(new Fraction(expN, expD));
        }

        [Fact]
        public void Division_ByZeroNumerator_ShouldThrow()
        {
            var a = new Fraction(1, 2);
            var b = new Fraction(0, 3);

            Action act = () => { var _ = a / b; };
            act.Should().Throw<DivideByZeroException>()
                .WithMessage("Cannot divide by zero.");
        }

        // ===== Остальные методы =====
        [Fact]
        public void ToDouble_ShouldReturnCorrectValue()
        {
            var f = new Fraction(1, 4);
            f.ToDouble().Should().BeApproximately(0.25, 1e-10);
        }

        [Fact]
        public void ToString_ShouldReturnExpectedFormat()
        {
            var f = new Fraction(3, 5);
            f.ToString().Should().Be("3/5");
        }

        [Fact]
        public void Equals_ShouldReturnTrueForEquivalentFractions()
        {
            var a = new Fraction(2, 4);
            var b = new Fraction(1, 2);

            a.Equals(b).Should().BeTrue();
        }

        [Fact]
        public void Equals_ShouldReturnFalseForDifferentFractions()
        {
            var a = new Fraction(1, 2);
            var b = new Fraction(2, 3);

            a.Equals(b).Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_ShouldMatchForEquivalentFractions()
        {
            var a = new Fraction(2, 4);
            var b = new Fraction(1, 2);

            a.GetHashCode().Should().Be(b.GetHashCode());
        }
    }
}