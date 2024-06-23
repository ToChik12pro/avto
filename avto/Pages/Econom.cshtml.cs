using avto.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace avto.Pages
{
    public class EconomModel : PageModel
    {
        private readonly CarDealershipDbContext _context;

        public EconomModel(CarDealershipDbContext context)
        {
            _context = context;
        }

        public IList<Car> Cars { get; set; }

        public void OnGet()
        {
            Cars = _context.Cars
                .Include(c => c.CarImages) // �������� ��������� ����������� �����, ���� ��� ����������
                .Where(c => c.Category == CarCategory.Economy) // ��������� �� ��������� Elite (����)
                .ToList(); // ����������� ��������� � ������
        }
    }
}
  