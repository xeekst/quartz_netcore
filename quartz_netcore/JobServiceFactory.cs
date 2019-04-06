using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;

namespace quartz_netcore
{
    public class JobServiceFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public JobServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var job = _serviceProvider.GetService(bundle.JobDetail.JobType);
            return job as IJob;
        }

        public void ReturnJob(IJob job)
        {
            
        }
    }
}
