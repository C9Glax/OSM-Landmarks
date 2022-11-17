namespace OSM_Landmarks
{
    public class Landmarks
    {
        private List<Address> addresses = new();

        public Landmarks(List<Address> addresses)
        {
            this.addresses = addresses;
        }

        public List<Address> GetAddressesForQuery(string query)
        {
            List<Address> ret = new();
            foreach(Address address in addresses)
            {
                foreach (string partQuery in query.Split(new char[] {' ', ','}))
                {
                    foreach(string partAddress in address.ToString().Split(new char[] {' ', ',' }))
                    {
                        if(partAddress.ToUpper() == partQuery.ToUpper() && !ret.Contains(address))
                        {
                            ret.Add(address);
                        }
                    }
                }
            }
            return ret;
        }
    }
}
