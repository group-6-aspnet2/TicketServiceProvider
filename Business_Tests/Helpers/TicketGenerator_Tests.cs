using Business.Helpers;

namespace Business_Tests.Helpers;

public class TicketGenerator_Tests
{
    [Fact]
    public void GenerateSeatsAndGate_ShouldReturnCorrectNumberOfEVouchers()
    {
        int ticketCount = 5;

        var result = TicketGenerator.GenerateSeatsAndGate(ticketCount);

        Assert.NotNull(result);
        Assert.Equal(ticketCount, result.Count);
    }

    [Fact]
    public void GenerateSeatsAndGate_ShouldGenerateSeatNumbersWithSameRowAndConsecutiveLetters()
    {
        int ticketCount = 3;

        var result = TicketGenerator.GenerateSeatsAndGate(ticketCount);

        Assert.NotNull(result);
        Assert.Equal(ticketCount, result.Count);

        var rowPrefix = result[0].SeatNumber[..^1]; 
        foreach (var e in result)
        {
            Assert.StartsWith(rowPrefix, e.SeatNumber);
        }

        for (int i = 0; i < ticketCount; i++)
        {
            char expectedLetter = (char)('A' + i);
            char actualLetter = result[i].SeatNumber[^1];
            Assert.Equal(expectedLetter, actualLetter);
        }
    }

    [Fact]
    public void GenerateSeatsAndGate_ShouldSetSameGateForAllEVouchers()
    {
        int ticketCount = 4;

        var result = TicketGenerator.GenerateSeatsAndGate(ticketCount);

        string gate = result[0].Gate;
        foreach (var ev in result)
        {
            Assert.Equal(gate, ev.Gate);
        }

        Assert.True(gate.Length == 1);
        Assert.InRange(gate[0], 'A', 'Z');
    }

    [Fact]
    public void GenerateSeatsAndGate_ShouldThrowArgumentException_WhenCountExceeds26()
    {
        int ticketCount = 27;

        var ex = Assert.Throws<ArgumentException>(() =>
            TicketGenerator.GenerateSeatsAndGate(ticketCount)
        );

        Assert.Equal("Max 26 platser kan bokas", ex.Message);
    }

    [Fact]
    public void GenerateGate_ShouldReturnSingleUppercaseLetter()
    {
        string gate = TicketGenerator.GenerateGate();

        Assert.NotNull(gate);
        Assert.Equal(1, gate.Length);
        Assert.InRange(gate[0], 'A', 'Z');
    }
}
