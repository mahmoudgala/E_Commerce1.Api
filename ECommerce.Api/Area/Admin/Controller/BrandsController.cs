using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Area.Admin.Controller
{
    [Area(SD.AdminArea)]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IRepository<Brand> _brandRepository;

        public BrandsController(IRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
        }
        // CRUD
        // GET / brands
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var brands = await _brandRepository.GetAsync();
            return Ok(brands);
        }

        // GET / brands / {id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var brandgory = await _brandRepository.GetOneAsync(e=>e.Id == id);
            if (brandgory is null)
                return NotFound();
            return Ok(brandgory);
        }

        // POST / brands

        [HttpPost]
        public async Task<IActionResult> Create(BrandRequest brand)
        {
            await _brandRepository.CreateAsync(brand.Adapt<Brand>());
            await _brandRepository.CommitAsync();
            return Ok(new NotificationResponse
            {
                MSG = "Brand added successfully",
                TraceId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now
            });
        }
        // PUT / brands / {id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BrandRequest brand)
        {
            var brandgoryInDB = await _brandRepository.GetOneAsync(e => e.Id == id);
            brandgoryInDB.BrandName = brand.BrandName;
            brandgoryInDB.Description = brand.Description;
            brandgoryInDB.Status = brand.Status;
            await _brandRepository.CommitAsync();
            return Ok(new NotificationResponse
            {
                MSG = "Brand updated successfully",
                TraceId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now
            });
        }
        // DELETE / brands / {id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var brandgoryInDB = await _brandRepository.GetOneAsync(e=>e.Id == id);
            if(brandgoryInDB is null)
                return NotFound();
            _brandRepository.Delete(brandgoryInDB);
            await _brandRepository.CommitAsync();
            return Ok(new NotificationResponse
            {
                MSG = "Brand Deleted successfully",
                TraceId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now
            });

        }
    }
}
