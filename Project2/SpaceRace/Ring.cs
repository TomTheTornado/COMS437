using System;
using System.Collections.Generic;
using System.Text;

using BEPUphysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceRace
{
    class Ring : DrawableGameComponent
    {
        private Model model;
        private Texture2D activeTexture, defaultTexture, finishTexture, passedTexture;
        public BEPUphysics.Entities.Prefabs.Sphere physicsObject;
        public bool activeRing = false;
        public bool finishRing = false;
        public bool passedRing = false;
        public bool missedRing = false;

        public Vector3 CurrentPosition
        {
            get
            {
                return ConversionHelper.MathConverter.Convert(physicsObject.Position);
            }
        }

        public Ring(Game game) : base(game)
        {
            game.Components.Add(this);
        }

        public Ring(Game game, Vector3 pos, string id) : this(game)
        {
            physicsObject = new BEPUphysics.Entities.Prefabs.Sphere(ConversionHelper.MathConverter.Convert(pos), 1);
            physicsObject.AngularDamping = 0f;
            physicsObject.LinearDamping = 0f;
            physicsObject.CollisionInformation.Events.InitialCollisionDetected += Events_InitialCollisionDetected;
            physicsObject.Tag = id;
            Game.Services.GetService<Space>().Add(physicsObject);
        }



        public Ring(Game game, Vector3 pos, string id, float mass) : this(game, pos, id)
        {
            physicsObject.Mass = mass;
            physicsObject.BecomeKinematic();//add infinite mass
        }

        public Ring(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum) : this(game, pos, id, mass)
        {
            physicsObject.LinearMomentum = ConversionHelper.MathConverter.Convert(linMomentum);
        }

        public Ring(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum, Vector3 angMomentum) : this(game, pos, id, mass, linMomentum)
        {
            physicsObject.AngularMomentum = ConversionHelper.MathConverter.Convert(angMomentum);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()//loading the model and textures
        {
            activeTexture = Game.Content.Load<Texture2D>("Textures\\striped3");
            defaultTexture = Game.Content.Load<Texture2D>("Textures\\gold");
            finishTexture = Game.Content.Load<Texture2D>("Textures\\checkerboard");
            passedTexture = Game.Content.Load<Texture2D>("Textures\\orange");
            model = Game.Content.Load<Model>("Models\\torus");
            physicsObject.Radius = 1f * model.Meshes[0].BoundingSphere.Radius;


            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Camera c = Game.Services.GetService<Camera>();
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (finishRing)//texture uses checkerboard
                    {
                        effect.Texture = finishTexture;
                    }
                    else if (passedRing)//texture is orange
                    {
                        effect.Texture = passedTexture;
                    }
                    else if (activeRing)//texture is striped
                    {
                        effect.Texture = activeTexture;
                    }
                    else//texture is gold
                    {
                        effect.Texture = defaultTexture;
                    }
                    effect.Alpha = 1f;
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = ConversionHelper.MathConverter.Convert(
                        ConversionHelper.MathConverter.Convert(Matrix.CreateScale(200f)) * physicsObject.WorldTransform);//scaling the skybox up
                    effect.View = Matrix.CreateLookAt(c.CameraPosition, c.CameraDirection, c.CameraUp);
                    float aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
                    float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
                    float nearClipPlane = 0.1f;
                    float farClipPlane = 8000;
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }


        private void Events_InitialCollisionDetected(BEPUphysics.BroadPhaseEntries.MobileCollidables.EntityCollidable sender, BEPUphysics.BroadPhaseEntries.Collidable other, BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler pair)
        {
            int i = 0;
        }

    }
}
