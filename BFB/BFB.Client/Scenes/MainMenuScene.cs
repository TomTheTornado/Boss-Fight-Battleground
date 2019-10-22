using System;
using BFB.Client.UI;
using BFB.Engine.Scene;
using BFB.Engine.TileMap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Client.Scenes
{
    public class MainMenuScene : Scene
    {
        
        private const int HeightY = 320;
        private const int WidthX = 480;

        private readonly Random _random;

        private readonly int _scale;
        private int _offset;
        private bool _grow;

        private enum Blocks {
            Air = 0,
            Grass,
            Dirt,
            Stone
        }
        
        private readonly TileMapManager _tileMap;

        public MainMenuScene() : base(nameof(MainMenuScene))
        {
            _tileMap = new TileMapManager();
            _random = new Random();
            _scale = 15;
            _grow = true;
            _offset = 0;
        }

        protected override void Init()
        {
            UIManager.Start(nameof(MainMenuUI));

            for (int x = 0; x < WidthX; x++)
            {
                for (int y = 0; y < HeightY; y++)
                {
                    
                    if(y < 11)
                    {
                        _tileMap.setBlock(x, y, (int)Blocks.Air);
                    }
                    else if (y < 12)
                    {
                        _tileMap.setBlock(x, y, (int)Blocks.Grass);
                    }
                    else if (y < 16)
                    {
                        _tileMap.setBlock(x, y, (int)Blocks.Dirt);
                    }
                    else if (y < 25)
                    {
                        if (_random.Next(y) + 2 > 16)
                        {
                            _tileMap.setBlock(x,y,(int)Blocks.Stone);
                        }
                        else
                        {
                            _tileMap.setBlock(x, y, (int) Blocks.Dirt);
                        }

                    }
                    else
                    {
                        _tileMap.setBlock(x, y, (int)Blocks.Stone);
                    }
                }
            }
        }
        
        protected override void Load()
        {
          
        }
        
        #region Update
        public override void Update(GameTime gameTime)
        {
            if (_grow)
            {
                _offset+=1;
            }
            else
            {
                _offset-=1;
            }
            if(_offset > 6400)
            {
                _grow = false;
            }
            if(_offset <= 0)
            {
                _grow = true;
            }
        }
        #endregion
        


        #region Draw
        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
            for (int x = 0; x < WidthX; x++)
            {
                for(int y = 0; y < HeightY; y++)
                {
                    switch(_tileMap.getBlock(x, y))
                    {
                        case (int)Blocks.Grass:
                            graphics.Draw(ContentManager.GetTexture("grass"), new Vector2(x * _scale - _offset, y * _scale), Color.White);
                            break;
                        case (int)Blocks.Dirt:
                            graphics.Draw(ContentManager.GetTexture("dirt"), new Vector2(x * _scale - _offset, y * _scale), Color.White);
                            break;
                        case (int)Blocks.Stone:
                            graphics.Draw(ContentManager.GetTexture("stone"), new Vector2(x * _scale - _offset, y * _scale), Color.White);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        #endregion

    }
    
}