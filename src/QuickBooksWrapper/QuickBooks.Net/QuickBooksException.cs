using System;

namespace QuickBooks.Net
{
    public class QuickBooksException : Exception
    {
        public QuickBooksException(Int32 errorCode, String message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public Int32 ErrorCode { get; set; }
    }
}
