
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
            [Required(ErrorMessage = "��� ������������ �����������.")]
            [Display(Name = "��� ������������")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "������ ����������.")]
            [DataType(DataType.Password)]
            [Display(Name = "������")]
            public string Password { get; set; }

            [Display(Name = "��������� ����")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // ����� ������������
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == Input.UserName);
            if (user != null && BCrypt.Net.BCrypt.Verify(Input.Password, user.Password))
            {
                // ��������� cookie ����� ��������� �����
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                return RedirectToPage("/Index");  // ��������������� �� �������
            }

            ModelState.AddModelError(string.Empty, "�������� ��� ������������ ��� ������.");
            return Page();
        }
    }
}
