namespace AdventOfCode2022.DayThree;

public class RucksackEstimator
{
    public int GetPriorityErrorItems()
    {
        var rucksacks = File.ReadAllLines("DayThree/input.txt");

        int prioritySum = 0;
        foreach (string rucksack in rucksacks)
        {
            var compartmentA = rucksack.Substring(0, rucksack.Length / 2);
            var compartmentB = rucksack.Substring(rucksack.Length / 2, rucksack.Length / 2);

            foreach (char item in compartmentA)
            {
                if (compartmentB.Contains(item))
                {
                    prioritySum += GetPriority(item);
                    break;
                }
            }
        }

        return prioritySum;
    }

    public int GetBadgePrioritySum()
    {
        var rucksacks = File.ReadAllLines("DayThree/input.txt");

        int prioritySum = 0;
        for (int i = 0; i <= rucksacks.Length; i++)
        {
            if ((i % 3 == 0 && i != 0) || i == rucksacks.Length)
            {
                var a = rucksacks[i - 3];
                var b = rucksacks[i - 2];
                var c = rucksacks[i - 1];

                foreach (char item in a)
                {
                    if (b.Contains(item) && c.Contains(item))
                    {
                        prioritySum += GetPriority(item);
                        break;
                    }
                }
            }
        }

        return prioritySum;
    }

    private int GetPriority(char item)
    {
        if (char.IsLower(item))
        {
            return item - 96;
        }

        if (char.IsUpper(item))
        {
            return (item - 64) + 26;
        }

        throw new NotSupportedException("Character is not a letter of the alphabet");
    }
}