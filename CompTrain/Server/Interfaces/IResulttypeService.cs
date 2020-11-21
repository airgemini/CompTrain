using CompTrain.Shared.Models.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompTrain.Server.Interfaces
{
    public interface IResulttypeService
    {
        Task<IList<Resulttype>> GetResulttypes();
    }
}