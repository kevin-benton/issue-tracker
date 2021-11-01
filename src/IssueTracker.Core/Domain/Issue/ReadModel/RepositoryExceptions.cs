using System;

namespace IssueTracker.Core.Domain.Issue.ReadModel
{
    public class DocumentNotFoundException : Exception
    {
        public DocumentNotFoundException(string message) : base(message)
        {
        }

        public DocumentNotFoundException(string message, Exception ex) : base(message, ex)
        {
        }
    }

    public class DocumentConflictException : Exception
    {
        public DocumentConflictException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
