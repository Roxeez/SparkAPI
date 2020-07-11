﻿using System.Collections.Generic;
using Spark.Core;
using Spark.Core.Enum;
using Spark.Game.Abstraction.Entities;

namespace Spark.Game.Abstraction
{
    public interface IMap
    {
        public int Id { get; }
        public string NameKey { get; }
        public byte[] Grid { get; }
        public int Height { get; }
        public int Width { get; }

        public IEnumerable<IMonster> Monsters { get; }
        public IEnumerable<IPlayer> Players { get; }
        public IEnumerable<INpc> Npcs { get; }
        public IEnumerable<IMapObject> Objects { get; }

        public IEnumerable<IEntity> Entities { get; }

        IEntity GetEntity(EntityType entityType, long id);

        void AddEntity(IEntity entity);
        void RemoveEntity(IEntity entity);

        bool IsWalkable(Position position);
    }
}