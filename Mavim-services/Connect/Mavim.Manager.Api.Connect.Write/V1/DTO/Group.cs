using Mavim.Manager.Api.Connect.Write.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Connect.Write.V1.DTO
{
    /// <summary>
    /// Add Group Dto
    /// </summary>
    public record AddGroupDto
    {
        /// <summary>
        /// Group Name
        /// </summary>
        [Required]
        public string Name { get; init; }

        /// <summary>
        /// Group Description
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public AddGroupDto([Required] string name, string description) => (Name, Description) = (name, description);

        /// <summary>
        /// Deconstruct
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public void Deconstruct(out string name, out string description)
        {
            name = Name;
            description = Description;
        }

    }

    /// <summary>
    /// Update Group Dto
    /// </summary>
    public record UpdateGroupDto(string Name, string Description)
    {
        /// <summary>
        /// Group Name
        /// </summary>
        public string Name { get; init; } = Name;

        /// <summary>
        /// Group Description
        /// </summary>
        public string Description { get; init; } = Description;

    }

    /// <summary>
    /// Update UserGroup Dto
    /// </summary>
    public record UpdateUserGroupDto
    {
        /// <summary>
        /// UserIds Add To UserGroup
        /// </summary>
        [RequiredGuid]
        public IReadOnlyList<Guid> UserIds { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userIds"></param>
        public UpdateUserGroupDto([RequiredGuid] IReadOnlyList<Guid> userIds) => (UserIds) = (userIds);

        /// <summary>
        /// Deconstruct
        /// </summary>
        /// <param name="userIds"></param>
        public void Deconstruct(out IReadOnlyList<Guid> userIds)
        {
            userIds = UserIds;
        }

    }

    /// <summary>
    /// Delete UserGroup Dto
    /// </summary>
    public record DeleteUserGroupDto
    {
        /// <summary>
        /// UserIds To remove from UserGroup
        /// </summary>
        [RequiredGuid]
        public IReadOnlyList<Guid> UserIds { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userIds"></param>
        public DeleteUserGroupDto([RequiredGuid] IReadOnlyList<Guid> userIds) => (UserIds) = (userIds);

        /// <summary>
        /// Deconstruct
        /// </summary>
        /// <param name="userIds"></param>
        public void Deconstruct(out IReadOnlyList<Guid> userIds)
        {
            userIds = UserIds;
        }

    }
}