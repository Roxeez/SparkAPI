﻿using NLog;
using Spark.Database;
using Spark.Database.Data;
using Spark.Game.Abstraction;
using Spark.Game.Abstraction.Factory;

namespace Spark.Game.Factory
{
    public class MapFactory : IMapFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IDatabase database;

        public MapFactory(IDatabase database) => this.database = database;

        public IMap CreateMap(int mapId)
        {
            MapData data = database.Maps.GetValue(mapId);
            if (data == null)
            {
                Logger.Error($"Can't get map data with id {mapId} from database");
                return default;
            }

            var map = new Map(mapId, data);
            
            Logger.Debug($"Map {map.Id} created");
            return map;
        }
    }
}