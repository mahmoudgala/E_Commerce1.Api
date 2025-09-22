namespace ECommerce.Api.Repositories.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task AddRangeAsync(List<Product> products);
    }
}
