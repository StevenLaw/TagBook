using System;

namespace TagModel.ViewModel
{
    public enum ErrorType 
    { 
        ExceptionFound,
        FilenameNotSet
    }
    public class ErrorEncounteredErrorEventArgs : EventArgs
    {
        public ErrorType Type { get; set; }
        public Exception Exception { get; set; }

        public ErrorEncounteredErrorEventArgs(ErrorType type)
        {
            Type = type;
        }

        public ErrorEncounteredErrorEventArgs(Exception exception)
        {
            Exception = exception;
            Type = ErrorType.ExceptionFound;
        }
    }
}
