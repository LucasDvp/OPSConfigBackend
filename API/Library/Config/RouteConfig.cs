// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.IO;

namespace Microsoft.OpenPublishing.ConfigService.Library
{
    public sealed class RouteConfig
    {
        /// <summary>
        /// The source file name of source folder name.
        /// Source folder name should end with "/".
        /// </summary>
        public readonly string Source = string.Empty;

        /// <summary>
        /// The destination URL.
        /// </summary>
        public readonly string Destination = string.Empty;

        /// <summary>
        /// return the path after route.
        /// return null when the current RouteConfig does not apply to <paramref name="path"/>.
        /// </summary>
        public string GetOutputPath(string path)
        {
            Debug.Assert(!string.IsNullOrEmpty(path));

            var (match, isFileMatch, remainingPath) = PathUtility.Match(path, Source);

            if (match)
            {
                if (isFileMatch)
                {
                    return Path.Combine(Destination, Path.GetFileName(path));
                }

                return Path.Combine(Destination, remainingPath);
            }

            return null;
        }
    }
}
