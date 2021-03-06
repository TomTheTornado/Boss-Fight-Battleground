using System.Collections.Generic;

namespace BFB.Engine.Simulation.Configuration
{
    public class ComponentJSONSchema
    {
        /// <summary>
        /// The configuration data to construct all simulation components
        /// </summary>
        public Dictionary<string, EntityComponentConfiguration> EntityComponents { get; set; }
        
//        /// <summary>
//        /// The configuration data to construct all ITem components
//        /// </summary>
        public Dictionary<string, ItemComponentConfiguration> ItemComponents { get; set; }
//        
//        /// <summary>
//        /// The configuration data to construct all Block components
//        /// </summary>
        public Dictionary<string, BlockComponentConfiguration> BlockComponents { get; set; }
    }
}