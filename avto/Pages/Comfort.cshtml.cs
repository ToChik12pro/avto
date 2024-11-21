using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using avto.DataBase; // ���������� ������������ ����, ��� ��������� ����� Car
using Microsoft.AspNetCore.Mvc;
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

        // ����� ��� ��������� ������ �����
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string Message { get; set; }
        }

        public async Task OnGetAsync()
        {
            Cars = await _context.Cars
                .Include(c => c.CarImages)
                .Where(c => c.Category == CarCategory.Comfort)
                .ToListAsync();

            if (Cars == null)
            {
                Cars = new List<Car>();  // ������������� ������ �������, ���� ��������� ������� ����� null
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // ���������, ���������� �� ��� ������ � ����� email
            var existingClient = await _context.Clients
                .FirstOrDefaultAsync(c => c.Email == Input.Email);

            if (existingClient != null)
            {
                // ���� ������ � ����� email ��� ����������, ���������� ������ ��� �����������
                ModelState.AddModelError("Input.Email", "������ � ����� email ��� ����������.");
                return Page();
            }

            // ������ ����� ���������� ������
            var client = new Client
            {
                Name = Input.Name,
                Email = Input.Email,
                Phone = Input.Phone,
                Address = Input.Address,
                Message = Input.Message,
            };

            // ��������� ������� � ���� ������
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // ������������� ��������� �� ������ � TempData
            TempData["SuccessMessage"] = "��� ������ ��������� ���������� AVTOSHOW, �������� ������.";

            // �������������� �� ������� ��������, ����� �������� ������ � �������� ���������
            return RedirectToPage();
        }
    }
}
