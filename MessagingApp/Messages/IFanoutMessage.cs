namespace MessagingApp.Messages;

public interface IFanoutMessage
{
    public string Message { get; set; }
}