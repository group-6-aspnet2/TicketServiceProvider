using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using Domain.Models;
using Domain.Responses;

namespace Data.Repositories;

public class TicketRepository(DataContext context) : BaseRepository<TicketEntity, TicketModel>(context), ITicketRepository
{

    public async Task<TicketResponse<IEnumerable<TicketModel>>> AddRangeAsync (IEnumerable<TicketEntity> entities)
    {
		try
		{
			 await _table.AddRangeAsync(entities);
			var result =await _context.SaveChangesAsync();
			if( result == 0)
			{
                return new TicketResponse<IEnumerable<TicketModel>>
                {
                    Succeeded = false,
                    StatusCode = 500,
                    Error = "Failed to add tickets"
                };
            }
			var models = entities.Select(e => e.MapTo<TicketModel>());
			return new TicketResponse<IEnumerable<TicketModel>>
            {
                Succeeded = true,
                StatusCode = 201,
                Result = models
            };

        }
		catch (Exception ex)
		{

			throw;
		}
    }
}

