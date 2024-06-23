using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using avto.DataBase; // Подключаем пространство имен, где определен класс Car
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace avto.Pages
{

    public class ComfortModel : PageModel
    {
        private readonly CarDealershipDbContext _context;

        public ComfortModel(CarDealershipDbContext context)
        {
            _context = context;
        }

        public IList<Car> Cars { get; set; }

        public async Task OnGetAsync()
        {
            Cars = await _context.Cars
                .Include(c => c.CarImages)
                .Where(c => c.Category == CarCategory.Comfort)
                .ToListAsync();
        }
    }
}
