namespace AdventOfCode2022.DayOne;

public class CalorieCalculator
{
    public double BiggestElfInventory()
    {
        var elfInventories = new Dictionary<int, double>();

        var file = File.ReadAllLines("DayOne/input.txt");

        int index = 0;

        foreach (string line in file)
        {
            if (line == "")
            {
                index++;
            }
            else
            {
                if (!elfInventories.ContainsKey(index))
                {
                    elfInventories[index] = 0;
                }

                elfInventories[index] += int.Parse(line);
            }
        }

        return elfInventories.Max(elf => elf.Value);
    }

    public double GetTop3BiggestInventories()
    {
        var elfInventories = new Dictionary<int, double>();

        var file = File.ReadAllLines("DayOne/input.txt");

        int index = 0;

        foreach (string line in file)
        {
            if (line == "")
            {
                index++;
            }
            else
            {
                if (!elfInventories.ContainsKey(index))
                {
                    elfInventories[index] = 0;
                }

                elfInventories[index] += int.Parse(line);
            }
        }

        var ordered = elfInventories.OrderByDescending(elf => elf.Value).Take(3);

        return ordered.Sum(elf => elf.Value);
    }
}