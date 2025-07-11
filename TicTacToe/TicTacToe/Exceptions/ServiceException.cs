namespace TicTacToe.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException(string title, string detail, int statusCode) : base(detail)
        {
            Title = title;
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }
        public string Title { get; set; }
    }
}
