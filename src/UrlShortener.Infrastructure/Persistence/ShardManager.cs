using Project.Application.Common.Interfaces;
using Shared.Options;
using System.Security.Cryptography;

namespace Project.Infrastructure.Persistence
{
    public class ShardManager : IShardManager
    {


        private readonly SortedDictionary<int, ShardInfo> _ring = new();
        private readonly int _replicas;
        IEnumerable<ShardInfo> _shards;



        public ShardManager(IEnumerable<ShardInfo> nodes, int replicas = 100)
        {
            _shards = nodes;
            _replicas = replicas;
            foreach (var node in nodes)
                AddNode(node);
        }



        public void AddNode(ShardInfo node)
        {
            for (int i = 0; i < _replicas; i++)
            {
                int hash = Hash($"{node.Name}-{i}");
                _ring[hash] = node;
            }
        }


        public IEnumerable<ShardInfo> GetAllNodes()
        {

            return _shards;
        }



        public void RemoveNode(ShardInfo node)
        {
            for (int i = 0; i < _replicas; i++)
            {
                int hash = Hash($"{node.Name}-{i}");
                _ring.Remove(hash);
            }
        }



        public ShardInfo GetNode(string key)
        {
            if (_ring.Count == 0) return null;

            int hash = Hash(key);

            if (_ring.TryGetValue(hash, out var exact))
                return exact;


            foreach (var kv in _ring)
            {
                if (kv.Key >= hash)
                    return kv.Value;
            }

            // wrap around
            return _ring.First().Value;
        }

        private int Hash(string input)
        {
            using var md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            uint value = BitConverter.ToUInt32(hashBytes, 0); //--> takes first 4 bytes
            return (int)(value & 0x7FFFFFFF);
        }




    }
}
