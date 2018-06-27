using Microsoft.Xna.Framework;

namespace MapEditor.CustomControls
{
    public class MapDisplay : WinFormsGraphicsDevice.GraphicsDeviceControl
    {
        protected override void Draw()
        {

        }

        protected override void Initialize()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
        }
    }
}
