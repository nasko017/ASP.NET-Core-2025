using PhoneXchange.Data.Models;

namespace PhoneXchange.Data.Repository.Interfaces
{
    public interface IReviewRepository : IRepository<Review, int>, IAsyncRepository<Review, int>
    {
    }
}
