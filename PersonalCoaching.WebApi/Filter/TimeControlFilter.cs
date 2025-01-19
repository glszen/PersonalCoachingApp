using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PersonalCoaching.WebApi.Filter
{
    public class TimeControlFilter : ActionFilterAttribute
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }


        public override void OnActionExecuting(ActionExecutingContext context) //We use Action Filter to take precautions before the process starts.
        {
            var now = DateTime.Now;

            StartTime = "23:00";
            EndTime = "23:59";

            if (now.TimeOfDay >= TimeSpan.Parse(StartTime) && now.TimeOfDay <= TimeSpan.Parse(EndTime))
            {
                base.OnActionExecuting(context);
            }
            else
            {
                context.Result = new ContentResult
                {
                    Content = "Requests cannot be sent to the endpoint between these hours.",
                    StatusCode = 403
                };
            }
        }
    }
}
