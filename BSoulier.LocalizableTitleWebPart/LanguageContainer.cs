using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Web;
using System.IO;
using Microsoft.SharePoint;

namespace BSoulier.LocalizableTitleWebPart
{
    public class LangageContainer
    {
        public LangageContainer()
        {
            Languages = new List<LanguageItem>();
        }

        public LangageContainer(SPLanguageCollection coll)
        {
            Languages = new List<LanguageItem>();

        }

        public List<LanguageItem> Languages { get; set; }

        public LanguageItem GetOrInsertLanguage()
        {
            return GetOrInsertLanguage(LocalizationHelper.GetCurrentLanguage());
        }

        public LanguageItem GetOrInsertLanguage(string languageCode)
        {
            var sameLang = this.Languages.Where(l => l.LanguageCode == languageCode).SingleOrDefault();
            if (sameLang == null)
            {
                var lang = new LanguageItem() { LanguageCode = languageCode };
                this.Languages.Add(lang);
                return lang;
            }
            return sameLang;
        }

        public void UpdateLanguage(string languageCode, string content)
        {
            var lang = this.Languages.Where(l => l.LanguageCode == languageCode).SingleOrDefault();
            lang.EncodedLanguageContent = content;
        }

        private static XmlSerializer GetSerializer()
        {
            return new XmlSerializer(typeof(LangageContainer));
        }

        internal static string Serialize(LangageContainer container)
        {
            var serializer = GetSerializer();
            MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, container);
            stream.Position = 0;

            var sr = new StreamReader(stream);
            var str = sr.ReadToEnd();

            stream.Dispose();
            return str;
        }

        internal static LangageContainer Deserialize(string strContainer)
        {
            var serializer = GetSerializer();
            MemoryStream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(strContainer));
            return serializer.Deserialize(stream) as LangageContainer;
        }
    }

    public class LanguageItem
    {
        private string _content;

        public string LanguageCode { get; set; }

        public string LanguageContent
        {
            get { return _content; }
            set { _content = value; }
        }

        [XmlIgnore]
        public string EncodedLanguageContent
        {
            get { return HttpUtility.HtmlDecode(_content); }
            set { _content = HttpUtility.HtmlEncode(value); }
        }
    }
}
