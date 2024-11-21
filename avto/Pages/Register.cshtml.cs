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
        // �������� ������ �����
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
            [Compare("Password", ErrorMessage = "������ �� ���������.")]
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
            // �������� ���������� ������
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "����������, ��������� ������ � ���������� �����.");
                return Page();
            }

            try
            {
                // ��������, ���������� �� ������������ � ����� ������ ��� email
                var userExists = await _context.Users.AnyAsync(u => u.Username == Input.Username || u.Email == Input.Email);

                if (userExists)
                {
                    ModelState.AddModelError(string.Empty, "������������ � ����� ������ ��� email ��� ����������.");
                    return Page();
                }

                // �������� ������ ������������
                var user = new User
                {
                    Username = Input.Username,
                    Email = Input.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(Input.Password), // ����������� ������
                    Phone = Input.Phone,
                    Address = Input.Address
                };

                // ���������� ������������ � ���� ������
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // �������� �����������
                TempData["Message"] = "����������� ������ �������! �� ������ ����� � �������.";

                return RedirectToPage("/Log_IN");
            }
            catch (DbUpdateException dbEx)
            {
                // ��������� ������ ���� ������
                ModelState.AddModelError(string.Empty, "������ ��� ���������� ������ � ����. ���������� ����� �����.");
                // ����������� ������ ��� ������������
                // Log.Error(dbEx, "������ ���� ������ ��� ����������� ������������");
                return Page();
            }
            catch (Exception ex)
            {
                // ����� ��������� ������
                ModelState.AddModelError(string.Empty, "��������� ������. ����������, ���������� ����� �����.");
                // ����������� ������ ��� ������������
                // Log.Error(ex, "����� ������ ��� ����������� ������������");
                return Page();
            }
        }
    }
}