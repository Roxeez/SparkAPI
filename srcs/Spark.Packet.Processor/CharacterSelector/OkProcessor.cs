﻿using Spark.Game.Abstraction;
using Spark.Packet.CharacterSelector;

namespace Spark.Packet.Processor.CharacterSelector
{
    public class OkProcessor : PacketProcessor<Ok>
    {
        protected override void Process(IClient client, Ok packet)
        {
            client.SendPacket("game_start");
        }
    }
}