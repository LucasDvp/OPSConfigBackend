using System;

namespace Microsoft.OpenPublishing.ConfigService.Common
{
    public static class Guard
    {
        public static void ArgumentNotNull(object argument)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(nameof(argument));
            }
        }

        public static void ArgumentNotNullOrEmpty(string argument)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(nameof(argument));
            }
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentException($"{nameof(argument)} can't be empty");
            }
        }
    }
}
