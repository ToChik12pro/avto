
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

            // Поиск пользователя
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == Input.UserName);
            if (user != null && BCrypt.Net.BCrypt.Verify(Input.Password, user.Password))
            {
                // Установка cookie после успешного входа
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                return RedirectToPage("/Index");  // Перенаправление на главную
            }

            ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль.");
            return Page();
        }
    }
}
