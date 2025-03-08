using avto.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;

namespace avto.Pages
{
    public class CatalogModel : PageModel
    {
        private readonly CarDealershipDbContext _context;
        public List<Car> Cars { get; set; } = new();

        public CatalogModel(CarDealershipDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Load the cars along with their associated images
            Cars = await _context.Cars
                .Include(car => car.CarImages)  // Include the related images
                .ToListAsync();

            return Page();
        }
    }
}
