using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023.Days;

public static class Day7
{
    public static List<char> CardOrder = ['A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'];
    public enum HandType
    {
        FiveOfAKind = 0,
        FourOfAKind = 1,
        FullHouse = 2,
        ThreeOfAKind = 3,
        TwoPair = 4,
        OnePair = 5,
        HighCard = 6
    }

    public class Hand : IComparable<Hand>
    {
        public List<char> Cards { get; set; }
        public HandType Type { get; set; }

        public Hand(string cards)
        {
            Cards = [.. cards];
            Type = GetHandType();
        }

        private HandType GetHandType()
        {
            // Count the number of jokers
            int jokerCount = Cards.Count(c => c == 'J');
            List<char> NoJokers = Cards.Where(c => c != 'J').ToList();

            // Handle the all jokers case
            if (jokerCount == 5) {
                return HandType.FiveOfAKind;
            }

            // Do the count as in part 1 with the jokers swapped
            // Sort instances of each card in descending order
            List<int> counts = NoJokers.Distinct().Select(distinctCard => NoJokers.Count(c => c == distinctCard)).ToList();
            counts.Sort();
            counts.Reverse();
            counts[0] += jokerCount;

            if (counts[0] == 5) { return HandType.FiveOfAKind; }
            else if (counts[0] == 4) { return HandType.FourOfAKind; }
            else if (counts[0] == 3 && counts[1] == 2) { return HandType.FullHouse; }
            else if (counts[0] == 3) { return HandType.ThreeOfAKind; }
            else if (counts[0] == 2 && counts[1] == 2) { return HandType.TwoPair; }
            else if (counts[0] == 2) { return HandType.OnePair; }
            else { return HandType.HighCard; }

        }

        public int CompareTo(Hand? other)
        {
            if (other == null) 
                return 1;

            if (Type != other.Type)
            {
                // Lower HandType values are greater - so they should be sorted later (ascending order)
                return other.Type - Type;
            }
            else
            {
                for (int idx = 0; idx < 5; idx++)
                {
                    if (Cards[idx] != other.Cards[idx])
                    {
                        return CardOrder.IndexOf(other.Cards[idx]) - CardOrder.IndexOf(Cards[idx]);
                    }
                }
            }

            // Equal
            return 0;
        }

        public override string ToString()
        {
            return $"{string.Join("", Cards)} - {((HandType)Type).ToString()}";
        }

    }
    
    static List<(Hand hand, int bid)> ReadInput(string[] input)
    {
        List<(Hand hand, int bid)> hands = [];

        foreach (string line in input)
        {
            string hand = line.Split(' ')[0];
            int bid = int.Parse(line.Split(' ')[1]);
            hands.Add((new Hand(hand), bid));
        }

        return hands;
    }

    public static void Solution(string[] input)
    {
        // Read the cards
        List <(Hand hand, int bid)> hands = ReadInput(input);

        hands = hands.OrderBy(h => h.hand).ToList();

        foreach(var (hand, bid) in hands)
        {
            Console.WriteLine(hand + " " + bid);
        }

        int totalWinnings = 0;
        for (int idx = 0; idx < hands.Count; idx++)
        {
            totalWinnings += (idx + 1) * hands[idx].bid;
        }

        Console.WriteLine($"Part 1 - {totalWinnings}");
    }
}