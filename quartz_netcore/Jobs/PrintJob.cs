using Quartz;
using quartz_netcore.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace quartz_netcore.Jobs
{
    public class PrintJob : IJob
    {
        private IPrintInfo _printInfo;
        public PrintJob(IPrintInfo printInfo)
        {
            _printInfo = printInfo;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _printInfo.PrintMsgAsync($"{DateTime.Now.ToString()} execute !");
        }
    }
}
