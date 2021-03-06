﻿using System;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.EntityComponents
{
    class ParticleEffect1 : EntityComponent
    {
        private readonly Random _random;

        public ParticleEffect1() : base(false)
        {
            _random = new Random();
        }
        public override void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            //Creates the new velocity
            simulationEntity.Velocity.X = _random.Next(1, 10);
            simulationEntity.Velocity.Y = _random.Next(1, 10);

            //Updates the position
            simulationEntity.Position.Add(simulationEntity.Velocity);
        }
    }
}
