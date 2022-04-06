using System.Globalization;

namespace UIM.Core.Services;

public class DashboardService : Service, IDashboardService
{
    public DashboardService(
        IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager
    ) : base(mapper, sieveProcessor, unitOfWork, userManager) { }

    public IEnumerable<SubmissionsSum> SubmissionsSumForEachMonthInYear(string year)
    {
        if (
            !DateTime.TryParseExact(
                year,
                "yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var tmpYear
            )
        )
        {
            throw new HttpException(HttpStatusCode.BadRequest);
        }

        var theYear = tmpYear.Year;
        var submissionsInYear = _unitOfWork.Submissions.Set.Where(
            _ => _.InitialDate.Year == theYear
        );

        var mappedSubs = new List<SubmissionsSum>();
        for (var i = 1; i <= 12; i++)
        {
            var subsInMonth = submissionsInYear.Where(_ => _.InitialDate.Month == i);
            mappedSubs.Add(
                new()
                {
                    Month = i.ToString(),
                    ActiveSubmissions = subsInMonth.Where(_ => _.IsActive).Count(),
                    InactiveSubmissions = subsInMonth.Where(_ => !_.IsActive).Count(),
                }
            );
        }

        return mappedSubs;
    }

    public IEnumerable<TopIdea> TopIdeasInMonthYear(string year, string month )
    {
        var theYear = int.Parse(year);
        var theMonth = int.Parse(month);
        var ideasInMonthYear = _unitOfWork.Ideas.Set.Where(
            _ => _.CreatedDate.Year == theYear && _.CreatedDate.Month == theMonth
        ).Include(x=>x.Comments);
        var topIdeas = ideasInMonthYear.OrderBy(_ => _.Comments.Count);
        var mappedIdeas = new List<TopIdea>();
        foreach (var item in topIdeas)
        {
            mappedIdeas.Add(new()
            {
                Idea = new()
                {
                    Id = EncryptHelpers.EncodeBase64Url(item.Id),
                    Title = item.Title
                },
                CommentNumber = item.Comments.Count
            });
        }
        return mappedIdeas;
    }

    public IEnumerable<MonthActivity> ActivitiesOfEachDayInMonth(string year, string month)
    {
        var theMonth = int.Parse(month);
        var theYear = int.Parse(year);
        var theDays = DateTime.DaysInMonth(theYear, theMonth);

        var ideasInMonth = _unitOfWork.Ideas.Set.Where(
            _ => _.CreatedDate.Year == theYear && _.CreatedDate.Month == theMonth
        );

        var commentsInMonth = new List<Comment>();
        var likesInMonth = new List<Like>();
        var dislikesInMonth = new List<Like>();
        foreach (var idea in ideasInMonth)
        {
            if (idea.Comments != null && idea.Comments.Any())
            {
                commentsInMonth.AddRange(idea.Comments);
            }
            if (idea.Likes != null && idea.Likes.Any())
            {
                likesInMonth.AddRange(idea.Likes.Where(_ => _.IsLike));
                dislikesInMonth.AddRange(idea.Likes.Where(_ => !_.IsLike));
            }
        }

        var activitiesList = new List<MonthActivity>();
        for (var i = 1; i < theDays; i++)
        {
            activitiesList.Add(
                new()
                {
                    Date = new DateTime(theYear, theMonth, i),
                    TotalIdeas = ideasInMonth.Where(_ => _.CreatedDate.Day == i).Count(),
                    TotalLikes = likesInMonth.Where(_ => _.CreatedDate.Day == i).Count(),
                    TotalComments = commentsInMonth.Where(_ => _.CreatedDate.Day == i).Count(),
                    TotalDislikes = dislikesInMonth.Where(_ => _.CreatedDate.Day == i).Count(),
                }
            );
        }

        return activitiesList;
    }
}
