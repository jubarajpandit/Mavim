using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields
{
    public interface IFieldService
    {
        Task<IEnumerable<IField>> GetFields(string dcvId);
        Task<IField> GetField(string dcvId, string fieldSetDefinitionId, string fieldDefinitionId);
        Task<IField> UpdateFieldValue(IField field);
        Task<IBulkResult<IField>> UpdateFieldValues(List<IField> fields);
    }
}
