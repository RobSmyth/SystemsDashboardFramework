using System;
using System.Linq;


namespace NoeticTools.SystemsDashboard.Framework
{
    public class RunOptions : IRunOptions
    {
        public bool EmulateMode
        {
            get
            {
                return
                    Environment.GetCommandLineArgs()
                        .Any(x => x.Equals("/emulate", StringComparison.InvariantCultureIgnoreCase));
            }
        }
    }
}