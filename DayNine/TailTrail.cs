using System.Drawing;

namespace AdventOfCode2022.DayNine;

public class TailTrail
{
    public int GetAreasCountTailPassed()
    {
        var headInstructions = File.ReadAllLines("DayNine/headInstructions.txt");

        var instructions = new List<Instruction>();

        foreach (string rawInstruction in headInstructions)
        {
            instructions.Add(new Instruction(rawInstruction));
        }

        var field = new Field(new Point(0, 0), 9);

        foreach (Instruction instruction in instructions)
        {
            field.Update(instruction);
        }

        return field.TailTrail.Distinct().Count();
        //Too high 6799
    }

    class Field
    {
        private Point _head;
        private Point _start;

        private Point _size;

        private List<Tail> _snake;

        public List<Point> TailTrail { get; }

        public Field(Point start, int numberOfTrails)
        {
            _head = new Point(start.X, start.Y);
            _start = new Point(start.X, start.Y);
            _size = new Point(15, 15);
            _snake = new List<Tail>();

            for (int n = 0; n < numberOfTrails; n++)
            {
                _snake.Add(new Tail(start));
            }

            TailTrail = new List<Point>();

            TailTrail.Add(start);
        }

        public void Update(Instruction instruction)
        {
            _size.X = _head.X > _size.X ? _head.X : _size.X;
            _size.Y = _head.Y > _size.Y ? _head.Y : _size.Y;

            /*Console.WriteLine(instruction);
            Console.WriteLine();*/

            for (int step = 0; step < instruction.Steps; step++)
            {
                UpdateHead(instruction.Movement);

                for (int i = 0; i < _snake.Count; i++)
                {
                    if (i == 0)
                    {
                        UpdateTail(_head, _snake[i]);
                    }
                    else
                    {
                        UpdateTail(_snake[i - 1].Pos, _snake[i]);
                    }
                }

                //Draw();
            }

            /*Console.WriteLine();*/
        }

        private void Draw()
        {
            for (int y = _size.Y - 1; y >= 0; y--)
            {
                Console.WriteLine();

                for (int x = 0; x < _size.X; x++)
                {
                    if (x == _head.X && y == _head.Y) Console.Write("H");
                    else if (_snake.Contains(new Tail(x, y))) Console.Write(_snake.FindIndex(p => p.Equals(new Tail(x, y))));
                    else if (x == _start.X && y == _start.Y) Console.Write("s");
                    else if (TailTrail.Contains(new Point(x, y))) Console.Write("#");
                    else Console.Write(".");
                }
            }

            Console.WriteLine();
        }

        private void UpdateTail(Point parentPos, Tail child)
        {
            if (!InRange(parentPos, child.Pos))
            {
                var direction = GetDirectionOfParent(parentPos, child);

                if (direction == Movement.Down || direction == Movement.BottomLeft || direction == Movement.BottomRight)
                {
                    child.Pos = child.Pos with { Y = child.Pos.Y + 1 };
                }

                if (direction == Movement.Up || direction == Movement.TopLeft || direction == Movement.TopRight)
                {
                    child.Pos = child.Pos with { Y = child.Pos.Y - 1 };
                }

                if (direction == Movement.Right || direction == Movement.BottomRight || direction == Movement.TopRight)
                {
                    child.Pos = child.Pos with { X = child.Pos.X + 1 };
                }

                if (direction == Movement.Left || direction == Movement.TopLeft || direction == Movement.BottomLeft)
                {
                    child.Pos = child.Pos with { X = child.Pos.X - 1 };
                }

                if (_snake.Last().Equals(child)) TailTrail.Add(child.Pos);
            }
        }

        private Movement GetDirectionOfParent(Point parentPos, Tail child)
        {
            var matchX = parentPos.X == child.Pos.X;
            var matchY = parentPos.Y == child.Pos.Y;

            var right = parentPos.X > child.Pos.X;
            var left = parentPos.X < child.Pos.X;
            var up = parentPos.Y < child.Pos.Y;
            var down = parentPos.Y > child.Pos.Y;

            if (matchY && right) return Movement.Right;
            if (matchY && left) return Movement.Left;
            if (matchX && down) return Movement.Down;
            if (matchX && up) return Movement.Up;

            if (right && up) return Movement.TopRight;
            if (right && down) return Movement.BottomRight;
            if (left && up) return Movement.TopLeft;
            if (left && down) return Movement.BottomLeft;

            throw new NotSupportedException("Child is occupying the same space as parent");
        }

        private void UpdateHead(Movement movement)
        {
            if (movement == Movement.Left) _head.X--;
            if (movement == Movement.Right) _head.X++;
            if (movement == Movement.Up) _head.Y++;
            if (movement == Movement.Down) _head.Y--;
        }

        private bool InRange(Point a, Point b)
        {
            var xDistance = Math.Abs(a.X - b.X);
            var yDistance = Math.Abs(a.Y - b.Y);

            return xDistance <= 1 && yDistance <= 1;
        }
    }

    class Tail
    {
        public Tail(Point pos)
        {
            Pos = pos;
        }

        public Tail(int x, int y)
        {
            Pos = new Point(x, y);
        }

        public Point Pos { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is Tail tail) return tail.Pos.Equals(Pos);

            return base.Equals(obj);
        }
    }

    enum Movement
    {
        Left,
        Right,
        Up,
        Down,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    class Instruction
    {
        public Instruction(string rawInstruction)
        {
            var line = rawInstruction.Split(' ');

            if (line[0] == "L") Movement = Movement.Left;
            if (line[0] == "R") Movement = Movement.Right;
            if (line[0] == "U") Movement = Movement.Up;
            if (line[0] == "D") Movement = Movement.Down;

            Steps = int.Parse(line[1]);
        }

        public Movement Movement { get; set; }

        public int Steps { get; set; }

        public override string ToString()
        {
            return $"== {Movement} {Steps} ==";
        }
    }
}