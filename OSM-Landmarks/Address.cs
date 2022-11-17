namespace OSM_Landmarks
{
    public struct Address
    {
        public string house;
        public string street;
        public string city;
        public string zipCode;
        public string country;
        public ulong locationId; //NodeId
        public float lat, lon;

        public override string ToString()
        {
            return string.Format("{0} {1}, {2} {3}, {4} - ID:{5} Coords:{6}#{7}", street, house, zipCode, city, country, locationId, lat, lon);
        }
    }
}
