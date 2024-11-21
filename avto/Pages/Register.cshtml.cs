using avto.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace avto.Pages
{
    public class RegisterModel : PageModel
    {
        // Вводимые данные формы
        public class InputModel
        {
            [Required]
            [StringLength(100)]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [StringLength(100)]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [StringLength(100, MinimumLength = 6)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
            public string ConfirmPassword { get; set; }

            [Phone]
            [StringLength(15)]
            public string Phone { get; set; }

            [StringLength(200)]
            public string Address { get; set; }
        }

        private readonly CarDealershipDbContext _context;

        public RegisterModel(CarDealershipDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Проверка валидности модели
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Пожалуйста, исправьте ошибки и попробуйте снова.");
                return Page();
            }

            try
            {
                // Проверка, существует ли пользователь с таким именем или email
                var userExists = await _context.Users.AnyAsync(u => u.Username == Input.Username || u.Email == Input.Email);

                if (userExists)
                {
                    ModelState.AddModelError(string.Empty, "Пользователь с таким именем или email уже существует.");
                    return Page();
                }

                // Создание нового пользователя
                var user = new User
                {
                    Username = Input.Username,
                    Email = Input.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(Input.Password), // Хэширование пароля
                    Phone = Input.Phone,
                    Address = Input.Address
                };

                // Добавление пользователя в базу данных
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Успешная регистрация
                TempData["Message"] = "Регистрация прошла успешно! Вы можете войти в систему.";

                return RedirectToPage("/Log_IN");
            }
            catch (DbUpdateException dbEx)
            {
                // Обработка ошибки базы данных
                ModelState.AddModelError(string.Empty, "Ошибка при сохранении данных в базе. Попробуйте снова позже.");
                // Логирование ошибки для разработчика
                // Log.Error(dbEx, "Ошибка базы данных при регистрации пользователя");
                return Page();
            }
            catch (Exception ex)
            {
                // Общая обработка ошибок
                ModelState.AddModelError(string.Empty, "Произошла ошибка. Пожалуйста, попробуйте снова позже.");
                // Логирование ошибки для разработчика
                // Log.Error(ex, "Общая ошибка при регистрации пользователя");
                return Page();
            }
        }
    }
}