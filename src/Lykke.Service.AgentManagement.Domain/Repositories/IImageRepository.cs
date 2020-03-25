using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.AgentManagement.Domain.Entities.Agents;

namespace Lykke.Service.AgentManagement.Domain.Repositories
{
    public interface IImageRepository
    {
        Task UpdateAsync(IReadOnlyList<EncryptedImage> images);
    }
}
