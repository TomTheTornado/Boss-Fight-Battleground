﻿using BFB.Engine.Entity;
using BFB.Engine.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.Input.PlayerInput
{
    /// <summary>
    ///  This class will handle the input that the player gives
    /// transforming mouse & keyboard input to game controls.
    /// </summary>
    public class PlayerInput
    {

        private PlayerState _playerState;
        private bool _inputChange;

        /// <summary>
        /// Handles the input for each scene here.
        /// </summary>
        /// <param name="scene"></param>
        public PlayerInput(Scene.Scene scene)
        {
            _playerState = new PlayerState();
            _inputChange = false;
            
            scene.AddInputListener("keypress", (e) =>
            {
                switch (e.Keyboard.KeyEnum)
                {
                    case Keys.Left:
                    case Keys.A:
                        _playerState.Left = true;
                        _inputChange = true;
                        break;
                    case Keys.Right:
                    case Keys.D:
                        _playerState.Right = true;
                        _inputChange = true;
                        break;
                    case Keys.W:
                    case Keys.Space:
                        _playerState.Jump = true;
                        _inputChange = true;
                        break;
                }
                
                _playerState.Mouse.X = e.Mouse.X;
                _playerState.Mouse.Y = e.Mouse.Y;
            });
            
            scene.AddInputListener("keyup", (e) =>
            {
                switch (e.Keyboard.KeyEnum)
                {
                    case Keys.Left:
                    case Keys.A:
                        _playerState.Left = false;
                        _inputChange = true;
                        break;
                    case Keys.Right:
                    case Keys.D:
                        _playerState.Right = false;
                        _inputChange = true;
                        break;
                    case Keys.W:
                    case Keys.Space:
                        _playerState.Jump = false;
                        _inputChange = true;
                        break;
                }
                
                _playerState.Mouse.X = e.Mouse.X;
                _playerState.Mouse.Y = e.Mouse.Y;
            });
            
            scene.AddInputListener("mousemove", (e) =>
            {
                _playerState.Mouse.X = e.Mouse.X;
                _playerState.Mouse.Y = e.Mouse.Y;
                
                _inputChange = true;
            });
            
            scene.AddInputListener("mouseclick", (e) =>
            {
                _playerState.LeftClick = e.Mouse.LeftButton == ButtonState.Pressed;
                _playerState.RightClick = e.Mouse.RightButton == ButtonState.Pressed;
                
                _playerState.Mouse.X = e.Mouse.X;
                _playerState.Mouse.Y = e.Mouse.Y;
                
                _inputChange = true;
            });
            
            scene.AddInputListener("mouseup", (e) =>
            {
                _playerState.LeftClick = e.Mouse.LeftButton == ButtonState.Pressed;
                _playerState.RightClick = e.Mouse.RightButton == ButtonState.Pressed;
                
                _playerState.Mouse.X = e.Mouse.X;
                _playerState.Mouse.Y = e.Mouse.Y;
                
                _inputChange = true;
            });
        }

        /// <summary>
        /// Gets the player state, shockingly enough.
        /// </summary>
        /// <returns></returns>
        public PlayerState GetPlayerState()
        {
            _inputChange = false;
            return _playerState.Clone();
        }
        
        ///<summary>
        /// Allows viewing of the player state without indicating that the input had been updated
        /// </summary>
        /// <returns></returns>
        public PlayerState PeekPlayerState()
        {
            return _playerState.Clone();
        }

        /// <summary>
        /// Returns if the input has changed from the last game update.
        /// </summary>
        /// <returns></returns>
        public bool InputChanged()
        {
            return _inputChange;
        }
    }
}