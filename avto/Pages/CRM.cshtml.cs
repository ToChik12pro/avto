using avto.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace avto.Pages
{
    [Authorize]
    public class CRMPageModel : PageModel
    {
        private readonly CarDealershipDbContext _context;
        private readonly ILogger<CRMPageModel> _logger;

        public CRMPageModel(CarDealershipDbContext context, ILogger<CRMPageModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // Получаем UserId из claims
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"userIdString: {userIdString}");

            if (string.IsNullOrEmpty(userIdString))
            {
                Console.WriteLine("UserId is empty or not found.");
                return RedirectToPage("/AccessDenied"); // Если нет userId
            }

            // Преобразуем строку в int
            if (!int.TryParse(userIdString, out var userId))
            {
                Console.WriteLine($"Invalid userId value: {userIdString}");
                return RedirectToPage("/AccessDenied");
            }

            // Загружаем пользователя из базы данных по userId
            var user = _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                Console.WriteLine($"User with id {userId} not found.");
                return RedirectToPage("/AccessDenied");  // Пользователь не найден
            }

            Console.WriteLine($"User found: {user.Username}, Roles: {string.Join(", ", user.UserRoles.Select(ur => ur.Role.RoleName))}");

            // Проверяем наличие роли с RoleId == 1
            if (user.UserRoles.Any(ur => ur.RoleId == 1))  // Проверка по RoleId
            {
                Console.WriteLine("Access granted.");
                return Page();  // Доступ разрешен
            }

            Console.WriteLine("Access denied.");
            return RedirectToPage("/AccessDenied");  // Перенаправление на страницу отказа в доступе
        }

        public async Task<IActionResult> OnGetSectionAsync(string section)
        {
            switch (section)
            {
                case "Users":
                    var users = await _context.Users.ToListAsync();
                    return PartialView("_Users", users);

                case "Clients":
                    var clients = await _context.Clients.ToListAsync();
                    return PartialView("_Clients", clients);

                case "Cars":
                    var cars = await _context.Cars.Include(c => c.CarImages).ToListAsync();
                    return PartialView("_Cars", cars);

                case "Orders":
                    var orders = await _context.Orders.Include(o => o.Client).Include(o => o.User).ToListAsync();
                    return PartialView("_Orders", orders);

                default:
                    return Content("<p>Выбранный раздел не найден.</p>");
            }
        }

        private PartialViewResult PartialView<T>(string partialViewName, T model)
        {
            return new PartialViewResult
            {
                ViewName = $"Shared/{partialViewName}",
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<T>(
                    ViewData,
                    model
                )
            };
        }
    }
}
