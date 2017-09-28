using System;
namespace ChilliSource.Mobile.Location
{
    public class AuthorizationEventArgs : EventArgs
    {
        public AuthorizationEventArgs(LocationAuthorizationType authorizationType)
        {
            AuthorizationType = authorizationType;
        }

        public LocationAuthorizationType AuthorizationType { get; private set; }
    }
}
