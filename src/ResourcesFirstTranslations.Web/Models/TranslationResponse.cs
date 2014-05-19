using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourcesFirstTranslations.Web.Models
{
    public class TranslationResponse
    {
        public TranslationResponse(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public TranslationResponse(bool succeeded, string message)
        {
            Succeeded = succeeded;
            Message = message;
        }

        public TranslationResponse(bool succeeded, string message, DateTime lastUpdated, string lastUpdatedBy, bool originalResxValueChangedSinceTranslation)
        {
            Succeeded = succeeded;
            Message = message;
            LastUpdated = lastUpdated;
            LastUpdatedBy = lastUpdatedBy;
            OriginalResxValueChangedSinceTranslation = originalResxValueChangedSinceTranslation;
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool OriginalResxValueChangedSinceTranslation { get; set; }
    }
}