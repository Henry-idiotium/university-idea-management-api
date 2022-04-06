namespace UIM.Core.Controllers.Admin;

[JwtAuthorize(RoleNames.Admin)]
[Route("api/[controller]")]
public class DashboardController : UimController
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("sum-submissions")]
    public IActionResult ReadSubmissionsSums(string year) =>
        ResponseResult(_dashboardService.SubmissionsSumForEachMonthInYear(year));

    [HttpGet("top-ideas")]
    public IActionResult ReadTopIdeas(string year, string month) =>
        ResponseResult(_dashboardService.TopIdeasInMonthYear(year, month));

    [HttpGet("activities")]
    public IActionResult ReadActivities(string year, string month) =>
        ResponseResult(_dashboardService.ActivitiesOfEachDayInMonth(year, month));
}
