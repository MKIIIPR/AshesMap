namespace AshesMap
{
    public class ResourcesClass
    {
        public class Resource
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public int RespawnTimer { get; set; }
        }

        public class ResourcePosition
        {
            public Resource Resource { get; set; }
            public string Desc { get; set; }
            public double PosX { get; set; }
            public double PosY { get; set; }
            public string Rarity { get; set; }
            public string Img { get;set; }
            public DateTime LastHarvest { get; set; }
            public DateTime RespawnAt => LastHarvest.AddMinutes(Resource?.RespawnTimer ?? 0);
        }
        public class Coordinates
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }
    }
}
