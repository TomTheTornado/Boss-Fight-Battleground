using System.Collections.Generic;
using BFB.Engine.TileMap;

namespace BFB.Engine.Inventory.Configuration
{
    public class ItemConfiguration
    {
        public string TextureKey { get; set; }
        
        public int StackLimit { get; set; }
        
        public List<string> LeftHoldComponents { get; set; }
        
        public List<string> LeftClickComponents { get; set; }
        
        public List<string> RightHoldComponents { get; set; }
        
        public List<string> RightClickComponents { get; set; }
        
        public ItemType ItemType { get; set; }
        
        public WorldTile TileKey { get; set; }
        
    }
}