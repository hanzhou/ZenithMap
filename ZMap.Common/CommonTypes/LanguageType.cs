using System;
using System.ComponentModel;
using System.Reflection;

namespace ZMap
{
    public static partial class EnumHelper
    {
        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }

    public enum LanguageType
    {
        [Description("ar")]
        Arabic,

        [Description("bg")]
        Bulgarian,

        [Description("bn")]
        Bengali,

        [Description("ca")]
        Catalan,

        [Description("cs")]
        Czech,

        [Description("zh-CN")]
        ChineseSimplified,

        [Description("zh-TW")]
        ChineseTraditional,

        [Description("da")]
        Danish,

        [Description("de")]
        German,

        [Description("el")]
        Greek,

        [Description("en")]
        English,

        [Description("en-AU")]
        EnglishAustralian,

        [Description("en-GB")]
        EnglishGreatBritain,

        [Description("es")]
        Spanish,

        [Description("eu")]
        Basque,

        [Description("fi")]
        Finnish,

        [Description("fil")]
        Filipino,

        [Description("fr")]
        French,

        [Description("gl")]
        Galician,

        [Description("gu")]
        Gujarati,
        [Description("hi")]
        Hindi,

        [Description("hr")]
        Croatian,

        [Description("hu")]
        Hungarian,

        [Description("id")]
        Indonesian,

        [Description("it")]
        Italian,

        [Description("iw")]
        Hebrew,

        [Description("ja")]
        Japanese,

        [Description("kn")]
        Kannada,

        [Description("ko")]
        Korean,

        [Description("lt")]
        Lithuanian,

        [Description("lv")]
        Latvian,

        [Description("ml")]
        Malayalam,

        [Description("mr")]
        Marathi,

        [Description("nl")]
        Dutch,

        [Description("nn")]
        NorwegianNynorsk,

        [Description("no")]
        Norwegian,

        [Description("or")]
        Oriya,

        [Description("pl")]
        Polish,

        [Description("pt")]
        Portuguese,

        [Description("pt-BR")]
        PortugueseBrazil,

        [Description("pt-PT")]
        PortuguesePortugal,

        [Description("rm")]
        Romansch,
        [Description("ro")]
        Romanian,

        [Description("ru")]
        Russian,

        [Description("sk")]
        Slovak,

        [Description("sl")]
        Slovenian,

        [Description("sr")]
        Serbian,

        [Description("sv")]
        Swedish,

        [Description("ta")]
        Tamil,

        [Description("te")]
        Telugu,

        [Description("th")]
        Thai,

        [Description("tr")]
        Turkish,

        [Description("uk")]
        Ukrainian,

        [Description("vi")]
        Vietnamese
    }
}
