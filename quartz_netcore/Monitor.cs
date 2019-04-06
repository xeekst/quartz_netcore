using Quartz;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Quartz.Impl;

namespace quartz_netcore
{
    public class Monitor
    {

        private IScheduler _scheduler;

        private List<(IJobDetail, ITrigger)> _jobTriggers = new List<(IJobDetail, ITrigger)>();

        private readonly IServiceProvider container;

        public Monitor(IServiceProvider container, IConfiguration configuration)
        {
            this.container = container;
            SetJobs(configuration);
        }

        public void SetJobs(IConfiguration configuration)
        {
            var quartzJobConfigs = configuration.GetSection("QuartzJob").GetChildren();
            foreach (var jobConfig in quartzJobConfigs)
            {
                string jobClassFullName = jobConfig.GetValue<string>("JobClassFullName");
                string jobName = jobConfig.GetValue<string>("JobName");
                string group = jobConfig.GetValue<string>("Group");
                string cronString = jobConfig.GetValue<string>("CronString");
                string triggerName = jobConfig.GetValue<string>("JobClassFullName");
                bool isRun = jobConfig.GetValue<bool>("IsRun");
                if (isRun)
                {
                    IJobDetail job = JobBuilder.Create(Type.GetType(jobClassFullName)).WithIdentity(jobName, group).Build();
                    ITrigger trigger = TriggerBuilder.Create().WithIdentity(triggerName, group).WithCronSchedule(cronString).ForJob(jobName, group).Build();
                    _jobTriggers.Add((job, trigger));
                }

            }
        }

        public async Task StartAsync()
        {
            //Logging.LogManager.Adapter = new Logging.Simple.ConsoleOutLoggerFactoryAdapter { Level = Logging.LogLevel.Info };
            try
            {
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
                StdSchedulerFactory factory = new StdSchedulerFactory();
                _scheduler = await factory.GetScheduler();
                _scheduler.JobFactory = new JobServiceFactory(this.container);
                await AddJobsAsync();
                await _scheduler.Start();


            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }

        public async Task AddJobsAsync()
        {
            foreach ((IJobDetail, ITrigger) jt in _jobTriggers)
            {
                await _scheduler.ScheduleJob(jt.Item1, jt.Item2);
            }
        }
        public void PauseTrigger(string triggerName, string triggerGroup)
        {
            if (_scheduler != null)
            {
                _scheduler.PauseTrigger(new TriggerKey(triggerName, triggerGroup));
            }
        }

        public void ResumeTrigger(string triggerName, string triggerGroup)
        {
            if (_scheduler != null)
            {
                _scheduler.ResumeTrigger(new TriggerKey(triggerName, triggerGroup));
            }
        }

        public void Stop()
        {
            if (_scheduler != null)
            {
                _scheduler.Shutdown();
            }
        }
    }
}