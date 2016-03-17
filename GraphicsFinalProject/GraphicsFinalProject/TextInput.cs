using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace NanozinProject
{
    public class TextInput : Sprite
    {
        public TextInput( string function, string caption, string start, int index )
        {
            mTexture = Nanozin.cloudsTexture;
            mPosition = new Vector2(Nanozin.SCREEN_MID.X, Nanozin.SCREEN_MID.Y);
            mOrigin = new Vector2(32, 32);
            mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mSourceRectangle = new Rectangle((Nanozin.rand.Next() % 3) * 64, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mRotation = 0f;
            mScale = 9f;
            mDepth = .99f;
            mTint = Color.Black;

            mFunction = function;
            mCaption = caption;
            mString = start;
            justPushed = Convert.ToChar(0x0);
            targetIndex = index;
        }
        ~TextInput() { }

        string mFunction,
               mCaption,
               mString;
        char justPushed;
        int targetIndex;

        public bool update()
        {
            bool done = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                done = true;
                Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
            }
            else
            {
                //get current input
                char currentPush = Convert.ToChar(0x0);

                if (!Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Back))
                    {
                        currentPush = Convert.ToChar(0x8);
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'A';
                        else
                            currentPush = 'a';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.B))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'B';
                        else
                            currentPush = 'b';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.C))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'C';
                        else
                            currentPush = 'c';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'D';
                        else
                            currentPush = 'd';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.E))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'E';
                        else
                            currentPush = 'e';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.F))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'F';
                        else
                            currentPush = 'f';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.G))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'G';
                        else
                            currentPush = 'g';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.H))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'H';
                        else
                            currentPush = 'h';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.I))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'I';
                        else
                            currentPush = 'i';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.J))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'J';
                        else
                            currentPush = 'j';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.K))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'K';
                        else
                            currentPush = 'k';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.L))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'L';
                        else
                            currentPush = 'l';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.M))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'M';
                        else
                            currentPush = 'm';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.N))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'N';
                        else
                            currentPush = 'n';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.O))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'O';
                        else
                            currentPush = 'o';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.P))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'P';
                        else
                            currentPush = 'p';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Q))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'Q';
                        else
                            currentPush = 'q';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.R))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'R';
                        else
                            currentPush = 'r';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'S';
                        else
                            currentPush = 's';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.T))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'T';
                        else
                            currentPush = 't';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.U))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'U';
                        else
                            currentPush = 'u';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.V))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'V';
                        else
                            currentPush = 'v';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'W';
                        else
                            currentPush = 'w';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.X))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'X';
                        else
                            currentPush = 'x';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Y))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'Y';
                        else
                            currentPush = 'y';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Z))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            currentPush = 'Z';
                        else
                            currentPush = 'z';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        currentPush = Convert.ToChar(0x5F);
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
                    {
                        currentPush = Convert.ToChar(0x2D);
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.OemPeriod))
                    {
                        currentPush = Convert.ToChar(0x2E);
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D1))
                    {
                        currentPush = '1';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D2))
                    {
                        currentPush = '2';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D3))
                    {
                        currentPush = '3';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D4))
                    {
                        currentPush = '4';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D5))
                    {
                        currentPush = '5';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D6))
                    {
                        currentPush = '6';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D7))
                    {
                        currentPush = '7';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D8))
                    {
                        currentPush = '8';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D9))
                    {
                        currentPush = '9';
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D0))
                    {
                        currentPush = '0';
                    }
                    else
                    {
                        currentPush = Convert.ToChar(0x0);
                    }
                }
                else
                {
                    if (mString.Length > 0)
                    {
                        int shift;
                        Rectangle theLevel = new Rectangle(0, 0, Nanozin.levelWidth, Nanozin.levelHeight);
                        switch (mFunction)
                        {
                            case "getFileName":
                                done = true;

                                // Check that there are no levels called this, unless this is one of those custom levels
                                if (File.Exists(Path.Combine(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName, "GraphicsFinalProjectContent\\mainLevels"), mString + ".txt"))
                                    || (File.Exists(Path.Combine(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName, "GraphicsFinalProjectContent\\customLevels"), mString + ".txt")) && Nanozin.levelName != mString + ".txt"))
                                    Nanozin.mustRenameLevel = true;
                                else
                                {
                                    Nanozin.mustRenameLevel = false;
                                    Nanozin.levelName = mString;
                                    Functions.saveLevel();
                                }
                                break;
                            case "setSalvagerAmmo":
                                if ((int)Convert.ToDouble(mString) >= 0)
                                {
                                    done = true;
                                    Nanozin.editorLevelAmmo = (int)Convert.ToDouble(mString);
                                }
                                break;
                            case "setAmmoSupply":
                                if ((int)Convert.ToDouble(mString) >= 0)
                                {
                                    done = true;
                                    Nanozin.ammos[targetIndex].supply = (int)Convert.ToDouble(mString);
                                }
                                break;
                            case "setLancerCD":
                                if (Convert.ToDouble(mString) >= .5f)
                                {
                                    done = true;
                                    Nanozin.plasmaLancers[targetIndex].coolDown = (float)Convert.ToDouble(mString);
                                }
                                break;
                            case "setTogglerCD":
                                if (Convert.ToDouble(mString) >= .5f)
                                {
                                    done = true;
                                    Nanozin.togglers[targetIndex].toggleCD = (float)Convert.ToDouble(mString);
                                }
                                break;
                            case "setLevelWidth":
                                if (Convert.ToInt32(mString) >= 20 && Convert.ToInt32(mString) <= 60)
                                {
                                    done = true;
                                    Nanozin.levelWidth = Convert.ToInt32(mString) * 64;
                                    Nanozin.levelCenter.X = Nanozin.levelWidth / 2;
                                }
                                break;
                            case "setLevelHeight":
                                if (Convert.ToInt32(mString) >= 12 && Convert.ToInt32(mString) <= 60)
                                {
                                    done = true;
                                    Nanozin.levelHeight = Convert.ToInt32(mString) * 64;
                                    Nanozin.levelCenter.Y = Nanozin.levelWidth / 2;
                                }
                                break;
                            case "loadLevel":
                                bool fileValid = false;

                                //Check that file exists
                                string pathEnd = "GraphicsFinalProjectContent\\mainLevels",
                                       folder = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName, pathEnd),
                                       file = mString + ".txt";
    
                                bool loadingMainLevel = false;

                                if (File.Exists(Path.Combine(folder, file))) // Check for main levels
                                {
                                    fileValid = true;
                                    loadingMainLevel = true;
                                }

                                pathEnd = "GraphicsFinalProjectContent\\customLevels"; // Check for custom levels
                                folder = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName, pathEnd);
                                if (File.Exists(Path.Combine(folder, file)))
                                    fileValid = true;

                                if (fileValid)
                                {
                                    done = true;
                                    Nanozin.levelName = mString;

                                    Nanozin.playTesting = true;

                                    Functions.endPlay();
                                    Functions.startPlay();

                                    Nanozin.cameraPosition = Nanozin.salvagerEdit;

                                    Nanozin.playTesting = false;

                                    //Some object manipulation
                                    if (Nanozin.theSalvager != null)
                                    {
                                        Nanozin.cameraPosition = Nanozin.theSalvager.mPosition;
                                        Nanozin.editorLevelAmmo = Nanozin.theSalvager.mAmmo;
                                        Nanozin.salvagerEdit = new Vector2(Nanozin.theSalvager.mPosition.X - 32, Nanozin.theSalvager.mPosition.Y - 32);
                                        Nanozin.salvEditRot = Nanozin.theSalvager.mRotation;

                                        if (Nanozin.theSalvager.mTexture == Nanozin.salvagerRifleTexture)
                                            Nanozin.theRifle.mPosition = new Vector2(Nanozin.theSalvager.mPosition.X, Nanozin.theSalvager.mPosition.Y);
                                        Nanozin.theSalvager = null;
                                    }

                                    if (loadingMainLevel)
                                    {
                                        Nanozin.mustRenameLevel = true;
                                    }
                                }
                                break;

                            case "shiftX":
                                done = true;
                                shift = Convert.ToInt32(mString) * 64;

                                //If player on screen, shift them
                                if (new Rectangle((int)Nanozin.salvagerEdit.X, (int)Nanozin.salvagerEdit.Y, 64, 64).Intersects(theLevel))
                                {
                                    Nanozin.salvagerEdit.X += shift;
                                    if (!new Rectangle((int)Nanozin.salvagerEdit.X, (int)Nanozin.salvagerEdit.Y, 64, 64).Intersects(theLevel))
                                        Nanozin.salvagerEdit = new Vector2(-64, -64);
                                }
                                //If rifle on screen shift it
                                if (Nanozin.theRifle.mBoundingBox.Intersects(new Rectangle(0, 0, Nanozin.levelWidth, Nanozin.levelHeight)))
                                {
                                    Nanozin.theRifle.mPosition.X += shift;
                                    Nanozin.theRifle.mBoundingBox = new Rectangle((int)Nanozin.theRifle.mPosition.X - 32, (int)Nanozin.theRifle.mPosition.Y - 32, 64, 64);
                                    if (!Nanozin.theRifle.mBoundingBox.Intersects(theLevel))
                                        Nanozin.theRifle.mPosition = new Vector2(-64, -64);
                                }

                                for (int i = 0; i < Nanozin.fuelCells.Count; i++)
                                {
                                    Nanozin.fuelCells[i].mPosition.X += shift;
                                    Nanozin.fuelCells[i].mBoundingBox = new Rectangle((int)Nanozin.fuelCells[i].mPosition.X - 32, (int)Nanozin.fuelCells[i].mPosition.Y - 32, 64, 64);
                                    if (!new Rectangle((int)Nanozin.fuelCells[i].mPosition.X - 32, (int)Nanozin.fuelCells[i].mPosition.Y - 32, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.fuelCells.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.ammos.Count; i++)
                                {
                                    Nanozin.ammos[i].mPosition.X += shift;
                                    Nanozin.ammos[i].mBoundingBox = new Rectangle((int)Nanozin.ammos[i].mPosition.X, (int)Nanozin.ammos[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.ammos[i].mPosition.X, (int)Nanozin.ammos[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.ammos.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.plasmaFilms.Count; i++)
                                {
                                    Nanozin.plasmaFilms[i].mPosition.X += shift;
                                    Nanozin.plasmaFilms[i].mBoundingBox = new Rectangle((int)Nanozin.plasmaFilms[i].mPosition.X - 32, (int)Nanozin.plasmaFilms[i].mPosition.Y - 32, 64, 64);
                                    if (!new Rectangle((int)Nanozin.plasmaFilms[i].mPosition.X - 32, (int)Nanozin.plasmaFilms[i].mPosition.Y - 32, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.plasmaFilms.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.nodes.Count; i++)
                                {
                                    Nanozin.nodes[i].mPosition.X += shift;
                                    Nanozin.nodes[i].mBoundingBox = new Rectangle((int)Nanozin.nodes[i].mPosition.X, (int)Nanozin.nodes[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.nodes[i].mPosition.X, (int)Nanozin.nodes[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.nodes.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.receivers.Count; i++)
                                {
                                    Nanozin.receivers[i].mPosition.X += shift;
                                    Nanozin.receivers[i].mBoundingBox = new Rectangle((int)Nanozin.receivers[i].mPosition.X, (int)Nanozin.receivers[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.receivers[i].mPosition.X, (int)Nanozin.receivers[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.receivers.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.togglers.Count; i++)
                                {
                                    Nanozin.togglers[i].mPosition.X += shift;
                                    Nanozin.togglers[i].mBoundingBox = new Rectangle((int)Nanozin.togglers[i].mPosition.X, (int)Nanozin.togglers[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.togglers[i].mPosition.X, (int)Nanozin.togglers[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.togglers.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.detectors.Count; i++)
                                {
                                    Nanozin.detectors[i].mPosition.X += shift;
                                    Nanozin.detectors[i].mBoundingBox = new Rectangle((int)Nanozin.detectors[i].mPosition.X - 32, (int)Nanozin.detectors[i].mPosition.Y - 32, 64, 64);
                                    if (!new Rectangle((int)Nanozin.detectors[i].mPosition.X - 32, (int)Nanozin.detectors[i].mPosition.Y - 32, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.detectors.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.pressurePlates.Count; i++)
                                {
                                    Nanozin.pressurePlates[i].mPosition.X += shift;
                                    Nanozin.pressurePlates[i].mBoundingBox = new Rectangle((int)Nanozin.pressurePlates[i].mPosition.X, (int)Nanozin.pressurePlates[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.pressurePlates[i].mPosition.X, (int)Nanozin.pressurePlates[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.pressurePlates.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.mirrors.Count; i++)
                                {
                                    Nanozin.mirrors[i].mPosition.X += shift;
                                    Nanozin.mirrors[i].mirrorPos.X += shift;
                                    Nanozin.mirrors[i].mBoundingBox = new Rectangle((int)Nanozin.mirrors[i].mPosition.X, (int)Nanozin.mirrors[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.mirrors[i].mPosition.X, (int)Nanozin.mirrors[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.mirrors.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.plasmaLancers.Count; i++)
                                {
                                    Nanozin.plasmaLancers[i].mPosition.X += shift;
                                    Nanozin.plasmaLancers[i].lancerPos.X += shift;
                                    Nanozin.plasmaLancers[i].mBoundingBox = new Rectangle((int)Nanozin.plasmaLancers[i].mPosition.X, (int)Nanozin.plasmaLancers[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.plasmaLancers[i].mPosition.X, (int)Nanozin.plasmaLancers[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.plasmaLancers.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.furnaces.Count; i++)
                                {
                                    Nanozin.furnaces[i].mPosition.X += shift;
                                    Nanozin.furnaces[i].mBoundingBox = new Rectangle((int)Nanozin.furnaces[i].mPosition.X, (int)Nanozin.furnaces[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.furnaces[i].mPosition.X, (int)Nanozin.furnaces[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.furnaces.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.walls.Count; i++)
                                {
                                    Nanozin.walls[i].mPosition.X += shift;
                                    Nanozin.walls[i].mBoundingBox = new Rectangle((int)Nanozin.walls[i].mPosition.X, (int)Nanozin.walls[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.walls[i].mPosition.X, (int)Nanozin.walls[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.walls.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.hardWalls.Count; i++)
                                {
                                    Nanozin.hardWalls[i].mPosition.X += shift;
                                    Nanozin.hardWalls[i].mBoundingBox = new Rectangle((int)Nanozin.hardWalls[i].mPosition.X, (int)Nanozin.hardWalls[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.hardWalls[i].mPosition.X, (int)Nanozin.hardWalls[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.hardWalls.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.crackedWalls.Count; i++)
                                {
                                    Nanozin.crackedWalls[i].mPosition.X += shift;
                                    Nanozin.crackedWalls[i].mBoundingBox = new Rectangle((int)Nanozin.crackedWalls[i].mPosition.X, (int)Nanozin.crackedWalls[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.crackedWalls[i].mPosition.X, (int)Nanozin.crackedWalls[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.crackedWalls.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.bombWalls.Count; i++)
                                {
                                    Nanozin.bombWalls[i].mPosition.X += shift;
                                    Nanozin.bombWalls[i].mBoundingBox = new Rectangle((int)Nanozin.bombWalls[i].mPosition.X, (int)Nanozin.bombWalls[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.bombWalls[i].mPosition.X, (int)Nanozin.bombWalls[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.bombWalls.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.icePatches.Count; i++)
                                {
                                    Nanozin.icePatches[i].mPosition.X += shift;
                                    Nanozin.icePatches[i].mBoundingBox = new Rectangle((int)Nanozin.icePatches[i].mPosition.X, (int)Nanozin.icePatches[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.icePatches[i].mPosition.X, (int)Nanozin.icePatches[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.icePatches.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.wastes.Count; i++)
                                {
                                    Nanozin.wastes[i].mPosition.X += shift;
                                    Nanozin.wastes[i].mBoundingBox = new Rectangle((int)Nanozin.wastes[i].mPosition.X, (int)Nanozin.wastes[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.wastes[i].mPosition.X, (int)Nanozin.wastes[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.wastes.RemoveAt(i);
                                        i--;
                                    }
                                }
                                break;
                            case "shiftY":
                                done = true;
                                shift = Convert.ToInt32(mString) * 64;

                                //If player on screen, shift them
                                if (new Rectangle((int)Nanozin.salvagerEdit.X, (int)Nanozin.salvagerEdit.Y, 64, 64).Intersects(theLevel))
                                {
                                    Nanozin.salvagerEdit.Y += shift;
                                    if (!new Rectangle((int)Nanozin.salvagerEdit.X, (int)Nanozin.salvagerEdit.Y, 64, 64).Intersects(theLevel))
                                        Nanozin.salvagerEdit = new Vector2(-64, -64);
                                }
                                //If rifle on screen shift it
                                if (Nanozin.theRifle.mBoundingBox.Intersects(new Rectangle(0, 0, Nanozin.levelWidth, Nanozin.levelHeight)))
                                {
                                    Nanozin.theRifle.mPosition.Y += shift;
                                    Nanozin.theRifle.mBoundingBox = new Rectangle((int)Nanozin.theRifle.mPosition.X - 32, (int)Nanozin.theRifle.mPosition.Y - 32, 64, 64);
                                    if (!Nanozin.theRifle.mBoundingBox.Intersects(theLevel))
                                        Nanozin.theRifle.mPosition = new Vector2(-64, -64);
                                }

                                for (int i = 0; i < Nanozin.fuelCells.Count; i++)
                                {
                                    Nanozin.fuelCells[i].mPosition.Y += shift;
                                    Nanozin.fuelCells[i].mBoundingBox = new Rectangle((int)Nanozin.fuelCells[i].mPosition.X - 32, (int)Nanozin.fuelCells[i].mPosition.Y - 32, 64, 64);
                                    if (!new Rectangle((int)Nanozin.fuelCells[i].mPosition.X - 32, (int)Nanozin.fuelCells[i].mPosition.Y - 32, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.fuelCells.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.ammos.Count; i++)
                                {
                                    Nanozin.ammos[i].mPosition.Y += shift;
                                    Nanozin.ammos[i].mBoundingBox = new Rectangle((int)Nanozin.ammos[i].mPosition.X, (int)Nanozin.ammos[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.ammos[i].mPosition.X, (int)Nanozin.ammos[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.ammos.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.plasmaFilms.Count; i++)
                                {
                                    Nanozin.plasmaFilms[i].mPosition.Y += shift;
                                    Nanozin.plasmaFilms[i].mBoundingBox = new Rectangle((int)Nanozin.plasmaFilms[i].mPosition.X - 32, (int)Nanozin.plasmaFilms[i].mPosition.Y - 32, 64, 64);
                                    if (!new Rectangle((int)Nanozin.plasmaFilms[i].mPosition.X, (int)Nanozin.plasmaFilms[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.plasmaFilms.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.nodes.Count; i++)
                                {
                                    Nanozin.nodes[i].mPosition.Y += shift;
                                    Nanozin.nodes[i].mBoundingBox = new Rectangle((int)Nanozin.nodes[i].mPosition.X, (int)Nanozin.nodes[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.nodes[i].mPosition.X, (int)Nanozin.nodes[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.nodes.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.receivers.Count; i++)
                                {
                                    Nanozin.receivers[i].mPosition.Y += shift;
                                    Nanozin.receivers[i].mBoundingBox = new Rectangle((int)Nanozin.receivers[i].mPosition.X, (int)Nanozin.receivers[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.receivers[i].mPosition.X, (int)Nanozin.receivers[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.receivers.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.togglers.Count; i++)
                                {
                                    Nanozin.togglers[i].mPosition.Y += shift;
                                    Nanozin.togglers[i].mBoundingBox = new Rectangle((int)Nanozin.togglers[i].mPosition.X, (int)Nanozin.togglers[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.togglers[i].mPosition.X, (int)Nanozin.togglers[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.togglers.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.detectors.Count; i++)
                                {
                                    Nanozin.detectors[i].mPosition.Y += shift;
                                    Nanozin.detectors[i].mBoundingBox = new Rectangle((int)Nanozin.detectors[i].mPosition.X, (int)Nanozin.detectors[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.detectors[i].mPosition.X - 32, (int)Nanozin.detectors[i].mPosition.Y - 32, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.detectors.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.pressurePlates.Count; i++)
                                {
                                    Nanozin.pressurePlates[i].mPosition.Y += shift;
                                    Nanozin.pressurePlates[i].mBoundingBox = new Rectangle((int)Nanozin.pressurePlates[i].mPosition.X, (int)Nanozin.pressurePlates[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.pressurePlates[i].mPosition.X, (int)Nanozin.pressurePlates[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.pressurePlates.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.mirrors.Count; i++)
                                {
                                    Nanozin.mirrors[i].mPosition.Y += shift;
                                    Nanozin.mirrors[i].mirrorPos.Y += shift;
                                    Nanozin.mirrors[i].mBoundingBox = new Rectangle((int)Nanozin.mirrors[i].mPosition.X, (int)Nanozin.mirrors[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.mirrors[i].mPosition.X, (int)Nanozin.mirrors[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.mirrors.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.plasmaLancers.Count; i++)
                                {
                                    Nanozin.plasmaLancers[i].mPosition.Y += shift;
                                    Nanozin.plasmaLancers[i].lancerPos.Y += shift;
                                    Nanozin.plasmaLancers[i].mBoundingBox = new Rectangle((int)Nanozin.plasmaLancers[i].mPosition.X, (int)Nanozin.plasmaLancers[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.plasmaLancers[i].mPosition.X, (int)Nanozin.plasmaLancers[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.plasmaLancers.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.furnaces.Count; i++)
                                {
                                    Nanozin.furnaces[i].mPosition.Y += shift;
                                    Nanozin.furnaces[i].mBoundingBox = new Rectangle((int)Nanozin.furnaces[i].mPosition.X, (int)Nanozin.furnaces[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.furnaces[i].mPosition.X, (int)Nanozin.furnaces[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.furnaces.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.walls.Count; i++)
                                {
                                    Nanozin.walls[i].mPosition.Y += shift;
                                    Nanozin.walls[i].mBoundingBox = new Rectangle((int)Nanozin.walls[i].mPosition.X, (int)Nanozin.walls[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.walls[i].mPosition.X, (int)Nanozin.walls[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.walls.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.hardWalls.Count; i++)
                                {
                                    Nanozin.hardWalls[i].mPosition.Y += shift;
                                    Nanozin.hardWalls[i].mBoundingBox = new Rectangle((int)Nanozin.hardWalls[i].mPosition.X, (int)Nanozin.hardWalls[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.hardWalls[i].mPosition.X, (int)Nanozin.hardWalls[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.hardWalls.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.crackedWalls.Count; i++)
                                {
                                    Nanozin.crackedWalls[i].mPosition.Y += shift;
                                    Nanozin.crackedWalls[i].mBoundingBox = new Rectangle((int)Nanozin.crackedWalls[i].mPosition.X, (int)Nanozin.crackedWalls[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.crackedWalls[i].mPosition.X, (int)Nanozin.crackedWalls[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.crackedWalls.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.bombWalls.Count; i++)
                                {
                                    Nanozin.bombWalls[i].mPosition.Y += shift;
                                    Nanozin.bombWalls[i].mBoundingBox = new Rectangle((int)Nanozin.bombWalls[i].mPosition.X, (int)Nanozin.bombWalls[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.bombWalls[i].mPosition.X, (int)Nanozin.bombWalls[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.bombWalls.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.icePatches.Count; i++)
                                {
                                    Nanozin.icePatches[i].mPosition.Y += shift;
                                    Nanozin.icePatches[i].mBoundingBox = new Rectangle((int)Nanozin.icePatches[i].mPosition.X, (int)Nanozin.icePatches[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.icePatches[i].mPosition.X, (int)Nanozin.icePatches[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.icePatches.RemoveAt(i);
                                        i--;
                                    }
                                }

                                for (int i = 0; i < Nanozin.wastes.Count; i++)
                                {
                                    Nanozin.wastes[i].mPosition.Y += shift;
                                    Nanozin.wastes[i].mBoundingBox = new Rectangle((int)Nanozin.wastes[i].mPosition.X, (int)Nanozin.wastes[i].mPosition.Y, 64, 64);
                                    if (!new Rectangle((int)Nanozin.wastes[i].mPosition.X, (int)Nanozin.wastes[i].mPosition.Y, 64, 64).Intersects(theLevel))
                                    {
                                        Nanozin.wastes.RemoveAt(i);
                                        i--;
                                    }
                                }
                                break;
                        }
                    }
                }

                if (justPushed != currentPush && justPushed != Convert.ToChar(0x0))
                {
                    //Backspace
                    if (justPushed == Convert.ToChar(0x8))
                    {
                        if (mString.Length > 0)
                            mString = mString.Remove(mString.Length - 1);
                    }
                    else
                    {
                        //Add released key
                        if (mString.Length < 16)
                            mString = mString + justPushed;

                        justPushed = Convert.ToChar(0x0);
                    }
                }

                justPushed = currentPush;
            }

            return done;
        }

        public new void draw(SpriteBatch sb)
        {
            string output = mString;

            if (Nanozin.currentScreenTimer % .8f <= .4f)
                output += "|";

            sb.Draw(mTexture,
                    mPosition,
                    mSourceRectangle,
                    mTint,
                    mRotation,
                    mOrigin,
                    mScale,
                    SpriteEffects.None,
                    mDepth);

            sb.DrawString(Nanozin.fontMenu, 
                          mCaption, 
                          new Vector2(mPosition.X - 200, mPosition.Y - 140),
                          Color.White,
                          0f,
                          Vector2.Zero,
                          1f,
                          SpriteEffects.None,
                          1f);

            sb.DrawString(Nanozin.fontMenu,
                          output,
                          new Vector2(mPosition.X - 185, mPosition.Y - 32),
                          Color.White,
                          0f,
                          Vector2.Zero,
                          1f,
                          SpriteEffects.None,
                          1f);
            sb.DrawString(Nanozin.fontMenu,
                          "(Enter)",
                          new Vector2(mPosition.X - 80, mPosition.Y + 80),
                          Color.White,
                          0f,
                          Vector2.Zero,
                          1f,
                          SpriteEffects.None,
                          1f);
        }
    };
}