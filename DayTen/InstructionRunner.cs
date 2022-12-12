namespace AdventOfCode2022.DayTen;

public class InstructionRunner
{
    private readonly List<int> _marks = new() { 20, 60, 100, 140, 180, 220 };

    public int GetMarkerTotal()
    {
        var rawInstructions = File.ReadAllLines("DayTen/instructions.txt");

        var instructions = new List<Instruction>();

        foreach (var rawInstruction in rawInstructions)
        {
            instructions.Add(new Instruction(rawInstruction));
        }

        int instructionCounter = 0;
        int registerX = 1;

        int markCounter = 0;

        foreach (Instruction instruction in instructions)
        {
            for (int i = 0; i < instruction.Complexity; i++)
            {
                instructionCounter++;

                if (_marks.Contains(instructionCounter))
                {
                    markCounter += instructionCounter * registerX;
                }

                if (instruction.Type == InstructionType.Addx && i == instruction.Complexity - 1)
                {
                    registerX += instruction.Value;
                }
            }
        }

        return markCounter;
    }

    public void DrawImage()
    {
        var rawInstructions = File.ReadAllLines("DayTen/instructions.txt");

        var instructions = new List<Instruction>();

        Image = new char[CrtHeight * CrtWidth];

        for (int y = 0; y < CrtHeight; y++)
            for (int x = 0; x < CrtWidth; x++)
                Image[y * CrtWidth + x] = '.';

        foreach (var rawInstruction in rawInstructions)
        {
            instructions.Add(new Instruction(rawInstruction));
        }

        int instructionCounter = 0;
        int registerX = 1;
        int crtCounter = 0;

        foreach (Instruction instruction in instructions)
        {
            for (int i = 0; i < instruction.Complexity; i++)
            {
                instructionCounter++;

                if (crtCounter == CrtWidth * CrtHeight) crtCounter = 0;

                var relativeCrtCounter = crtCounter % 40;
                if (relativeCrtCounter == registerX || relativeCrtCounter == registerX - 1 || relativeCrtCounter == registerX + 1)
                {
                    Image[crtCounter] = '#';
                }

                if (instruction.Type == InstructionType.Addx && i == instruction.Complexity - 1)
                {
                    registerX += instruction.Value;
                }

                crtCounter++;
            }
        }

        Draw();
    }

    public int CrtWidth = 40;
    public int CrtHeight = 6;
    public char[] Image;

    private void Draw()
    {
        Console.WriteLine();
        Console.WriteLine();

        for (int y = 0; y < CrtHeight; y++)
        {
            Console.WriteLine();

            for (int x = 0; x < CrtWidth; x++)
            {
                Console.Write(Image[y * CrtWidth + x]);
            }
        }

        Console.WriteLine();
    }

    public enum InstructionType
    {
        Addx,
        Noop
    }

    public class Instruction
    {
        public InstructionType Type { get; set; }

        public int Value { get; set; }

        public int Complexity => Type == InstructionType.Addx ? 2 : 1;

        public Instruction(string rawInstruction)
        {
            var instruction = rawInstruction.Split(' ');

            if (instruction[0] == "addx") Type = InstructionType.Addx;
            if (instruction[0] == "noop") Type = InstructionType.Noop;

            if (HasArgument())
            {
                Value = int.Parse(instruction[1]);
            }
        }

        public bool HasArgument()
        {
            return Type == InstructionType.Addx;
        }
    }
}