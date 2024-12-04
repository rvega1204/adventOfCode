using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // .txt file with all the lines
        string filePath = "puzzleInput.txt";

        // Validate the file
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Error: File not found.");
            return;
        }

        // Read all lines from .txt file
        // This creates a list of lists of integers.
        // File.ReadAllLines(filePath) reads the entire file and returns an array of strings,
        // where each string corresponds to one line in the file.
        List<List<int>> reports = File.ReadLines(filePath)
                .Where(line => !string.IsNullOrWhiteSpace(line)) // Filter out empty or whitespace-only lines
                .Select(line => line.Split(' ') // Split by space
                        .Where(number => int.TryParse(number, out _)) // Only take valid integers
                        .Select(int.Parse) // Convert valid strings to integers
                        .ToList()) // Create a list for each line
                .ToList();

        // Counts how many reports are considered safe by calling the IsSafe method on each report (which is a List<int>).
        int safeCount = reports.Count(report => IsSafe(report));
        // Counts how many reports are considered safe by calling the IsSafeWithDamper method on each report (which is a List<int>).
        int safeWithDamperCount = reports.Count(report => IsSafeWithDamper(report));
        Console.WriteLine($"Number of safe reports: {safeCount}");
        Console.WriteLine($"Number of safe reports with the Problem Dampener: {safeWithDamperCount}");
    }

    static bool IsSafe(List<int> levels)
    {
        // This condition checks if the levels list contains fewer than 2 elements.
        if (levels.Count < 2) return true;

        bool isIncreasing = true, isDecreasing = true;

        // A report only counts as safe if both of the following are true:
        // The levels are either all increasing or all decreasing.
        // Any two adjacent levels differ by at least one and at most three.
        for (int i = 1; i < levels.Count; i++)
        {
            int diff = levels[i] - levels[i - 1];

            // This condition checks whether the absolute value of diff is outside the allowed range of [1, 3].
            // Math.Abs(diff) calculates the absolute value of diff, ensuring that both positive and negative differences are treated the same.
            if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3) return false;

            // Determine if the list is consistently increasing or decreasing
            if (diff > 0) isDecreasing = false;
            if (diff < 0) isIncreasing = false;
        }

        // Checks if the list is either entirely increasing or entirely decreasing.
        return isIncreasing || isDecreasing;
    }

    static bool IsSafeWithDamper(List<int> levels)
    {
        // Step 1: Check if the report is already safe using the original IsSafe method
        if (IsSafe(levels)) return true;

        // Step 2: Try removing each level one by one
        for (int i = 0; i < levels.Count; i++)
        {
            // Step 2.1: Create a copy of the original levels list
            List<int> modifiedLevels = [.. levels];

            // Step 2.2: Remove the level at the current index (i)
            modifiedLevels.RemoveAt(i);

            // Step 2.3: Check if the modified list is safe after removing one level
            if (IsSafe(modifiedLevels)) return true;
        }

        // Step 3: If no removal makes the report safe, return false
        return false;
    }
}
