using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class TicketRepository(DataContext context) : BaseRepository<TicketEntity, TicketModel>(context), ITicketRepository
{
}

