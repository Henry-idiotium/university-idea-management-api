namespace UIM.Core.Data.Repositories.Interfaces;

public interface IDashboardService
{
    IEnumerable<MonthActivity> ActivitiesOfEachDayInMonth(string month, string year);
    IEnumerable<SubmissionsSum> SubmissionsSumForEachMonthInYear(string year);
    IEnumerable<TopIdea> TopIdeasInMonthYear(string month, string year);
}
