using BFB.Engine.UI.Constraints;
using Microsoft.Xna.Framework;

namespace BFB.Engine.UI
{
    public static class UIConstraintExtensions
    {
        
        #region Width

        public static UIComponent Width(this UIComponent component, int pixels)
        {
            return component.AddConstraint(new UIWidthConstraint(pixels));
        }
        
        public static UIComponent Width(this UIComponent component, float percent)
        {
            return component.AddConstraint(new UIWidthConstraint(percent));
        }
        
        #endregion
        
        #region Height

        public static UIComponent Height(this UIComponent component, int pixels)
        {
            return component.AddConstraint(new UIHeightConstraint(pixels));
        }
        
        public static UIComponent Height(this UIComponent component, float percent)
        {
            return component.AddConstraint(new UIHeightConstraint(percent));
        }
        
        #endregion
        
        #region Top

        public static UIComponent Top(this UIComponent component, int pixels)
        {
            return component.AddConstraint(new UIPositionConstraint(top: pixels));
        }
        
        #endregion
        
        #region Right
        
        public static UIComponent Right(this UIComponent component, int pixels)
        {
            return component.AddConstraint(new UIPositionConstraint(right: pixels));
        }
        
        #endregion
        
        #region Bottom
        
        public static UIComponent Bottom(this UIComponent component, int pixels)
        {
            return component.AddConstraint(new UIPositionConstraint(bottom: pixels));
        }
        
        #endregion
        
        #region Left
        
        public static UIComponent Left(this UIComponent component, int pixels)
        {
            return component.AddConstraint(new UIPositionConstraint(left: pixels));
        }
        
        #endregion
        
        #region Image

        public static UIComponent Image(this UIComponent component, string textureKey)
        {
            component.DefaultAttributes.TextureKey = textureKey;
            component.DefaultAttributes.Background = Microsoft.Xna.Framework.Color.White;
            return component;
        }
        
        #endregion
        
        #region Center

        public static UIComponent Center(this UIComponent component)
        {
            return component.AddConstraint(new UICenterConstraint());
        }

        #endregion
        
        #region Grow

        public static UIComponent Grow(this UIComponent component, int proportion)
        {
            component.DefaultAttributes.Grow = proportion;
            return component;
        }
        
        #endregion
        
        #region AspectRatio

        /**
         * Define the aspect ratio as a float. Ex: 16:9 ~= 1.78f
         */
        public static UIComponent AspectRatio(this UIComponent component, float ratio)
        {
            return component.AddConstraint(new UIAspectRatioConstraint(ratio));
        }
        
        #endregion
        
        #region FontSize

        public static UIComponent FontSize(this UIComponent component, float fontScale)
        {
            return component.AddConstraint(new UIFontSizeConstraint(fontScale));
        }   
        
        #endregion
        
        #region Background

        public static UIComponent Background(this UIComponent component, Color? color)
        {
            return component.AddConstraint(new UIBackgroundConstraint(color ?? Microsoft.Xna.Framework.Color.Transparent));
        }

        #endregion

        #region Color

        public static UIComponent Color(this UIComponent component, Color? color)
        {
            return component.AddConstraint(new UIColorConstraint(color ?? Microsoft.Xna.Framework.Color.Black));
        }

        #endregion
        
        #region Position

        public static UIComponent Position(this UIComponent component, Position position)
        {
            component.DefaultAttributes.Position = position;
            return component;
        }
        
        #endregion
        
        #region JustifyText

        public static UIComponent JustifyText(this UIComponent component, JustifyText justifyText)
        {
            component.DefaultAttributes.JustifyText = justifyText;
            return component;
        }
        
        #endregion
        
        #region VerticalAlignText

        public static UIComponent VerticalAlignText(this UIComponent component, VerticalAlignText verticalAlignText)
        {
            component.DefaultAttributes.VerticalAlignText = verticalAlignText;
            return component;
        }
        
        #endregion
        
        #region FontScaleMode

        public static UIComponent FontScaleMode(this UIComponent component,  FontScaleMode fontScaleMode)
        {
            component.DefaultAttributes.FontScaleMode = fontScaleMode;
            return component;
        }
        
        #endregion
        
        #region Overflow

        public static UIComponent Overflow(this UIComponent component, Overflow overflow)
        {
            component.DefaultAttributes.Overflow = overflow;
            return component;

        }
        
        #endregion
        
        #region Border

        public static UIComponent Border(this UIComponent component, int borderSize = 1,  Color? borderColor = null)
        {
            component.DefaultAttributes.BorderSize = borderSize;
            component.DefaultAttributes.BorderColor = borderColor ?? Microsoft.Xna.Framework.Color.Black;
            return component;
        }
        
        #endregion
        
        #region Sizing

        public static UIComponent Sizing(this UIComponent component, Sizing sizing)
        {
            component.DefaultAttributes.Sizing = sizing;
            return component;
        }
        
        #endregion
        
    }
}