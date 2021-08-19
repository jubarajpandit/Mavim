using Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Services.v1.Models.Fields.Abstract;
using System;

namespace Mavim.Manager.Api.Topic.Services.v1.Models.Fields
{
    public class SingleHyperlinkField : SingleField<Uri>, ISingleHyperlinkField { }
}
