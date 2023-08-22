using LetsEncrypt.Client.Interfaces;
using LetsEncrypt.Client.IO;
using LetsEncrypt.Client.Loggers;
using Microsoft.Extensions.DependencyInjection;
namespace BW2LetsEncrypt.FormApp
{
    internal static class Program
    {


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new BW2LetsEncryptForm());
        }

 
    }
}