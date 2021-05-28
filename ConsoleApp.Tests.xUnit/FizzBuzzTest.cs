using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace ConsoleApp.Tests.xUnit
{

    public class FizzBuzzTest
    {
        [Fact]
        public void Work_ReturnsFizzBuzzString()
        {
            //Arrage
            var fizzBuzz = new FizzBuzz();

            //Act
            var result = fizzBuzz.Work();

            //Assert
            var values = result.Split("\r\n");
            using (new AssertionScope())
            {
                values.Should().HaveCount(100);
                values.Where(x => x.Contains("Fizz")).Should().HaveCount(33);
                values.Where(x => x.Contains("Buzz")).Should().HaveCount(20);
            }
        }

    //    [Fact]
    //    public void Work_ReturnsStringWith100Lines()
    //    {
    //        //Arrage
    //        var fizzBuzz = new FizzBuzz();

    //        //Act
    //        var result = fizzBuzz.Work();

    //        //Assert
    //        result.Split("\n").Should().HaveCount(100);
    //    }

    //    [Fact]
    //    public void Work_ReturnsStringWithValuesFrom1To100()
    //    {
    //        //Arrage
    //        var fizzBuzz = new FizzBuzz();

    //        //Act
    //        var result = fizzBuzz.Work();

    //        //Assert
    //        result.Split("\r\n").Should().Contain("1").And.Contain("2").And.Contain("98");
    //    }

    //    [Fact]
    //    public void Work_ReturnsStringWithFizz()
    //    {
    //        //Arrage
    //        var fizzBuzz = new FizzBuzz();

    //        //Act
    //        var result = fizzBuzz.Work();

    //        //Assert
    //        var values = result.Split("\r\n");
    //        values.Where(x => x.Contains("Fizz")).Should().HaveCount(33);
    //    }

    //    [Fact]
    //    public void Work_ReturnsStringWithBuzz()
    //    {
    //        //Arrage
    //        var fizzBuzz = new FizzBuzz();

    //        //Act
    //        var result = fizzBuzz.Work();

    //        //Assert
    //        var values = result.Split("\r\n");
    //        values.Where(x => x.Contains("Buzz")).Should().HaveCount(20);
    //    }
    }
}
