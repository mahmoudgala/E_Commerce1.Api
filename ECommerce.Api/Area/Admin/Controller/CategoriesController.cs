using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Area.Admin.Controller
{
    [Area(SD.AdminArea)]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepository<Cate> _cateRepository;

        public CategoriesController(IRepository<Cate> cateRepository)
        {
            _cateRepository = cateRepository;
        }
        // CRUD
        // GET / categories
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var categories = await _cateRepository.GetAsync();
            return Ok(categories);
        }

        // GET / categories / {id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _cateRepository.GetOneAsync(e=>e.Id == id);
            if (category is null)
                return NotFound();
            return Ok(category);
        }

        // POST / categories

        [HttpPost]
        public async Task<IActionResult> Create(CategoryRequest cate)
        {
            await _cateRepository.CreateAsync(cate.Adapt<Cate>());
            await _cateRepository.CommitAsync();
            return Ok(new NotificationResponse
            {
                MSG = "category added successfully",
                TraceId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now
            });
        }
        // PUT / categories / {id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryRequest cate)
        {
            var categoryInDB = await _cateRepository.GetOneAsync(e => e.Id == id);
            categoryInDB.CatName = cate.CatName;
            categoryInDB.Description = cate.Description;
            categoryInDB.Status = cate.Status;
            await _cateRepository.CommitAsync();
            return Ok(new NotificationResponse
            {
                MSG = "Category updated successfully",
                TraceId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now
            });
        }
        // DELETE / categories / {id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryInDB = await _cateRepository.GetOneAsync(e=>e.Id == id);
            if(categoryInDB is null)
                return NotFound();
            _cateRepository.Delete(categoryInDB);
            await _cateRepository.CommitAsync();
            return Ok(new NotificationResponse
            {
                MSG = "Category Deleted successfully",
                TraceId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now
            });

        }
    }
}
