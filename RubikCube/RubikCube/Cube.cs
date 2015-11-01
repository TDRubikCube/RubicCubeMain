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
using System.Threading;
using System.Diagnostics;

namespace RubikCube
{
    public class Cube
    {
        #region class vars
        Model model;
        Save save;
        Timer timer = new Timer(500);
        Matrix[] meshTransforms;
        readonly CubeState cubeState = new CubeState();
        readonly Random rand = new Random();
        public List<Vector3> ScramblingVectors;
        #endregion

        #region normal vars
        public int ScrambleIndex = 0;
        public int Angle;
        const int RotationSpeed = 10;
        int howManyTurns;
        public bool ShouldAddScrambleToOrder { get; set; }
        #endregion

        #region constructor

        public Cube()
        {
            meshTransforms = new Matrix[26];
            OriginalCubeDraw();
            ScramblingVectors = new List<Vector3>();
            save = new Save();
            model = null;
        }

        #endregion

        #region void
        public void Scramble()
        {
            List<Vector3> possibleTurns = new List<Vector3> { Vector3.Up, Vector3.Down, Vector3.Right, Vector3.Left, Vector3.Forward, Vector3.Backward };
            for (int i = 0; i < 50; i++)
            {
                Vector3 randomDirection = possibleTurns[rand.Next(0, 5)];
                ScramblingVectors.Add(randomDirection);
            }
        }

        public void Solve()
        {
            cubeState.OriginalCubeState();
            OriginalCubeDraw();
            ScramblingVectors.Clear();
        }

        public void Update(GameTime gameTime, bool shouldRotate, List<Vector3> rotatingPlan, bool isInverted)
        {
            if (shouldRotate)
            {
                //ADD SCRAMBLE

                //Scramble();
                //if (timer.CallTimer(gameTime) && rotatingPlan != null && rotatingPlan.Count != 0)
                //{
                //    Rotate(rotatingPlan[ScrambleIndex], isInverted);
                //    ScrambleIndex++;
                //}
            }
        }

        private void OriginalCubeDraw()
        {
            for (int i = 0; i < 26; i++)
            {
                meshTransforms[i] = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            }
        }

        public void Rotate(Vector3 side, bool isClockWise,string algOrder)
        {
            Angle -= RotationSpeed;
            float sidePosition = Game1.CubieSize;
            //Debug.WriteLine("animAngle=     " + Angle);
            //Debug.WriteLine("Angle " + MathHelper.ToRadians(Angle));

            if (side == Vector3.Left)
            {
                howManyTurns++;
                foreach (int i in cubeState.FindCubiesOnSide(side, isClockWise))
                {
                    Debug.WriteLine(i);
                    if (isClockWise)
                    {
                        RotateSide(i, MathHelper.PiOver2 / 10f, 1.5f * sidePosition, -1.5f * sidePosition, 0, 'x');
                    }
                    else
                    {
                        RotateSide(i, -MathHelper.PiOver2 / 10f, 1.5f * sidePosition, -1.5f * sidePosition, 0, 'x');
                    }
                }
            }
            else if (side == Vector3.Right)
            {
                howManyTurns++;
                foreach (int i in cubeState.FindCubiesOnSide(side, isClockWise))
                {
                    if (isClockWise)
                    {
                        RotateSide(i, -MathHelper.PiOver2 / 10f, -1.5f * sidePosition, -1.5f * sidePosition, 0, 'x');
                    }
                    else
                    {
                        RotateSide(i, MathHelper.PiOver2 / 10f, -1.5f * sidePosition, -1.5f * sidePosition, 0, 'x');
                    }
                }

            }
            else if (side == Vector3.Up)
            {
                howManyTurns++;
                foreach (int i in cubeState.FindCubiesOnSide(side, isClockWise))
                {
                    if (isClockWise)
                    {
                        RotateSide(i, -MathHelper.PiOver2 / 10f, 0, 0, 0, 'y');
                    }
                    else
                    {
                        RotateSide(i, MathHelper.PiOver2 / 10f, 0, 0, 0, 'y');
                    }
                }
            }
            else if (side == Vector3.Down)
            {
                howManyTurns++;
                foreach (int i in cubeState.FindCubiesOnSide(side, isClockWise))
                {
                    if (isClockWise)
                    {
                        RotateSide(i, -MathHelper.PiOver2 / 10f, 0, 0, 0, 'y');
                    }
                    else
                    {
                        RotateSide(i, MathHelper.PiOver2 / 10f, 0, 0, 0, 'y');
                    }
                }
            }
            else if (side == Vector3.Forward)
            {
                howManyTurns++;
                foreach (int i in cubeState.FindCubiesOnSide(side, isClockWise))
                {
                    if (isClockWise)
                    {
                        RotateSide(i, -MathHelper.PiOver2 / 10f, 0, -1.5f * sidePosition, -1.5f * sidePosition, 'z');
                    }
                    else
                    {
                        RotateSide(i, MathHelper.PiOver2 / 10f, 0, -1.5f * sidePosition, 1.5f * sidePosition, 'z');
                    }
                }
            }
            else if (side == Vector3.Backward)
            {
                howManyTurns++;
                foreach (int i in cubeState.FindCubiesOnSide(side, isClockWise))
                {
                    if (isClockWise)
                    {
                        RotateSide(i, MathHelper.PiOver2 / 10f, 0, -1.5f * sidePosition, 1.5f * sidePosition, 'z');
                    }
                    else
                    {
                        RotateSide(i, -MathHelper.PiOver2 / 10f, 0, -1.5f * sidePosition, -1.5f * sidePosition, 'z');
                    }
                }
            }
            if (howManyTurns == 10)
            {
                if ((side == Vector3.Backward || side == Vector3.Forward))
                {
                    if (isClockWise)
                        isClockWise = false;
                    else
                        isClockWise = true;
                }
                cubeState.Rotate(side, isClockWise);
                howManyTurns = 0;
            }
        }

        private void RotateSide(int i, float angle, float x, float y, float z, char rotationAxis)
        {
            if (rotationAxis == 'x')
            {
                MeshTransforms[i] *= Matrix.CreateTranslation(new Vector3(x, y, z)) * Matrix.CreateRotationX(angle) * Matrix.CreateTranslation(new Vector3(-x, -y, -z));
            }
            else if (rotationAxis == 'y')
            {
                MeshTransforms[i] *= Matrix.CreateTranslation(new Vector3(x, y, z)) * Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(new Vector3(-x, -y, -z));
            }
            else if (rotationAxis == 'z')
            {
                MeshTransforms[i] *= Matrix.CreateTranslation(new Vector3(x, y, z)) * Matrix.CreateRotationZ(angle) * Matrix.CreateTranslation(new Vector3(-x, -y, -z));
            }

        }
        #endregion

        #region model & matrix
        public Model Model
        {
            get { return model; }
            set { model = value; }
        }

        public Matrix[] MeshTransforms
        {
            get { return meshTransforms; }
            set { meshTransforms = value; }
        }
        #endregion
    }
}
 