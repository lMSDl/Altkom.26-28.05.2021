using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using FluentAssertions.Execution;

namespace ConsoleApp.Tests.xUnit
{
    public class CalculatorTest
    {
        [Theory]
        [InlineData(1f, float.NaN, "b")]
        [InlineData(float.NaN, 1f, "a")]
        public void Add_PassNaN_ThrowsArgumentException(float a, float b, string name)
        {
            //Arrange
            var calculator = new Calculator();

            //Act && Assert
            var exception = Assert.Throws<ArgumentException>(() => calculator.Add(a, b));
            Assert.Equal($"Value can't be NaN (Parameter '{name}')", exception.Message);        
        }

        [Theory]
        [InlineData(1f, 1f, 2f)]
        [InlineData(-1f, 1f, 0)]
        [InlineData(-1f, -1f, -2f)]
        [InlineData(1f, -1f, 0)]
        [InlineData(0f, 0f, 0)]
        [InlineData(0.5f, 0.25f, 0.75f)]
        [InlineData(float.MaxValue, float.MaxValue, float.PositiveInfinity)]
        [InlineData(float.MaxValue, float.MinValue, 0)]
        public void Add_PassValues_ResturnsResult(float a, float b, float result)
        {
            //Arrange
            var calculator = new Calculator();

            //Act
            var actResult = calculator.Add(a, b);

            //Assert
            Assert.Equal(result, actResult);
        }

        [Fact]
        public void Add_PassValues_InvokesCaluclationEvent()
        {
            //Arrange
            var calculator = new Calculator();
            Calculator.CalculationEventArgs calculationEventArgs = null;
            calculator.CalculationEvent += (sender, args) => calculationEventArgs = args;

            //Act
            var actResult = calculator.Add(0, 1);

            //Assert

            using (var scope = new AssertionScope())
            {
                calculationEventArgs.Should().NotBeNull();
                calculationEventArgs.Expression.Should().Be("0+1");
                calculationEventArgs.Result.Should().Be(actResult);
            }
            //calculationEventArgs.Should().NotBeNull().And
            //    .Match<Calculator.CalculationEventArgs>(x => x.Expression == "0+0" && x.Result == actResult);

            //Assert.NotNull(calculationEventArgs);
            //Assert.Equal("0+1", calculationEventArgs.Expression);
            //Assert.Equal(actResult, calculationEventArgs.Result);
        }
    }
}
