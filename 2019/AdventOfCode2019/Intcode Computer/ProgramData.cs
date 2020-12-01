using System;
using System.IO;

namespace AdventOfCode2019.Intcode_Computer
{
    class ProgramData
    {
        #region Private Variables
        Int64[] _baseData;
        Int64[] _programData;
        Int64 _pointer;
        Int64 _relativeBase;
        #endregion

        #region Properties
        public bool Halted { get; set; }
        public ProgramIO ProgramIO { get; set; }
        public bool Paused { get; set; }

        public ProgramState ProgramState { get; set; }
        #endregion

        #region Constructor
        public ProgramData(Int64[] startingData) : this(startingData, new ProgramIO())
        {

        }

        public ProgramData(Int64[] startingData, ProgramIO programIO)
        {
            _baseData = new Int64[startingData.Length];
            _programData = new Int64[startingData.Length];

            startingData.CopyTo(_baseData, 0);
            startingData.CopyTo(_programData, 0);

            Halted = false;
            _pointer = 0;

            ProgramIO = programIO;
        }
        #endregion

        #region Program Operation
        public void RunNextInstruction()
        {
            string instStr = _programData[_pointer].ToString().PadLeft(5, '0');
            int opcode = Int32.Parse(instStr.Substring(instStr.Length - 2));

            ParamModes[] modes = new ParamModes[3];
            for (int i = 0; i < 3; i++)
            {
                modes[i] = (ParamModes)Int32.Parse(instStr.Substring(instStr.Length - 3 - i, 1));
            }

            switch (opcode)
            {
                case 1:
                    Operation1(modes);
                    break;
                case 2:
                    Operation2(modes);
                    break;
                case 3:
                    Operation3(modes);
                    break;
                case 4:
                    Operation4(modes);
                    break;
                case 5:
                    Operation5(modes);
                    break;
                case 6:
                    Operation6(modes);
                    break;
                case 7:
                    Operation7(modes);
                    break;
                case 8:
                    Operation8(modes);
                    break;
                case 9:
                    Operation9(modes);
                    break;

                case 99:
                    Operation99(modes);
                    break;
                default:
                    throw new NotImplementedException($"Operation {opcode} is not yet implemented");
            }
        }
        #endregion

        #region Data Access
        public Int64 GetAddressValue(int address)
        {
            return _programData[address];
        }

        public void SetAddressValue(int address, int value)
        {
            _programData[address] = value;
        }

        public Int64[] GetProgramData()
        {
            return _programData;
        }

        public Int64[] GetBaseProgramData()
        {
            return _baseData;
        }

        public void ResetProgram()
        {
            _baseData.CopyTo(_programData, 0);
            _pointer = 0;
            ProgramIO.ResetState();
            ProgramState = ProgramState.NotStarted;
            Halted = false;
        }
        #endregion

        #region Operations
        private void Operation1(ParamModes[] modes)
        {
            Int64 val1 = GetInputParameter(1, modes[0]);
            Int64 val2 = GetInputParameter(2, modes[1]);
            Int64 storageAddress = GetOutputPosition(3, modes[2]);
            SetAddressValueSafe(storageAddress, val1 + val2);
            _pointer += 4;
        }
        private void Operation2(ParamModes[] modes)
        {
            Int64 val1 = GetInputParameter(1, modes[0]);
            Int64 val2 = GetInputParameter(2, modes[1]);
            Int64 storageAddress = GetOutputPosition(3, modes[2]);
            SetAddressValueSafe(storageAddress, val1 * val2);
            _pointer += 4;
        }
        private void Operation3(ParamModes[] modes)
        {
            if (!ProgramIO.HasInput())
            {
                ProgramState = ProgramState.Paused;
                return;
            }

            long value = ReadInput();
            Int64 storageAddress = GetOutputPosition(1, modes[0]);
            SetAddressValueSafe(storageAddress, value);
            _pointer += 2;
        }
        private void Operation4(ParamModes[] modes)
        {
            Int64 value = GetInputParameter(1, modes[0]);
            WriteOutput(value.ToString());
            _pointer += 2;
        }
        private void Operation5(ParamModes[] modes)
        {
            Int64 param1 = GetInputParameter(1, modes[0]);
            if (param1 != 0)
            {
                _pointer = GetInputParameter(2, modes[1]);
            }
            else
            {
                _pointer += 3;
            }
        }
        private void Operation6(ParamModes[] modes)
        {
            Int64 param1 = GetInputParameter(1, modes[0]);
            if (param1 == 0)
            {
                _pointer = GetInputParameter(2, modes[1]);
            }
            else
            {
                _pointer += 3;
            }
        }
        private void Operation7(ParamModes[] modes)
        {
            Int64 param1 = GetInputParameter(1, modes[0]);
            Int64 param2 = GetInputParameter(2, modes[1]);
            int value = (param1 < param2) ? 1 : 0;
            Int64 storageAddress = GetOutputPosition(3, modes[2]);
            SetAddressValueSafe(storageAddress, value);
            _pointer += 4;
        }
        private void Operation8(ParamModes[] modes)
        {
            Int64 param1 = GetInputParameter(1, modes[0]);
            Int64 param2 = GetInputParameter(2, modes[1]);
            int value = (param1 == param2) ? 1 : 0;
            Int64 storageAddress = GetOutputPosition(3, modes[2]);
            SetAddressValueSafe(storageAddress, value);
            _pointer += 4;
        }
        private void Operation9(ParamModes[] modes)
        {
            Int64 param1 = GetInputParameter(1, modes[0]);
            _relativeBase += param1;
            _pointer += 2;
        }

        private void Operation99(ParamModes[] modes)
        {
            ProgramState = ProgramState.Halted;
            Halted = true;
            _pointer++;
        }
        #endregion

        #region Operation Helpers
        private Int64 GetInputParameter(int paramNo, ParamModes mode)
        {
            Int64 addressValue = GetAddressValueSafe(_pointer + paramNo);

            switch (mode)
            {
                case ParamModes.Position:
                    return GetAddressValueSafe(addressValue);
                case ParamModes.Immediate:
                    return addressValue;
                case ParamModes.Relative:
                    return GetAddressValueSafe(addressValue + _relativeBase);
            }

            throw new NotImplementedException($"Mode {mode} not implemented.");
        }

        private Int64 GetOutputPosition(int paramNo, ParamModes mode)
        {
            Int64 addressValue = _pointer + paramNo;

            switch (mode)
            {
                case ParamModes.Position:
                    return GetAddressValueSafe(addressValue);
                case ParamModes.Immediate:
                    return addressValue;
                case ParamModes.Relative:
                    return GetAddressValueSafe(addressValue) + _relativeBase;
            }

            throw new NotImplementedException($"Mode {mode} not implemented.");
        } 
        #endregion

        #region IO Interactions
        private long ReadInput()
        {
            try
            {
                return Int64.Parse(ProgramIO.ReadLineInput());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void WriteOutput(string value)
        {
            ProgramIO.WriteLineOutput(value);
        }
        #endregion


        private void SetAddressValueSafe(Int64 address, Int64 value)
        {
            VerifyProgramSize(address);
            _programData[address] = value;
        }
        /// <summary>
        /// Gets the address value for the programData. If the address value is outside the 
        /// programData range then it will resize the data range
        /// </summary>
        /// <param name="addressValue"></param>
        /// <returns></returns>
        private Int64 GetAddressValueSafe(Int64 addressValue)
        {
            VerifyProgramSize(addressValue);
            return _programData[addressValue];
        }
        private void VerifyProgramSize(Int64 address)
        {
            if (address >= _programData.Length)
            {
                ResizeProgramData(address + 1);
            }
        }
        private void ResizeProgramData(Int64 newSize)
        {
            Int64[] newData = new Int64[newSize];
            _programData.CopyTo(newData, 0);
            _programData = newData;
        }
    }

    enum ParamModes
    {
        Position,
        Immediate,
        Relative
    }
}
