using System.Collections.Concurrent;

namespace AdventOfCode2022.DaySix;

public class StartOfStreamPacketEstimator
{
    public int GetStartOfStreamIndex()
    {
        var input = File.ReadAllText("DaySix/dataStream.txt");

        var packetQueue = new PacketQueue<char>(14);

        int counter = 1;
        foreach (char character in input)
        {
            packetQueue.Enqueue(character);

            if (packetQueue.IsStartOfStreamPacket())
            {
                return counter;
            }

            counter++;
        }

        return counter;
    }

    class PacketQueue<T>
    {
        private readonly int _limit;

        private readonly ConcurrentQueue<T> _queue = new();

        private readonly object _lockObject = new();

        public PacketQueue(int limit)
        {
            _limit = limit;
        }

        public void Enqueue(T obj)
        {
            _queue.Enqueue(obj);

            lock (_lockObject)
            {
                while (_queue.Count > _limit && _queue.TryDequeue(out _)) { }
            }
        }

        public bool IsStartOfStreamPacket()
        {
            var list = _queue.ToList();

            if (_queue.Count < _limit) return false;

            List<T> uniqueBuffer = new List<T>();
            foreach (T var in list)
            {
                if (uniqueBuffer.Contains(var))
                {
                    return false;
                }
                else
                {
                    uniqueBuffer.Add(var);
                }
            }

            return true;
        }
    }
}