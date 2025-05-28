using Data.Contexts;
using Data.Entities;
using Data.Repositories;
using Data_Tests.SeedData;
using Microsoft.EntityFrameworkCore;

namespace Data_Tests.Repositories;

public class TicketRepository_Tests
{

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTickets_IfSucceeded()
    {
        var context = new DataContextSeeder().GetDataContext();
        context.Tickets.AddRange(TestData.TicketEntities);
        await context.SaveChangesAsync();

        var ticketRepository = new TicketRepository(context);

        var result = await ticketRepository.GetAllAsync();

        Assert.True(result.Succeeded);
        Assert.NotNull(result.Result);
        Assert.Equal(TestData.TicketEntities.Length, result.Result.Count());
        await context.DisposeAsync();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnTicket_WhenExists()
    {
        var context = new DataContextSeeder().GetDataContext();
        context.Tickets.AddRange(TestData.TicketEntities);
        await context.SaveChangesAsync();

        var repo = new TicketRepository(context);

        var result = await repo.GetAsync(x => x.Id == "id-123-id");

        Assert.True(result.Succeeded);
        Assert.NotNull(result.Result);
        Assert.Equal("id-123-id", result.Result.Id);

        await context.DisposeAsync();
    }

    [Fact]
    public async Task GetAsync_ShouldReturn404_WhenNotFound()
    {
        var context = new DataContextSeeder().GetDataContext();
        var repo = new TicketRepository(context);

        var result = await repo.GetAsync(x => x.Id == "non-existent-id");

        Assert.False(result.Succeeded);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Entity not found.", result.Error);

        await context.DisposeAsync();
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenEntityExists()
    {
        var context = new DataContextSeeder().GetDataContext();
        context.Tickets.AddRange(TestData.TicketEntities);
        await context.SaveChangesAsync();

        var repo = new TicketRepository(context);

        var result = await repo.ExistsAsync(x => x.Id == "id-456-id");

        Assert.True(result.Succeeded);
        Assert.Equal("Entity exists.", result.Error);

        await context.DisposeAsync();
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenEntityDoesNotExist()
    {
        var context = new DataContextSeeder().GetDataContext();
        var repo = new TicketRepository(context);

        var result = await repo.ExistsAsync(x => x.Id == "nope");

        Assert.False(result.Succeeded);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Entity not found.", result.Error);

        await context.DisposeAsync();
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity_WhenValid()
    {
        var context = new DataContextSeeder().GetDataContext();
        var repo = new TicketRepository(context);

        var entity = new TicketEntity
        {
            Id = "new-id-001",
            BookingId = "book-new",
            UserId = "user-x",
            EventId = "event-y",
            TicketCategoryName = "Gold",
            TicketPrice = 500,
            SeatNumber = "B4",
            Gate = "C"
        };

        var result = await repo.AddAsync(entity);

        Assert.True(result.Succeeded);
        Assert.Equal(201, result.StatusCode);
        Assert.NotNull(result.Result);
        Assert.Equal(entity.Id, result.Result.Id);

        await context.DisposeAsync();
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity_WhenExists()
    {
        var context = new DataContextSeeder().GetDataContext();
        context.Tickets.AddRange(TestData.TicketEntities);
        await context.SaveChangesAsync();

        var repo = new TicketRepository(context);

        var result = await repo.DeleteAsync(x => x.Id == "id-543241-id");

        Assert.True(result.Succeeded);
        Assert.Equal(204, result.StatusCode);
        Assert.Null(result.Error);

        // Verifiera att entiteten inte längre finns
        var exists = await repo.ExistsAsync(x => x.Id == "id-543241-id");
        Assert.False(exists.Succeeded);

        await context.DisposeAsync();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturn404_WhenEntityNotFound()
    {
        var context = new DataContextSeeder().GetDataContext();
        var repo = new TicketRepository(context);

        var result = await repo.DeleteAsync(x => x.Id == "nonexistent");

        Assert.False(result.Succeeded);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Entity not found.", result.Error);

        await context.DisposeAsync();
    }


    [Fact]
    public async Task AddRangeAsync_ShouldAddEntities_WhenValid()
    {
        var context = new DataContextSeeder().GetDataContext();
        var repo = new TicketRepository(context);

        var newEntities = new List<TicketEntity>
    {
        new TicketEntity {
            Id = "bulk-id-001",
            BookingId = "bulk-booking",
            UserId = "user-001",
            EventId = "event-001",
            TicketCategoryName = "Premium",
            TicketPrice = 700,
            SeatNumber = "Z1",
            Gate = "K"
        },
        new TicketEntity {
            Id = "bulk-id-002",
            BookingId = "bulk-booking",
            UserId = "user-001",
            EventId = "event-001",
            TicketCategoryName = "Premium",
            TicketPrice = 700,
            SeatNumber = "Z2",
            Gate = "K"
        }
    };

        var result = await repo.AddRangeAsync(newEntities);

        Assert.True(result.Succeeded);
        Assert.Equal(201, result.StatusCode);
        Assert.NotNull(result.Result);
        Assert.Equal(2, result.Result.Count());

        var dbEntities = context.Tickets.Where(t => t.BookingId == "bulk-booking").ToList();
        Assert.Equal(2, dbEntities.Count);

        await context.DisposeAsync();
    }


}
