namespace AdventOfCode2022.DayEight;

public class TreeHouseFinder
{
    public int ForestWidth { get; set; }

    public int ForestHeight { get; set; }

    public Tree[,] Trees { get; set; }

    public int GetMaximumScenicIndex()
    {
        var rawData = File.ReadAllLines("DayEight/forest.txt");

        ForestWidth = rawData[0].Length;
        ForestHeight = rawData.Length;
        Trees = new Tree[ForestWidth, ForestHeight];

        PopulateForest(rawData);

        int visibleTrees = 0;

        int maxScenicIndex = 0;
        for (int row = 0; row < ForestHeight; row++)
        {
            for (int column = 0; column < ForestWidth; column++)
            {
                var tree = Trees[row, column];

                if ((VisibleFromLeft(tree) ||
                    VisibleFromRight(tree) ||
                    VisibleFromBottom(tree) ||
                    VisibleFromTop(tree)))
                {
                    visibleTrees++;
                }

                var left = TreesSeenLeft(tree);
                var right = TreesSeenRight(tree);
                var top = TreesSeenTop(tree);
                var bottom = TreesSeenBottom(tree);

                var scenicIndex = left * right * top * bottom;
                Console.WriteLine($"{top} * {left} * {right} * {bottom} = {scenicIndex}");

                if (scenicIndex > maxScenicIndex) maxScenicIndex = scenicIndex;
            }
        }

        return maxScenicIndex;
        //374556 too low
        //5764801 too high
    }

    public bool VisibleFromLeft(Tree tree)
    {
        for (int i = tree.Column - 1; i >= 0; i--)
        {
            var leftTree = Trees[tree.Row, i];

            if (leftTree.Height >= tree.Height)
            {
                return false;
            }
        }

        return true;
    }

    public int TreesSeenLeft(Tree tree)
    {
        int counter = 0;

        for (int i = tree.Column - 1; i >= 0; i--)
        {
            var leftTree = Trees[tree.Row, i];

            counter++;

            if (leftTree.Height >= tree.Height)
            {
                break;
            }
        }

        return counter;
    }

    public bool VisibleFromRight(Tree tree)
    {
        for (int i = tree.Column + 1; i < ForestWidth; i++)
        {
            var rightTree = Trees[tree.Row, i];

            if (rightTree.Height >= tree.Height)
            {
                return false;
            }
        }

        return true;
    }

    public int TreesSeenRight(Tree tree)
    {
        int counter = 0;

        for (int i = tree.Column + 1; i < ForestWidth; i++)
        {
            var rightTree = Trees[tree.Row, i];

            counter++;

            if (rightTree.Height >= tree.Height)
            {
                break;
            }
        }

        return counter;
    }

    public bool VisibleFromTop(Tree tree)
    {
        for (int i = tree.Row - 1; i >= 0; i--)
        {
            var topTree = Trees[i, tree.Column];

            if (topTree.Height >= tree.Height)
            {
                return false;
            }
        }

        return true;
    }

    public int TreesSeenTop(Tree tree)
    {
        int counter = 0;

        for (int i = tree.Row - 1; i >= 0; i--)
        {
            var topTree = Trees[i, tree.Column];

            counter++;

            if (topTree.Height >= tree.Height)
            {
                break;
            }
        }

        return counter;
    }

    public bool VisibleFromBottom(Tree tree)
    {
        for (int i = tree.Row + 1; i < ForestHeight; i++)
        {
            var bottomTree = Trees[i, tree.Column];

            if (bottomTree.Height >= tree.Height)
            {
                return false;
            }
        }

        return true;
    }

    public int TreesSeenBottom(Tree tree)
    {
        int counter = 0;

        for (int i = tree.Row + 1; i < ForestHeight; i++)
        {
            var bottomTree = Trees[i, tree.Column];

            counter++;

            if (bottomTree.Height >= tree.Height)
            {
                break;
            }
        }

        return counter;
    }


    public class Tree
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public char Height { get; set; }
    }

    private void PopulateForest(string[] rawData)
    {
        int row = 0;
        foreach (string rowOfTrees in rawData)
        {
            int column = 0;
            foreach (char treeHeight in rowOfTrees)
            {
                Trees[row, column] = new Tree
                {
                    Column = column,
                    Row = row,
                    Height = treeHeight
                };

                column++;
            }

            row++;
        }
    }
}