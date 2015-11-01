using System;
using System.Linq;


namespace NoeticTools.Dashboard.Framework
{
    public class RunOptions
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