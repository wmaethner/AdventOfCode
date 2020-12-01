using AdventOfCode2019.HelperClasses;
using AdventOfCode2019.Intcode_Computer;
using Algorithms4DataStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace AdventOfCode2019
{
    class Program
    {
        static Stopwatch stopwatch = new Stopwatch();
        static void Main(string[] args)
        {
            PerformChallenge();
            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed.Hours}:{stopwatch.Elapsed.Minutes}:{stopwatch.Elapsed.Seconds}:{stopwatch.Elapsed.Milliseconds}");
            Console.Read();
        }

        private static void PerformChallenge()
        {
            MethodInfo dayMethod = ChooseDay();

            stopwatch.Start();
            dayMethod.Invoke(new Program(), new object[0]);
            stopwatch.Stop();
        }

        private static MethodInfo ChooseDay()
        {
            bool found = false;
            MethodInfo dayMethod = null;
            while (!found)
            {
                Console.Write("Day: ");
                string day = Console.ReadLine();
                dayMethod = (new Program()).GetType().GetMethod($"Day{day}", BindingFlags.NonPublic | BindingFlags.Static);
                if (dayMethod == null)
                {
                    Console.WriteLine($"No method created for day {day}");
                }
                else
                {
                    found = true;
                }
            }
            return dayMethod;
        }

        #region Day Challenges

        #region Day 1
        private static void Day1()
        {
            string[] fileContents = HelperFunctions.FileContents(1, 1);

            #region Part 1
            int totalFuel = 0;

            foreach (string mass in fileContents)
            {
                int massInt = Int32.Parse(mass);
                totalFuel += (massInt / 3) - 2;
            }
            Console.WriteLine($"Part 1: Total fuel {totalFuel}");
            #endregion

            #region Part 2
            totalFuel = 0;
            foreach (string mass in fileContents)
            {
                int massInt = Int32.Parse(mass);
                totalFuel += AddFuelRecurse(massInt);
            }

            Console.WriteLine($"Part 2: Total fuel {totalFuel}");
            #endregion
        }
        private static int AddFuelRecurse(int mass)
        {
            int fuel = (mass / 3) - 2;
            if (fuel >= 0)
            {
                fuel += AddFuelRecurse(fuel);
                return fuel;
            }
            return 0;
        }
        #endregion

        #region Day 2
        private static void Day2()
        {
            string[] fileContents = HelperFunctions.FileContents(2, 1);
            string code = fileContents[0];
            //string code = "1,1,1,4,99,5,6,0,99";
            string programKey = "Day2";

            #region Part 1
            IntcodeComputer computer = new IntcodeComputer();
            computer.AddProgram(programKey, code);
            Int64[] programData = computer.ProgramData(programKey);
            //programData[1] = 12;
            //programData[2] = 2;
            computer.SetProgramAddressValue(programKey, 1, 12);
            computer.SetProgramAddressValue(programKey, 2, 2);
            //computer.SetProgramData(programKey, programData);

            computer.RunProgram(programKey);

            programData = computer.ProgramData(programKey);
            Console.WriteLine($"Position 0 value: {programData[0]}");
            #endregion

            #region Part 2
            int desiredOutput = 19690720;

            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    computer.ResetProgramData(programKey);

                    computer.SetProgramAddressValue(programKey, 1, noun);
                    computer.SetProgramAddressValue(programKey, 2, verb);

                    //programData = computer.ProgramData(programKey);
                    //programData[1] = noun;
                    //programData[2] = verb;

                    computer.RunProgram(programKey);

                    programData = computer.ProgramData(programKey);
                    if (programData[0] == desiredOutput)
                    {
                        Console.WriteLine($"Noun={noun} and verb={verb}. Value={100 * noun + verb}");
                        return;
                    }
                }
            }

            #endregion
        }
        #endregion

        #region Day 3
        private static void Day3()
        {
            string[] fileContents = HelperFunctions.FileContents(3, 1);

            #region Part 1
            Grid<int> grid = new Grid<int>(Int32.MaxValue);
            List<(int, int)> crosses = new List<(int, int)>();

            for (int wire = 0; wire < fileContents.Length; wire++)
            {
                string[] wireInstructions = fileContents[wire].Split(',');
                Digraph digraph = new Digraph(5);
                AddWire(wire, wireInstructions, ref grid, ref crosses, ref digraph);
            }

            int minDistance = Int32.MaxValue;
            foreach ((int, int) pos in crosses)
            {
                if (HelperFunctions.ManhattanDistance(0, 0, pos.Item1, pos.Item2) < minDistance)
                {
                    minDistance = HelperFunctions.ManhattanDistance(0, 0, pos.Item1, pos.Item2);
                }
            }

            Console.WriteLine($"Minimum distance to cross: {minDistance}");
            #endregion

            #region Part 2




            #endregion
        }
        private static void AddWire(int wire, string[] wireInstructions, ref Grid<int> grid, ref List<(int, int)> crosses, ref Digraph wireDigraph)
        {
            int x = 0, y = 0;
            int CROSS = -1;
            foreach (string instruction in wireInstructions)
            {
                string direction = instruction.Substring(0, 1);
                int distance = Int32.Parse(instruction.Substring(1));

                for (int i = 0; i < distance; i++)
                {
                    switch (direction)
                    {
                        case "U":
                            y++;
                            break;
                        case "D":
                            y--;
                            break;
                        case "L":
                            x--;
                            break;
                        case "R":
                            x++;
                            break;
                        default:
                            break;
                    }

                    // Don't worry about origin
                    if ((x == 0) && (y == 0))
                    {
                        Console.WriteLine($"x and y are 0");
                    }
                    else
                    {
                        if (grid.PointSet(x, y) && (grid[x, y] != wire))
                        {
                            grid[x, y] = CROSS;
                            crosses.Add((x, y));
                        }
                        else
                        {
                            grid[x, y] = wire;
                        }
                    }
                }
            }
        }
        private static void AddWirePaths(string[] newWireInstructions, ref List<PathNode> wireRoots, ref List<PathNode> crosses)
        {
            int x = 0, y = 0;
            PathNode root = new PathNode()
            {
                X = x,
                Y = y
            };

            PathNode currentNode = root;

            foreach (string instruction in newWireInstructions)
            {
                string direction = instruction.Substring(0, 1);
                int distance = Int32.Parse(instruction.Substring(1));

                for (int i = 0; i < distance; i++)
                {
                    switch (direction)
                    {
                        case "U":
                            y++;
                            break;
                        case "D":
                            y--;
                            break;
                        case "L":
                            x--;
                            break;
                        case "R":
                            x++;
                            break;
                        default:
                            break;
                    }



                    // Don't worry about origin
                    if ((x == 0) && (y == 0))
                    {
                        Console.WriteLine($"x and y are 0");
                    }
                    else
                    {
                        PathNode tempNode = new PathNode()
                        {
                            X = x,
                            Y = y
                        };

                        //
                        //foreach (PathNode wireRoot in wireRoots)
                        //{
                        //    if (wireRoot.NodeOnPath(tempNode))
                        //    {
                        //        crosses.Add(tempNode);
                        //        currentNode.NextNode = tempNode;
                        //        currentNode.DistToNextNode = i + 1;
                        //        currentNode = tempNode;
                        //    }
                        //}
                    }
                }
            }


            wireRoots.Add(root);
        }
        #endregion

        #region Day 4
        private static void Day4()
        {
            //string[] fileContents = HelperFunctions.FileContents(4, 1);

            int lowerBound = 353096;
            int higherBound = 843212;

            int count = 0;
            int countPart2 = 0;
            for (int i = lowerBound; i <= higherBound; i++)
            {
                if (PassesRules(i, lowerBound, higherBound, false))
                {
                    count++;
                }
                if (PassesRules(i, lowerBound, higherBound, true))
                {
                    countPart2++;
                }
            }

            Console.WriteLine($"Valid passwords: {count}");
            Console.WriteLine($"Valid passwords part 2: {countPart2}");
        }
        private static bool PassesRules(int password, int lowerBound, int higherBound, bool onlyTwoAdjacent)
        {
            string passwordAsString = password.ToString();

            // 6-digit number
            if (passwordAsString.Length != 6)
            {
                return false;
            }

            // Within range
            if (password < lowerBound || password > higherBound)
            {
                return false;
            }

            // Two matching adjacent digits
            Dictionary<int, int> indexAndCount = new Dictionary<int, int>();
            int currIndex = 0;
            char currChar = passwordAsString[currIndex];
            indexAndCount[currIndex] = 1;

            for (int i = 1; i < passwordAsString.Length; i++)
            {
                if (passwordAsString[i] == currChar)
                {
                    indexAndCount[currIndex]++;
                }
                else
                {
                    currIndex = i;
                    currChar = passwordAsString[i];
                    indexAndCount[currIndex] = 1;
                }
            }

            bool found = false;
            foreach (int count in indexAndCount.Values)
            {
                if (count == 2)
                {
                    found = true;
                    break;
                }
                if (!onlyTwoAdjacent && count > 2)
                {
                    found = true;
                    break;
                }
            }


            //for (int i = 0; i < passwordAsString.Length - 1; i++)
            //{
            //    if (passwordAsString[i] == passwordAsString[i + 1])
            //    {
            //        found = true;
            //        break;
            //    }
            //}
            if (!found)
            {
                return false;
            }

            // No decreasing digits
            for (int i = 0; i < passwordAsString.Length - 1; i++)
            {
                if (Int32.Parse(passwordAsString[i + 1].ToString()) < Int32.Parse(passwordAsString[i].ToString()))
                {
                    return false;
                }
            }


            return true;
        }

        #endregion

        #region Day 5
        private static void Day5()
        {
            string[] fileContents = HelperFunctions.FileContents(5, 1);
            string code = fileContents[0];
            //string code = "3,3,1105,-1,9,1101,0,0,12,4,12,99,1";
            string programKey = "Day5";

            #region Part 1
            IntcodeComputer computer = new IntcodeComputer();
            ProgramIO programIO = new ProgramIO();
            programIO.OutputDataAdded += (sender, e) =>
            {
                Console.WriteLine(programIO.ReadLineOutput());
            };

            computer.AddProgram(programKey, code, programIO);

            computer.RunProgram(programKey);

            #endregion
        }
        #endregion

        #region Day 6
        private static void Day6()
        {
            string[] fileContents = HelperFunctions.FileContents(6, 1);

            Tree tree = new Tree("COM");

            foreach (string line in fileContents)
            {
                string[] planets = line.Split(")");
                tree.AddNode(planets[1], planets[0]);
            }

            string ancestor = tree.CommonAncestor("YOU", "SAN");
            int distance = (tree.NodeSize("YOU") - tree.NodeSize(ancestor)) + (tree.NodeSize("SAN") - tree.NodeSize(ancestor));

            Console.WriteLine($"Total size: {tree.TotalSize()}");
            Console.WriteLine($"Distance from YOU to SAN: {distance}");
        }
        #endregion

        #region Day 7
        private static void Day7()
        {
            string[] fileContents = HelperFunctions.FileContents(7, 1);
            string code = fileContents[0];
            //string code = "3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0";

            #region Part 1
            // Generate all phase setting permutations
            int[] nums = new int[] { 0, 1, 2, 3, 4 };
            List<List<int>> phaseSettings = new List<List<int>>();
            heapPermutation(nums, nums.Length, nums.Length, ref phaseSettings);

            //Setup computer
            char[] amps = new char[] { 'A', 'B', 'C', 'D', 'E' };
            IntcodeComputer computer = new IntcodeComputer();

            Dictionary<char, ProgramIO> programIOs = new Dictionary<char, ProgramIO>();


            foreach (char key in amps)
            {
                ProgramIO io = new ProgramIO();
                programIOs.Add(key, io);
                computer.AddProgram(key.ToString(), code, io);
            }

            int maxOutput = 0;
            foreach (List<int> phaseList in phaseSettings)
            {
                int input = 0;
                for (int phasePos = 0; phasePos < amps.Length; phasePos++)
                {
                    char key = amps[phasePos];
                    computer.ResetProgramData(key.ToString());
                    programIOs[key].WriteLineInput(phaseList[phasePos].ToString()); //First add phase settings
                    programIOs[key].WriteLineInput(input.ToString());   //Then add input

                    computer.RunProgram(key.ToString());

                    input = Int32.Parse(programIOs[key].ReadLineOutput());
                }
                if (input > maxOutput)
                {
                    maxOutput = input;
                }
            }

            Console.WriteLine($"Max thruster signal: {maxOutput}");
            #endregion

            #region Part 2
            // Generate all phase setting permutations
            nums = new int[] { 5, 6, 7, 8, 9 };
            phaseSettings = new List<List<int>>();
            heapPermutation(nums, nums.Length, nums.Length, ref phaseSettings);

            //Setup computer
            computer = new IntcodeComputer();
            programIOs = new Dictionary<char, ProgramIO>();

            foreach (char key in amps)
            {
                ProgramIO io = new ProgramIO();
                programIOs.Add(key, io);
                computer.AddProgram(key.ToString(), code, io);
            }

            maxOutput = 0;
            foreach (List<int> phaseList in phaseSettings)
            {
                //First add phase settings
                for (int phasePos = 0; phasePos < amps.Length; phasePos++)
                {
                    char key = amps[phasePos];
                    computer.ResetProgramData(key.ToString());
                    programIOs[key].WriteLineInput(phaseList[phasePos].ToString()); //First add phase settings
                }

                int currentProgIndex = 0;
                int nextInput = 0;
                string currentProgram = amps[0].ToString();
                programIOs[amps[currentProgIndex]].WriteLineInput(nextInput.ToString());

                // Run until final program halts
                while (computer.GetProgramState(amps[amps.Count() - 1].ToString()) != ProgramState.Halted)
                {
                    //Run current program
                    computer.RunProgram(amps[currentProgIndex].ToString());

                    //Get output and add to next programs input
                    nextInput = Int32.Parse(programIOs[amps[currentProgIndex]].ReadLineOutput());

                    // Get next program
                    currentProgIndex = HelperFunctions.GetNextIndex(currentProgIndex, amps.Count());

                    //Add nextInput to next program
                    programIOs[amps[currentProgIndex]].WriteLineInput(nextInput.ToString());
                }

                //Done running programs check final output
                if (nextInput > maxOutput)
                {
                    maxOutput = nextInput;
                }
            }

            Console.WriteLine($"Max thruster signal: {maxOutput}");
            #endregion
        }

        static void heapPermutation(int[] a, int size, int n, ref List<List<int>> permuteList)
        {
            // if size becomes 1 then prints the obtained 
            // permutation 
            if (size == 1)
            {
                permuteList.Add(a.ToList());
            }

            for (int i = 0; i < size; i++)
            {
                heapPermutation(a, size - 1, n, ref permuteList);

                // if size is odd, swap first and last 
                // element 
                if (size % 2 == 1)
                {
                    int temp = a[0];
                    a[0] = a[size - 1];
                    a[size - 1] = temp;
                }

                // If size is even, swap ith and last 
                // element 
                else
                {
                    int temp = a[i];
                    a[i] = a[size - 1];
                    a[size - 1] = temp;
                }
            }
        }
        #endregion

        #region Day 8
        private static void Day8()
        {
            string[] fileContents = HelperFunctions.FileContents(8, 1);
            string data = fileContents[0];

            int width = 25, height = 6;

            List<string> layers = HelperFunctions.SplitStringBySize(data, width * height).ToList();
            //List<string> layers = HelperFunctions.SplitStringBySize(data, width * height).ToList();

            int zeroLayer = 0;
            int minZeros = int.MaxValue;

            for (int i = 0; i < layers.Count; i++)
            {
                int zeros = 0;
                foreach (char c in layers[i])
                {
                    if (c == '0')
                    {
                        zeros++;
                    }
                }
                if (zeros < minZeros)
                {
                    minZeros = zeros;
                    zeroLayer = i;
                }
            }

            int oneCount = 0, twoCount = 0;
            foreach (char c in layers[zeroLayer])
            {
                if (c == '1')
                {
                    oneCount++;
                }
                if (c == '2')
                {
                    twoCount++;
                }
            }

            Console.WriteLine($"Result: {oneCount * twoCount}");

            //Initialize the layers into grids
            List<Grid<int>> grids = new List<Grid<int>>();
            foreach (string layer in layers)
            {
                Grid<int> grid = new Grid<int>(-1);

                List<string> rows = HelperFunctions.SplitStringBySize(layer, width).ToList();

                for (int row = 0; row < rows.Count; row++)
                {
                    for (int col = 0; col < rows[row].Length; col++)
                    {
                        grid[col, row] = Int32.Parse(rows[row][col].ToString());
                    }
                }

                grids.Add(grid);
            }

            //Create final layer from grids
            Grid<int> finalGrid = new Grid<int>();
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int val = 2, layer = 0;
                    while (val == 2)
                    {
                        val = grids[layer++][col, row];
                    }
                    finalGrid[col, row] = val;
                }
            }

            finalGrid.DisplayGridFlipped(new Dictionary<char, char>() { { '0', '.' }, { '1', 'X' } });
        }
        #endregion

        #region Day 9
        private static void Day9()
        {
            string[] fileContents = HelperFunctions.FileContents(9, 1);
            string code = fileContents[0];
            //string code = "109,-1,203,6,104,1,99";
            string programKey = "Day9";

            #region Part 1
            IntcodeComputer computer = new IntcodeComputer();
            ProgramIO programIO = new ProgramIO();
            programIO.WriteLineInput("2");
            programIO.OutputDataAdded += (sender, e) =>
            {
                Console.WriteLine(programIO.ReadLineOutput());
            };

            computer.AddProgram(programKey, code, programIO);

            computer.RunProgram(programKey);
            #endregion
        }
        #endregion

        #region Day 10
        private static void Day10()
        {
            string[] fileContents = HelperFunctions.FileContents(10, 1);

        
            //Look back at Algorithms I, the 3 collinear points assignment
            // Should be able to count the number of asteroids then use the collinear aspect of the points
            // to subtract ones that would be blocked.
        }
        #endregion

        #region Day 11
        private static void Day11()
        {
            string[] fileContents = HelperFunctions.FileContents(11, 1);
            string code = fileContents[0];
            string key = "Day11";

            //Grid setup
            Grid<int> grid = new Grid<int>(0);
            int curX = 0, curY = 0;
            int direction = 0;  //0=up, 1=right, 2=down, 3=left
            int panelsPainted = 0;
            grid[curX, curY] = 1;

            IntcodeComputer computer = new IntcodeComputer();
            ProgramIO programIO = new ProgramIO();
            computer.AddProgram(key, code, programIO);

            int color = 0;
            while (computer.GetProgramState(key) != ProgramState.Halted)
            {
                color = grid[curX, curY];
                programIO.WriteLineInput(color.ToString());

                computer.RunProgram(key);

                //Paint grid
                color = Int32.Parse(programIO.ReadLineOutput());
                if (!grid.PointSet(curX, curY))
                {
                    panelsPainted++;
                }
                grid[curX, curY] = color;

                //Move robot
                int dir = Int32.Parse(programIO.ReadLineOutput());
                direction = HelperFunctions.GetNextIndex(direction, 4, dir == 1);
                switch (direction)
                {
                    case 0:
                        curY++;
                        break;
                    case 1:
                        curX++;
                        break;
                    case 2:
                        curY--;
                        break;
                    case 3:
                        curX--;
                        break;
                }
            }

            Console.WriteLine($"Panels painted: {panelsPainted}");

            grid.DisplayGrid();
        }
        #endregion

        #region Day 12
        private static void Day12()
        {
            string[] fileContents = HelperFunctions.FileContents(12, 1);

            //Initialize moons
            List<Point3D> moonPositions = new List<Point3D>();
            List<Point3D> moonVelocities = new List<Point3D>();
            foreach (string moonData in fileContents)
            {
                string[] points = moonData.Substring(1, moonData.Length - 2).Split(',').Select(x => x.Trim()).ToArray();
                Point3D position = new Point3D()
                {
                    X = Int32.Parse(points[0].Split('=')[1]),
                    Y = Int32.Parse(points[1].Split('=')[1]),
                    Z = Int32.Parse(points[2].Split('=')[1])
                };
                moonPositions.Add(position);
                moonVelocities.Add(new Point3D());
            }


            //Loop a certain number of steps
            for (int step = 0; step < 1000; step++)
            {
                //First update velocities
                for (int moon1 = 0; moon1 < moonPositions.Count - 1; moon1++)
                {
                    for (int moon2 = moon1 + 1; moon2 < moonPositions.Count; moon2++)
                    {
                        if (moonPositions[moon1].X > moonPositions[moon2].X)
                        {
                            moonVelocities[moon1].X--;
                            moonVelocities[moon2].X++;
                        }
                        else if (moonPositions[moon1].X < moonPositions[moon2].X)
                        {
                            moonVelocities[moon1].X++;
                            moonVelocities[moon2].X--;
                        }

                        if (moonPositions[moon1].Y > moonPositions[moon2].Y)
                        {
                            moonVelocities[moon1].Y--;
                            moonVelocities[moon2].Y++;
                        }
                        else if (moonPositions[moon1].Y < moonPositions[moon2].Y)
                        {
                            moonVelocities[moon1].Y++;
                            moonVelocities[moon2].Y--;
                        }

                        if (moonPositions[moon1].Z > moonPositions[moon2].Z)
                        {
                            moonVelocities[moon1].Z--;
                            moonVelocities[moon2].Z++;
                        }
                        else if (moonPositions[moon1].Z < moonPositions[moon2].Z)
                        {
                            moonVelocities[moon1].Z++;
                            moonVelocities[moon2].Z--;
                        }
                    }
                }

                //Now apply the velocities
                for (int moon = 0; moon < moonPositions.Count; moon++)
                {
                    moonPositions[moon].X += moonVelocities[moon].X;
                    moonPositions[moon].Y += moonVelocities[moon].Y;
                    moonPositions[moon].Z += moonVelocities[moon].Z;
                }
            }

            //Calculate energies
            int totalEnergy = 0;
            for (int moon = 0; moon < moonPositions.Count; moon++)
            {
                int potential = Math.Abs(moonPositions[moon].X) + Math.Abs(moonPositions[moon].Y) + Math.Abs(moonPositions[moon].Z);
                int kinetic = Math.Abs(moonVelocities[moon].X) + Math.Abs(moonVelocities[moon].Y) + Math.Abs(moonVelocities[moon].Z);
                totalEnergy += potential * kinetic;
            }

            Console.WriteLine($"Total energy: {totalEnergy}");
        }
        #endregion

        #region Day 13
        private static void Day13()
        {
            string[] fileContents = HelperFunctions.FileContents(13, 1);
            string code = fileContents[0];
            string key = "Day13";

            //Grid setup
            Grid<int> grid = new Grid<int>(0);
            int blockTiles = 0;
            int score = 0;

            IntcodeComputer computer = new IntcodeComputer();
            ProgramIO programIO = new ProgramIO();
            computer.AddProgram(key, code, programIO);

            bool playAgain = true;
            while (playAgain)
            {
                computer.ResetProgramData(key);
                computer.SetProgramAddressValue(key, 0, 2); //Play for FREE!!!
                score = 0;
                while (computer.GetProgramState(key) != ProgramState.Halted)
                {
                    computer.RunProgram(key);

                    int paddleX = 0, ballX = 0;
                    while (programIO.HasOutput())
                    {
                        int xPos = programIO.ReadLineOutputInt();
                        int yPos = programIO.ReadLineOutputInt();
                        int id = programIO.ReadLineOutputInt();

                        if (xPos == -1 && yPos == 0)
                        {
                            score = id;
                        }
                        else
                        {
                            grid[xPos, yPos] = id;
                            if (id == 3)
                            {
                                paddleX = xPos;
                            }
                            else if (id == 4)
                            {
                                ballX = xPos;
                            }
                        }
                    }

                    //Console.Clear();
                    //grid.DisplayGrid(new Dictionary<char, char>() { { '0', ' ' }, { '1', '|' }, { '2', 'B' }, { '3', '_' }, { '4', 'O' } });
                    //Console.WriteLine($"Score: {score}");

                    //Thread.Sleep(50);

                    int input = ballX.CompareTo(paddleX);
                    programIO.WriteLineInput(input.ToString());

                    //bool gotKey = false;
                    //while (!gotKey)
                    //{
                    //    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    //    if (keyInfo.Key == ConsoleKey.LeftArrow)
                    //    {
                    //        programIO.WriteLineInput("-1");
                    //        gotKey = true;
                    //    }
                    //    else if (keyInfo.Key == ConsoleKey.RightArrow)
                    //    {
                    //        programIO.WriteLineInput("1");
                    //        gotKey = true;
                    //    }
                    //    else if (keyInfo.Key == ConsoleKey.UpArrow)
                    //    {
                    //        programIO.WriteLineInput("0");
                    //        gotKey = true;
                    //    }
                    //}
                }

                Console.WriteLine($"Score: {score}");

                Console.Write("Play again (Y/N)? ");
                ConsoleKeyInfo again = Console.ReadKey();
                playAgain = again.Key == ConsoleKey.Y;
            }


        }
        #endregion

        #region Day 14

        #endregion

        #region Day 15
        private static void Day15()
        {
            string[] fileContents = HelperFunctions.FileContents(15, 1);
            string code = fileContents[0];
            string key = "Day15";


            //List<Point> visited = new List<Point>();
            //List<Point> wall = new List<Point>();

            //Dictionary<Point, bool> marked = new Dictionary<Point, bool>();
            //Dictionary<Point, bool> walls = new Dictionary<Point, bool>();
            //Dictionary<Point, Point> pathTo = new Dictionary<Point, Point>();           
            //marked.Add(source, true);

            Digraph digraph = new Digraph(1000);    //random high number
            Dictionary<int, Point> points = new Dictionary<int, Point>();
            Dictionary<Point, int> pointMap = new Dictionary<Point, int>();
            Queue<Point> availablePoints = new Queue<Point>();
            Grid<int> grid = new Grid<int>(5);

            Point source = new Point(0, 0);
            grid[0, 0] = 1;
            Point? oxygenSystem = null;
            Point currentPoint = source;

            Stack<Point> path = new Stack<Point>();
            //path.Push(currentPoint);

            availablePoints.Enqueue(source);
            int pointCount = 0;
            points.Add(pointCount, source);
            pointMap.Add(source, pointCount);

            IntcodeComputer computer = new IntcodeComputer();
            ProgramIO programIO = new ProgramIO();
            computer.AddProgram(key, code, programIO);
            //computer.RunProgram(key);
            bool finished = false;

            //int curX = 0, curY = 0;
            int distTraveled = 0;

            while (!finished)
            {
                Point nextPoint = new Point();
                // Get next point by looking for points that haven't been visited
                // Order to check N -> E -> S -> W
                if (grid[currentPoint.X, currentPoint.Y + 1] == 5)
                {
                    nextPoint.X = currentPoint.X;
                    nextPoint.Y = currentPoint.Y + 1;
                }
                else if (grid[currentPoint.X + 1, currentPoint.Y] == 5)
                {
                    nextPoint.X = currentPoint.X + 1;
                    nextPoint.Y = currentPoint.Y;
                }
                else if (grid[currentPoint.X, currentPoint.Y - 1] == 5)
                {
                    nextPoint.X = currentPoint.X;
                    nextPoint.Y = currentPoint.Y - 1;
                }
                else if (grid[currentPoint.X - 1, currentPoint.Y] == 5)
                {
                    nextPoint.X = currentPoint.X - 1;
                    nextPoint.Y = currentPoint.Y;
                }
                else
                {
                    // Hit a dead end back up a spot
                    nextPoint = path.Pop();
                }


                programIO.WriteLineInput(GetNextPointInput(currentPoint, nextPoint).ToString());
                computer.RunProgram(key);
                int result = programIO.ReadLineOutputInt();              
                if (result == 1)
                {
                    // Only add to path if it is a new point
                    if (grid[nextPoint.X,nextPoint.Y] == 5)
                    {
                        path.Push(currentPoint);
                    }                    
                    currentPoint = nextPoint;                   
                    distTraveled++;
                }
                else if (result == 2)
                {
                    path.Push(nextPoint);
                    currentPoint = nextPoint;
                    distTraveled++;
                    finished = true;
                }

                grid[nextPoint.X, nextPoint.Y] = result;

                if (distTraveled % 10 == 0)
                {
                    grid.DisplayGrid(new Dictionary<char, char>() { { '5', '.' } });
                    Console.WriteLine();
                }
            }

            Console.WriteLine($"distance traveled: {path.Count}");

            //int curX = 0, curY = 0;
            //int nextX = 0, nextY = 0;

            //while (!finished)
            //{
            //    nextX = curX;
            //    nextY = curY;

            //    Console.Clear();
            //    grid.DisplayGrid();
            //    Console.WriteLine();

            //    bool gotKey = false;
            //    while (!gotKey)
            //    {
            //        ConsoleKeyInfo keyInfo = Console.ReadKey();
            //        if (keyInfo.Key == ConsoleKey.LeftArrow)
            //        {
            //            nextX--;
            //            programIO.WriteLineInput("3");
            //            gotKey = true;
            //        }
            //        else if (keyInfo.Key == ConsoleKey.RightArrow)
            //        {
            //            nextX++;
            //            programIO.WriteLineInput("4");
            //            gotKey = true;
            //        }
            //        else if (keyInfo.Key == ConsoleKey.UpArrow)
            //        {
            //            nextY++;
            //            programIO.WriteLineInput("1");
            //            gotKey = true;
            //        }
            //        else if (keyInfo.Key == ConsoleKey.DownArrow)
            //        {
            //            nextY--;
            //            programIO.WriteLineInput("2");
            //            gotKey = true;
            //        }
            //        else if (keyInfo.Key == ConsoleKey.Enter)
            //        {
            //            finished = true;
            //            gotKey = true;
            //        }
            //    }

            //    computer.RunProgram(key);
            //    int result = programIO.ReadLineOutputInt();
            //    if (result == 0)
            //    {
            //        grid[nextX, nextY] = 0;
            //        //grid[nextPoint.X, nextPoint.Y] = 0;
            //        //walls[next] = true;
            //        //marked[next] = true;
            //    }
            //    else if (result == 1)
            //    {
            //        //marked[next] = true;
            //        //pathTo[next] = currentPoint;
            //        grid[nextX, nextY] = 1;
            //        curX = nextX;
            //        curY = nextY;
            //    }
            //    else if (result == 2)
            //    {
            //        grid[nextX, nextY] = 2;
            //        curX = nextX;
            //        curY = nextY;
            //        //pathTo[next] = currentPoint;
            //        //currentPoint = next;
            //    }
            //}


            //while (oxygenSystem == null)
            //{
            //    //Return to source
            //    if (currentPoint != source)
            //    {
            //        BreadthFirstDirectedPaths bfsToSource = new BreadthFirstDirectedPaths(digraph.Reverse(), pointMap[currentPoint]);
            //        foreach (int pointIndex in bfsToSource.PathTo(pointMap[source]))
            //        {
            //            Point next = points[pointIndex];
            //            programIO.WriteLineInput(GetNextPointInput(currentPoint, next).ToString());
            //            computer.RunProgram(key);
            //            programIO.ReadLineOutputInt(); //Ignore output on way back to source
            //            currentPoint = next;
            //        }
            //    }

            //    //Dequeue next point
            //    Point nextPoint = availablePoints.Dequeue();

            //    //Travel to next point
            //    if (currentPoint != nextPoint)
            //    {
            //        BreadthFirstDirectedPaths bfsToPoint = new BreadthFirstDirectedPaths(digraph, pointMap[currentPoint]);
            //        foreach (int pointIndex in bfsToPoint.PathTo(pointMap[nextPoint]))
            //        {
            //            Point next = points[pointIndex];
            //            programIO.WriteLineInput(GetNextPointInput(currentPoint, next).ToString());
            //            computer.RunProgram(key);

            //            //If the next point is the one we're traveling to then check the return value
            //            if (next == nextPoint)
            //            {
            //                int result = programIO.ReadLineOutputInt();
            //                if (result == 0)
            //                {
            //                    grid[nextPoint.X, nextPoint.Y] = 0;
            //                    //walls[next] = true;
            //                    //marked[next] = true;
            //                }
            //                else if (result == 1)
            //                {
            //                    //marked[next] = true;
            //                    //pathTo[next] = currentPoint;
            //                    grid[nextPoint.X, nextPoint.Y] = 1;
            //                    currentPoint = next;
            //                }
            //                else if (result == 2)
            //                {
            //                    grid[nextPoint.X, nextPoint.Y] = 2;
            //                    oxygenSystem = next;
            //                    //pathTo[next] = currentPoint;
            //                    currentPoint = next;
            //                }
            //            }
            //            else
            //            {
            //                programIO.ReadLineOutputInt();
            //                currentPoint = next;
            //            }
            //        }
            //    }

            //    grid[currentPoint.X, currentPoint.Y] = 4;
            //    grid[source.X, source.Y] = 3;
            //    grid.DisplayGrid(new Dictionary<char, char> { { '1', ' ' } });
            //    grid[currentPoint.X, currentPoint.Y] = 1;
            //    Console.WriteLine();

            //    //Create new points and add edges
            //    //BreadthFirstDirectedPaths bfs = new BreadthFirstDirectedPaths(digraph, pointMap[source]);

            //    Point above = new Point(currentPoint.X, currentPoint.Y + 1);
            //    Point right = new Point(currentPoint.X + 1, currentPoint.Y);
            //    Point below = new Point(currentPoint.X, currentPoint.Y - 1);
            //    Point left = new Point(currentPoint.X - 1, currentPoint.Y);

            //    if (!pointMap.ContainsKey(above))
            //    {
            //        pointCount++;
            //        pointMap.Add(above, pointCount);
            //        points.Add(pointCount, above);
            //        digraph.AddEdge(pointMap[currentPoint], pointMap[above]);
            //        availablePoints.Enqueue(above);
            //    }
            //    if (!pointMap.ContainsKey(right))
            //    {
            //        pointCount++;
            //        pointMap.Add(right, pointCount);
            //        points.Add(pointCount, right);
            //        digraph.AddEdge(pointMap[currentPoint], pointMap[right]);
            //        availablePoints.Enqueue(right);
            //    }
            //    if (!pointMap.ContainsKey(below))
            //    {
            //        pointCount++;
            //        pointMap.Add(below, pointCount);
            //        points.Add(pointCount, below);
            //        digraph.AddEdge(pointMap[currentPoint], pointMap[below]);
            //        availablePoints.Enqueue(below);
            //    }
            //    if (!pointMap.ContainsKey(left))
            //    {
            //        pointCount++;
            //        pointMap.Add(left, pointCount);
            //        points.Add(pointCount, left);
            //        digraph.AddEdge(pointMap[currentPoint], pointMap[left]);
            //        availablePoints.Enqueue(left);
            //    }

            //}

            //BreadthFirstDirectedPaths bfsToOx = new BreadthFirstDirectedPaths(digraph, pointMap[source]);
            //Console.WriteLine($"Distance to oxygen: {bfsToOx.DistanceTo(pointMap[(Point)oxygenSystem])}");
        }
        private static List<Point> Path(Point from, Point to, Dictionary<Point, Point> pathTos, Point source)
        {
            if (from != source && to != source)
            {
                throw new ArgumentException("From or To must be the source");
            }

            if (from == source)
            {
                Stack<Point> stackPath = new Stack<Point>();
                Point temp = to;
                while (temp != source)
                {
                    stackPath.Push(temp);
                    temp = pathTos[temp];
                }
                return stackPath.ToList();
            }
            else
            {
                Queue<Point> queuePath = new Queue<Point>();
                Point temp = to;
                while (temp != source)
                {
                    queuePath.Enqueue(temp);
                    temp = pathTos[temp];
                }
                return queuePath.ToList();
            }
        }

        private static int TravelPath(List<Point> path, IntcodeComputer computer, string key, ProgramIO programIO, Point current, ref Dictionary<Point, Point> pathTos)
        {
            string output = "";
            foreach (Point nextPoint in path)
            {
                programIO.WriteLineInput(GetNextPointInput(current, nextPoint).ToString());
                computer.RunProgram(key);
                output = programIO.ReadLineOutput(); //No need to check output since this is a known open spot
                if (Int32.Parse(output) == 1)
                {
                    pathTos[nextPoint] = current;
                }
                current = nextPoint;
            }
            return Int32.Parse(output); //Return final output, matters when traveling to a point to check if oxygen system
        }

        private static void AddSurroundingPoints(Point currentPoint, ref Queue<Point> queue, List<Point> visited)
        {
            Point nextPoint;

            //North
            nextPoint = new Point(currentPoint.X, currentPoint.Y + 1);
            if (!visited.Contains(nextPoint))
            {
                queue.Enqueue(nextPoint);
            }
            //South
            nextPoint = new Point(currentPoint.X, currentPoint.Y - 1);
            if (!visited.Contains(nextPoint))
            {
                queue.Enqueue(nextPoint);
            }
            //East
            nextPoint = new Point(currentPoint.X + 1, currentPoint.Y);
            if (!visited.Contains(nextPoint))
            {
                queue.Enqueue(nextPoint);
            }
            //West
            nextPoint = new Point(currentPoint.X - 1, currentPoint.Y);
            if (!visited.Contains(nextPoint))
            {
                queue.Enqueue(nextPoint);
            }
        }

        private static int GetNextPointInput(Point currentPoint, Point nextPoint)
        {
            if (currentPoint.X == nextPoint.X)
            {
                return (currentPoint.Y > nextPoint.Y) ? 2 : 1;
            }
            else
            {
                return (currentPoint.X > nextPoint.X) ? 3 : 4;
            }
        }
        #endregion

        #region Day 16

        #endregion

        #region Day 17
        private static void Day17()
        {
            string[] fileContents = HelperFunctions.FileContents(17, 1);
            string code = fileContents[0];
            string key = "Day17";

            Grid<char> grid = new Grid<char>();

            IntcodeComputer computer = new IntcodeComputer();
            ProgramIO programIO = new ProgramIO();
            computer.AddProgram(key, code, programIO);
            computer.RunProgram(key);

            int curX = 0, curY = 0;

            while (programIO.HasOutput())
            {
                int result = programIO.ReadLineOutputInt();
                if (result == 10)
                {
                    curY--;
                    curX = 0;
                }
                else
                {
                    grid[curX, curY] = (char)result;
                    curX++;
                }
            }

            grid.DisplayGrid();

            int sum = 0;
            char pathway = '#';
            for (int x = grid.MinX(); x <= grid.MaxX(); x++)
            {
                for (int y = grid.MaxY(); y >= grid.MinY(); y--)
                {
                    // Check for intersection

                    try
                    {
                        if (grid[x, y + 1] == pathway && grid[x, y - 1] == pathway &&
                            grid[x + 1, y] == pathway && grid[x - 1, y] == pathway)
                        {
                            // Get distances
                            // Add to sum
                            sum += x * Math.Abs(y);
                        }
                    }
                    catch (Exception)
                    {
                        //Catch index out of range exception
                    }
                }
            }

            Console.WriteLine($"Sum of intersections: {sum}");

        }
        #endregion

        #region Day 18

        #endregion

        #region Day 19
        private static void Day19()
        {
            string[] fileContents = HelperFunctions.FileContents(19, 1);
            string code = fileContents[0];
            string key = "Day19";

            Grid<int> grid = new Grid<int>(-1);

            IntcodeComputer computer = new IntcodeComputer();
            ProgramIO programIO = new ProgramIO();
            computer.AddProgram(key, code, programIO);

            int gridSize = 100;
            int counter = 0;

            for (int y = 0; y < gridSize; y++)
            {
                bool hitRay = false;
                for (int x = 0; x < gridSize; x++)
                {
                    // Grid spot already set
                    if (grid[x, y] != -1)
                    {
                        continue;
                    }

                    if (hitRay && grid[x - 1, y] == 0)
                    {
                        grid[x, y] = 0;
                        continue;
                    }

                    computer.ResetProgramData(key);

                    programIO.WriteLineInput(x.ToString());
                    programIO.WriteLineInput(y.ToString());

                    computer.RunProgram(key);

                    int result = programIO.ReadLineOutputInt();
                    if (result == 1)
                    {
                        // If this is the first ray of the line then everything to the left
                        // and below should be empty
                        if (!hitRay && x > 0)
                        {
                            for (int tempy = y; tempy < gridSize; tempy++)
                            {
                                for (int tempx = x - 1; tempx >= 0; tempx--)
                                {
                                    grid[tempx, tempy] = 0;
                                }
                            }

                            //for (int tempY = x; tempY >= 0; tempY--)
                            //{
                            //    for (int tempX = 0; tempX >= 0; tempX--)
                            //    {
                            //        grid[tempX, tempY] = 0;
                            //    }
                            //}

                            hitRay = true;
                        }

                        counter++;
                    }
                    grid[x, y] = result;
                }
            }

            //for (int x = 0; x < 50; x++)
            //{
            //    for (int y = 0; y < 50; y++)
            //    {
            //        computer.ResetProgramData(key);

            //        programIO.WriteLineInput(x.ToString());
            //        programIO.WriteLineInput(y.ToString());

            //        computer.RunProgram(key);

            //        int result = programIO.ReadLineOutputInt();
            //        if (result == 1)
            //        {
            //            counter++;
            //        }
            //        //grid[x, y] = result;
            //    }
            //}

            grid.DisplayGrid(new Dictionary<char, char>() { { '0', '.' }, { '1', '#' } });

            Console.WriteLine($"Count: {counter}");
            //computer.RunProgram(key);

        }
        #endregion

        #region Day 23
        private static void Day23()
        {
            string[] fileContents = HelperFunctions.FileContents(23, 1);
            string code = fileContents[0];

            IntcodeComputer computer = new IntcodeComputer();

            Dictionary<int, ProgramIO> programs = new Dictionary<int, ProgramIO>();

            //Initialize programs
            for (int address = 0; address < 50; address++)
            {
                ProgramIO programIO = new ProgramIO();
                programs.Add(address, programIO);
                programIO.WriteLineInput(address.ToString());

                computer.AddProgram(address.ToString(), code, programIO);
            }

            bool finished = false;
            long natX = 0, natY = 0;
            long lastY = 0;
            while (!finished)
            {
                bool allInputsEmpty = true;
                for (int address = 0; address < 50; address++)
                {
                    ProgramIO programIO = programs[address];
                    if (!programIO.HasInput())
                    {
                        programIO.WriteLineInput("-1");
                    }
                    else
                    {
                        allInputsEmpty = false;
                    }

                    computer.RunProgram(address.ToString());

                    if (!programIO.HasOutput())
                    {
                        continue;
                    }

                    int destinationAddress = programIO.ReadLineOutputInt();
                    long destinationX = programIO.ReadLineOutputLong();
                    long destinationY = programIO.ReadLineOutputLong();

                    if (destinationAddress == 255)
                    {
                        natX = destinationX;
                        natY = destinationY;
                    }
                    else
                    {
                        ProgramIO destinationIO = programs[destinationAddress];
                        destinationIO.WriteLineInput(destinationX.ToString());
                        destinationIO.WriteLineInput(destinationY.ToString());
                    }                                    
                }

                if (allInputsEmpty)
                {
                    ProgramIO programIOForZero = programs[0];
                    programIOForZero.WriteLineInput(natX.ToString());
                    programIOForZero.WriteLineInput(natY.ToString());

                    //Console.WriteLine($"Nat writing X: {natX} and Y: {natY}");

                    if (natY == lastY)
                    {
                        break;
                    }
                    lastY = natY;
                }

            }

            //Console.WriteLine($"Final y: {finalY}");
            Console.WriteLine($"Duplicate Y: {lastY}");
        }
        #endregion

        #endregion

    }
}
