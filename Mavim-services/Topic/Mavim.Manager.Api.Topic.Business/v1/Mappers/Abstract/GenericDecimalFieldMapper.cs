using Mavim.Manager.Api.Topic.Business.Constants;
using System.Globalization;

namespace Mavim.Manager.Api.Topic.Business.v1.Mappers.Abstract
{
    internal abstract class GenericDecimalFieldMapper<TRepo, TBusiness> : GenericFieldMapper<TRepo, TBusiness> where TRepo : Repository.Interfaces.v1.Fields.IField where TBusiness : Interfaces.v1.Fields.IField
    {
        private readonly CultureInfo _cultureInfo = new CultureInfo(CultureInfoConstant.Dutch);

        protected decimal? Map(string value)
        {
            if (decimal.TryParse(value, NumberStyles.AllowDecimalPoint, _cultureInfo, out decimal mappedValue))
            {
                return mappedValue;
            }

            return null;
        }

        protected string Map(decimal? value)
        {
            string field = value?.ToString(_cultureInfo);
            return field;
        }
    }
}