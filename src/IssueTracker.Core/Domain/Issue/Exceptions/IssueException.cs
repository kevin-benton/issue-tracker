using System;

namespace IssueTracker.Core.Domain.Issue.Exceptions
{
    public class IssueException : Exception
    {
        public IssueException(string message) : base(message)
        {
        }

        public IssueException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
