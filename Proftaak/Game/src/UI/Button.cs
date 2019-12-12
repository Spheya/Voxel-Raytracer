using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Rendering;
using Game.Engine.Maths;
using Game.Engine.Input;
using OpenTK;

namespace Game.UI
{
    class Button
    {
        public void AddToRenderer(SpriteRenderer renderer)
        {
            renderer.Add(_sprite);
        }
        public void RemoveToRenderer(SpriteRenderer renderer)
        {
            renderer.Remove(_sprite);
        }
        public void Update()
        {
            //Vector2 test = MouseInput.GetMousePos();
            //Console.WriteLine(test);
        }
        Sprite _sprite;
        public Button(Texture texture, Transform transformation)
        {
            _sprite = new Sprite(texture, transformation);
        }
        public Vector2 GetPosition()
        {
            return new Vector2(_sprite.Transform.Position.X, _sprite.Transform.Position.Y);
        }
        public Vector2 GetSize()
        {
            return new Vector2(_sprite.Transform.Scale.X, _sprite.Transform.Scale.Y);
        }
    }
}
