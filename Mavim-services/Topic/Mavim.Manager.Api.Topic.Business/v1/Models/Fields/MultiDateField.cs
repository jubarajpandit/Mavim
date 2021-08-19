using Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Business.v1.Models.Fields.Abstract;
using System;

namespace Mavim.Manager.Api.Topic.Business.v1.Models.Fields
{
    public class MultiDateField : MultiField<DateTime?>, IMultiDateField { }
}