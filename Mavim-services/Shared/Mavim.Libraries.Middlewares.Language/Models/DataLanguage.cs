using Mavim.Libraries.Middlewares.Language.Enums;
using Mavim.Libraries.Middlewares.Language.Interfaces;

namespace Mavim.Libraries.Middlewares.Language.Models
{
    class DataLanguage : IDataLanguage
    {
        public DataLanguageType Type { get; set; }
    }
}
