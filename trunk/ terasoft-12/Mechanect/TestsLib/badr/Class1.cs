using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Mechanect.Classes;
using Microsoft.Xna.Framework;
using Mechanect;
using System.Threading;

namespace Tests
{
    [TestFixture]
    public class Class1
    {
        Game1 game1 = new Game1();
        
        [Test]
        public void Test1()
        {
            game1.Content.RootDirectory = @"F:\SE\ terasoft-12\Mechanect\Tests\bin\Debug\Content";
            game1.Run();
        }
    }
}
