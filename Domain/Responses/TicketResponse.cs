namespace Domain.Responses;

public class TicketResponse : ResponseResult
{
}

public class TicketResponse<T> : TicketResponse
{
    public T? Result { get; set; }
}
