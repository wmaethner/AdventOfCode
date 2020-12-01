using AdventOfCode2019.Intcode_Computer;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace IntcodeComputerUnitTests
{
    public class ProgramIOUnitTests
    {
        [Fact]
        public void WriteInput_ReadInput_SameValue()
        {
            ProgramIO programIO = new ProgramIO();
            string value = "1";

            programIO.WriteLineInput(value);

            Assert.Equal(value, programIO.ReadLineInput());
        }

        [Fact]
        public void WriteMultipleInput_ReadInputs_CorrectValueInOrder()
        {
            ProgramIO programIO = new ProgramIO();

            string value1 = "1";
            programIO.WriteLineInput(value1);

            Assert.Equal(value1, programIO.ReadLineInput());

            string value2 = "2";
            programIO.WriteLineInput(value2);
            string value3 = "3";
            programIO.WriteLineInput(value3);

            Assert.Equal(value2, programIO.ReadLineInput());
            Assert.Equal(value3, programIO.ReadLineInput());
        }
    }
}
