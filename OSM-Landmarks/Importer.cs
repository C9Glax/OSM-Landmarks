using Logging;
using System.Xml;
namespace OSM_Landmarks
{
    public class Importer
    {

        private static XmlReaderSettings readerSettings = new()
        {
            IgnoreWhitespace = true,
            IgnoreComments = true
        };

        public static Landmarks Import(string filePath = "", Logger? logger = null)
        {
            List<Address> addresses = new List<Address>();
            Stream mapData = File.Exists(filePath) ? new FileStream(filePath, FileMode.Open, FileAccess.Read) : new MemoryStream(OSM_Data.map);
            XmlReader reader = XmlReader.Create(mapData, readerSettings);
            reader.MoveToContent();

            Address currentAddress = new Address();
            string[] newAddressTrigger = { "node", "way" };

            while (reader.Read())
            {
                if (newAddressTrigger.Contains(reader.Name))
                {
                    addresses.Add(currentAddress);
                    currentAddress = new Address();

                    if (reader.Name == "node" || reader.Name == "nd")
                    {
                        currentAddress.locationId = Convert.ToUInt64(reader.GetAttribute("id"));
                    }

                }
                else if(reader.Name == "tag")
                {
#pragma warning disable CS8600, CS8601 //tags always have k and v
                    string key = (string)reader.GetAttribute("k");
                    string value = (string)reader.GetAttribute("v");
                    switch (key)
                    {
                        case "addr:street":
                        case "addr:conscriptionnumber":
                        case "addr:place":
                            currentAddress.street = value;
                            break;
                        case "addr:housenumber":
                        case "addr:housename":
                        case "addr:flats":
                            currentAddress.house = value;
                            break;
                        case "addr:postcode":
                            currentAddress.zipCode = value;
                            break;
                        case "addr:city":
                            currentAddress.city = value;
                            break;
                        case "addr:country":
                            currentAddress.country = value;
                            break;
                    }
#pragma warning restore CS8600
                }
            }
            

            return new Landmarks(addresses);
        }



    }
}
