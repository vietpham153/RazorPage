using Microsoft.AspNetCore.Authorization;

namespace RazorPage.Security.Requirements
{
    public class ArticleUpdateRequirement : IAuthorizationRequirement
    {
        public ArticleUpdateRequirement(int year =2021,int month =6, int day =30)
        {
            Day = day;
            Month = month;
            Year = year;
        }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}
