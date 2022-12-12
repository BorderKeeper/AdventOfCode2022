namespace AdventOfCode2022.DayFour;

public class SectionCleaning
{
    public int CalculateFullyOverlappedPairs()
    {
        var file = File.ReadAllLines("DayFour/input.txt");

        int counter = 0;
        foreach (string rawPair in file)
        {
            var pair = rawPair.Split(',');

            var a = new Workload(pair[0]);
            var b = new Workload(pair[1]);

            if (a.ContainsOtherWorkload(b) || b.ContainsOtherWorkload(a)) counter++;
        }

        return counter;
    }

    public int CalculatePartiallyOverlappedPairs()
    {
        var file = File.ReadAllLines("DayFour/input.txt");

        int counter = 0;
        foreach (string rawPair in file)
        {
            var pair = rawPair.Split(',');

            var a = new Workload(pair[0]);
            var b = new Workload(pair[1]);

            if (a.Overlaps(b)) counter++;
        }

        return counter;
    }

    class Workload
    {
        public Workload(string rawSection)
        {
            var section = rawSection.Split('-');

            Lower = int.Parse(section[0]);
            Higher = int.Parse(section[1]);
        }

        private int Lower { get; }

        private int Higher { get; }

        public bool ContainsOtherWorkload(Workload otherWorkload)
        {
            return otherWorkload.Lower >= Lower && otherWorkload.Higher <= Higher;
        }

        public bool Overlaps(Workload otherWorkload)
        {
            var isFullyOutside = otherWorkload.Higher < Lower || otherWorkload.Lower > Higher;

            return !isFullyOutside;
        }
    }
}