namespace OSM_Landmarks
{
    public class Landmarks
    {
        private List<Address> addresses = new();

        public Landmarks(List<Address> addresses)
        {
            this.addresses = addresses;
        }
    }
}
