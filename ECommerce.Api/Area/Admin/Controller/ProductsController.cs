using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Api.Area.Admin.Controller
{
    [Area(SD.AdminArea)]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAsync(includes:[e=>e.Cate, e=>e.Brand]);
            var productResponse= products.Adapt<List<ProductResponse>>();
            return Ok (productResponse);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetOneAsync(e => e.Id == id, includes: [e => e.Cate, e => e.Brand]);
            if (product is null)
                return NotFound();
            return Ok(product.Adapt<ProductResponse>());
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ProductCreateRequest productCreateRequest)
        {
            if (productCreateRequest.MainImg is not null && productCreateRequest.MainImg.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productCreateRequest.MainImg.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images" , fileName); // "wwwroot\\images" _Should be Server Link

                //save img in root 
                using (var stream = System.IO.File.Create(filePath))
                {
                    await productCreateRequest.MainImg.CopyToAsync(stream);
                }

                //save img name in DB
                var product = productCreateRequest.Adapt<Product>();
                product.MainImg = fileName;

                //save in DB
                var productReturned = await _productRepository.CreateAsync(product);
                await _productRepository.CommitAsync();
                //return Created($"{Request.Scheme}//{Request.Host}/api/Admin/products/{productReturned.Id}",new NotificationResponse
                //{
                //    MSG = "Product created successfully",
                //    TraceId = Guid.NewGuid().ToString(),
                //    CreatedAt = DateTime.Now  
                //});
                return CreatedAtAction(nameof(Details), new { id = product.Id } ,
                    new NotificationResponse { 
                    
                        MSG = "Product created successfully",
                        TraceId = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.Now

                    });

            }
            return BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id,[FromForm] ProductUpdateRequest productUpdateRequest)
        {
            var productInDB = await _productRepository.GetOneAsync(e=>e.Id == id, tracked: false);

            if (productInDB is null)
                return BadRequest();
            var product = productUpdateRequest.Adapt<Product>();
            product.Id = id;
            if(productUpdateRequest.MainImg is not  null&& productUpdateRequest.MainImg.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productUpdateRequest.MainImg.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await productUpdateRequest.MainImg.CopyToAsync(stream);
                }
                //delete old img 
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", productInDB.MainImg);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
                //update img in db 
                product.MainImg = fileName;

            }
           
            else
            {
                product.MainImg = productInDB.MainImg;
            }
            //update in DB
            _productRepository.Update(product);
            await _productRepository.CommitAsync();
            return NoContent();
        }
    }
}
