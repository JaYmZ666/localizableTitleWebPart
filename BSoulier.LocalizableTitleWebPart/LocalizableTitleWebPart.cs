using System;
using Microsoft.SharePoint.WebPartPages;

namespace BSoulier.LocalizableTitleWebPart
{
    public abstract class LocalizableTitleWebPart : WebPart
    {
        [WebPartStorage(Storage = Storage.Shared)]
        public string LocalizedTitle { get; set; }

        private LangageContainer langContainer = null;

        protected string GetLocalizedProperty(LangageContainer container, string content)
        {
            if (container != null)
            {
                return container.GetOrInsertLanguage().EncodedLanguageContent;
            }
            else
            {
                container = LocalizationHelper.GetLanguagesContent(content);
                return container.GetOrInsertLanguage().EncodedLanguageContent;
            }
        }

        protected string SetLocalizedProperty(LangageContainer container, string content, string value)
        {
            container = LocalizationHelper.GetLanguagesContent(content);
            var lang = container.GetOrInsertLanguage();

            if (this.Page != null && this.Page.IsPostBack)
            {
                container.UpdateLanguage(lang.LanguageCode, value);
                return LangageContainer.Serialize(container);
            }
            return content;
        }

        public override string Title
        {
            get
            {
                return GetLocalizedProperty(langContainer, this.LocalizedTitle);
            }
            set
            {
                this.LocalizedTitle = SetLocalizedProperty(langContainer, this.LocalizedTitle, value);
            }
        }
    }
}
