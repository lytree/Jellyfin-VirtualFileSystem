using System.Reflection;
using System.Text.RegularExpressions;
using Jellyfin.Plugin.FS.Configuration;
using Jellyfin.Plugin.FS.Model;

namespace Jellyfin.Plugin.FS.Helpers
{
    public static class TransformationPatches
    {
        public static string IndexHtml(PatchRequestPayload payload)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(FSPlugin).Namespace}.Inject.VFS.js")!;
            using TextReader reader = new StreamReader(stream);

            string regex = Regex.Replace(payload.Contents!, "(</body>)", $"<script defer>{reader.ReadToEnd()}</script>$1");

            return regex;
        }

        public static string HomeHtmlChunk(PatchRequestPayload payload)
        {
            string buffer = payload.Contents!;
            {
                Stream stream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream($"{typeof(FSPlugin).Namespace}.Inject.VFS.html")!;
                using TextReader reader = new StreamReader(stream);

                string tabTemplate = reader.ReadToEnd();
                string finalReplacement = tabTemplate;
                finalReplacement = finalReplacement
                    .Replace('\r', ' ')
                    .Replace('\n', ' ')
                    .Replace("  ", " ")
                    .Replace("'undefined'", "\\'undefined\\'");
                buffer = Regex.Replace(buffer, "(id=\"favoritesTab\" data-index=\"1\"> <div class=\"sections\"></div> </div>)", $"$1{finalReplacement}");
            }

            return buffer;
        }

        public static string MainBundle(PatchRequestPayload payload)
        {
            string replacementText =
                "window.PlaybackManager=this.playbackManager;console.log(\"PlaybackManager is now globally available:\",window.PlaybackManager);";

            string regex = Regex.Replace(payload.Contents!, @"(this\.playbackManager=e,)", $"$1{replacementText}");

            return regex;
        }
    }
}
