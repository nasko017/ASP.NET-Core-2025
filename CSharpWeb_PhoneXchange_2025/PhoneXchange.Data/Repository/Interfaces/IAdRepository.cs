using PhoneXchange.Data.Models;


namespace PhoneXchange.Data.Repository.Interfaces
{
    public interface IAdRepository:IRepository<Ad,int>,IAsyncRepository<Ad, int>
    {
    }
}
