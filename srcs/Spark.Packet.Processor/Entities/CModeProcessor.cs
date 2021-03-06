﻿using NLog;
using Spark.Event;
using Spark.Event.Entities.Player;
using Spark.Game.Abstraction;
using Spark.Game.Abstraction.Entities;
using Spark.Packet.Entities;

namespace Spark.Packet.Processor.Entities
{
    public class CModeProcessor : PacketProcessor<CMode>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IEventPipeline eventPipeline;

        public CModeProcessor(IEventPipeline eventPipeline) => this.eventPipeline = eventPipeline;

        protected override void Process(IClient client, CMode packet)
        {
            IMap map = client.Character.Map;
            if (map == null)
            {
                return;
            }

            ILivingEntity entity = map.GetEntity<ILivingEntity>(packet.EntityType, packet.EntityId);
            if (entity == null)
            {
                Logger.Debug($"Can't found entity {packet.EntityType} with id {packet.EntityId} in map {map.Id} (happens when joining a map)");
                return;
            }

            entity.MorphId = packet.MorphId;

            if (!entity.Equals(client.Character))
            {
                if (entity.MorphId > 0 && entity.MorphId <= 34)
                {
                    eventPipeline.Emit(new SpecialistWearEvent(client, entity, packet.MorphId));
                    Logger.Debug($"Entity {entity.EntityType} with id {entity.Id} wear SP {packet.MorphId}");
                }

                if (entity.MorphId == 0)
                {
                    eventPipeline.Emit(new SpecialistUnwearEvent(client, entity));
                    Logger.Debug($"Entity {entity.EntityType} with id {entity.Id} removed SP");
                }
            }
        }
    }
}