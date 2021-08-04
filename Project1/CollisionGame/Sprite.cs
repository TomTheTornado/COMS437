using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CollisionGame
{
    class Sprite
    {
        public Rectangle rectangle;
        public Vector2 origin;
        public Vector2 position;
        public Vector2 velocity;
        public Texture2D texture;
        public Color[] colorData;
        public Color[,] spriteColor;

        /*Used to set the color data to use when checking for collisions.*/
        public void setColor()
        {
            colorData = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(colorData);
            spriteColor = new Color[texture.Width, texture.Height];

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    spriteColor[x, y] = colorData[x + y * texture.Width];
                }
            }
        }
    }


}
