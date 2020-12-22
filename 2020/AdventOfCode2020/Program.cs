using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using DataStructures;
using Validators;

namespace AdventOfCode2020
{
    class Program
    {
        static Stopwatch stopwatch = new Stopwatch();

        static void Main(string[] args)
        {
            PerformChallenge();
            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed.Hours}:{stopwatch.Elapsed.Minutes}:" +
                                            $"{stopwatch.Elapsed.Seconds}:{stopwatch.Elapsed.Milliseconds}");
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
            MethodInfo dayMethod = null;
            while(dayMethod == null)
            {
                Console.WriteLine("Day: ");
                string day = Console.ReadLine();
                if (day == "q")
                {
                    break;
                }
                dayMethod = new Program().GetType().GetMethod($"Day{day}", BindingFlags.NonPublic | BindingFlags.Static);
                if (dayMethod == null)
                {
                    Console.WriteLine($"No method found for day {day}");
                }
            }
            return dayMethod;
        }

        #region Challenges

        #region Day 1
        private static void Day1()
        {
            string[] fileContents = HelperFunctions.FileContents(1, 1);

            int[] values = new int[fileContents.Length];
            for (int i = 0; i < fileContents.Length; i++)
            {
                values[i] = int.Parse(fileContents[i]);
            }

            #region Part 1
            //for (int i = 0; i < values.Length - 1; i++)
            //{
            //    for (int j = i + 1; j < values.Length; j++)
            //    {
            //        if (values[i] + values[j] == 2020)
            //        {
            //            Console.WriteLine($"{values[i] * values[j]}");
            //            return;
            //        }
            //    }
            //}
            #endregion

            #region Part 2

            for (int i = 0; i < values.Length - 2; i++)
            {
                for (int j = i + 1; j < values.Length - 1; j++)
                {
                    if (values[i] + values[j] > 2020)
                    {
                        continue;
                    }

                    for (int k = j + 1; k < values.Length; k++)
                    {
                        if (values[i] + values[j] + values[k] == 2020)
                        {
                            Console.WriteLine($"{values[i] * values[j] * values[k]}");
                            return;
                        }
                    }
                }
            }
            #endregion

            Console.WriteLine("No value");
        }


        #endregion

        #region Day 2
        private static void Day2()
        {
            string[] fileContents = HelperFunctions.FileContents(2, 1);
            //string[] fileContents = { "1-3 a: abcde",
            //                          "1-3 b: cdefg",
            //                          "2-9 c: ccccccccc" };


            int count = 0;

            foreach (string combo in fileContents)
            {
                string[] parts = combo.Split(" ");

                //Get amounts
                int min = int.Parse(parts[0].Split("-")[0]);
                int max = int.Parse(parts[0].Split("-")[1]);

                //Get char
                char ch = parts[1][0];

                //Actual password
                string password = parts[2];

                #region Part 1
                ////Test password
                //int occurrences = password.Length - password.Replace(ch.ToString(), string.Empty).Length;

                //if (occurrences >= min && occurrences <= max)
                //{
                //    count++;
                //}
                #endregion

                #region Part 2
                //Test password
                int occurrences = 0;
                if (password[min - 1] == ch)
                {
                    occurrences++;
                }
                if (password[max - 1] == ch)
                {
                    occurrences++;
                }

                if (occurrences == 1)
                {
                    count++;
                }
                #endregion
            }


            Console.WriteLine($"Valid passwords: {count}");
        }
        #endregion

        #region Day 3
        private static void Day3()
        {
            string[] fileContents = HelperFunctions.FileContents(3, 1);
            //string[] fileContents = {"..##.......",
            //                        "#...#...#..",
            //                        ".#....#..#.",
            //                        "..#.#...#.#",
            //                        ".#...##..#.",
            //                        "..#.##.....",
            //                        ".#.#.#....#",
            //                        ".#........#",
            //                        "#.##...#...",
            //                        "#...##....#",
            //                        ".#..#...#.#", };


            //Variables
            int width = fileContents[0].Length;
            Int64 treeProduct = 1;
            List<Tuple<int, int>> slopes = new List<Tuple<int, int>>();
            slopes.Add(Tuple.Create<int, int>(1, 1));
            slopes.Add(Tuple.Create<int, int>(3, 1));
            slopes.Add(Tuple.Create<int, int>(5, 1));
            slopes.Add(Tuple.Create<int, int>(7, 1));
            slopes.Add(Tuple.Create<int, int>(1, 2));

            foreach (Tuple<int, int> slope in slopes)
            {
                int curX = 0, curY = 0;
                int dX = slope.Item1, dY = slope.Item2;
                int treeCount = 0;

                while (curY < fileContents.Length)
                {
                    if (fileContents[curY][curX] == '#')
                    {
                        treeCount++;
                    }

                    curX = (curX + dX) % width;
                    curY += dY;
                }

                treeProduct *= treeCount;
            }

            Console.WriteLine($"Tree product: {treeProduct}");
        }
        #endregion

        #region Day 4
        private static void Day4()
        {
            string[] fileContents = HelperFunctions.FileContents(4, 1);
            List<Passport> passports = new List<Passport>();

            List<string> passportLines = new List<string>();
            foreach (string line in fileContents)
            {
                if (string.IsNullOrEmpty(line))
                {
                    passports.Add(new Passport(passportLines));
                    passportLines.Clear();
                    continue;
                }

                passportLines.Add(line);
            }
            if (passportLines.Count > 0)
            {
                passports.Add(new Passport(passportLines));
            }

            int count = 0;
            foreach (Passport passport in passports)
            {
                if (passport.Valid())
                {
                    count++;
                }
            }

            Console.WriteLine($"Valid passports: {count}");
        }

        class Passport
        {
            #region Properties
            public string byr { get; set; }
            public string iyr { get; set; }
            public string eyr { get; set; }
            public string hgt { get; set; }
            public string hcl { get; set; }
            public string ecl { get; set; }
            public string pid { get; set; }
            public string cid { get; set; }
            #endregion

            public Passport(List<string> lines)
            {
                foreach (string line in lines)
                {
                    string[] props = line.Split(" ");
                    foreach (string prop in props)
                    {
                        string key = prop.Split(":")[0];
                        string value = prop.Split(":")[1];

                        switch (key)
                        {
                            case "byr":
                                byr = value;
                                break;
                            case "iyr":
                                iyr = value;
                                break;
                            case "eyr":
                                eyr = value;
                                break;
                            case "hgt":
                                hgt = value;
                                break;
                            case "hcl":
                                hcl = value;
                                break;
                            case "ecl":
                                ecl = value;
                                break;
                            case "pid":
                                pid = value;
                                break;
                            case "cid":
                                cid = value;
                                break;
                        }
                    }
                }
            }

            public bool Valid()
            {
                ////For now missing cid is ok
                if (string.IsNullOrEmpty(byr) ||
                    string.IsNullOrEmpty(iyr) ||
                    string.IsNullOrEmpty(eyr) ||
                    string.IsNullOrEmpty(hgt) ||
                    string.IsNullOrEmpty(hcl) ||
                    string.IsNullOrEmpty(ecl) ||
                    string.IsNullOrEmpty(pid))
                {
                    return false;
                }

                if (!ValidYear(byr, 1920, 2002) ||
                    !ValidYear(iyr, 2010, 2020) ||
                    !ValidYear(eyr, 2020, 2030) ||
                    !ValidHeight(hgt) ||
                    !ValidHairColor(hcl) ||
                    !ValidEyeColor(ecl) ||
                    !ValidPassportId(pid))
                {
                    return false;
                }

              
                return true;
            }

            private bool ValidYear(string value, int min, int max)
            {
                return value.ValidNumberRange(min, max);
            }

            private bool ValidHeight(string value)
            {
                if (value.Contains("cm"))
                {
                    return value.Replace("cm", "").ValidNumberRange(150, 193);
                }

                if (value.Contains("in"))
                {
                    return value.Replace("in", "").ValidNumberRange(59, 76);
                }

                return false;
            }

            private bool ValidHairColor(string value)
            {
                return Regex.IsMatch(value, @"#[0-9a-f]{6}");
            }

            private bool ValidEyeColor(string value)
            {
                switch (value)
                {
                    case "amb":
                    case "blu":
                    case "brn":
                    case "gry":
                    case "grn":
                    case "hzl":
                    case "oth":
                        return true;
                }

                return false;
            }

            private bool ValidPassportId(string value)
            {
                return Regex.IsMatch(value, @"^[0-9]{9}$");
            }
        }
        #endregion

        #region Day 5
        private static void Day5()
        {
            string[] fileContents = HelperFunctions.FileContents(5, 1);
            //string[] fileContents = { "FBFBBFFRLR" };

            Dictionary<int, Tuple<int, int>> seatIds = new Dictionary<int, Tuple<int, int>>();
            bool[][] seatsTaken = new bool[128][];
            for (int i = 0; i < 128; i++)
            {
                seatsTaken[i] = new bool[8];
                for (int j = 0; j < 8; j++)
                {
                    seatsTaken[i][j] = false;
                }
            }
            int maxSeatId = 0;
            foreach (string seat in fileContents)
            {
                int rowMin = 0, rowMax = 127;
                //Get row
                foreach (char ch in seat.Substring(0, 7))
                {
                    if (ch == 'F')
                    {
                        rowMax -= (int)Math.Ceiling((double)(rowMax - rowMin) / 2);
                    }
                    else
                    {
                        rowMin += (int)Math.Ceiling((double)(rowMax - rowMin) / 2);
                    }
                }

                int colMin = 0, colMax = 7;
                //Get col
                foreach (char ch in seat.Substring(7, 3))
                {
                    if (ch == 'L')
                    {
                        colMax -= (int)Math.Ceiling((double)(colMax - colMin) / 2);
                    }
                    else
                    {
                        colMin += (int)Math.Ceiling((double)(colMax - colMin) / 2);
                    }
                }

                seatsTaken[rowMin][colMin] = true;
                int seatId = rowMin * 8 + colMin;
                if (seatId > maxSeatId)
                {
                    maxSeatId = seatId;
                }

                if (rowMin > 0 && rowMin < 127)
                {
                    seatIds[seatId] = Tuple.Create<int, int>(rowMin, colMin);
                }
            }

            for (int i = 1; i < 127; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (!seatsTaken[i][j])
                    {
                        int id = i * 8 + j;
                        if (seatIds.ContainsKey(id+1) && seatIds.ContainsKey(id-1))
                        {
                            Console.WriteLine($"Row: {i} Col: {j}");
                            Console.WriteLine($"Id: {id}");
                        }
                    }
                }
            }

            //Console.WriteLine($"Max seat id: {maxSeatId}");
        }
        #endregion

        #region Day 6
        private static void Day6()
        {
            string[] fileContents = HelperFunctions.FileContents(6, 1);

            int sum = 0;
            Dictionary<char, bool> values = new Dictionary<char, bool>();
            bool first = true;

            foreach (string line in fileContents)
            {
                if (string.IsNullOrEmpty(line))
                {
                    sum += values.Count;
                    values.Clear();
                    first = true;
                    continue;
                }

                if (first)
                {
                    first = false;
                    foreach (char ch in line)
                    {
                        values[ch] = true;
                    }
                }
                else
                {
                    List<char> toRemove = new List<char>();
                    foreach (char ch in values.Keys)
                    {
                        if (!line.Contains(ch))
                        {
                            toRemove.Add(ch);
                        }
                    }
                    foreach (char ch in toRemove)
                    {
                        values.Remove(ch);
                    }
                }                           
            }

            sum += values.Count;

            Console.WriteLine($"Sum: {sum}");
        }
        #endregion

        #region Day 7
        private static void Day7()
        {
            string[] fileContents = HelperFunctions.FileContents(7, 2);

            Digraph<string> digraph = new Digraph<string>();
            WeightedDigraph<string> weightedDigraph = new WeightedDigraph<string>();
            foreach (string line in fileContents)
            {
                //Remove takes off the period
                string[] parts = line.Remove(line.Length - 1).Split(" bags contain ");
                if (parts[1].Contains("no other bags"))
                {
                    continue;
                }

                string containingBag = parts[0];
                digraph.AddVertex(containingBag);
                weightedDigraph.AddVertex(containingBag);

                string[] childBags = parts[1].Split(",").Select(x => x.Trim()).Select(x => x.Replace(" bags", "")).ToArray();
                foreach (string child in childBags)
                {
                    string[] childParts = child.Split(" ");
                    string childBag = $"{childParts[1]} {childParts[2]}";
                    digraph.AddVertex(childBag);
                    digraph.AddEdge(containingBag, childBag);

                    weightedDigraph.AddVertex(childBag);
                    weightedDigraph.AddEdge(containingBag, childBag, int.Parse(childParts[0]));
                }
            }

            //List<string> goldContainingBags = digraph.Reverse().GetDescendants("shiny gold").ToList();
            List<string> goldContainingBags = weightedDigraph.Reverse().GetDescendants("shiny gold").Select(x => x.Child).ToList();
            Console.WriteLine($"Gold containing count: {goldContainingBags.Count}");
            foreach (var bag in goldContainingBags)
            {
                Console.WriteLine($"{bag}");
            }

            List<int> weights = weightedDigraph.GetDescendants("shiny gold").Select(x => x.Weight).ToList();
            Console.WriteLine($"weight sum: {weights.Sum()}");

            //Dictionary<string, Dictionary<string, int>> bags = new Dictionary<string, Dictionary<string, int>>();
            //foreach (string line in fileContents)
            //{
            //    //Remove takes off the period
            //    string[] parts = line.Remove(line.Length - 1).Split(" bags contain ");  
            //    if (parts[1].Contains("no other bags"))
            //    {
            //        continue;
            //    }

            //    string containingBag = parts[0];
            //    string[] childBags = parts[1].Split(",").Select(x => x.Trim()).Select(x => x.Replace(" bags", "")).ToArray();
            //    foreach (string child in childBags)
            //    {
            //        string[] childParts = child.Split(" ");
            //        string childBag = $"{childParts[1]} {childParts[2]}";
            //        if (!bags.ContainsKey(childBag))
            //        {
            //            bags[childBag] = new Dictionary<string, int>();
            //        }
            //        bags[childBag][containingBag] = int.Parse(childParts[0]);
            //    }
            //}

            //List<string> children = new List<string>();
            //ExploreChildren(bags, "shiny gold", ref children);
            //Console.WriteLine($"Children count: {children.Count}");
        }

        private static void ExploreChildren(Dictionary<string, Dictionary<string, int>> bags, string parent, ref List<string> children)
        {
            if (bags.ContainsKey(parent))
            {
                foreach (string childKey in bags[parent].Keys)
                {
                    if (!children.Contains(childKey))
                    {
                        children.Add(childKey);
                    }

                    ExploreChildren(bags, childKey, ref children);
                }
            }            
        }
        #endregion

        #region Day 8
        private static bool infiniteLoop;
        private static bool endOfProgram;
        private static void Day8()
        {
            string[] fileContents = HelperFunctions.FileContents(8, 1);

            GameConsole gameConsole = new GameConsole();
            gameConsole.HitInfiniteLoop += GameConsole_HitInfiniteLoop;
            gameConsole.HitEndOfProgram += GameConsole_HitEndOfProgram;

            int lastIndexChanged = -1;
            string[] updatedProgram = (string[])fileContents.Clone();
            while (true)
            {
                gameConsole.RunProgram(updatedProgram);

                if (endOfProgram)
                {
                    break;
                }

                updatedProgram = (string[])fileContents.Clone();
                for (int i = lastIndexChanged + 1; i < fileContents.Length; i++)
                {
                    if (updatedProgram[i].Contains("jmp"))
                    {
                        updatedProgram[i] = updatedProgram[i].Replace("jmp", "nop");
                        lastIndexChanged = i;
                        break;
                    }

                    if (updatedProgram[i].Contains("nop"))
                    {
                        updatedProgram[i] = updatedProgram[i].Replace("nop", "jmp");
                        lastIndexChanged = i;
                        break;
                    }
                }
            }
            

            Console.WriteLine($"Finished program");
        }

        private static void GameConsole_HitEndOfProgram(object sender, EndOfProgramArgs e)
        {
            Console.WriteLine($"Hit end of program: {e.Accumulator}");
            endOfProgram = true;
        }

        private static void GameConsole_HitInfiniteLoop(object sender, HitInfiniteLoopEventArgs e)
        {
            Console.WriteLine($"Infinite loop: {e.Accumulator}");
        }
        #endregion

        #region Day 9
        private static void Day9()
        {
            string[] fileContents = HelperFunctions.FileContents(9, 1);

            Int64[] values = fileContents.Select(x => Int64.Parse(x)).ToArray();
                    
            int preambleSize = 25;
            Int64 invalidNumber = 0;
            bool found = false;

            //Part 1 - find the invalid number
            for (int i = preambleSize; i < values.Length; i++)
            {
                found = false;
                Int64 curVal = values[i];
                for (int val1 = i - preambleSize; val1 < i - 1; val1++)
                {
                    for (int val2 = val1 + 1; val2 < i; val2++)
                    {
                        if (values[val1] + values[val2] == curVal)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                    {
                        break;
                    }
                }

                if (!found)
                {
                    Console.WriteLine($"First number: {curVal}");
                    invalidNumber = curVal;
                    break;
                }
            }

            //Part 2 - find rang that adds to invalid number
            found = false;
            int lowIndex = -1;
            int highIndex = 1;
            while (!found)
            {
                lowIndex++;
                Int64 sum = values[lowIndex];
                highIndex = lowIndex;
                while (sum < invalidNumber && highIndex < values.Length)
                {
                    highIndex++;
                    sum += values[highIndex];               
                }

                if (sum == invalidNumber)
                {
                    found = true;
                }
            }

            Console.WriteLine($"Low: {lowIndex} High: {highIndex}");
            Int64 small = Int64.MaxValue;
            Int64 big = Int64.MinValue;
            for (int i = lowIndex; i <= highIndex; i++)
            {
                if (values[i] < small)
                {
                    small = values[i];
                }

                if (values[i] > big)
                {
                    big = values[i];
                }
            }
            Console.WriteLine($"Sum: {small + big}");
        }
        #endregion

        #region Day 10
        private static void Day10()
        {
            string[] fileContents = HelperFunctions.FileContents(10, 1);

            int[] joltages = fileContents.Select(x => int.Parse(x)).ToArray();        
            Array.Sort(joltages);

            int oneJoltDiff = 0, threeJoltDiff = 0, prevJolt = 0;
            foreach (int jolt in joltages)
            {
                oneJoltDiff += (jolt - prevJolt) == 1 ? 1 : 0;
                threeJoltDiff += (jolt - prevJolt) == 3 ? 1 : 0;
                prevJolt = jolt;
            }

            threeJoltDiff++;    //Adds your device

            Console.WriteLine($"One jolt: {oneJoltDiff}");
            Console.WriteLine($"Three jolt: {threeJoltDiff}");
            Console.WriteLine($"Product: {oneJoltDiff * threeJoltDiff}");

            Dictionary<int, Int64> valsAndPathAmts = new Dictionary<int, Int64>();
            Int64 sum = 0;
            valsAndPathAmts[joltages.Last()] = 1;
            for (int i = joltages.Length - 2; i >= 0; i--)
            {
                sum = 0;
                int jolt = joltages[i];
                //Loop over three possible values
                for (int j = 1; j < 4; j++)
                {
                    sum += valsAndPathAmts.ContainsKey(jolt + j) ? valsAndPathAmts[jolt + j] : 0;
                }
                valsAndPathAmts[joltages[i]] = sum;
            }


            //Loop over three possible values
            sum = 0;
            for (int j = 1; j < 4; j++)
            {
                sum += valsAndPathAmts.ContainsKey(j) ? valsAndPathAmts[j] : 0;
            }
            valsAndPathAmts[0] = sum;

            Console.WriteLine($"Arrangements: {valsAndPathAmts[0]}");
            //GetNextJoltRecurse(0, joltages);
            //Console.WriteLine($"Arrangements: {joltArrangementCount}");
        }
        #endregion

        #region Day 11
        private static void Day11()
        {
            string[] fileContents = HelperFunctions.FileContents(11, 1);

            char[][] currentGrid = new char[fileContents.Length][];

            #region Part 1
            for (int i = 0; i < fileContents.Length; i++)
            {
                currentGrid[i] = new char[fileContents[i].Length];
                for (int j = 0; j < fileContents[i].Length; j++)
                {
                    currentGrid[i][j] = fileContents[i][j];
                }
            }

            char[][] nextGrid = CloneGrid(currentGrid);
            bool didChange = true;
            while (didChange)
            {
                didChange = false;                
                for (int row = 0; row < currentGrid.Length; row++)
                {
                    for (int col = 0; col < currentGrid[row].Length; col++)
                    {
                        int occupiedSeats = 0;
                        //Check all neighboring values
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                if (i == 0 && j == 0)
                                {
                                    continue;
                                }

                                if (!HelperFunctions.WithinRange(currentGrid, row + i, col + j))
                                {
                                    continue;
                                }

                                occupiedSeats += currentGrid[row + i][col + j] == '#' ? 1 : 0;
                            }
                        }

                        if (currentGrid[row][col] == 'L' && occupiedSeats == 0)
                        {
                            nextGrid[row][col] = '#';
                            didChange = true;
                            continue;
                        }

                        if (currentGrid[row][col] == '#' && occupiedSeats >= 4)
                        {
                            nextGrid[row][col] = 'L';
                            didChange = true;
                            continue;
                        }
                    }
                }
                currentGrid = CloneGrid(nextGrid);
            }

            int count = 0;
            for (int i = 0; i < currentGrid.Length; i++)
            {
                for (int j = 0; j < currentGrid[i].Length; j++)
                {
                    if (currentGrid[i][j] == '#')
                    {
                        count++;
                    }
                }
            }
            Console.WriteLine($"Occupied seats: {count}");
            #endregion

            //Part 2
            #region Part 2
            currentGrid = new char[fileContents.Length][];
            for (int i = 0; i < fileContents.Length; i++)
            {
                currentGrid[i] = new char[fileContents[i].Length];
                for (int j = 0; j < fileContents[i].Length; j++)
                {
                    currentGrid[i][j] = fileContents[i][j];
                }
            }

            nextGrid = CloneGrid(currentGrid);
            didChange = true;
            while (didChange)
            {
                didChange = false;
                for (int row = 0; row < currentGrid.Length; row++)
                {
                    for (int col = 0; col < currentGrid[row].Length; col++)
                    {
                        int occupiedSeats = 0;
                        //Check all directions for seats
                        int rowToCheck = 0, colToCheck = 0;
                        //Left
                        rowToCheck = row;
                        colToCheck = col - 1;
                        while(HelperFunctions.WithinRange(currentGrid, rowToCheck, colToCheck))
                        {
                            if (currentGrid[rowToCheck][colToCheck] == '#')
                            {
                                occupiedSeats++;
                                break;
                            }
                            if (currentGrid[rowToCheck][colToCheck] == 'L')
                            {                                
                                break;
                            }
                            colToCheck--;
                        }

                        //Left-up
                        rowToCheck = row - 1;
                        colToCheck = col - 1;
                        while (HelperFunctions.WithinRange(currentGrid, rowToCheck, colToCheck))
                        {
                            if (currentGrid[rowToCheck][colToCheck] == '#')
                            {
                                occupiedSeats++;
                                break;
                            }
                            if (currentGrid[rowToCheck][colToCheck] == 'L')
                            {
                                break;
                            }
                            rowToCheck--;
                            colToCheck--;
                        }

                        //Up
                        rowToCheck = row - 1;
                        colToCheck = col;
                        while (HelperFunctions.WithinRange(currentGrid, rowToCheck, colToCheck))
                        {
                            if (currentGrid[rowToCheck][colToCheck] == '#')
                            {
                                occupiedSeats++;
                                break;
                            }
                            if (currentGrid[rowToCheck][colToCheck] == 'L')
                            {
                                break;
                            }
                            rowToCheck--;
                        }

                        //Right-up
                        rowToCheck = row - 1;
                        colToCheck = col + 1;
                        while (HelperFunctions.WithinRange(currentGrid, rowToCheck, colToCheck))
                        {
                            if (currentGrid[rowToCheck][colToCheck] == '#')
                            {
                                occupiedSeats++;
                                break;
                            }
                            if (currentGrid[rowToCheck][colToCheck] == 'L')
                            {
                                break;
                            }
                            rowToCheck--;
                            colToCheck++;
                        }

                        //Right
                        rowToCheck = row;
                        colToCheck = col + 1;
                        while (HelperFunctions.WithinRange(currentGrid, rowToCheck, colToCheck))
                        {
                            if (currentGrid[rowToCheck][colToCheck] == '#')
                            {
                                occupiedSeats++;
                                break;
                            }
                            if (currentGrid[rowToCheck][colToCheck] == 'L')
                            {
                                break;
                            }
                            colToCheck++;
                        }

                        //Right-down
                        rowToCheck = row + 1;
                        colToCheck = col + 1;
                        while (HelperFunctions.WithinRange(currentGrid, rowToCheck, colToCheck))
                        {
                            if (currentGrid[rowToCheck][colToCheck] == '#')
                            {
                                occupiedSeats++;
                                break;
                            }
                            if (currentGrid[rowToCheck][colToCheck] == 'L')
                            {
                                break;
                            }
                            rowToCheck++;
                            colToCheck++;
                        }

                        //Down
                        rowToCheck = row + 1;
                        colToCheck = col;
                        while (HelperFunctions.WithinRange(currentGrid, rowToCheck, colToCheck))
                        {
                            if (currentGrid[rowToCheck][colToCheck] == '#')
                            {
                                occupiedSeats++;
                                break;
                            }
                            if (currentGrid[rowToCheck][colToCheck] == 'L')
                            {
                                break;
                            }
                            rowToCheck++;
                        }

                        //Left-down
                        rowToCheck = row + 1;
                        colToCheck = col - 1;
                        while (HelperFunctions.WithinRange(currentGrid, rowToCheck, colToCheck))
                        {
                            if (currentGrid[rowToCheck][colToCheck] == '#')
                            {
                                occupiedSeats++;
                                break;
                            }
                            if (currentGrid[rowToCheck][colToCheck] == 'L')
                            {
                                break;
                            }
                            rowToCheck++;
                            colToCheck--;
                        }

                        if (currentGrid[row][col] == 'L' && occupiedSeats == 0)
                        {
                            nextGrid[row][col] = '#';
                            didChange = true;
                            continue;
                        }

                        if (currentGrid[row][col] == '#' && occupiedSeats >= 5)
                        {
                            nextGrid[row][col] = 'L';
                            didChange = true;
                            continue;
                        }
                    }
                }
                currentGrid = CloneGrid(nextGrid);
                DisplayGrid(currentGrid);
            }

            count = 0;
            for (int i = 0; i < currentGrid.Length; i++)
            {
                for (int j = 0; j < currentGrid[i].Length; j++)
                {
                    if (currentGrid[i][j] == '#')
                    {
                        count++;
                    }
                }
            }
            Console.WriteLine($"Occupied seats: {count}");
            #endregion
        }

        private static char[][] CloneGrid(char[][] grid)
        {
            char[][] nextGrid = new char[grid.Length][];
            for (int i = 0; i < grid.Length; i++)
            {
                nextGrid[i] = new char[grid[i].Length];
                for (int j = 0; j < grid[i].Length; j++)
                {
                    nextGrid[i][j] = grid[i][j];
                }
            }
            return nextGrid;
        }

        private static void DisplayGrid(char[][] grid)
        {
            for (int i = 0; i < grid.Length; i++)
            {
                string st = "";
                for (int j = 0; j < grid[i].Length; j++)
                {
                    st += grid[i][j];
                }
                Console.WriteLine(st);
            }
            Console.WriteLine();
        }
        #endregion

        #region Day 12
        private static void Day12()
        {
            string[] fileContents = HelperFunctions.FileContents(12, 1);

            Direction shipDirection = Direction.East;
            int xPos = 0, yPos = 0;

            foreach (string line in fileContents)
            {
                char action = line[0];
                int value = Int32.Parse(line.Substring(1));

                switch (action)
                {
                    case 'N':
                        yPos += value;
                        break;
                    case 'S':
                        yPos -= value;
                        break;
                    case 'E':
                        xPos += value;
                        break;
                    case 'W':
                        xPos -= value;
                        break;
                    case 'L':
                        shipDirection = UpdateDirection(shipDirection, value, ClockDirection.CounterClockwise);
                        break;
                    case 'R':
                        shipDirection = UpdateDirection(shipDirection, value, ClockDirection.Clockwise);
                        break;
                    case 'F':
                        switch (shipDirection)
                        {
                            case Direction.North:
                                yPos += value;
                                break;
                            case Direction.East:
                                xPos += value;
                                break;
                            case Direction.South:
                                yPos -= value;
                                break;
                            case Direction.West:
                                xPos -= value;
                                break;
                        }
                        break;
                }
            }

            Console.WriteLine($"Manhattan dist: {HelperFunctions.ManhattanDistance(0, 0, xPos, yPos)}");
        }

        private static Direction UpdateDirection(Direction current, int degrees, ClockDirection clockDirection)
        {
            int steps = degrees / 90;
            int cur = (int)current;
            for (int i = 0; i < steps; i++)
            {
                cur += clockDirection == ClockDirection.Clockwise ? 1 : -1;
                if (cur < 0)
                {
                    cur = 3;
                }
                if (cur > 3)
                {
                    cur = 0;
                }
            }

            return (Direction)(cur);
        }
        #endregion

        #region Day 13
        private static void Day13()
        {
            string[] fileContents = HelperFunctions.FileContents(13, 1);

            int earliest = int.Parse(fileContents[0]);
            int[] busses = fileContents[1].Split(",").Where(x => x != "x").Select(x => int.Parse(x)).ToArray();

            int shortestWait = int.MaxValue;
            int shortestBus = int.MaxValue;
            foreach (int bus in busses)
            {
                int nextTime = ((int)Math.Ceiling((double)(earliest / bus)) + 1) * bus;
                if ((nextTime - earliest) < shortestWait)
                {
                    shortestWait = nextTime - earliest;
                    shortestBus = bus;
                }
            }

            Console.WriteLine($"Value: {shortestWait * shortestBus}");
        }
        #endregion

        #region Day 14
        private static void Day14()
        {
            string[] fileContents = HelperFunctions.FileContents(14, 1);
            
            Dictionary<int, char> bitMask = new Dictionary<int, char>();
            Dictionary<int, string> addresses = new Dictionary<int, string>();
            foreach (string line in fileContents)
            {
                string[] parts = line.Split(" = ");
                if (parts[0] == "mask")
                {
                    //Update mask values
                    bitMask = new Dictionary<int, char>();
                    string mask = parts[1];
                    for (int i = 0; i < mask.Length; i++)
                    {
                        if (mask[i] != 'X')
                        {
                            bitMask[i] = mask[i];
                        }
                    }
                }
                else
                {
                    //Update memory address
                    //Get memory address                              
                    int address = int.Parse(parts[0].Replace("mem[", "").Replace("]", ""));
                    int value = int.Parse(parts[1]);
                    StringBuilder sb = new StringBuilder(Convert.ToString(value, 2).PadLeft(36, '0'));
                    foreach (int bit in bitMask.Keys)
                    {
                        sb[bit] = bitMask[bit];
                    }
                    addresses[address] = sb.ToString();
                }
            }

            Int64 sum = 0;           
            foreach (int address in addresses.Keys)
            {
                string val = addresses[address];
                sum += Convert.ToInt64(val, 2);
            }

            Console.WriteLine($"Sum: {sum}");


            //Part 2
            bitMask = new Dictionary<int, char>();
            Dictionary<Int64, Int64> bigAddresses = new Dictionary<long, long>();
            foreach (string line in fileContents)
            {
                string[] parts = line.Split(" = ");
                if (parts[0] == "mask")
                {
                    //Update mask values
                    bitMask = new Dictionary<int, char>();
                    string mask = parts[1];
                    for (int i = 0; i < mask.Length; i++)
                    {
                        if (mask[i] != '0')
                        {
                            bitMask[i] = mask[i];
                        }
                    }
                }
                else
                {
                    //Update memory address
                    //Get memory address                              
                    int address = int.Parse(parts[0].Replace("mem[", "").Replace("]", ""));
                    Int64 value = Int64.Parse(parts[1]);
                   
                    StringBuilder sb = new StringBuilder(Convert.ToString(address, 2).PadLeft(36, '0'));
                    foreach (int bit in bitMask.Keys)
                    {
                        sb[bit] = bitMask[bit];
                    }
                    List<string> maskedAddresses = new List<string>();
                    PermuteAddresses(sb.ToString(), ref maskedAddresses);
                    foreach (string add in maskedAddresses)
                    {
                        bigAddresses[Convert.ToInt64(add, 2)] = value;
                    }
                }
            }

            sum = 0;
            foreach (Int64 address in bigAddresses.Keys)
            {
                sum += bigAddresses[address];
            }

            Console.WriteLine($"Sum: {sum}");
        }

        private static void PermuteAddresses(string address, ref List<string> addresses)
        {
            if (!address.Contains('X'))
            {
                addresses.Add(address);
                return;
            }

            int index = address.IndexOf('X');
            PermuteAddresses(address.Remove(index, 1).Insert(index, "0"), ref addresses);
            PermuteAddresses(address.Remove(index, 1).Insert(index, "1"), ref addresses);
        }

        #endregion

        #region Day 15
        private static void Day15()
        {
            //string[] fileContents = HelperFunctions.FileContents(15, 1);
            string input = "0,14,1,3,7,9";
            //string input = "0,3,6";

            int count = 0, lastNum = 0;
            string[] startingVals = input.Split(",");
            Dictionary<int, Stack<int>> numberSaidAndWhen = new Dictionary<int, Stack<int>>();
            for (int i = 0; i < startingVals.Length; i++)
            {
                int val = int.Parse(startingVals[i]);
                lastNum = val;

                numberSaidAndWhen[val] = new Stack<int>();
                numberSaidAndWhen[val].Push(count++);
            }

            while (count < 30000000)
            {
                if (numberSaidAndWhen[lastNum].Count == 1)
                {
                    //Only said once
                    numberSaidAndWhen[0].Push(count);
                    lastNum = 0;                  
                }
                else
                {
                    Stack<int> temp = new Stack<int>();
                    temp.Push(numberSaidAndWhen[lastNum].Pop());
                    temp.Push(numberSaidAndWhen[lastNum].Pop());

                    int valLow = temp.Pop();
                    numberSaidAndWhen[lastNum].Push(valLow);
                    int valHigh = temp.Pop();
                    numberSaidAndWhen[lastNum].Push(valHigh);

                    int nextVal = valHigh - valLow;
                    if (!numberSaidAndWhen.ContainsKey(nextVal))
                    {
                        numberSaidAndWhen[nextVal] = new Stack<int>();
                    }
                    numberSaidAndWhen[nextVal].Push(count);
                    lastNum = nextVal;
                }

                count++;
                if (count % 100000 == 0)
                {
                    Console.WriteLine(count);
                }
            }

            Console.WriteLine($"Last num: {lastNum}");
        }
        #endregion

        #region Day 16
        private static void Day16()
        {
            string[] fileContents = HelperFunctions.FileContents(16, 1);

            int phase = 0;
            List<string> rules = new List<string>();
            string myTicket = "";
            List<string> otherTickets = new List<string>();

            IntervalTree intervalTree = new IntervalTree();
            List<int> invalidVals = new List<int>();

            //Store lines
            foreach (string line in fileContents)
            {
                if (string.IsNullOrEmpty(line))
                {
                    phase++;
                    continue;
                }

                switch (phase)
                {
                    case 0:
                        rules.Add(line);
                        break;
                    case 1:
                        if (line.Contains("ticket"))
                        {
                            continue;
                        }
                        myTicket = line;
                        break;
                    case 2:
                        if (line.Contains("ticket"))
                        {
                            continue;
                        }
                        otherTickets.Add(line);
                        break;
                }
            }

            //Parse rules
            foreach (string line in rules)
            {
                string[] ranges = line.Split(": ")[1].Split(" or ");

                foreach (string range in ranges)
                {
                    int[] vals = range.Split("-").Select(x => int.Parse(x)).ToArray();
                    intervalTree.AddRange(vals[0], vals[1]);
                }
            }

            //Parse nearby tickets
            foreach (string line in otherTickets)
            {
                int[] vals = line.Split(",").Select(x => int.Parse(x)).ToArray();
                foreach (int val in vals)
                {
                    if (!intervalTree.WithinRange(val))
                    {
                        invalidVals.Add(val);
                    }
                }
            }

            Console.WriteLine($"Sum: {invalidVals.Sum()}");
        }
        #endregion

        #region Day 17
        private static void Day17()
        {
            string[] fileContents = HelperFunctions.FileContents(17, 1);

            Dictionary<int, Grid> grids = new Dictionary<int, Grid>();
            grids[0] = new Grid();
            for (int row = 0; row < fileContents.Length; row++)
            {
                for (int col = 0; col < fileContents[row].Length; col++)
                {
                    if (fileContents[row][col] == '#')
                    {
                        grids[0].SetNode(row, col, true);
                    }
                }
            }

            //Six trials
            for (int i = 0; i < 6; i++)
            {




            }
        }
        #endregion

        #endregion
    }

    public enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    public enum ClockDirection
    {
        Clockwise,
        CounterClockwise
    }
}
