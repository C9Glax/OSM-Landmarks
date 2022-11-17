using Logging;
using System.Xml;
using GeoGraph;
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
            Dictionary<ulong, Node> nodes = new();

            List<Address> addresses = new();
            Address currentAddress;
            Stream mapData = File.Exists(filePath) ? new FileStream(filePath, FileMode.Open, FileAccess.Read) : new MemoryStream(OSM_Data.map);

            XmlReader reader = XmlReader.Create(mapData, readerSettings);
            while (reader.ReadToFollowing("node"))
            {
                currentAddress = new Address();
                XmlReader nodeReader = reader.ReadSubtree();
                currentAddress.locationId = Convert.ToUInt64(reader.GetAttribute("id"));
                currentAddress.lat = Convert.ToSingle(reader.GetAttribute("lat"));
                currentAddress.lon = Convert.ToSingle(reader.GetAttribute("lon"));
                nodes.Add(currentAddress.locationId, new Node(currentAddress.lat, currentAddress.lon));
                while (nodeReader.ReadToDescendant("tag"))
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
                if(currentAddress.street != null)
                {
                    addresses.Add(currentAddress);
                }
            }

            mapData.Position = 0;
            reader = XmlReader.Create(mapData, readerSettings);
            while (reader.ReadToFollowing("way"))
            {
                currentAddress = new Address();
                XmlReader wayReader = reader.ReadSubtree();
                while (wayReader.Read())
                {
                    if(wayReader.Name == "nd")
                    {
                        ulong id = Convert.ToUInt64(wayReader.GetAttribute("ref"));
                        if (nodes.ContainsKey(id))
                        {
                            currentAddress.locationId = id;
                            currentAddress.lat = nodes[id].lat;
                            currentAddress.lon = nodes[id].lon;
                        }
                    }
                    else if(wayReader.Name == "tag")
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
                if (currentAddress.street != null)
                {
                    addresses.Add(currentAddress);
                }
            }
            return new Landmarks(addresses);
        }



    }
}
