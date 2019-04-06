using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace quartz_netcore.Services
{
    public class PrintInfo : IPrintInfo
    {
        public async Task PrintMsgAsync(string msg)
        {
            await Console.Out.WriteLineAsync(msg);
        }
    }
}
