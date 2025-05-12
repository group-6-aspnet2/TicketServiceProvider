using Data.Entities;
using Domain.Models;
using Domain.Responses;

namespace Data.Interfaces;

public interface ITicketRepository : IBaseRepository<TicketEntity, TicketModel>
{
    Task<TicketResponse<IEnumerable<TicketModel>>> AddRangeAsync(IEnumerable<TicketEntity> entities);
}

