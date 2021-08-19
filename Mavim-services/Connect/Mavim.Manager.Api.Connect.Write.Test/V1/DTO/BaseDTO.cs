using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Connect.Write.Test.V1.DTO
{
    public abstract class BaseDTO
    {
        //Unit Test DataAnnotations
        //http://stackoverflow.com/questions/2167811/unit-testing-asp-net-dataannotations-validation
        protected static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
