using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Components
{
    public class UIVstackComponent : UIComponent
    {
        public UIVstackComponent() : base(nameof(UIVstackComponent))
        {
            DefaultAttributes.StackDirection = StackDirection.Vertical;
        }
    }
}