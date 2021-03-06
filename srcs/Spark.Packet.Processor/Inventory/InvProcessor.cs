﻿using NLog;
using Spark.Core.Enum;
using Spark.Game.Abstraction;
using Spark.Game.Abstraction.Entities;
using Spark.Game.Abstraction.Factory;
using Spark.Game.Abstraction.Inventory;
using Spark.Packet.Inventory;

namespace Spark.Packet.Processor.Inventory
{
    public class InvProcessor : PacketProcessor<Inv>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        private readonly IObjectFactory objectFactory;

        public InvProcessor(IObjectFactory objectFactory) => this.objectFactory = objectFactory;

        protected override void Process(IClient client, Inv packet)
        {
            ICharacter character = client.Character;
            BagType bagType = packet.BagType;
            
            foreach (Inv.ObjectInfo objectInfo in packet.Objects)
            {
                IObjectStack objectStack = objectFactory.CreateObjectStack(bagType, objectInfo.ObjectKey, objectInfo.Slot, objectInfo.Amount);
                if (objectStack == null)
                {
                    continue;
                }
                
                character.Inventory.AddObject(objectStack);
            }

            Logger.Debug($"Inventory {bagType} successfully initialized");
            if (bagType == BagType.Costume)
            {
                Logger.Debug($"{character.Name} inventory successfully initialized");
            }
        }
    }
}