using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserManagementApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace UserManagementApp.Controllers
{
    [Authorize]
    public class UserManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display all users, including the current user
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Where(u => !u.IsDeleted)  // Display all non-deleted users, including the current user
                .ToListAsync();

            return View(users);
        }

        // Display all users, including the current user
        public async Task<IActionResult> AllUsers()
        {
            var users = await _context.Users
                .Where(u => !u.IsDeleted)
                .ToListAsync();

            return View(users);
        }

        // Block selected users, and log out if the current user is blocked
        [HttpPost]
        public async Task<IActionResult> BlockUsers(string[] selectedUsers)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Get the current user's ID
            var users = _context.Users.Where(u => selectedUsers.Contains(u.Id)).ToList();
            bool currentUserBlocked = false;

            foreach (var user in users)
            {
                user.Status = "Blocked";
                if (user.Id == currentUserId)
                {
                    currentUserBlocked = true;  // Mark if current user is blocked
                }
            }

            await _context.SaveChangesAsync();

            if (currentUserBlocked)
            {
                // Sign the user out and redirect to the login page
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);  // Explicitly sign out
                return Redirect("/Identity/Account/Login");  // Redirect to login page after sign out
            }

            // If current user is not blocked, remain on the index page
            return RedirectToAction("Index");
        }

        // Unblock selected users
        [HttpPost]
        public async Task<IActionResult> UnblockUsers(string[] selectedUsers)
        {
            var users = _context.Users.Where(u => selectedUsers.Contains(u.Id)).ToList();
            foreach (var user in users)
            {
                user.Status = "Active";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Delete (soft delete) selected users
        [HttpPost]
        public async Task<IActionResult> DeleteUsers(string[] selectedUsers)
        {
            var users = _context.Users.Where(u => selectedUsers.Contains(u.Id)).ToList();
            foreach (var user in users)
            {
                user.IsDeleted = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Block all users, including the current user, and log out if necessary
        [HttpPost]
        public async Task<IActionResult> BlockAllUsers()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Get the current user's ID
            var users = _context.Users.Where(u => !u.IsDeleted).ToList();  // Get all non-deleted users
            bool currentUserBlocked = false;

            foreach (var user in users)
            {
                user.Status = "Blocked";
                if (user.Id == currentUserId)
                {
                    currentUserBlocked = true;  // Mark if current user is blocked
                }
            }

            await _context.SaveChangesAsync();

            if (currentUserBlocked)
            {
                // Sign the user out and redirect to the login page
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return Redirect("/Identity/Account/Login");
            }

            // If current user is not blocked, remain on the index page
            return RedirectToAction("Index");
        }
    }
}
