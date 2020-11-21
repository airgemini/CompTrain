using CompTrain.Shared.Models.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompTrain.Server.Interfaces
{
    public interface IWodService
    {
        Task<Wod> GetWodByDate(DateTime Date);
        Task<IList<Wod>> GetWodsByDate(DateTime Start, DateTime End);

        Task<Wod> Add(Wod wod);

        Task<bool> Delete(Wod wod);

        Task<Wod> Get(int WodId);
    }
}