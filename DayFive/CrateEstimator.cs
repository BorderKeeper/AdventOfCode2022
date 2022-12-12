using System.Text;

namespace AdventOfCode2022.DayFive;

public class CrateEstimator
{
    public string GetListOfTopCrates()
    {
        var file = File.ReadAllLines("DayFive/input.txt");

        var rawPicture = file.Take(new Range(0, 8)).ToArray();

        var stackingArea = new StackingArea(rawPicture);

        var instructions = file.Take(Range.StartAt(10));

        foreach (string rawInstruction in instructions)
        {
            var instruction = rawInstruction.Split(" ");

            var number = int.Parse(instruction[1]);
            var from = int.Parse(instruction[3]);
            var to = int.Parse(instruction[5]);

            stackingArea.Manipulate(number, from, to);
        }

        return stackingArea.GetTopOfStacks();
    }

    class StackingArea
    {
        public StackingArea(string[] rawPicture)
        {
            Stacks = new Dictionary<int, Stack<char>>();

            for (int stackIndex = 1; stackIndex <= 9; stackIndex++)
            {
                Stacks.Add(stackIndex, new Stack<char>());
            }

            var firstIndex = 1;
            var indexStep = 4;

            var headers = new int[9];
            for (int i = rawPicture.Length - 1; i >= 0; i--)
            {
                var line = rawPicture[i];

                for (int stackIndex = 0; stackIndex < 9; stackIndex++)
                {
                    var box = line[firstIndex + indexStep * stackIndex];

                    if (box != ' ')
                    {
                        Stacks[stackIndex + 1].Push(box);
                    }
                }
            }
        }

        private Dictionary<int, Stack<char>> Stacks { get; }

        public void Manipulate(int count, int from, int to)
        {
            var list = new List<char>();

            for (int i = 0; i < count; i++)
            {
                var box = Stacks[from].Pop();

                list.Add(box);
            }

            list.Reverse();

            for (int i = 0; i < count; i++)
            {
                Stacks[to].Push(list[i]);
            }
        }

        private void Manipulate(int from, int to)
        {
            var movedBox = Stacks[from].Pop();

            Stacks[to].Push(movedBox);
        }

        public string GetTopOfStacks()
        {
            var top = new StringBuilder();

            foreach (var stack in Stacks)
            {
                top.Append(stack.Value.Peek());
            }

            return top.ToString();
        }
    }
}