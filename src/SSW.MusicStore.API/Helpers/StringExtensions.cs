using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSW.MusicStore.API.Helpers
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        public static bool IsNotNullOrEmpty(this string input)
        {
            return !string.IsNullOrEmpty(input);
        }
    }
}
