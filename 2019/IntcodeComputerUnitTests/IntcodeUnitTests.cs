using AdventOfCode2019.Intcode_Computer;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace IntcodeComputerUnitTests
{
    public class IntcodeUnitTests
    {
        #region OperationTests

        #region Operation 1
        [Fact]
        public void Operation1_PositionMode_AddsValues()
        {
            string program = "1,0,0,0,99";
            IntcodeComputer computer = new IntcodeComputer();
            computer.AddProgram("key", program);
            computer.RunProgram("key");

            Assert.Equal(2, computer.GetProgramAddressValue("key", 0));
        }

        [Fact]
        public void Operation1_ImmediateMode_AddsValues()
        {
            // First param is in immediate mode so its value is 0
            string program = "101,0,0,0,99";
            IntcodeComputer computer = new IntcodeComputer();
            computer.AddProgram("key", program);
            computer.RunProgram("key");

            Assert.Equal(101, computer.GetProgramAddressValue("key", 0));
        }
        #endregion

        #region Operation 2
        [Fact]
        public void Operation2_PositionMode_MultipliesValues()
        {
            string program = "2,3,0,3,99";
            IntcodeComputer computer = new IntcodeComputer();
            computer.AddProgram("key", program);
            computer.RunProgram("key");

            Assert.Equal(6, computer.GetProgramAddressValue("key", 3));
        }

        [Fact]
        public void Operation2_ImmediateMode_MultipliesValues()
        {
            // First param is in immediate mode so its value is 0
            string program = "1002,4,3,4,33";
            IntcodeComputer computer = new IntcodeComputer();
            computer.AddProgram("key", program);
            computer.RunProgram("key");

            Assert.Equal(99, computer.GetProgramAddressValue("key", 4));
        }
        #endregion

        #region Operation 3
        [Fact]
        public void Operation3_PositionMode_TakesInput_SetsValue()
        {
            string program = "3,0,99";
            IntcodeComputer computer = new IntcodeComputer();

            //StringReader reader = new StringReader("1");

            ProgramIO programIO = new ProgramIO();
            programIO.WriteLineInput("1");

            computer.AddProgram("key", program, programIO);
            computer.RunProgram("key");

            Assert.Equal(1, computer.GetProgramAddressValue("key", 0));
        }
        //[Fact]
        //public void Operation3_ImmediateMode_TakesInput_SetsValue()
        //{
        //    string program = "103,0,99";
        //    IntcodeComputer computer = new IntcodeComputer();

        //    //StringReader reader = new StringReader("1");

        //    ProgramIO programIO = new ProgramIO();
        //    programIO.WriteLineInput("1");

        //    computer.AddProgram("key", program, programIO);
        //    computer.RunProgram("key");

        //    Assert.Equal(1, computer.GetProgramAddressValue("key", 0));
        //}
        [Fact]
        public void Operation3_RelativeMode()
        {
            string code = "109,-1,203,6,104,1,99";
            string programKey = "Day9";

            IntcodeComputer computer = new IntcodeComputer();
            ProgramIO programIO = new ProgramIO();
            programIO.WriteLineInput("2");

            computer.AddProgram(programKey, code, programIO);

            computer.RunProgram(programKey);

            Assert.Equal("2", programIO.ReadLineOutput());
        }
        #endregion

        #region Operation 4
        [Fact]
        public void Operation4_PositionMode_TakesValue_OutputsValue()
        {
            string program = "4,0,99";
            IntcodeComputer computer = new IntcodeComputer();

            ProgramIO programIO = new ProgramIO();

            computer.AddProgram("key", program, programIO);
            computer.RunProgram("key");

            Assert.Equal("4", programIO.ReadLineOutput());
        }
        [Fact]
        public void Operation4_ImmediateMode_TakesValue_OutputsValue()
        {
            string program = "104,0,99";
            IntcodeComputer computer = new IntcodeComputer();

            ProgramIO programIO = new ProgramIO();

            computer.AddProgram("key", program, programIO);
            computer.RunProgram("key");

            Assert.Equal("0", programIO.ReadLineOutput());
        }
        #endregion

        #region Operation 5
        [Fact]
        public void Operation5_PositionMode_ValueTrue_JumpsToSecondOutput()
        {
            string program = "5,1,3,7,4,1,99,4,0,99";
            IntcodeComputer computer = new IntcodeComputer();

            ProgramIO programIO = new ProgramIO();

            computer.AddProgram("key", program, programIO);
            computer.RunProgram("key");

            Assert.Equal("5", programIO.ReadLineOutput());
        }
        [Fact]
        public void Operation5_PositionMode_ValueFalse_DoesntJumpFirstOutput()
        {
            string program = "5,2,0,4,2,99";
            IntcodeComputer computer = new IntcodeComputer();

            ProgramIO programIO = new ProgramIO();

            computer.AddProgram("key", program, programIO);
            computer.RunProgram("key");

            Assert.Equal("0", programIO.ReadLineOutput());
        }
        [Fact]
        public void Operation5_ImmediateMode_ValueTrue_JumpsToSecondOutput()
        {
            string program = "1105,1,6,4,1,99,4,0,99";
            IntcodeComputer computer = new IntcodeComputer();

            ProgramIO programIO = new ProgramIO();

            computer.AddProgram("key", program, programIO);
            computer.RunProgram("key");

            Assert.Equal("1105", programIO.ReadLineOutput());
        }
        [Fact]
        public void Operation5_ImmediateMode_ValueFalse_DoesntJumpFirstOutput()
        {
            string program = "1105,0,0,4,2,99";
            IntcodeComputer computer = new IntcodeComputer();

            ProgramIO programIO = new ProgramIO();

            computer.AddProgram("key", program, programIO);
            computer.RunProgram("key");

            Assert.Equal("0", programIO.ReadLineOutput());
        }
        #endregion

        #region Operation 6
        [Fact]
        public void Operation6_PositionMode_ValueTrue_DoesntJumpFirstOutput()
        {
            string program = "6,3,4,0,4,2,99";           
            IntcodeComputer computer = new IntcodeComputer();

            ProgramIO programIO = new ProgramIO();

            computer.AddProgram("key", program, programIO);
            computer.RunProgram("key");

            Assert.Equal("4", programIO.ReadLineOutput());
        }
        [Fact]
        public void Operation6_PositionMode_ValueFalse_JumpsToSecondOutput()
        {
            string program = "6,1,3,4,1,99,4,0,99";
            IntcodeComputer computer = new IntcodeComputer();

            ProgramIO programIO = new ProgramIO();

            computer.AddProgram("key", program, programIO);
            computer.RunProgram("key");

            Assert.Equal("1", programIO.ReadLineOutput());
        }
        [Fact]
        public void Operation6_ImmediateMode_ValueTrue_DoesntJumpFirstOutput()
        {
            string program = "1106,0,6,4,1,99,4,0,99"; 
            IntcodeComputer computer = new IntcodeComputer();

            ProgramIO programIO = new ProgramIO();

            computer.AddProgram("key", program, programIO);
            computer.RunProgram("key");

            Assert.Equal("1106", programIO.ReadLineOutput());
        }
        [Fact]
        public void Operation6_ImmediateMode_ValueFalse_JumpsToSecondOutput()
        {
            string program = "1106,1,6,4,1,99,4,0,99";
            IntcodeComputer computer = new IntcodeComputer();

            ProgramIO programIO = new ProgramIO();

            computer.AddProgram("key", program, programIO);
            computer.RunProgram("key");

            Assert.Equal("1", programIO.ReadLineOutput());
        }
        #endregion


        #endregion
    }
}
