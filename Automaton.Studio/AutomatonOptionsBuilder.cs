using Automaton.Studio.Activity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Automaton.Studio
{
    public class AutomatonOptionsBuilder
    {
        public IServiceCollection Services { get; }
        public AutomatonOptions AutomatonOptions { get; }

        public AutomatonOptionsBuilder(IServiceCollection services)
        {
            AutomatonOptions = new AutomatonOptions();
            Services = services;
        }

        public AutomatonOptionsBuilder AddActivitiesFrom(Assembly assembly) => AddActivitiesFrom(new[] { assembly });
        public AutomatonOptionsBuilder AddActivitiesFrom(params Assembly[] assemblies) => AddActivitiesFrom((IEnumerable<Assembly>)assemblies);
        public AutomatonOptionsBuilder AddActivitiesFrom(params Type[] assemblyMarkerTypes) => AddActivitiesFrom(assemblyMarkerTypes.Select(x => x.Assembly).Distinct());
        public AutomatonOptionsBuilder AddActivitiesFrom<TMarker>() where TMarker : class => AddActivitiesFrom(typeof(TMarker));
        public AutomatonOptionsBuilder AddActivitiesFrom(IEnumerable<Assembly> assemblies)
        {
            var types = assemblies.SelectMany(x => x.GetAllWithBaseClass<DynamicActivity>());

            foreach (var type in types)
            {
                AddActivity(type);
            }

            return this;
        }

        public AutomatonOptionsBuilder AddActivity<T>() where T : DynamicActivity => AddActivity(typeof(T));
        public AutomatonOptionsBuilder AddActivity(Type activityType)
        {
            Services.AddTransient(activityType);
            AutomatonOptions.Add(activityType);

            return this;
        }
    }
}
