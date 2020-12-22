using System;
namespace AdventOfCode2020
{
    public class Operation
    {
        public OperationType OperationType { get; set; }
        public int Value { get; set; }

        public Operation(string operation)
        {
            string[] parts = operation.Split(" ");
            ParseType(parts[0]);
            ParseValue(parts[1]);
        }

        private void ParseType(string type)
        {
            switch (type)
            {
                case "acc":
                    OperationType = OperationType.Accumulator;
                    break;
                case "jmp":
                    OperationType = OperationType.Jumper;
                    break;
                case "nop":
                    OperationType = OperationType.NoOp;
                    break;
                default:
                    OperationType = OperationType.Unknown;
                    break;
            }
        }

        private void ParseValue(string value)
        {
            Value = Int32.Parse(value);
        }

        internal void PerformOperation(ConsoleState consoleState)
        {
            switch (OperationType)
            {
                case OperationType.Accumulator:
                    consoleState.UpdateAccumulator(Value);
                    consoleState.UpdateOperationIndex(1);
                    break;
                case OperationType.Jumper:
                    consoleState.UpdateOperationIndex(Value);
                    break;
                case OperationType.NoOp:
                    consoleState.UpdateOperationIndex(1);
                    break;
                default:
                    break;
            }
        }
    }

    public enum OperationType
    {
        Accumulator,
        Jumper,
        NoOp,
        Unknown
    }
}
