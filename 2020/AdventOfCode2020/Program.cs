using System;
using System.Diagnostics;
using System.Reflection;

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

        #endregion
    }
}
