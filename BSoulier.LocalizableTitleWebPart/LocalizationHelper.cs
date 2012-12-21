using System;

namespace BSoulier.LocalizableTitleWebPart
{
    public static class LocalizationHelper
    {

        public static LangageContainer GetLanguagesContent(string content)
        {
            if (!String.IsNullOrEmpty(content))
            {
                return LangageContainer.Deserialize(content);
            }
            return new LangageContainer();
        }

        public static string GetCurrentLanguage()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.LCID.ToString();
        }
    }
}
