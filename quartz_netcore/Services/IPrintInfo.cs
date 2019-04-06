using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace quartz_netcore.Services
{
    public interface IPrintInfo
    {
        Task PrintMsgAsync(string msg);
    }
}
