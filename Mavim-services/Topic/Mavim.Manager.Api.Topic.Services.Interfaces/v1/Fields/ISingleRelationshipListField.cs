﻿using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields
{
    public interface ISingleRelationshipListField : ISingleField<Dictionary<string, IRelationshipElement>>
    {
        Dictionary<string, IRelationshipElement> Options { get; set; }
    }
}