using Microsoft.Xna.Framework.Graphics;

namespace GameJAM.Components {
    public interface IComponent {

        int AbsoluteX { get; }
        int AbsoluteY { get; }

        void Update( );
        void Render( );
        void Display( );

    }
}