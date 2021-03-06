using BFB.Engine.UI.Components;

namespace BFB.Engine.UI.Constraints
{
    public class UIPositionConstraint : UIConstraint
    {
        private readonly int? _top;
        private readonly int? _right;
        private readonly int? _bottom;
        private readonly int? _left;
        
        public UIPositionConstraint(int? top = null, int? right = null, int? bottom = null, int? left = null) : base(nameof(UIPositionConstraint))
        {
            _top = top;
            _right = right;
            _bottom = bottom;
            _left = left;
        }

        public override void Apply(UIComponent component)
        {
            if (component.Parent == null)
                return;
                
            if (_top != null)
            {
                component.DefaultAttributes.Y = component.Parent.DefaultAttributes.Y + (int) _top;
                component.DefaultAttributes.OffsetY = (int)_top;
                component.DefaultAttributes.Height -= (int) _top;
            }

            if (_right != null)
            {
                component.DefaultAttributes.Width -= (int) _right;
                component.DefaultAttributes.X += component.Parent.DefaultAttributes.Width - component.DefaultAttributes.Width;
                component.DefaultAttributes.OffsetX = component.Parent.DefaultAttributes.Width - component.DefaultAttributes.Width;
            }

            if (_bottom != null)
            {
                component.DefaultAttributes.Height -= (int) _bottom;
                
                component.DefaultAttributes.Y += component.Parent.DefaultAttributes.Height -  component.DefaultAttributes.Height;
                
                component.DefaultAttributes.OffsetY = component.Parent.DefaultAttributes.Height -  component.DefaultAttributes.Height;
            }

            if (_left != null)
            {
                component.DefaultAttributes.X += (int) _left;
                component.DefaultAttributes.OffsetX = (int) _left;
                component.DefaultAttributes.Width -= (int) _left;
            }
                
        }
    }
}