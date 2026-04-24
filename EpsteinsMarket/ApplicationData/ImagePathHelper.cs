using System;
using System.Collections.Generic;
using System.IO;

namespace EpsteinMarket.ApplicationData
{
    internal static class ImagePathHelper
    {
        private const string DefaultPlaceholderResourcePath = "/Pages/free-icon-user-847969.png";

        public static string ResolveImageSource(string rawPath)
        {
            return ResolveImageSourceOrDefault(rawPath);
        }

        public static string ResolveImageSourceOrDefault(string rawPath)
        {
            string resolved = ResolveImageSourceInternal(rawPath);

            if (!string.IsNullOrWhiteSpace(resolved))
                return resolved;

            return ResolvePlaceholderPath();
        }

        private static string ResolveImageSourceInternal(string rawPath)
        {
            if (string.IsNullOrWhiteSpace(rawPath))
                return null;

            string trimmedPath = rawPath.Trim();
            string normalizedPath = trimmedPath.Replace("\\", "/");

            if (normalizedPath.StartsWith("pack://", StringComparison.OrdinalIgnoreCase) || normalizedPath.StartsWith("/", StringComparison.Ordinal))
                return normalizedPath;

            if (Path.IsPathRooted(trimmedPath) && File.Exists(trimmedPath))
                return trimmedPath;

            foreach (string root in GetSearchRoots())
            {
                string directCandidate = Path.Combine(root, trimmedPath);
                if (File.Exists(directCandidate))
                    return directCandidate;

                string photoCandidate = Path.Combine(root, "Photo", trimmedPath);
                if (File.Exists(photoCandidate))
                    return photoCandidate;

                string legacyImagesCandidate = Path.Combine(root, "Images", trimmedPath);
                if (File.Exists(legacyImagesCandidate))
                    return legacyImagesCandidate;
            }

            return null;
        }

        private static string ResolvePlaceholderPath()
        {
            return DefaultPlaceholderResourcePath;
        }

        private static IEnumerable<string> GetSearchRoots()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            yield return baseDir;
            yield return Path.GetFullPath(Path.Combine(baseDir, ".."));
            yield return Path.GetFullPath(Path.Combine(baseDir, "..", ".."));
            yield return Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));
        }
    }
}
