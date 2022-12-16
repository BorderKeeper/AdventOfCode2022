namespace AdventOfCode2022.DayEleven;

public class KeepAwayGame
{
    private List<Monkey> _monkeys;

    public long GetMostActiveMonkeys()
    {
        checked
        {
            int numberOfRounds = 10000;

            CreateMonkeys();

            for (int i = 1; i <= numberOfRounds; i++)
            {
                ProcessRound();

                if (i % 1000 == 0 || i == 20)
                {
                    Console.WriteLine($"{i}: " + string.Join(',', _monkeys.Select(e => e.InspectCounter).ToArray()));
                }
            }

            var twoMostActive = _monkeys.OrderByDescending(m => m.InspectCounter).Take(2).ToArray();

            Console.WriteLine(string.Join(',', _monkeys.Select(e => e.InspectCounter).ToArray()));

            //1480621618 too low
            //2637600014 too low 
            //2637590098 too low
            //10092384051 wrong
            //2637600014
            //2567194800
            long x = (long)twoMostActive[0].InspectCounter * (long)twoMostActive[1].InspectCounter;

            return x;
        }
    }

    private void ProcessRound()
    {
        foreach (Monkey monkey in _monkeys)
        {
            monkey.ProcessRound();
        }
    }

    private void CreateMonkeys()
    {
        _monkeys = new List<Monkey>();

        var rawInput = File.ReadAllLines("DayEleven/monkeyBusiness.txt");

        for (int i = 0; i < rawInput.Length; i++)
        {
            var line = rawInput[i];

            if (line.Contains("Monkey"))
            {
                var monkeyLine = line.Split(' ');

                var monkey = new Monkey(int.Parse(monkeyLine[1].TrimEnd(':')), _monkeys);

                i++; line = rawInput[i]; //Move to Starting Items

                var startingItems = line.Split(':')[1].Split(',', StringSplitOptions.TrimEntries);

                monkey.Items = startingItems.Select(int.Parse).ToList();

                i++; line = rawInput[i]; //Move to operation

                var split = line.Split("=")[1].Trim().Split(' ');

                monkey.Operation = new Operation
                {
                    Op = split[1] == "*" ? Operator.Times : Operator.Plus,
                    Amount = split[2]
                };

                i++; line = rawInput[i]; //Move to test

                monkey.DivisibleBy = int.Parse(line.Trim().Split(' ')[3]);

                i++; line = rawInput[i]; //Move to true section

                monkey.ThrowTrue = int.Parse(line.Trim().Split(' ')[5]);

                i++; line = rawInput[i]; //Move to false section

                monkey.ThrowFalse = int.Parse(line.Trim().Split(' ')[5]);

                _monkeys.Add(monkey);
            }
        }
    }

    class Monkey
    {
        public int Id { get; }

        public List<int> Items { get; set; }

        public Operation Operation { get; set; }

        public int DivisibleBy { get; set; }

        public int ThrowTrue { get; set; }

        public int ThrowFalse { get; set; }

        public readonly List<Monkey> OtherMonkeys;

        public int InspectCounter = 0;

        public Monkey(int id, List<Monkey> monkeys)
        {
            Id = id;
            OtherMonkeys = monkeys;
        }

        public void ProcessRound()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                ChangeWorry(i);

                if (Items[i] % DivisibleBy == 0)
                {
                    OtherMonkeys[ThrowTrue].Pass(Items[i]);
                }
                else
                {
                    OtherMonkeys[ThrowFalse].Pass(Items[i]);
                }

                InspectCounter++;
            }

            Items.Clear();
        }

        private void Pass(int item)
        {
            Items.Add(item);
        }

        private void ChangeWorry(int i)
        {
            var amount = Operation.Amount == "old" ? Items[i] : int.Parse(Operation.Amount);

            if (Operation.Op == Operator.Times)
            {
                Items[i] *= amount;
            }

            if (Operation.Op == Operator.Plus)
            {
                Items[i] += amount;
            }

            //Items[i] /= 3;
        }
    }

    class Operation
    {
        public Operator Op { get; set; }

        public string Amount { get; set; }
    }

    enum Operator
    {
        Plus,
        Times
    }
}