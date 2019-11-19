using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Claims;
using BFB.Engine.Entity;
using BFB.Engine.Input.PlayerInput;
using BFB.Engine.Math;
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.TileMap;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace BFB.Engine.Simulation.InputComponents
{
    /// <summary>
    /// A Input component that is used for remotely controlling a entity from a socket connection
    /// </summary>
    public class RemoteInputComponent : IInputComponent
    {
        
        #region Properties

        private readonly object _lock;

        private PlayerState _playerState;
        
        #endregion

        /// <summary>
        /// Constructs a RemoteInputComponent using a ClientSocket
        /// </summary>
        /// <param name="socket">The controlling players ClientSocket</param>
        public RemoteInputComponent(ClientSocket socket)
        {
            _lock = new object();
            _playerState = new PlayerState();
            
            socket.On("/player/input", (m) =>
            {
                InputMessage mm = (InputMessage) m;
                lock (_lock)
                {
                    _playerState = mm.PlayerInputState;//Update new position
                }
            });
            
        }

        public void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            lock (_lock)
            {
                //Set Mouse Position
                int mouseX = (int)(_playerState.Mouse.X + 0);
                int mouseY = (int)(_playerState.Mouse.Y + 0);

                //Add an AI monster
                if (_playerState.RightClick)
                {

                    //Add to simulation
                    /*simulation.AddEntity(new SimulationEntity(
                        Guid.NewGuid().ToString(),
                        new EntityOptions
                        {
                            AnimatedTextureKey = "Skeleton",
                            Position = new BfbVector(_playerState.Mouse.X, _playerState.Mouse.Y),
                            Dimensions = new BfbVector(2 * simulation.World.WorldOptions.WorldScale, 3 * simulation.World.WorldOptions.WorldScale),
                            Rotation = 0,
                            Origin = new BfbVector(0, 0),
                        }, new ComponentOptions
                        {
                            Physics = new SkeletonPhysicsComponent(),
                            Input = new AIInputComponent()
                        }));*/

                        BfbVector directionVector = new BfbVector(mouseX - simulationEntity.Position.X, mouseY - simulationEntity.Position.Y);
                        float direction = (float)System.Math.Atan2(directionVector.Y, directionVector.X);
                        simulation.AddEntity(new SimulationEntity(
                            Guid.NewGuid().ToString(),
                            new EntityOptions()
                            {
                                AnimatedTextureKey = "Fireball",
                                Position = new BfbVector(simulationEntity.Position.X, simulationEntity.Position.Y),
                                Dimensions = new BfbVector(50, 50),
                                Rotation = direction + (float)(System.Math.PI / 2),
                                Origin = new BfbVector(25,25),
                            }, new ComponentOptions()
                            {
                                Physics = new SpellPhysicsComponent(directionVector, simulationEntity)
                            }));
                }

                //Check block placement
                if (_playerState.RightClick || _playerState.LeftClick)
                {
                    //Combat
                    if (_playerState.LeftClick)
                    {
                        List<SimulationEntity> targets = new List<SimulationEntity>();
                        for (int i = 0; i < 100; i++)
                        {
                            int xPos = (int) simulationEntity.Position.X + i;
                            if (!simulationEntity.isFacingRight)
                                xPos = (int) simulationEntity.Position.X - i;

                            SimulationEntity target = simulation.GetEntityAtPosition(xPos
                                , (int) simulationEntity.Position.Y);
                            if (target != null && target != simulationEntity && !targets.Contains(target))
                                targets.Add(target);
                        }

                        Helpers.CombatService.FightPeople(simulationEntity, targets, simulation);
                    }

                    

                    Tuple<int, int, int, int> chunkInformation =
                        simulation.World.TranslatePixelPosition(mouseX, mouseY);

                    if (chunkInformation != null)
                    {

                        Chunk targetChunk =
                            simulation.World.ChunkFromChunkLocation(chunkInformation.Item1, chunkInformation.Item2);

                        int xSelection = chunkInformation.Item3;
                        int ySelection = chunkInformation.Item4;

                        if (targetChunk != null)
                        {
                            if (_playerState.RightClick)
                                targetChunk.ApplyBlockUpdate(new TileUpdate
                                {
                                    X = (byte) xSelection,
                                    Y = (byte) ySelection,
                                    Mode = true,
                                    TileValue = (ushort) WorldTile.Dirt
                                });
                            else
                            {
                                targetChunk.ApplyBlockUpdate(new TileUpdate
                                {
                                    X = (byte) xSelection,
                                    Y = (byte) ySelection,
                                    Mode = true,
                                    TileValue = (ushort) WorldTile.Air
                                });
                            }
                        }
                    }
                }

                //Resets the player movement
                simulationEntity.DesiredVector.X = 0;
                simulationEntity.DesiredVector.Y = 0;
                //Moves player left
                if (_playerState.Left)
                {
                    simulationEntity.DesiredVector.Add(new BfbVector(-1,0));
                }
                //Moves player right
                if (_playerState.Right)
                {
                    simulationEntity.DesiredVector.Add(new BfbVector(1,0));
                }
                //Moves player up
                if (_playerState.Jump && simulationEntity.Grounded)
                {
                    simulationEntity.DesiredVector.Add(new BfbVector(0,-1));
                }
            }
        }
    }
}