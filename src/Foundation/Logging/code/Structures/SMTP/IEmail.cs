namespace Lotus.Foundation.Logging.Structures.SMTP
{
    public interface IEmail
    {
        string Header { get; set; }
        string Body { get; set; }
    }
}