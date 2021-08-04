using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceRace

{
    internal class Camera
    {

        public Vector3 CameraPosition
        {
            get;
            set;
        }

        public Vector3 CameraDirection
        {
            get;
            internal set;
        }

        public Vector3 CameraUp
        {
            get;
            internal set;
        }

        public Camera()
        {
            CameraPosition = Vector3.Zero;
            CameraDirection = Vector3.Forward;
            CameraUp = Vector3.Up;
        }
    }
}
