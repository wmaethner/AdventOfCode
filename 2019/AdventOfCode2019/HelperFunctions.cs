using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019
{
    public static class HelperFunctions
    {
        public static string[] FileContents(int day, int challengePart)
        {
            return System.IO.File.ReadAllLines($"{Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppContext.BaseDirectory))))}/Challenge Text Files/Day{day}Part{challengePart}.txt");
        }

        /// <summary>
        /// Given an input string this function will return a dictionary of all the chars in
        /// the string with their respective counts.
        /// </summary>
        /// <param name="inputString">String to count the chars of</param>
        /// <returns>Dictionary(char, int) giving the counts of each unique char</returns>
        public static Dictionary<char, int> LetterCountsInString(string inputString)
        {
            Dictionary<char, int> countsByChar = new Dictionary<char, int>();

            for (int x = 0; x < inputString.Length; x++)
            {
                if (countsByChar.ContainsKey(inputString[x]))
                {
                    countsByChar[inputString[x]]++;
                }
                else
                {
                    countsByChar[inputString[x]] = 1;
                }
            }
            return countsByChar;
        }

        /// <summary>
        /// Returns the Manhattan (or taxicab) distance between the two points.
        /// https://en.wikipedia.org/wiki/Taxicab_geometry
        /// </summary>
        /// <param name="x1">x position 1</param>
        /// <param name="y1">y position 1</param>
        /// <param name="x2">x position 2</param>
        /// <param name="y2">y position 2</param>
        /// <returns></returns>
        public static int ManhattanDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        public static double EuclideanDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        /// <summary>
        /// Gets the next index for a 0-based collection. Will wrap back to 0 if next
        /// index would be out of range
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <param name="collectionCount"></param>
        /// <returns></returns>
        public static int GetNextIndex(int currentIndex, int collectionCount, bool increase = true)
        {
            if (increase)
            {
                if (++currentIndex == collectionCount)
                {
                    return 0;
                }
            }
            else
            {
                if (--currentIndex < 0)
                {
                    return collectionCount - 1;
                }
            }
            
            return currentIndex;
        }

        public static IEnumerable<string> SplitStringBySize(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize).Select(i => str.Substring(i * chunkSize, chunkSize));
        }
    }
}
