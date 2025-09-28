using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Api.Area.Admin.Controller
{
    [Area(SD.AdminArea)]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public StatisticsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var products = (await _productRepository.GetAsync(includes: [equals => equals.Cate])).GroupBy(e => e.Cate.CatName).Select(e => new
            {
                e.Key,
                avg = e.Average(e => e.Price).ToString("c"),
                count = e.Count(),
                sum = e.Sum(e=>e.Price).ToString("c")
            });
            return Ok(products);
        }
    }
}
