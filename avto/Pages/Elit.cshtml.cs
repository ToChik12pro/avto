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
                .Include(c => c.CarImages) // Включаем связанные изображения машин, если это необходимо
                .Where(c => c.Category == CarCategory.Elite) // Фильтруем по категории Elite (Элит)
                .ToList(); // Преобразуем результат в список
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Проверяем, существует ли уже клиент с таким email
            var existingClient = await _context.Clients
                .FirstOrDefaultAsync(c => c.Email == Input.Email);

            if (existingClient != null)
            {
                // Если клиент с таким email уже существует, возвращаем ошибку или уведомление
                ModelState.AddModelError("Input.Email", "Клиент с таким email уже существует.");
                return Page();
            }

            // Создаём новый клиентский объект
            var client = new Client
            {
                Name = Input.Name,
                Email = Input.Email,
                Phone = Input.Phone,
                Address = Input.Address,
                Message = Input.Message,
            };

            // Добавляем клиента в базу данных
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Устанавливаем сообщение об успехе в TempData
            TempData["SuccessMessage"] = "Ваш запрос отправлен менеджероам AVTOSHOW, ожидайте ответа.";

            // Перенаправляем на текущую страницу, чтобы обновить данные и показать сообщение
            return RedirectToPage();
        }
    }
}
