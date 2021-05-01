using Elsa.Metadata;
using Elsa.Services;
using Elsa.Services.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityTypeService activityTypeService;

        public ActivityService(IActivityTypeService activityTypeService)
        {
            this.activityTypeService = activityTypeService;
        }

        public async Task<IEnumerable<ActivityDescriptor>> List()
        {
            var activityTypes = await activityTypeService.GetActivityTypesAsync();
            var tasks = activityTypes.Where(x => x.IsBrowsable).Select(x => DescribeActivity(x)).ToList();
            var descriptors = await Task.WhenAll(tasks);

            return descriptors;
        }

        private async Task<ActivityDescriptor> DescribeActivity(ActivityType activityType)
        {
            return await activityTypeService.DescribeActivityType(activityType);
        }
    }
}
