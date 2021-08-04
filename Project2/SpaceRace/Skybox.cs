using System;
using System.Collections.Generic;
using System.Text;

using BEPUphysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceRace
{
    class Skybox : DrawableGameComponent
    {
        private Model model;
        private Texture2D modelTexture;


        public Skybox(Game game) : base(game)
        {
            game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()//loading the model and texture
        {
            modelTexture = Game.Content.Load<Texture2D>("Textures\\space");
            model = Game.Content.Load<Model>("Models\\ssphere");

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Camera c = Game.Services.GetService<Camera>();
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;//turning culling off so we can see the texture on the inside of the skybox
            GraphicsDevice.RasterizerState = rs;
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.Alpha = 1f;
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = ConversionHelper.MathConverter.Convert(
                        ConversionHelper.MathConverter.Convert(Matrix.CreateScale(6500f))); //scaling the skybox up
                    effect.View = Matrix.CreateLookAt(c.CameraPosition, c.CameraDirection, c.CameraUp);
                    float aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
                    float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
                    float nearClipPlane = 0.1f;
                    float farClipPlane = 13000;
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
                }
                mesh.Draw();
            }
            RasterizerState rs2 = new RasterizerState();
            rs2.CullMode = CullMode.CullCounterClockwiseFace;//turning culling back on
            GraphicsDevice.RasterizerState = rs2;
            base.Draw(gameTime);
        }


        private void Events_InitialCollisionDetected(BEPUphysics.BroadPhaseEntries.MobileCollidables.EntityCollidable sender, BEPUphysics.BroadPhaseEntries.Collidable other, BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler pair)
        {
            int i = 0;
        }

    }
}
