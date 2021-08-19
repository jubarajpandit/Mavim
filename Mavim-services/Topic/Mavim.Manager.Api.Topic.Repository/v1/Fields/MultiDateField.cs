using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Fields.Base;
using System;

namespace Mavim.Manager.Api.Topic.Repository.v1.Fields
{
    public sealed class MultiDateField : MultiField<DateTime?>, IMultiDateField { }
}