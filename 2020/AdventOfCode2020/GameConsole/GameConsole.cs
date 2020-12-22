using System;
using System.Collections.Generic;

namespace AdventOfCode2020
{
    public class GameConsole
    {
        private string[] _program;
        private ConsoleState _consoleState;

        public event EventHandler<HitInfiniteLoopEventArgs> HitInfiniteLoop;
        public event EventHandler<EndOfProgramArgs> HitEndOfProgram;

        public GameConsole()
        {
        }

        public void LoadProgram(string[] program)
        {
            _program = program;
        }

        public void RunProgram(string[] program)
        {
            _program = program;
            _consoleState = new ConsoleState();

            while (true)
            {
                OperationOutcome outcome = RunNextOperation();

                if (outcome == OperationOutcome.EndOfProgram)
                {
                    HitEndOfProgram?.Invoke(this, new EndOfProgramArgs()
                    {
                        Accumulator = _consoleState.Accumulator
                    });
                    break;
                }

                if (outcome == OperationOutcome.InfiniteLoop)
                {
                    HitInfiniteLoop?.Invoke(this, new HitInfiniteLoopEventArgs()
                    {
                        Accumulator = _consoleState.Accumulator,
                        OperationLine = _consoleState.GetLastOperationIndex()
                    });
                    break;
                }
            }
        }

        private OperationOutcome RunNextOperation()
        {
            if (OperationLineHasBeenRun())
            {
                return OperationOutcome.InfiniteLoop;
            }

            if (EndOfProgram())
            {
                return OperationOutcome.EndOfProgram;
            }

            PerformOperation();

            return OperationOutcome.Succeeded;
        }

        private void PerformOperation()
        {
            Operation operation = new Operation(_program[_consoleState.NextOperationIndex]);

            operation.PerformOperation(_consoleState);         
        }

        private bool OperationLineHasBeenRun()
        {
            return _consoleState.OperationHasBeenPerformed(_consoleState.NextOperationIndex);
        }

        private bool EndOfProgram()
        {
            return _consoleState.NextOperationIndex >= _program.Length;
        }
    }

    enum OperationOutcome
    {
        Succeeded,
        EndOfProgram,
        InfiniteLoop
    }

    public class HitInfiniteLoopEventArgs : EventArgs
    {
        public int Accumulator { get; set; }
        public int OperationLine { get; set; }
    }

    public class EndOfProgramArgs : EventArgs
    {
        public int Accumulator { get; set; }
    }
}
