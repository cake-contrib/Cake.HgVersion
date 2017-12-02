using NUnit.Framework;
using Shouldly;
using System;
using VCSVersion;

namespace VCSVersionTests
{
    [SetUpFixture]
    public class ModuleInitializer
    {
        [OneTimeSetUp]
        public static void Initialize()
        {
            Logger.SetLoggers(
                s => Console.WriteLine(s),
                s => Console.WriteLine(s),
                s => Console.WriteLine(s),
                s => Console.WriteLine(s));
        }
    }
}
