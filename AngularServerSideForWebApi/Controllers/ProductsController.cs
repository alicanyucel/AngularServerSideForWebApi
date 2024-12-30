using AngularServerSideForWebApi.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DataTableApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly DataContext _context;

    public ProductsController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int start = 0,
        [FromQuery] int length = 10,
        [FromQuery] string searchValue = "",
        [FromQuery] string orderColumn = "Name",
        [FromQuery] string orderDir = "asc")
    {
        var query = _context.Products.AsQueryable();

        // Arama
        if (!string.IsNullOrEmpty(searchValue))
        {
            query = query.Where(p => p.Name.Contains(searchValue) || p.Description.Contains(searchValue));
        }

        // Sıralama
        switch (orderColumn)
        {
            case "Name":
                query = orderDir == "asc" ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
                break;
            case "Price":
                query = orderDir == "asc" ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
                break;
            default:
                query = query.OrderBy(p => p.Name); 
                break;
        }

        // Toplam kayıt sayısı
        var totalRecords = await query.CountAsync();

        // Sayfalama
        var products = await query
            .Skip(start)
            .Take(length)
            .ToListAsync();

        var result = new
        {
            draw = 1, // DataTables için
            recordsTotal = totalRecords,
            recordsFiltered = totalRecords,
            data = products
        };

        return Ok(result);
    }
}