
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Text;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.IdentityModel.Tokens;
    using avto.DataBase;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace avto.Pages
{

    public class Log_INModel : PageModel
    {
        private readonly CarDealershipDbContext _context;

        [BindProperty]
        public InputModel Input { get; set; }

        public Log_INModel(CarDealershipDbContext context)
        {
            _context = context;
        }

        public class InputModel
        {
            [Required(ErrorMessage = "Имя пользователя обязательно.")]
            [Display(Name = "Имя пользователя")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Пароль обязателен.")]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [Display(Name = "Запомнить меня")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Загрузка пользователя с ролями
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == Input.UserName);

            if (user != null && BCrypt.Net.BCrypt.Verify(Input.Password, user.Password))
            {
                // Проверка на наличие роли с RoleId = 1
                var hasManagerRole = user.UserRoles.Any(ur => ur.RoleId == 1);

                // Создаем claims
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())  // Добавление UserId
            };

                // Добавляем роли в claims
                foreach (var role in user.UserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Role.RoleName));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Устанавливаем cookie
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                // Перенаправление в зависимости от роли
                if (hasManagerRole)
                {
                    return RedirectToPage("/CRM");
                }

                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль.");
            return Page();
        }
    }
}