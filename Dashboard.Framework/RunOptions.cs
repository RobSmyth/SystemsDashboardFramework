using System;
using System.Linq;
using System.Net.Mime;

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