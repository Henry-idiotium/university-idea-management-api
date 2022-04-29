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

    public async Task<AllTotal> TotalAllAspectsAsync()
    {
        var likeness = _unitOfWork.Ideas.GetAllLikeness();
        return new()
        {
            TotalIdeas = await _unitOfWork.Ideas.CountAsync(),
            TotalViews = _unitOfWork.Ideas.GetViews().Count(),
            TotalComments = await _unitOfWork.Comments.CountAsync(),
            TotalLikes = likeness?.Where(_ => _.IsLike)?.Count() ?? 0,
            TotalSubmissions = await _unitOfWork.Submissions.CountAsync(),
            TotalDislikes = likeness?.Where(_ => !_.IsLike)?.Count() ?? 0,
        };
    }

    public IEnumerable<SubmissionsSum> SubmissionsSumForEachMonthInYear(string year)
    {
        var submissionsInYear = _unitOfWork.Submissions.Set.Where(
            _ => _.InitialDate.Year == int.Parse(year)
        );

        var mappedSubs = new List<SubmissionsSum>();

        for (var i = 1; i <= 12; i++)
        {
            var subsInMonth = submissionsInYear.Where(_ => _.InitialDate.Month == i).AsEnumerable();
            SubmissionsSum sub = new();

            if (!subsInMonth.IsNullOrEmpty())
            {
                sub.Month = i.ToString();
                sub.ActiveSubmissions = subsInMonth.Where(_ => _.IsFullyClose != true).Count();
                sub.InactiveSubmissions = subsInMonth.Where(_ => _.IsFullyClose == true).Count();
            }
            else
                sub = new()
                {
                    Month = i.ToString(),
                    ActiveSubmissions = 0,
                    InactiveSubmissions = 0,
                };

            mappedSubs.Add(sub);
        }

        return mappedSubs;
    }

    public IEnumerable<TopIdea> TopIdeasInMonthYear(string year, string month)
    {
        var theYear = int.Parse(year);
        var theMonth = int.Parse(month);
        var ideasInMonthYear = _unitOfWork.Ideas.Set
            .Where(_ => _.CreatedDate.Year == theYear && _.CreatedDate.Month == theMonth)
            .Include(x => x.Comments);
        var topIdeas = ideasInMonthYear.OrderByDescending(_ => _.Comments.Count).Take(3);

        var mappedIdeas = new List<TopIdea>();
        foreach (var item in topIdeas)
            mappedIdeas.Add(
                new()
                {
                    Idea = _mapper.Map<SimpleIdeaResponse>(item),
                    CommentNumber = item.Comments.Count
                }
            );
        return mappedIdeas;
    }

    public IEnumerable<MonthActivity> ActivitiesOfEachDayInMonth(string year, string month)
    {
        var theMonth = int.Parse(month);
        var theYear = int.Parse(year);
        var theDays = DateTime.DaysInMonth(theYear, theMonth);

        Console.WriteLine("-------------------------------------------");
        Console.WriteLine(theYear);
        Console.WriteLine(theMonth);
        Console.WriteLine("-------------------------------------------");

        var ideasInMonth = _unitOfWork.Ideas.Set.Where(
            _ => _.CreatedDate.Year == theYear && _.CreatedDate.Month == theMonth
        );

        var commentsInMonth = new List<Comment>();
        var likesInMonth = new List<Like>();
        var dislikesInMonth = new List<Like>();
        var viewsInMonth = new List<View>();

        foreach (var idea in ideasInMonth)
        {
            viewsInMonth.AddRange(_unitOfWork.Ideas.GetViews(idea.Id));
            commentsInMonth.AddRange(_unitOfWork.Comments.Set.Where(_ => _.IdeaId == idea.Id));
            likesInMonth.AddRange(_unitOfWork.Ideas.GetLikes(idea.Id));
            dislikesInMonth.AddRange(_unitOfWork.Ideas.GetDislikes(idea.Id));
        }

        var activitiesList = new List<MonthActivity>();
        for (var i = 1; i <= theDays; i++)
        {
            activitiesList.Add(
                new()
                {
                    Date = new DateTime(theYear, theMonth, i),
                    TotalIdeas = ideasInMonth.Where(_ => _.CreatedDate.Day == i).Count(),
                    TotalLikes = likesInMonth.Where(_ => _.CreatedDate.Day == i).Count(),
                    TotalComments = commentsInMonth.Where(_ => _.CreatedDate.Day == i).Count(),
                    TotalDislikes = dislikesInMonth.Where(_ => _.CreatedDate.Day == i).Count(),
                    TotalViews = viewsInMonth.Where(_ => _.CreatedDate.Day == i).Count(),
                }
            );
        }

        return activitiesList;
    }
}
