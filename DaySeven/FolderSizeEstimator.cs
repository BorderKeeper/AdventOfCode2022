namespace AdventOfCode2022.DaySeven;

public class FolderSizeEstimator
{
    public int SumAllSmallFolders()
    {
        var consoleOutput = System.IO.File.ReadAllLines("DaySeven/commandLineOutput.txt");

        var lines = new List<Line>();

        foreach (string rawLine in consoleOutput)
        {
            lines.Add(GetLine(rawLine));
        }

        var folderList = new List<Folder>();

        var root = new Folder("/");
        folderList.Add(root);

        var currentFolder = root;
        foreach (Line line in lines)
        {
            switch (line.Type)
            {
                case LineType.File:
                    currentFolder.Files.Add(new File(line));
                    break;
                case LineType.Directory:
                    var folder = new Folder(line, currentFolder);

                    currentFolder.Folders.Add(folder);
                    folderList.Add(folder);
                    break;
                case LineType.ChangeDirectory:
                    if (line.Argument == "..")
                    {
                        currentFolder = currentFolder.Parent;
                        break;
                    }

                    if (line.Argument == "/")
                    {
                        currentFolder = root;
                        break;
                    }

                    currentFolder = currentFolder.Folders.First(f => f.Name.Equals(line.Argument));
                    break;
                case LineType.ListContents:
                    break;
            }
        }

        var maximumSpace = 70000000;
        var neededSpace = 30000000;
        var rootSize = GetFolderSize(root);

        var spaceAvailable = maximumSpace - rootSize;
        var sizeToDelete = neededSpace - spaceAvailable;

        var smallestDirectoryThatFits = folderList.Select(GetFolderSize).Where(s => s >= sizeToDelete).Min();

        return smallestDirectoryThatFits;
    }

    public int GetFolderSize(Folder folder)
    {
        var size = 0;

        size += folder.Files.Sum(f => f.Size);

        foreach (Folder subFolder in folder.Folders)
        {
            size += GetFolderSize(subFolder);
        }

        return size;
    }

    public class Folder
    {
        public Folder(string name)
        {
            Name = name;
            Parent = null;
            Folders = new List<Folder>();
            Files = new List<File>();
        }

        public Folder(Line line, Folder parent)
        {
            if (line.Type != LineType.Directory)
            {
                throw new NotSupportedException("This line is not a directory but a " + line.Type);
            }

            Name = line.Name;
            Parent = parent;
            Folders = new List<Folder>();
            Files = new List<File>();
        }

        public string Name { get; set; }

        public Folder? Parent { get; set; }

        public List<Folder> Folders { get; set; }

        public List<File> Files { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }

    public class File
    {
        public File(Line line)
        {
            if (line.Type != LineType.File)
            {
                throw new NotSupportedException("This line is not a file but a " + line.Type);
            }

            Name = line.Name;
            Size = line.Size;
        }

        public string Name { get; set; }

        public int Size { get; set; }

        public override string ToString()
        {
            return $"{Size} {Name}";
        }
    }

    public enum LineType
    {
        ChangeDirectory,
        ListContents,
        Directory,
        File
    }

    public class Line
    {
        public LineType Type { get; set; }

        public string Argument { get; set; }

        public int Size { get; set; }

        public string Name { get; set; }
    }

    public Line GetLine(string line)
    {
        if (line[0] == '$')
        {
            if (line[2] == 'c')
            {
                return new Line
                {
                    Type = LineType.ChangeDirectory,
                    Argument = line.Substring(5)
                };
            }

            if (line[2] == 'l')
            {
                return new Line
                {
                    Type = LineType.ListContents
                };
            }
        }
        else
        {
            if (line[0] == 'd')
            {
                return new Line
                {
                    Type = LineType.Directory,
                    Name = line.Substring(4)
                };
            }

            var splitFileLine = line.Split(" ");

            return new Line
            {
                Type = LineType.File,
                Name = splitFileLine[1],
                Size = int.Parse(splitFileLine[0])
            };
        }

        throw new NotSupportedException("This line is not supported: " + line);
    }
}