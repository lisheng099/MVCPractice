using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCPractice.Areas.Identity.Data;
using MVCPractice.Dtos.Activities;
using MVCPractice.Interfaces;

namespace MVCPractice.Controllers
{
    public class ActivityController(
        ILogger<AccountController> logger,
        UserManager<MVCPracticeUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<MVCPracticeUser> signInManager,
        IActivityService activityService) : Controller
    {
        private readonly ILogger<AccountController> _logger = logger;
        private readonly UserManager<MVCPracticeUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly SignInManager<MVCPracticeUser> _signInManager = signInManager;
        private readonly IActivityService _activityService = activityService;

        public IActionResult Index()
        {
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [HttpGet]
        [Route("Activity/ActivityInfos/{id}")]
        public IActionResult GetActivityInfoById(int id)
        {
            ActivityInfoDto activitieyInfo = _activityService.GetActivityInfoById(id);
            if(_activityService.CheckParticipatedActivityInfoById(id, _userManager.GetUserName(User)))
            {
                activitieyInfo.Participated = true;
            }
            return View("ActivityInfo", activitieyInfo);
        }

        [HttpGet]
        public IActionResult ActivityInfos()
        {
            List<ActivityInfoDto> activityInfos = _activityService.GetActivityInfos(DateTime.Now);
            return View(activityInfos);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult ParticipatedActivityInfos()
        {
            List<ActivityInfoDto> activityInfos = _activityService.GetParticipatedActivityInfos(_userManager.GetUserName(User));
            return View("ActivityInfos", activityInfos);
        }


        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult ParticipatedActivityById(int ActivityId, string UserName, int PersonsNumber)
        {
            _activityService.ParticipatedActivityById( ActivityId,  UserName,  PersonsNumber, _userManager.GetUserName(User));

            return RedirectToAction("GetActivityInfoById", new { id = ActivityId });
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult CancelParticipatedActivityById(int ActivityId, string UserName, int PersonsNumber)
        {

            _activityService.CancelParticipatedActivityById(ActivityId, UserName, PersonsNumber, _userManager.GetUserName(User));

            return RedirectToAction("GetActivityInfoById", new { id = ActivityId });
        }
    }
}
