using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
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
        #endregion
    }
}
