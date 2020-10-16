using Microsoft.Xna.Framework.Graphics;

namespace GameJAM.Components {
    public interface IComponent {

        void Update( );
        void Render( );
        void Display(int x, int y);

    }
}