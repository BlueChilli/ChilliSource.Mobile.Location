using System;
namespace ChilliSource.Mobile.Location
{
    /// <summary>
    /// Event args for representing region event data, including beacon ids
    /// </summary>
    public class RegionEventArgs : EventArgs
    {
        public string Identifier
        {
            get;
            private set;
        }

        public string UUID
        {
            get;
            private set;
        }

        public string Major
        {
            get;
            private set;
        }

        public string Minor
        {
            get;
            private set;
        }

        public RegionEventArgs(string uuid, string major, string minor)
        {
            UUID = uuid;
            Major = major;
            Minor = minor;
            Identifier = $"{UUID}_{Major}_{Minor}";
        }

        public RegionEventArgs(string identifier)
        {
            Identifier = identifier;
        }
    }
}
