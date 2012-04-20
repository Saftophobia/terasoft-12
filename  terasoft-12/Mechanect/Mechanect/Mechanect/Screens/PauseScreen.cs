using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Mechanect.Cameras;

namespace Mechanect.Classes
{
    class PauseScreen:Mechanect.Common.GameScreen
    {
        ContentManager content;
        Viewport viewPort;
        
        Texture2D givens;
        Vector2 givensPosition;


        Texture2D velocityBar;
        Vector2 vBarPositio;

        CustomModel rotationArrow;
        Vector3 arrowPosition;
        Vector3 arrowRotation;
        Vector3 arrowScale;
        float deltaArrowRotation;

        Camera camera;
        Vector3 cameraPosition;
        Vector3 targetPosition;

           public PauseScreen()
        {
            content = ScreenManager.Game.Content;
            viewPort = ScreenManager.GraphicsDevice.Viewport;
            arrowRotation = new Vector3(0, 0, 0);
            arrowPosition = new Vector3(viewPort.Width, 0, -1000);
            deltaArrowRotation = 0.02f;
            cameraPosition = new Vector3(-3000, 900, 100);
            targetPosition = new Vector3(100, 1000, 100);

        }

         
          public override void LoadContent()
        {
            givens = content.Load<Texture2D>("Textures/screen");
            rotationArrow = new CustomModel(content.Load<Model>("Model/Arrow"),arrowPosition,arrowRotation,arrowScale,ScreenManager.GraphicsDevice);
            camera = new TargetCamera(cameraPosition, targetPosition, ScreenManager.GraphicsDevice);
        }

        public override void UnloadContent()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {
            camera.Update();
            base.Update(gameTime, covered);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {


            rotationArrow.Draw(camera);
            
        }



        public override void Remove()
        {
            base.Remove();
        }
    }
    
}
