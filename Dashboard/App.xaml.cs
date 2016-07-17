using System;
using System.Windows;
using log4net;


namespace NoeticTools.TeamStatusBoard
{
    public partial class App : Application
    {
        public App()
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += OnUnhandledException;
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            var exception = (Exception)args.ExceptionObject;
            var logger = LogManager.GetLogger("TeamStatusBoard.App");
            logger.Error("Unhandled exception.", exception);
        }
    }
}