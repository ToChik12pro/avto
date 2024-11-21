using avto.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace avto.Pages
{
    public class ElitModel : PageModel
    {
        private readonly CarDealershipDbContext _context;

        public ElitModel(CarDealershipDbContext context)
        {
            _context = context;
        }
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

        public IList<Car> Cars { get; set; }

        public void OnGet()
        {
            Cars = _context.Cars
                .Include(c => c.CarImages) // �������� ��������� ����������� �����, ���� ��� ����������
                .Where(c => c.Category == CarCategory.Elite) // ��������� �� ��������� Elite (����)
                .ToList(); // ����������� ��������� � ������
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
            TempData["SuccessMessage"] = "��� ������ ��������� ����������� AVTOSHOW, �������� ������.";

            // �������������� �� ������� ��������, ����� �������� ������ � �������� ���������
            return RedirectToPage();
        }
    }
}
