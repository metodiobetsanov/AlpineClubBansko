using System;
using System.Collections.Generic;
using System.Text;

namespace AlpineClubBansko.Services.Common
{
    public static class ArgumentValidator
    {
            public static void ThrowIfNull(object obj, string name)
            {
                if (obj == null)
                {
                    throw new ArgumentNullException($"{name} cannot be null", name);
                }
            }

            public static void ThrowIfNullOrEmpty(string text, string name)
            {
                if (string.IsNullOrEmpty(text))
                {
                    throw new ArgumentException($"{name} cannot be null or empty.", name);
                }
            }
    }
}
