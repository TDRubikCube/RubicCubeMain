using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace RubikCube
{
            
    class Camera
    {
        Vector3 cameraPos;
        private Vector3 oldCameraPos;
        Vector2 previousMousePosition;
        private MouseState oldMouseState;
        private Matrix view;
        float horizontalAngle;
        float verticalAngle;
        int previousMouseWheel;
        float radius = 27.32f;

        public Camera()
        {
            view = Matrix.CreateLookAt(new Vector3(20, 20, 20), new Vector3(0,0,0), Vector3.Up);
            //numbers
            verticalAngle = AngleBetweenVectorAndAxis(Matrix.Invert(view).Translation, "x");
            horizontalAngle = AngleBetweenVectorAndAxis(Matrix.Invert(view).Translation, "y");
        }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            cameraPos = Matrix.Invert(view).Translation;
            CameraMovement(mouseState,oldMouseState);
            oldMouseState = mouseState;
            previousMouseWheel = mouseState.ScrollWheelValue;
        }

        /// <summary>
        /// sets the vector3 vars according to the camera position
        /// </summary>
        /// <param name="cameraPos">the camera position as a vector3</param>
        public void RealRotate(Vector3 cameraPos)
        {
            if (cameraPos.X >= -radius / 1.61f && cameraPos.X <= radius / 1.61f && cameraPos.Z > radius / 1.438f && cameraPos.Z <= radius * 1.012f)
            {
                RealLeft = Vector3.Left;
                RealRight = Vector3.Right;
                RealForward = Vector3.Backward;
                RealBackward = Vector3.Forward;
            }
            if (cameraPos.X >= radius / 1.366f && cameraPos.X <= radius * 1.012f && cameraPos.Z >= -radius / 1.366f && cameraPos.Z <= radius / 1.366f)
            {
                RealLeft = Vector3.Backward;
                RealRight = Vector3.Forward;
                RealForward = Vector3.Right;
                RealBackward = Vector3.Left;
            }
            if (cameraPos.X >= -radius / 1.366f && cameraPos.X <= radius / 1.366f && cameraPos.Z >= -radius * 1.012f && cameraPos.Z <= -radius / 1.52f)
            {
                RealLeft = Vector3.Right;
                RealRight = Vector3.Left;
                RealForward = Vector3.Forward;
                RealBackward = Vector3.Backward;
            }
            if (cameraPos.X >= -radius * 1.012f && cameraPos.X <= -radius / 1.52f && cameraPos.Z >= -radius / 1.52f && cameraPos.Z <= radius / 1.52f)
            {
                RealLeft = Vector3.Forward;
                RealRight = Vector3.Backward;
                RealForward = Vector3.Left;
                RealBackward = Vector3.Right;
            }
        }

        /// <summary>
        /// makes the camera move using the mouse
        /// </summary>
        /// <param name="mouseState"></param>
        /// <param name="oldMouseState"></param>
        private void CameraMovement(MouseState mouseState, MouseState oldMouseState)
        {

            Vector2 currentMousePos = new Vector2(mouseState.X, mouseState.Y);
            previousMousePosition = new Vector2(oldMouseState.X, oldMouseState.Y);
            horizontalAngle += (currentMousePos.X - previousMousePosition.X) * 0.01f;
            //if (IsCameraAtRightAngle(oldCameraPos) || verticalAngle.Equals(0))
           // {
                verticalAngle += (currentMousePos.Y - previousMousePosition.Y) * 0.01f;
            //}
            Vector3 newCameraPosition = new Vector3(
               -(float)(radius * Math.Sin(horizontalAngle) * Math.Sin(verticalAngle)),  //x
                (float)(radius * Math.Cos(verticalAngle)),                              //y
                (float)(radius * Math.Sin(verticalAngle) * Math.Cos(horizontalAngle))); //z
            oldCameraPos = newCameraPosition;
            #region restrict camera(FIX)
            //if (!IsCameraAtRightAngle(newCameraPosition))
            //{
            //    float angle = AngleBetweenVectorAndAxis(newCameraPosition, "x");
            //    float positionY;
            //    if (angle > 0)
            //    {
            //        positionY = (float)(Math.Tan(51 / 57.3f) * newCameraPosition.Z);
            //    }
            //    else
            //    {
            //        positionY = (float)(Math.Tan(-51 / 57.3f) * newCameraPosition.Z);
            //    }
            //    Debug.WriteLine(positionY + " = fake " + newCameraPosition.Y + " = real");
            //    newCameraPosition = new Vector3(newCameraPosition.X, positionY, newCameraPosition.Z);
            //    oldCameraPos = newCameraPosition;
            //}
            //Debug.WriteLine(newCameraPosition.X + " =x");
            //Debug.WriteLine(newCameraPosition.Y + " =y");
            //Debug.WriteLine(newCameraPosition.Z + " =z");
            #endregion
            if (mouseState.ScrollWheelValue < previousMouseWheel && radius < 107.32)
            {
                radius += 2;
            }
            if (mouseState.ScrollWheelValue > previousMouseWheel && radius > 17.33)
            {
                radius -= 2;
            }
            view = Matrix.CreateLookAt(newCameraPosition, new Vector3(0,0,0), Vector3.Up);
        }

        private bool IsCameraAtRightAngle(Vector3 cameraPosition)
        {
            return AngleBetweenVectorAndAxis(cameraPosition, "x") < 50 && AngleBetweenVectorAndAxis(cameraPosition, "x") > -50;
        }

        private float AngleBetweenVectorAndAxis(Vector3 vector, string constant)
        {
            if (constant == "x" && !vector.Z.Equals(0))
            {
                //Debug.WriteLine(vector.Y/vector.Z + " = prop");
                //Debug.WriteLine(Math.Atan(vector.Y / vector.Z) + " = atan");
                Debug.WriteLine(Math.Atan(vector.Y / vector.Z) * 57.3 + " = final");
                return MathHelper.ToDegrees((float)(Math.Atan(vector.Y / vector.Z)));
            }
            if (constant == "y" && !vector.Z.Equals(0))
            {
                return MathHelper.ToDegrees((float)(Math.Atan(vector.X / vector.Z)));
            }
            if (constant == "z" && !vector.X.Equals(0))
            {
                return MathHelper.ToDegrees((float)(Math.Atan(vector.Y / vector.X)));
            }
            return 0;
        }

        public Matrix View
        {
            get {return view;}
            set {view = value;}
        }

        #region RealRotate
        public Vector3 RealLeft { get; set; }
        public Vector3 RealRight { get; set; }
        public Vector3 RealForward { get; set; }
        public Vector3 RealBackward { get; set; }

        #endregion
    }
}
