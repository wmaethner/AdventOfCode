using System;
using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode2020
{
    public class ConsoleState
    {
        public int Accumulator { get; private set; }
        public int NextOperationIndex { get; private set; }

        private Stack<int> _stackTrace;

        public ConsoleState()
        {
            Accumulator = 0;
            NextOperationIndex = 0;

            _stackTrace = new Stack<int>();
        }

        public void UpdateAccumulator(int offset)
        {
            Accumulator += offset;
        }

        public void UpdateOperationIndex(int offset)
        {
            _stackTrace.Push(NextOperationIndex);
            NextOperationIndex += offset;
        }

        public int GetLastOperationIndex()
        {
            return _stackTrace.Peek();
        }

        public bool OperationHasBeenPerformed(int operationIndex)
        {
            return _stackTrace.Contains(operationIndex);
        }
    }
}
