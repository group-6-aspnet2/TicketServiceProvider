using Data.Entities;
using Domain.Models;

namespace Data.Interfaces;

public interface ITicketRepository : IBaseRepository<TicketEntity, TicketModel>
{
}

