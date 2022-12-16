class Monkey
{
    public Queue<long> items = new Queue<long>();
    public Func<long, long>? operation;
    public Predicate<long>? test;
    public int TrueMonkey;
    public int FalseMonkey;
    public long numberOfInspections = 0;
}
