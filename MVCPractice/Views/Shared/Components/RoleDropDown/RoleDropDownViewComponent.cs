using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MyMvcWebsite.Views.Shared.Components.RoleDropDownViewComponent
{
    public class RoleDropDownViewComponent(
        RoleManager<IdentityRole> roleManager) : ViewComponent
    {
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task<IViewComponentResult> InvokeAsync(String Role)
        {
            ViewData["Roles"] = _roleManager.Roles.Select(r => r.Name).ToList();
            ViewData["CurrentRole"] = Role;
            return View();
        }
    }
}
