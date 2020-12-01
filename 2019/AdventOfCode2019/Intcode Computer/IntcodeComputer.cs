using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Intcode_Computer
{
    public class IntcodeComputer
    {


        Dictionary<string, ProgramData> _programData = new Dictionary<string, ProgramData>();

        public bool AddProgram(string key, string code)
        {
            return AddProgram(key, code, new ProgramIO());
        }

        public bool AddProgram(string key, string code, ProgramIO programIO)
        {
            try
            {
                string[] codeStrings = code.Split(',');
                Int64[] codes = codeStrings.Select(x => Int64.Parse(x)).ToArray();

                _programData.Add(key, new ProgramData(codes, programIO));

                return true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                return false;
            }
        }

        #region Program Operation
        public ProgramState RunProgram(string key)
        {
            ProgramData program = _programData[key];
            program.ProgramState = ProgramState.Running;

            while (program.ProgramState == ProgramState.Running)
            {
                program.RunNextInstruction();
            }
            return program.ProgramState;
        }
        #endregion

        #region Program Data
        public Int64[] ProgramData(string key)
        {
            return ProgramData(false, key);            
        }

        public Int64[] ProgramBaseData(string key)
        {
            return ProgramData(true, key);
        }

        private Int64[] ProgramData(bool baseProgram, string key)
        {
            try
            {
                return baseProgram ? _programData[key].GetBaseProgramData() : _programData[key].GetProgramData();
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public Int64 GetProgramAddressValue(string key, int address)
        {
            return _programData[key].GetAddressValue(address);
        }

        public void SetProgramAddressValue(string key, int address, int value)
        {
            _programData[key].SetAddressValue(address, value);
        }

        public void ResetProgramData(string key)
        {
            _programData[key].ResetProgram();
        }

        public ProgramState GetProgramState(string key)
        {
            return _programData[key].ProgramState;
        }
        #endregion

        #region Console Interactions
        private void LogError(Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        #endregion
    }

    public enum ProgramState
    {
        NotStarted,
        Running,
        Paused,
        Halted
    }
}
