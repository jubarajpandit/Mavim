using Mavim.Manager.Api.Topic.Business.Constants;
using System.Globalization;

namespace Mavim.Manager.Api.Topic.Business.v1.Mappers.Abstract
{
    internal abstract class GenericNumberFieldMapper<TRepo, TBusiness> : GenericFieldMapper<TRepo, TBusiness> where TRepo : Repository.Interfaces.v1.Fields.IField where TBusiness : Interfaces.v1.Fields.IField
    {
        private readonly CultureInfo _cultureInfo = new CultureInfo(CultureInfoConstant.Dutch);

        protected long? Map(string value)
        {
            if (long.TryParse(value, out long mappedValue))
            {
                return mappedValue;
            }

            return null;
        }

        protected string Map(long? value)
        {
            string field = value?.ToString(_cultureInfo);
            return field;
        }
    }
}