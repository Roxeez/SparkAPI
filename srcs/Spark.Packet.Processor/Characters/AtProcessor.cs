﻿using NLog;
using Spark.Event;
using Spark.Event.Characters;
using Spark.Game.Abstraction;
using Spark.Game.Abstraction.Factory;
using Spark.Packet.Characters;

namespace Spark.Packet.Processor.Characters
{
    public class AtProcessor : PacketProcessor<At>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IEventPipeline eventPipeline;
        private readonly IMapFactory mapFactory;

        public AtProcessor(IMapFactory mapFactory, IEventPipeline eventPipeline)
        {
            this.mapFactory = mapFactory;
            this.eventPipeline = eventPipeline;
        }

        protected override void Process(IClient client, At packet)
        {
            IMap map = mapFactory.CreateMap(packet.MapId);
            if (map == null)
            {
                Logger.Error($"Failed to create map {packet.MapId}");
                return;
            }

            IMap currentMap = client.Character.Map;
            if (currentMap != null)
            {
                eventPipeline.Emit(new MapLeaveEvent(client, currentMap));
            }
            
            client.Character.Position = packet.Position;
            client.Character.Direction = packet.Direction;

            map.AddEntity(client.Character);
            eventPipeline.Emit(new MapJoinEvent(client, map));
        }
    }
}