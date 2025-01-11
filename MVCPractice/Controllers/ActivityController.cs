using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCPractice.Dtos.Activities;
using MVCPractice.Interfaces;
using MVCPractice.Parameters.Activity;
using System.Security.Claims;

namespace MVCPractice.Controllers
{
    [Route("[controller]")]
    public class ActivityController(
        IActivityService activityService) : Controller
    {
        private readonly IActivityService _activityService = activityService;

        [HttpGet("ActivityInfos")]
        [AllowAnonymous]
        public async Task<IActionResult> ActivityInfos()
        {
            List<ActivityInfoDto> activityInfos =
                await _activityService.GetActivityInfos(
                new GetActivityParameter()
                {
                    StartDateTime = DateTime.Now,
                    EndDateTime = DateTime.MaxValue,
                });
            return View(activityInfos);
        }

        [HttpGet("ActivityInfos/{id}")]
        public async Task<IActionResult> GetActivityInfoById(Guid id)
        {
            ActivityInfoDto activitieyInfo =
                (await _activityService.GetActivityInfos(
                new GetActivityParameter()
                { 
                    ActivityId= id
                })).FirstOrDefault();
            if (await _activityService.CheckParticipatedActivityInfoById(
                new CheckParticipatedActivityParameter()
                {
                    ActivityId = id,
                    UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                }))
            {
                activitieyInfo.Participated = true;
            }
            return View("ActivityInfo", activitieyInfo);
        }

        [HttpGet("ParticipatedActivityInfos")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ParticipatedActivityInfos()
        {
            List<ActivityInfoDto> activityInfos =
                await _activityService.GetActivityInfos(
                new GetActivityParameter()
                {
                    UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    CheckIsParticipated = true
                });
            return View("ActivityInfos", activityInfos);
        }

        [HttpPost("ParticipatedActivityById")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ParticipatedActivityById(Guid ActivityId)
        {
            await _activityService.ParticipatedActivityById(
                new ParticipatedActivityDto()
                {
                    ActivityId = ActivityId,
                    UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    PersonsNumber = 1
                });

            return RedirectToAction("GetActivityInfoById", new { id = ActivityId });
        }

        [HttpPost("CancelParticipatedActivityById")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CancelParticipatedActivityById(Guid ActivityId)
        {
            await _activityService.CancelParticipatedActivityById(
                new CancelParticipatedActivityDto()
                {
                    ActivityId = ActivityId,
                    UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                });
            return RedirectToAction("GetActivityInfoById", new { id = ActivityId });
        }
    }
}