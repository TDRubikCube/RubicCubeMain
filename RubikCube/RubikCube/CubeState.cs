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

namespace RubikCube
{
    public class CubeState
    {
        readonly int[, ,] cubeArray = new int[3, 3, 3];

        public CubeState()
        {
            OriginalCubeState();
        }

        public void OriginalCubeState()
        {
            cubeArray[0, 0, 0] = 14;
            cubeArray[0, 0, 1] = 5;
            cubeArray[0, 0, 2] = 19;
            cubeArray[0, 1, 0] = 2;
            cubeArray[0, 1, 1] = 13;
            cubeArray[0, 1, 2] = 0;
            cubeArray[0, 2, 0] = 4;
            cubeArray[0, 2, 1] = 16;
            cubeArray[0, 2, 2] = 12;
            //check
            cubeArray[1, 0, 0] = 15;
            cubeArray[1, 1, 0] = 1;
            cubeArray[1, 2, 0] = 7;
            cubeArray[1, 2, 1] = 25;
            cubeArray[1, 2, 2] = 9;
            cubeArray[1, 1, 2] = 3;
            cubeArray[1, 0, 2] = 24;
            cubeArray[1, 0, 1] = 10;
            //check
            cubeArray[2, 0, 0] = 6;
            cubeArray[2, 0, 1] = 8;
            cubeArray[2, 0, 2] = 22;
            cubeArray[2, 1, 0] = 18;
            cubeArray[2, 1, 1] = 21;
            cubeArray[2, 1, 2] = 11;
            cubeArray[2, 2, 0] = 17;
            cubeArray[2, 2, 1] = 20;
            cubeArray[2, 2, 2] = 23;
        }

        public int GetCubie(int x, int y, int z)
        {
            return cubeArray[x, y, z];
        }

        public int[] FindCubiesOnSide(Vector3 side,bool isClockWise)
        {
            int x = 0, y = 0, z = 0;
            int limitX = 0, limitY = 0, limitZ = 0;
            FindLimitsX(ref side, ref x, ref limitX);
            FindLimitsY(ref side, ref y, ref limitY);
            FindLimitsZ(ref side, ref z, ref limitZ);
            List<int> returnValue = new List<int>();
            for (; x < limitX; x++)
            {
                if (side.Z >= 0) z = 0;
                if (side.Z < 0) z = 2;
                for (; z < limitZ; z++)
                {
                    if (side.Y <= 0) y = 0;
                    if (side.Y > 0) y = 2;
                    for (; y < limitY; y++)
                    {
                        returnValue.Add(cubeArray[x, y, z]);
                    }
                }
            }

            //Rotate(side,isClockWise);
            return returnValue.ToArray();
        }

        public void Rotate(Vector3 side, bool isClockWise)
        {
            if (side == Vector3.Backward)
            {
                if (isClockWise)
                {
                    int originalCenterNum = cubeArray[1, 0, 0];
                    cubeArray[1, 0, 0] = cubeArray[2, 1, 0];
                    cubeArray[2, 1, 0] = cubeArray[1, 2, 0];
                    cubeArray[1, 2, 0] = cubeArray[0, 1, 0];
                    cubeArray[0, 1, 0] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 0, 0];
                    cubeArray[0, 0, 0] = cubeArray[2, 0, 0];
                    cubeArray[2, 0, 0] = cubeArray[2, 2, 0];
                    cubeArray[2, 2, 0] = cubeArray[0, 2, 0];
                    cubeArray[0, 2, 0] = originalOuterNum;
                }
                else
                {
                    int originalCenterNum = cubeArray[1, 0, 0];
                    cubeArray[1, 0, 0] = cubeArray[0, 1, 0];
                    cubeArray[0, 1, 0] = cubeArray[1, 2, 0];
                    cubeArray[1, 2, 0] = cubeArray[2, 1, 0];
                    cubeArray[2, 1, 0] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 0, 0];
                    cubeArray[0, 0, 0] = cubeArray[0, 2, 0];
                    cubeArray[0, 2, 0] = cubeArray[2, 2, 0];
                    cubeArray[2, 2, 0] = cubeArray[2, 0, 0];
                    cubeArray[2, 0, 0] = originalOuterNum;
                }
            }
            else if (side == Vector3.Forward)
            {
                if (isClockWise)
                { 
                    int originalCenterNum = cubeArray[1, 0, 2];
                    cubeArray[1, 0, 2] = cubeArray[0, 1, 2];
                    cubeArray[0, 1, 2] = cubeArray[1, 2, 2];
                    cubeArray[1, 2, 2] = cubeArray[2, 1, 2];
                    cubeArray[2, 1, 2] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 0, 2];
                    cubeArray[0, 0, 2] = cubeArray[0, 2, 2];
                    cubeArray[0, 2, 2] = cubeArray[2, 2, 2];
                    cubeArray[2, 2, 2] = cubeArray[2, 0, 2];
                    cubeArray[2, 0, 2] = originalOuterNum;
                }
                else
                {
                    int originalCenterNum = cubeArray[1, 0, 2];
                    cubeArray[1, 0, 2] = cubeArray[2, 1, 2];
                    cubeArray[2, 1, 2] = cubeArray[1, 2, 2];
                    cubeArray[1, 2, 2] = cubeArray[0, 1, 2];
                    cubeArray[0, 1, 2] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 0, 2];
                    cubeArray[0, 0, 2] = cubeArray[2, 0, 2];
                    cubeArray[2, 0, 2] = cubeArray[2, 2, 2];
                    cubeArray[2, 2, 2] = cubeArray[0, 2, 2];
                    cubeArray[0, 2, 2] = originalOuterNum;
                }
            }
            else if (side == Vector3.Up)
            {
                if (isClockWise)
                {
                    int originalCenterNum = cubeArray[1, 2, 0];
                    cubeArray[1, 2, 0] = cubeArray[2, 2, 1];
                    cubeArray[2, 2, 1] = cubeArray[1, 2, 2];
                    cubeArray[1, 2, 2] = cubeArray[0, 2, 1];
                    cubeArray[0, 2, 1] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 2, 0];
                    cubeArray[0, 2, 0] = cubeArray[2, 2, 0];
                    cubeArray[2, 2, 0] = cubeArray[2, 2, 2];
                    cubeArray[2, 2, 2] = cubeArray[0, 2, 2];
                    cubeArray[0, 2, 2] = originalOuterNum;
                }
                else
                {
                    int originalCenterNum = cubeArray[1, 2, 0];
                    cubeArray[1, 2, 0] = cubeArray[0, 2, 1];
                    cubeArray[0, 2, 1] = cubeArray[1, 2, 2];
                    cubeArray[1, 2, 2] = cubeArray[2, 2, 1];
                    cubeArray[2, 2, 1] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 2, 0];
                    cubeArray[0, 2, 0] = cubeArray[0, 2, 2];
                    cubeArray[0, 2, 2] = cubeArray[2, 2, 2];
                    cubeArray[2, 2, 2] = cubeArray[2, 2, 0];
                    cubeArray[2, 2, 0] = originalOuterNum;
                }
            }
            else if (side == Vector3.Down)
            {
                if (isClockWise)
                {
                    int originalCenterNum = cubeArray[1, 0, 0];
                    cubeArray[1, 0, 0] = cubeArray[2, 0, 1];
                    cubeArray[2, 0, 1] = cubeArray[1, 0, 2];
                    cubeArray[1, 0, 2] = cubeArray[0, 0, 1];
                    cubeArray[0, 0, 1] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 0, 0];
                    cubeArray[0, 0, 0] = cubeArray[2, 0, 0];
                    cubeArray[2, 0, 0] = cubeArray[2, 0, 2];
                    cubeArray[2, 0, 2] = cubeArray[0, 0, 2];
                    cubeArray[0, 0, 2] = originalOuterNum;
                }
                else
                {
                    int originalCenterNum = cubeArray[1, 0, 0];
                    cubeArray[1, 0, 0] = cubeArray[0, 0, 1];
                    cubeArray[0, 0, 1] = cubeArray[1, 0, 2];
                    cubeArray[1, 0, 2] = cubeArray[2, 0, 1];
                    cubeArray[2, 0, 1] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 0, 0];
                    cubeArray[0, 0, 0] = cubeArray[0, 0, 2];
                    cubeArray[0, 0, 2] = cubeArray[2, 0, 2];
                    cubeArray[2, 0, 2] = cubeArray[2, 0, 0];
                    cubeArray[2, 0, 0] = originalOuterNum;
                }
            }
            else if (side == Vector3.Left)
            {
                if (isClockWise)
                { 
                    int originalCenterNum = cubeArray[0, 1, 0];
                    cubeArray[0, 1, 0] = cubeArray[0, 2, 1];
                    cubeArray[0, 2, 1] = cubeArray[0, 1, 2];
                    cubeArray[0, 1, 2] = cubeArray[0, 0, 1];
                    cubeArray[0, 0, 1] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 0, 0];
                    cubeArray[0, 0, 0] = cubeArray[0, 2, 0];
                    cubeArray[0, 2, 0] = cubeArray[0, 2, 2];
                    cubeArray[0, 2, 2] = cubeArray[0, 0, 2];
                    cubeArray[0, 0, 2] = originalOuterNum;
                }
                else
                {
                    int originalCenterNum = cubeArray[0, 1, 0];
                    cubeArray[0, 1, 0] = cubeArray[0, 0, 1];
                    cubeArray[0, 0, 1] = cubeArray[0, 1, 2];
                    cubeArray[0, 1, 2] = cubeArray[0, 2, 1];
                    cubeArray[0, 2, 1] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 0, 0];
                    cubeArray[0, 0, 0] = cubeArray[0, 0, 2];
                    cubeArray[0, 0, 2] = cubeArray[0, 2, 2];
                    cubeArray[0, 2, 2] = cubeArray[0, 2, 0];
                    cubeArray[0, 2, 0] = originalOuterNum;
                }
            }
            else if (side == Vector3.Right)
            {
                if (isClockWise)
                {                    
                    int originalCenterNum = cubeArray[2, 1, 0];
                    cubeArray[2, 1, 0] = cubeArray[2, 0, 1];
                    cubeArray[2, 0, 1] = cubeArray[2, 1, 2];
                    cubeArray[2, 1, 2] = cubeArray[2, 2, 1];
                    cubeArray[2, 2, 1] = originalCenterNum;
                    int originalOuterNum = cubeArray[2, 0, 0];
                    cubeArray[2, 0, 0] = cubeArray[2, 0, 2];
                    cubeArray[2, 0, 2] = cubeArray[2, 2, 2];
                    cubeArray[2, 2, 2] = cubeArray[2, 2, 0];
                    cubeArray[2, 2, 0] = originalOuterNum;
                }
                else
                {
                    int originalCenterNum = cubeArray[2, 1, 0];
                    cubeArray[2, 1, 0] = cubeArray[2, 2, 1];
                    cubeArray[2, 2, 1] = cubeArray[2, 1, 2];
                    cubeArray[2, 1, 2] = cubeArray[2, 0, 1];
                    cubeArray[2, 0, 1] = originalCenterNum;
                    int originalOuterNum = cubeArray[2, 0, 0];
                    cubeArray[2, 0, 0] = cubeArray[2, 2, 0];
                    cubeArray[2, 2, 0] = cubeArray[2, 2, 2];
                    cubeArray[2, 2, 2] = cubeArray[2, 0, 2];
                    cubeArray[2, 0, 2] = originalOuterNum; 
                }
            }
        }

        #region find limits

        private static void FindLimitsX(ref Vector3 side, ref int x, ref int limitX)
        {
            if (side.X == 0)
            {
                x = 0; limitX = 3;
            }
            if (side.X < 0)
            {
                x = 0; limitX = 1;
            }
            if (side.X > 0)
            {
                x = 2; limitX = 3;
            }
        }
        private static void FindLimitsY(ref Vector3 side, ref int y, ref int limitY)
        {
            if (side.Y == 0)
            {
                y = 0; limitY = 3;
            }
            if (side.Y < 0)
            {
                y = 0; limitY = 1;
            }
            if (side.Y > 0)
            {
                y = 2; limitY = 3;
            }
        }
        private static void FindLimitsZ(ref Vector3 side, ref int z, ref int limitZ)
        {
            if (side.Z == 0)
            {
                z = 0; limitZ = 3;
            }
            if (side.Z < 0)
            {
                z = 2; limitZ = 3;
            }
            if (side.Z > 0)
            {
                z = 0; limitZ = 1;
            }
        }

        #endregion

        #region constants

        //white
        public const int WhiteTopLeft = 12;
        public const int WhiteTopMid = 16;
        public const int WhiteTopRight = 4;
        public const int WhiteMidLeft = 0;
        public const int WhiteMidMid = 13;
        public const int WhiteMidRight = 2;
        public const int WhiteBottomLeft = 19;
        public const int WhiteBottomMid = 5;
        public const int WhiteBottomRight = 14;
        //BLUE
        public const int BlueTopLeft = 12;
        public const int BlueTopMid = 0;
        public const int BlueTopRight = 19;
        public const int BlueMidLeft = 9;
        public const int BlueMidMid = 3;
        public const int BlueMidRight = 24;
        public const int BlueBottomLeft = 23;
        public const int BlueBottomMid = 11;
        public const int BlueBottomRight = 22;
        //RED
        public const int RedTopLeft = 23;
        public const int RedTopMid = 20;
        public const int RedTopRight = 17;
        public const int RedMidLeft = 9;
        public const int RedMidMid = 25;
        public const int RedMidRight = 7;
        public const int RedBottomLeft = 12;
        public const int RedBottomMid = 16;
        public const int RedBottomRight = 4;
        //ORANGE
        public const int OrangeTopLeft = 19;
        public const int OrangeTopMid = 5;
        public const int OrangeTopRight = 14;
        public const int OrangeMidLeft = 24;
        public const int OrangeMidMid = 10;
        public const int OrangeMidRight = 15;
        public const int OrangeBottomLeft = 22;
        public const int OrangeBottomMid = 8;
        public const int OrangeBottomRight = 6;
        //GREEN
        public const int GreenTopLeft = 4;
        public const int GreenTopMid = 2;
        public const int GreenTopRight = 14;
        public const int GreenMidLeft = 15;
        public const int GreenMidMid = 1;
        public const int GreenMidRight = 7;
        public const int GreenBottomLeft = 6;
        public const int GreenBottomMid = 18;
        public const int GreenBottomRight = 17;
        //YELLOW
        public const int YellowTopLeft = 22;
        public const int YellowTopMid = 8;
        public const int YellowTopRight = 6;
        public const int YellowMidLeft = 11;
        public const int YellowMidMid = 21;
        public const int YellowMidRight = 18;
        public const int YellowBottomLeft = 23;
        public const int YellowBottomMid = 20;
        public const int YellowBottomRight = 17;

        #endregion
    }
}
