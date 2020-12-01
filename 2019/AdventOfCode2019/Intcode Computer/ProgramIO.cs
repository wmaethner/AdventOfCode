using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode2019.Intcode_Computer
{   
    public class ProgramIO
    {
        public event EventHandler InputDataAdded;
        public event EventHandler OutputDataAdded;

        private int _inputCounter;
        private int _outputCounter;
        private List<string> _inputStream = new List<string>();
        private List<string> _outputStream = new List<string>();

        public ProgramIO()
        {
            ResetState();
        }

        #region Input
        public void WriteLineInput(string value)
        {
            _inputStream.Add(value);
            InputDataAdded?.Invoke(this, new EventArgs());
        }

        public string ReadLineInput()
        {
            if (!HasInput())
            {
                Console.Write("No input value for program. Provide input: ");
                _inputStream.Add(Console.ReadLine());
            }
            return _inputStream[_inputCounter++];
        } 

        public bool HasInput()
        {
            return _inputCounter < _inputStream.Count;
        }
        #endregion

        #region Output
        public void WriteLineOutput(string value)
        {
            _outputStream.Add(value);
            OutputDataAdded?.Invoke(this, new EventArgs());
        }

        public string ReadLineOutput()
        {
            if (!HasOutput())
            {
                return "";
            }
            return _outputStream[_outputCounter++];
        }
        
        public int ReadLineOutputInt()
        {
            string output = ReadLineOutput();
            return Int32.Parse(output);
        }

        public long ReadLineOutputLong()
        {
            string output = ReadLineOutput();
            return Int64.Parse(output);
        }

        public bool HasOutput()
        {
            return _outputCounter < _outputStream.Count;
        }
        #endregion

        public void ResetState()
        {
            _inputCounter = 0;
            _outputCounter = 0;
            _inputStream.Clear();
            _outputStream.Clear();
        }
    }
}
