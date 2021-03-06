// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace Microsoft.OpenPublishing.ConfigService.Library
{
    public sealed class DocumentIdConfig
    {
        /// <summary>
        /// For backward compatibility, the source path prefix
        /// Used for resolving docId in <see cref="Document.LoadDocumentId()"/>
        /// </summary>
        public readonly string SourceBasePath = ".";

        /// <summary>
        /// For backward compatibility, the output site path prefix
        /// Used for resolving versionIndependentId in <see cref="Document.LoadDocumentId()"/>
        /// </summary>
        public readonly string SiteBasePath = ".";

        /// <summary>
        /// The mappings between depot and files/directory
        /// Used for backward compatibility
        /// </summary>
        public Dictionary<string, string> DepotMappings = new Dictionary<string, string>();

        /// <summary>
        /// The mappings between directory and files/directory
        /// Used for backward compatibility
        /// </summary>
        public Dictionary<string, string> DirectoryMappings = new Dictionary<string, string>();

        public (string depotName, string pathRelativeToSourceBasePath) GetMapping(string normalizedFilePathToSourceBasePath)
        {
            var (depotName, _) = GetReversedMapping(DepotMappings, normalizedFilePathToSourceBasePath);
            var (mappedDir, matchedDir) = GetReversedMapping(DirectoryMappings, normalizedFilePathToSourceBasePath);

            var mappedPathRelativeToSourceBasePath = !string.IsNullOrEmpty(matchedDir)
                ? PathUtility.NormalizeFile(Path.Combine(mappedDir, Path.GetRelativePath(matchedDir, normalizedFilePathToSourceBasePath)))
                : normalizedFilePathToSourceBasePath;

            return (depotName, mappedPathRelativeToSourceBasePath);
        }

        private static (string value, string matchedDirectory) GetReversedMapping(Dictionary<string, string> mappings, string normalizedFilePathToSourceBasePath)
        {
            foreach (var (path, value) in mappings)
            {
                var normalizedPath = path.EndsWith("/") || path.EndsWith("\\") ? PathUtility.NormalizeFolder(path) : PathUtility.NormalizeFile(path);
                var (match, isFileMatch, _) = PathUtility.Match(normalizedFilePathToSourceBasePath, normalizedPath);
                if (match)
                {
                    if (!isFileMatch)
                    {
                        return (value, normalizedPath);
                    }

                    var lastSlashIndex = normalizedFilePathToSourceBasePath.LastIndexOf("/");
                    var matchedDirectory = lastSlashIndex > 0 ? normalizedFilePathToSourceBasePath.Substring(0, lastSlashIndex) : string.Empty;
                    return (value, matchedDirectory);
                }
            }

            return (string.Empty, string.Empty);
        }
    }
}
