using Microsoft.AspNetCore.Mvc;
using MVCPractice.Interfaces;
using MVCPractice.servers;

namespace MyMvcWebsite.Views.Shared.Components.ActivityCategoryViewComponent
{
    public class ActivityCategoryDropDownViewComponent(
        IMVCPracticeDBServices MVCPracticeDBServices) : ViewComponent
    {
        private readonly IMVCPracticeDBServices _MVCPracticeDBServices = MVCPracticeDBServices;

        public async Task<IViewComponentResult> InvokeAsync(String ActivityCategoryDropDownId, String ActivityCategoryId)
        {
            ViewData["ActivityCategoryDropDownId"] = ActivityCategoryDropDownId;
            ViewData["CurrentActivityCategoryId"] = ActivityCategoryId;
            return View(_MVCPracticeDBServices.GetActivityCategories());
        }
    }
}
