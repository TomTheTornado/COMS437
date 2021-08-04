using System;
using System.Collections.Generic;
using System.Text;

using BEPUphysics;
using BEPUphysics.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceRace
{
    class Spaceship : DrawableGameComponent
    {
        private Model model;
        private Texture2D shipTexture;
        public BEPUphysics.Entities.Prefabs.Sphere physicsObject;
        //public BEPUphysics.Entities.Prefabs.MobileMesh physicsObject;
        Vector3[] vertices;
        int[] indices;

        public Vector3 CurrentPosition
        {
            get
            {
                return ConversionHelper.MathConverter.Convert(physicsObject.Position);
            } 
        }

        public Spaceship(Game game) : base(game)
        {
            game.Components.Add(this);
        }

        public Spaceship(Game game, Vector3 pos, string id) : this(game)
        {
            physicsObject = new BEPUphysics.Entities.Prefabs.Sphere(ConversionHelper.MathConverter.Convert(pos), 1);
            //TriangleMesh.GetVerticesAndIndicesFromModel(model)
            //physicsObject = new BEPUphysics.Entities.Prefabs.MobileMesh();
            //new MobileMesh(modelVertices, modelIndices, new AffineTransform(new Vector3(.2f, .2f, .2f), Quaternion.Identity, new Vector3(0, -10, 0)),
            physicsObject.AngularDamping = 0f;
            physicsObject.LinearDamping = 0f;
            physicsObject.CollisionInformation.Events.InitialCollisionDetected += Events_InitialCollisionDetected;
            physicsObject.Tag = id;
            Game.Services.GetService<Space>().Add(physicsObject);
        }   



        public Spaceship(Game game, Vector3 pos, string id, float mass) : this(game, pos, id)
        {
            physicsObject.Mass = mass;
            physicsObject.BecomeKinematic(); //TODO - change after fixing collisions
        }

        public Spaceship(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum) : this(game, pos, id, mass)
        {
            physicsObject.LinearMomentum = ConversionHelper.MathConverter.Convert(linMomentum);
        }

        public Spaceship(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum, Vector3 angMomentum) : this(game, pos, id, mass, linMomentum)
        {
            physicsObject.AngularMomentum = ConversionHelper.MathConverter.Convert(angMomentum);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()//loading the model and texture
        {
            shipTexture = Game.Content.Load<Texture2D>("Textures\\wedge_p1_diff_v1");
            model = Game.Content.Load<Model>("Models\\p1_wedge");
            //TriangleMesh.GetVerticesAndIndicesFromModel(model, out vertices, out indices);

            physicsObject.Radius = 0.1f * model.Meshes[0].BoundingSphere.Radius;

            
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
                    effect.Alpha = 0.7f;
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = ConversionHelper.MathConverter.Convert(
                        ConversionHelper.MathConverter.Convert(Matrix.CreateScale(0.1f)) * physicsObject.WorldTransform); //scaling the ship down
                    effect.View = Matrix.CreateLookAt(c.CameraPosition, c.CameraDirection, c.CameraUp);
                    float aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
                    float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
                    float nearClipPlane = 0.1f;
                    float farClipPlane = 3500;
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
