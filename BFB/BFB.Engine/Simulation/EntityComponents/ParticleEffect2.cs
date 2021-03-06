﻿using System;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.EntityComponents
{
    class ParticleEffect2 : EntityComponent
    {
        private readonly Random _random;

        public ParticleEffect2() : base(false)
        {
            _random = new Random();
        }
        public override void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            //Creates the new velocity
            simulationEntity.Velocity.X = System.Math.Abs(_random.Next(1, 10));
            simulationEntity.Velocity.Y = System.Math.Abs(_random.Next(1, 2) * 0);

            //Updates the position
            simulationEntity.Position.Add(simulationEntity.Velocity);
        }
    }
}
