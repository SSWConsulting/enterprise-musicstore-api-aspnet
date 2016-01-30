using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SSW.MusicStore.Data.Test.Int
{
    public class HelloXunit
    {

        [Fact]
        public void HelloTest()
        {
            Assert.True(2 == 1+1);
        }

    }
}
