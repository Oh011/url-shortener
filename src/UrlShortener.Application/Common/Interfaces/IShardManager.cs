using Shared.Options;

namespace Project.Application.Common.Interfaces
{
    public interface IShardManager
    {


        public void AddNode(ShardInfo node);
        IEnumerable<ShardInfo> GetAllNodes();
        public void RemoveNode(ShardInfo node);
        public ShardInfo GetNode(string key);

    }
}
