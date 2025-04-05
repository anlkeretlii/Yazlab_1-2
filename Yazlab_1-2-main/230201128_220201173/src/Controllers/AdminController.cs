using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEventPlanningPlatform.Models;
using SmartEventPlanningPlatform.Services;

namespace SmartEventPlanningPlatform.Controllers
{
    public class AdminController : Controller
    {
        public readonly AdminService _adminService;
        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }
        public IActionResult Index()
        {
            var eventts = _adminService.getAllEvents();
            var users = _adminService.getAllUsers();
            var model = new AdminViewModel
            {
                Events = eventts,
                Users = users
            };
            return View(model);
        }
        [HttpPost]
        
        public IActionResult DeleteUser(int userId)
        {
            _adminService.deleteUser(userId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        
        public IActionResult DeleteEvent(int eventId)
        {
            _adminService.deleteEvent(eventId);
            return RedirectToAction("Index");
        }

        public IActionResult Events()
        {
            var eventts = _adminService.getAllEvents();
            return View(eventts);
        }
        public IActionResult Users()
        {
            var users = _adminService.getAllUsers();
            return View(users);
        }
        public IActionResult EventDetails(int eventId)
        {
            var evnt = _adminService.getEventById(eventId);
            return View(evnt);
        }
        public IActionResult UserDetails(int userId)
        {
            var user = _adminService.getUserById(userId);
            return View(user);
        }


    }
    public class AdminViewModel
    {
        public List<Event> Events { get; set; }
        public List<User> Users { get; set; }
    }
}
