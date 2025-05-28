using Domain.Models;

namespace Business.Helpers;


public class TicketGenerator
{
    private static Random random = new Random();

    // Genererat från chatgpt. Skapar slumpmässigt seatnumber med ett tal 1-200 och en bokstav A-F, returnerar string. Om flera biljetter så sätts biljetterna nära varandra.
    public static List<EVoucherInformation> GenerateSeatsAndGate(int count)
    {
        if (count > 26)
            throw new ArgumentException("Max 26 platser kan bokas"); // finns max 26 bokstäver för att platserna ska vara på en rad

        var eVouchers = new List<EVoucherInformation>();
        int row = random.Next(1, 201); // radnummer 1–200
        string gate = GenerateGate();

        for (int i = 0; i < count; i++)
        {
            char seatLetter = (char)('A' + i); // A, B, C, ...
            string seat = $"{row}{seatLetter}";
            eVouchers.Add(new EVoucherInformation { SeatNumber = seat, Gate = gate });
        }

        return eVouchers;
    }

    // Genererat från chatgpt. Skapar slumpmässig gate, returnerar string med A-Z.
    public static string GenerateGate()
    {
        char gate = (char)('A' + random.Next(0, 26));
        return gate.ToString();
    }
}
