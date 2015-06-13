using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DriverLite.TestClient
{
    class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: <TestName>");
                return;
            }

            TestBase test = null;
            if (args[0].Equals("Login"))
                test = new LoginTest();

            if (test != null)
                test.Run(args);
            else
                Console.WriteLine("Unknown Test - " + args[0]);
        }
    }

    public abstract class TestBase
    {
        public virtual string TestName
        {
            get { return GetType().Name.Substring(0, GetType().Name.Length - 4); }
        }

        public abstract void Run(string [] args);
    }
}
