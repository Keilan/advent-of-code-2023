using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Days;

public static class Day4
{    
    static List<(List<int> winningNumbers, List<int> myNumbers)> ReadCards(string[] input)
    {
        List<(List<int> winningNumbers, List<int> myNumbers)> cards = [];

        foreach (string line in input)
        {
            string numbersString = line.Split(':')[1].Trim();
            string[] numbersSplit = numbersString.Split('|');

            List<int> winningNumbers = numbersSplit[0].Split(' ')
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(n => int.Parse(n.Trim())).ToList();
            List<int> myNumbers = numbersSplit[1].Split(' ')
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(n => int.Parse(n.Trim())).ToList();

            cards.Add((winningNumbers, myNumbers));
        }

        return cards;
    }
    
    public static void Solution(string[] input)
    {
        // Read the cards
        List<(List<int> winningNumbers, List<int> myNumbers)> cards = ReadCards(input);

        // Sum up points
        int pointSum = 0;
        foreach (var card in cards)
        {
            List<int> shared = card.winningNumbers.Intersect(card.myNumbers).ToList();
            if (shared.Count > 0)
            {
                pointSum += Convert.ToInt32(Math.Pow(2, shared.Count - 1));
            }
        }

        Console.WriteLine($"Part 1: {pointSum}");

        // Sum up cards - cardCounts[0] is the number of card 1
        List<int> cardCounts = Enumerable.Repeat(1, cards.Count).ToList();

        for (int i = 0; i < cards.Count; i++)
        {
            (List<int> winningNumbers, List<int> myNumbers) card = cards[i];
            int matches = card.winningNumbers.Intersect(card.myNumbers).Count();
            for (int matchIdx = 1; matchIdx <= matches; matchIdx++)
            {
                cardCounts[i + matchIdx] += cardCounts[i];
            }
        }

        Console.WriteLine($"Part 2: {cardCounts.Sum()}");
    }
}