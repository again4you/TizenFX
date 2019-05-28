using System;
using System.Linq;
using System.Reflection;
using System.Globalization;

using Tizen.NUI;
using System.ComponentModel;

namespace Tizen.NUI.Binding
{
    /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class PositionTypeConverter : TypeConverter
    {
        /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override object ConvertFromInvariantString(string value)
        {
            if (value != null)
            {
                string[] parts = value.Split('.');
                if (parts.Length == 1 || ( parts.Length == 2 && (parts[0].Trim() == "ParentOrigin" || parts[0].Trim() == "PivotPoint") ))
                {
                    string position = parts[parts.Length - 1].Trim();

                    switch(position)
                    {
                        case "Top":
                            return ParentOrigin.Top;
                        case "Bottom":
                            return ParentOrigin.Bottom;
                        case "Left":
                            return ParentOrigin.Left;
                        case "Right":
                            return ParentOrigin.Right;
                        case "Middle":
                            return ParentOrigin.Middle;
                        case "TopLeft":
                            return ParentOrigin.TopLeft;
                        case "TopCenter":
                            return ParentOrigin.TopCenter;
                        case "TopRight":
                            return ParentOrigin.TopRight;
                        case "CenterLeft":
                            return ParentOrigin.CenterLeft;
                        case "Center":
                            return ParentOrigin.Center;
                        case "CenterRight":
                            return ParentOrigin.CenterRight;
                        case "BottomLeft":
                            return ParentOrigin.BottomLeft;
                        case "BottomCenter":
                            return ParentOrigin.BottomCenter;
                        case "BottomRight":
                            return ParentOrigin.BottomRight;
                    }
                }

                parts = value.Split(',');
                if (parts.Length == 3)
                {
                    int x = (int)Tizen.NUI.GraphicsTypeManager.Instance.ConvertScriptToPixel(parts[0].Trim());
                    int y = (int)Tizen.NUI.GraphicsTypeManager.Instance.ConvertScriptToPixel(parts[1].Trim());
                    int z = (int)Tizen.NUI.GraphicsTypeManager.Instance.ConvertScriptToPixel(parts[2].Trim());
                    return new Position(x, y, z);
                }
            }

            throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Position)}");
        }
    }

    /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class Position2DTypeConverter : TypeConverter
    {
        /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override object ConvertFromInvariantString(string value)
        {
            return Position2D.ConvertFromString(value);
        }
    }
}
