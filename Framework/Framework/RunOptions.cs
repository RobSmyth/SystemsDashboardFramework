using System;
using System.Linq;


namespace NoeticTools.TeamStatusBoard.Framework
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