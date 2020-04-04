using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.AgentManagement.Domain.Entities.Agents;

namespace MAVN.Service.AgentManagement.Domain.Repositories
{
    public interface IImageRepository
    {
        Task UpdateAsync(IReadOnlyList<EncryptedImage> images);
    }
}
