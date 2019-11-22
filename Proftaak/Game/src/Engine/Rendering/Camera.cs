using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.src.Engine.Rendering
{
    class Camera
    {
        public Vector3 Position;
        public Vector3 Rotation;

        private Matrix4 RotationMatrix;

        public Camera(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;

            Matrix4 rotX = new Matrix4(
                1, 0,                            0,                           0,
                0, (float)Math.Cos(rotation.X),  (float)Math.Sin(rotation.X), 0,
                0, (float)-Math.Sin(rotation.X), (float)Math.Cos(rotation.X), 0,
                0, 0,                            0,                           1
                );

            Matrix4 rotY = new Matrix4(
                (float)Math.Cos(rotation.Y),  0, (float)Math.Sin(rotation.Y), 0,
                0,                            1, 0,                           0,
                (float)-Math.Sin(rotation.Y), 0, (float)Math.Cos(rotation.Y), 0,
                0,                            0, 0,                           1
                );
            
            Matrix4 rotZ = new Matrix4(
                (float)Math.Cos(rotation.Z),  (float)Math.Sin(rotation.Z), 0, 0,
                (float)-Math.Sin(rotation.Z), (float)Math.Cos(rotation.Z), 0, 0,
                0,                            0,                           1, 0,
                0,                            0,                           0, 1
                );

            RotationMatrix = rotZ * rotY * rotX;
        }

        //TODO: Add helper functions here, like setting rotation and such
    }
}
