using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importer.Corsavy
{
    public class ConfigAsp
    {
        public static List<string> IsoCodes = new List<string>{"en", "cz", "nl", "fr", "de", "it", "pt", "es", "se", 
            "goisern","ru","br","pl","jp", "th", "kr", "dk", "hu", 
            "ro", "cn-gb", "cn-big", "ca", "bg","urdu", "be", "el", "tr", "sk", "lt", "he", 
            "sl", "es-mx", "af", "vi", "ar", "no", "fa", "sr", "fi", "hr", "id"};

        public static List<string> LanguageNames = new List<string>{"English", "Czech", "Dutch", "French", "German", "Italian", "Portuguese", "Spanish", "Swedish", 
            "Goiserisch", "Russian","Brazilian Portuguese","Polish","Japanese", "Thai", "Korean", "Danish", "Hungarian", 
            "Romanian", "Chinese Simplified", "Chinese Traditional", "Catalan", "Bulgarian", "Urdu", "Belarusian", "Greek", 
            "Turkish", "Slovak", "Lithuanian", "Hebrew", "Slovenian", "Spanish (Mexico)", 
            "Afrikaans", "Vietnamese", "Arabic", "Norwegian", "Persian", "Serbian", 
                "Finnish", "Croatian", "Indonesian"};

        //arrLanguages = Array("en", "cz", "nl", "fr", "de", "it", "pt", "es", "se", _
        //    "goisern","ru","br","pl","jp", "th", "kr", "dk", "hu", _
        //    "ro", "cn-gb", "cn-big", "ca", "bg","urdu", "be", "el", "tr", "sk", "lt", "he", _
        //    "sl", "es-mx", "af", "vi", "ar", "no", "fa", "sr", "fi", "hr", "id") 

        //arrPrettyLanguages = Array("English", "Czech", "Dutch", "French", "German", "Italian", "Portuguese", "Spanish", "Swedish", _
        //    "Goiserisch", "Russian","Brazilian Portuguese","Polish","Japanese", "Thai", "Korean", "Danish", "Hungarian", _
        //    "Romanian", "Chinese Simplified", "Chinese Traditional", "Catalan", "Bulgarian", "Urdu", "Belarusian", "Greek", _
        //    "Turkish", "Slovak", "Lithuanian", "Hebrew", "Slovenian", "Spanish (Mexico)", _
        //    "Afrikaans", "Vietnamese", "Arabic", "Norwegian", "Persian", "Serbian", _
        //        "Finnish", "Croatian", "Indonesian")

        //arrCharSet = Array("windows-1252","windows-1250","windows-1252","windows-1252","windows-1252","windows-1252","windows-1252","windows-1252","windows-1252", _
        //    "windows-1252","windows-1251","windows-1252","windows-1250","shift_jis", "windows-874", "euc-kr", "windows-1252","windows-1250", _
        //    "windows-1250", "gb2312", "big5", "windows-1252", "windows-1251", "windows-1256", "windows-1251", "windows-1253", _
        //    "windows-1254","windows-1250", "windows-1257", "windows-1255", "windows-1250","windows-1252", "windows-1252", _
        //    "windows-1258", "windows-1256", "Windows-1252","windows-1258", "windows-1250", "Windows-1252", _
        //        "windows-1250", "windows-1252")

        //arrCodePage = Array(1252, 1250, 1252, 1252, 1252, 1252, 1252, 1252, _
        //    1252, 1252, 1251, 1252, 1250, 932, 874, 949, 1252, 1250, _
        //    1250, 936, 950, 1252, 1251, 1256, 1251, 1253, 1254, 1250, _
        //    1257, 1255, 1250, 1252, 1252, 1258, 1256, 1252, 1258, 1250, _
        //        1252, 1250, 1252)
    }
}
