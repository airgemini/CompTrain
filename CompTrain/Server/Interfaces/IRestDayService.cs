using CompTrain.Shared.Models.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompTrain.Server.Interfaces
{
    public interface IRestDayService
    {
        Task<IList<RestDay>> GetRestdays();
        Task<bool> IsRestDay(DateTime dateTime);
    }
}