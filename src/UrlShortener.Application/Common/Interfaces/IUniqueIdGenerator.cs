namespace Project.Application.Common.Interfaces
{
    public interface IUniqueIdGenerator
    {


        public Task<long> GetNextId();
    }
}
