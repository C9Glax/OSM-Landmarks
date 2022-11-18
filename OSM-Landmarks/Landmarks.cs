namespace OSM_Landmarks
{
    public class Landmarks
    {
        public List<Address> addresses = new();

        public Landmarks(List<Address> addresses)
        {
            this.addresses = addresses;
        }


        public List<Address> GetAddressesForQuery(string query)
        {
            List<Address> ret = new();
            string upquery = Upgrade(query);
            foreach(Address address in addresses)
            {
                string upaddress = Upgrade(address.ToString());
                foreach (string partQuery in upquery.Split(new char[] {' ', ',', '-'}))
                {
                    foreach (string partAddress in upaddress.ToString().Split(new char[] { ' ', ',', '-' }))
                    {
                        if (partAddress.ToUpper() == partQuery.ToUpper() && !ret.Contains(address))
                        {
                            ret.Add(address);
                        }
                    }
                }
            }
            return ret;
        }

        static string[] filterWords = { "am", "zum", "auf", "straße", "weg", "allee", "von" };
        private string Upgrade(string s)
        {
            string ret = s.ToLower();
            foreach(string filter in filterWords)
            {
                ret = ret.Replace(filter, "");
            }
            return ret;
        }
    }
}
