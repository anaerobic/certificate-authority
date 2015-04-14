using System;
using Microsoft.Owin.Hosting;

namespace OwinApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string url = "https://+:443";

            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Listening at: " + url);
                Console.ReadLine();
            }
        }
    }
}
