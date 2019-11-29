/*
 * Zachary Cook
 *
 * All of the exceptions used by the SDK.
 */

using System;

namespace Cnp.Sdk
{
    public class CnpOnlineException : Exception
    {
        public CnpOnlineException(string message) : base(message)
        {
            
        }

        public CnpOnlineException(string message, Exception e) : base(message, e)
        {

        }
    }
    
    public class CnpConnectionLimitExceededException : Exception
    {
        public CnpConnectionLimitExceededException(string message) : base(message)
        {
            
        }

        public CnpConnectionLimitExceededException(string message, Exception e) : base(message, e)
        {

        }
    }
    
    public class CnpInvalidCredentialException : Exception
    {
        public CnpInvalidCredentialException(string message) : base(message)
        {
            
        }

        public CnpInvalidCredentialException(string message, Exception e) : base(message, e)
        {

        }
    }
    
    public class CnpObjectionableContentException : Exception
    {
        public CnpObjectionableContentException(string message) : base(message)
        {
            
        }

        public CnpObjectionableContentException(string message, Exception e) : base(message, e)
        {

        }
    }
}