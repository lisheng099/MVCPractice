using Microsoft.AspNetCore.Mvc;
using MVCPractice.Interfaces;

namespace MyMvcWebsite.Views.Shared.Components.ActivityCategoryViewComponent
{
    public class ActivityCategoryDropDownViewComponent(
        IActivityService activityService) : ViewComponent
    {
        private readonly IActivityService _activityService = activityService;

        public async Task<IViewComponentResult> InvokeAsync(String ActivityCategoryDropDownId, String ActivityCategoryId)
        {
            ViewData["ActivityCategoryDropDownId"] = ActivityCategoryDropDownId;
            ViewData["CurrentActivityCategoryId"] = ActivityCategoryId;
            var activityCategories = await _activityService.GetActivityCategories();

            return View(activityCategories);
        }
    }
}