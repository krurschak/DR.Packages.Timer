using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DR.Packages.Timer.Interfaces
{
    public interface ITimer
    {
        Task ConsumeAsync();
    }
}
