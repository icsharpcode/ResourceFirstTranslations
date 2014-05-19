using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourcesFirstTranslations.Services;

namespace ResourcesFirstTranslations.Common
{
    public static class Cultures
    {
        // Format: |de| or |es|es-mx| - always with leading *and* trailing | to allow 
        //         for '%|es|%' queries (without finding es-mx inadvertently)

        public const char CultureDelimiter = '|';

        public static List<string> SplitUserCulturesList(string cultures)
        {
            if (String.IsNullOrWhiteSpace(cultures)) return new List<string>();

            string toSplit = cultures.Trim(new[] { CultureDelimiter });
            string[] parts = toSplit.Split(new[] { CultureDelimiter });
            return parts.ToList();
        }

        public static async Task<bool> IsValidCulturesListAsync(string cultures, ITranslationService translationService)
        {
            // Empty is ok, eg administrators that do not translate anything
            if (String.IsNullOrWhiteSpace(cultures)) return true;

            // First and last character must be the delimiter, thus we need at least three characters
            if (cultures.Length < 3 ||
                cultures[0] != CultureDelimiter || 
                cultures[cultures.Length - 1] != CultureDelimiter)
            {
                return false;
            }

            var culturesList = SplitUserCulturesList(cultures);
            var knownCultures = await translationService.GetCachedLanguagesAsync().ConfigureAwait(false);
            var knownCultureCodes = knownCultures.Select(kc => kc.Culture).ToList();

            // If the culture list for the user has a culture that we don't know of as an application -> invalid
            var result = culturesList.Except(knownCultureCodes);
            return !result.Any();
        }
    }
}
