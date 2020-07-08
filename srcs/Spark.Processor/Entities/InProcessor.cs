﻿using System.Threading.Tasks;
using NLog;
using Spark.Core.Enum;
using Spark.Event;
using Spark.Event.Entities;
using Spark.Game.Abstraction;
using Spark.Game.Abstraction.Entities;
using Spark.Game.Abstraction.Factory;
using Spark.Packet.Entities;

namespace Spark.Processor.Entities
{
    public class InProcessor : PacketProcessor<In>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IEventPipeline _eventPipeline;
        private readonly IEntityFactory _entityFactory;

        public InProcessor(IEventPipeline eventPipeline, IEntityFactory entityFactory)
        {
            _eventPipeline = eventPipeline;
            _entityFactory = entityFactory;
        }

        protected override Task Process(IClient client, In packet)
        {
            IMap map = client.Character.Map;
            if (map == null)
            {
                Logger.Warn("Can't process in packet, character map is null");
                return Task.CompletedTask;
            }

            IEntity entity;
            switch (packet.EntityType)
            {
                case EntityType.Monster:
                    entity = _entityFactory.CreateMonster(packet.EntityId, packet.GameKey);
                    break;
                case EntityType.Npc:
                    entity = _entityFactory.CreateNpc(packet.EntityId, packet.GameKey);
                    break;
                case EntityType.Player:
                    entity = _entityFactory.CreatePlayer(packet.EntityId);
                    break;
                case EntityType.MapObject:
                    entity = _entityFactory.CreateMapObject(packet.EntityId, packet.GameKey);
                    break;
                default:
                    Logger.Error($"Undefined switch clause for entity type {packet.EntityType}");
                    return Task.CompletedTask;
            }

            map.AddEntity(entity);
            _eventPipeline.Emit(new EntitySpawnEvent(map, entity));

            return Task.CompletedTask;
        }
    }
}