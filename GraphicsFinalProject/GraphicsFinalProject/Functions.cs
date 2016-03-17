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
    public class Functions : Microsoft.Xna.Framework.Game
    {
        //Functions
        static public void startPlay()
        {
            int[] theLevel;
            int count = -1;

            string file,
                   pathEnd;

            Nanozin.deathParticles.Clear();
            Nanozin.transitioning = true;
            Nanozin.currentScreenTimer = 0;
            Nanozin.numFuelCells = 0;
            Nanozin.beatLevel = false;
            Nanozin.timeTransitionStarted = -1;

            //Delete all Nanozin.particles
            Nanozin.particles.Clear();

            //Get level data:

            if (Nanozin.playTesting)
            {
                file = Nanozin.levelName + ".txt";
                pathEnd = "GraphicsFinalProjectContent\\mainLevels";

                if (!File.Exists(Path.Combine(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName, pathEnd), file)))
                    pathEnd = "GraphicsFinalProjectContent\\customLevels";
            }
            else
            {
                if (Nanozin.currentLevel <= Nanozin.NUM_LEVELS)
                    file = "level" + Nanozin.currentLevel.ToString() + ".txt";
                else
                    file = "level" + Nanozin.NUM_LEVELS.ToString() + ".txt";

                pathEnd = "GraphicsFinalProjectContent\\mainLevels";
            }

            string path = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName, pathEnd);
            using (StreamReader sr = new StreamReader(Path.Combine(path, file)))
            {

                string line = sr.ReadLine();
                string[] words = line.Split();
                int size;

                Nanozin.levelWidth = Convert.ToInt32(words[1]);
                Nanozin.levelHeight = Convert.ToInt32(words[3]);
                Nanozin.currentBackground = Convert.ToInt32(words[5]);

                theLevel = new int[Nanozin.levelWidth * Nanozin.levelHeight];

                while (count < Nanozin.levelHeight - 1)
                {
                    count++;
                    line = sr.ReadLine();
                    words = line.Split();
                    int j = 0;

                    for (int i = 0; i < Nanozin.levelWidth; i++)
                    {
                        //Skip empty strings
                        if (words[j] == "")
                            j++;
                        theLevel[i + (Nanozin.levelWidth * count)] = Convert.ToInt32(words[j]);
                        j++;
                    }
                }

                //Place objects
                Nanozin.theSalvager = new Salvager();
                Nanozin.theRifle = new Rifle();

                for (int i = 0; i < Nanozin.levelWidth * Nanozin.levelHeight; i++)
                {
                    switch (theLevel[i])
                    {
                        case 1:
                            Nanozin.theSalvager.mPosition = new Vector2(64 * (i % Nanozin.levelWidth) + 32, 64 * (i / Nanozin.levelWidth) + 32);
                            break;
                        case 2:
                            Nanozin.walls.Add(new Wall(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            break;
                        case 3:
                            Nanozin.crackedWalls.Add(new CrackedWall(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            break;
                        case 4:
                            Nanozin.hardWalls.Add(new HardWall(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            break;
                        case 5:
                            Nanozin.nodes.Add(new Node(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            break;
                        case 6:
                            Nanozin.receivers.Add(new Receiver(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            break;
                        case 7:
                            Nanozin.mirrors.Add(new Mirror(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            break;
                        case 8:
                            Nanozin.pressurePlates.Add(new PressurePlate(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            break;
                        case 9:
                            Nanozin.plasmaFilms.Add(new PlasmaFilm(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            break;
                        case 10:
                            Nanozin.ammos.Add(new Ammo(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            break;
                        case 11:
                            Nanozin.icePatches.Add(new Ice(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            break;
                        case 12:
                            Nanozin.bombWalls.Add(new BombWall(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            break;
                        case 13:
                            Nanozin.fuelCells.Add(new FuelCell(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            Nanozin.numFuelCells++;
                            break;
                        case 14:
                            Nanozin.theSalvager.mTexture = Nanozin.salvagerTexture;
                            Nanozin.theRifle.mPosition = new Vector2(64 * (i % Nanozin.levelWidth) + 32, 64 * (i / Nanozin.levelWidth) + 32);
                            break;
                        case 15:
                            Nanozin.plasmaLancers.Add(new PlasmaLancer(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            break;
                        case 16:
                            Nanozin.togglers.Add(new Toggler(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            break;
                        case 17:
                            Nanozin.furnaces.Add(new Furnace(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            break;
                        case 18:
                            Nanozin.detectors.Add(new Detector(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            break;
                        case 19:
                            Nanozin.wastes.Add(new Waste(new Vector2(64 * (i % Nanozin.levelWidth), 64 * (i / Nanozin.levelWidth))));
                            break;
                    }
                }

                //Get player rotation
                line = sr.ReadLine();
                words = line.Split();
                Nanozin.theSalvager.mRotation = (float)Math.PI / 2 * Convert.ToInt32(words[1]);
                Nanozin.theSalvager.targetRotation = Nanozin.theSalvager.mRotation;

                //Get if player has rifle
                line = sr.ReadLine();
                words = line.Split();
                if (Convert.ToInt32(words[1]) == 0)
                    Nanozin.theSalvager.mTexture = Nanozin.salvagerTexture;

                //Get player ammos
                line = sr.ReadLine();
                words = line.Split();
                Nanozin.theSalvager.mAmmo = Convert.ToInt32(words[1]);

                //Get mirror rotations
                line = sr.ReadLine();
                words = line.Split();

                if (words[1] != "X")
                {
                    size = words.Length - 1;

                    //Get rid of empty string
                    if (words[words.Length - 1] == "")
                        size -= 1;

                    for (int i = 0; i < size; i++)
                    {
                        Nanozin.mirrors[i].mirrorRot = (float)Math.PI / 2 * Convert.ToInt32(words[i+1]);
                    }
                }

                //Get lancer rotations
                line = sr.ReadLine();
                words = line.Split();

                if (words[1] != "X")
                {
                    size = words.Length - 1;

                    //Get rid of empty string
                    if (words[words.Length - 1] == "")
                        size -= 1;

                    for (int i = 0; i < size; i++)
                    {
                        Nanozin.plasmaLancers[i].lancerRot = (float)Math.PI / 2 * Convert.ToInt32(words[i+1]);
                    }
                }

                //Get lancer cooldowns
                line = sr.ReadLine();
                words = line.Split();

                if (words[1] != "X")
                {
                    size = words.Length - 1;

                    //Get rid of empty string
                    if (words[words.Length - 1] == "")
                        size -= 1;

                    for (int i = 0; i < size; i++)
                    {
                        Nanozin.plasmaLancers[i].coolDown = (float)Convert.ToDouble(words[i + 1]);
                    }
                }

                //Get toggler beginning states
                line = sr.ReadLine();
                words = line.Split();

                if (words[1] != "X")
                {
                    size = words.Length - 1;

                    //Get rid of empty string
                    if (words[words.Length - 1] == "")
                        size -= 1;

                    for (int i = 0; i < size; i++)
                    {
                        if (Convert.ToInt32(words[i + 1]) == 1)
                            Nanozin.togglers[i].powering = true;
                    }
                }

                //Get toggler pulse rates
                line = sr.ReadLine();
                words = line.Split();

                if (words[1] != "X")
                {
                    size = words.Length - 1;

                    //Get rid of empty string
                    if (words[words.Length - 1] == "")
                        size -= 1;

                    for (int i = 0; i < size; i++)
                    {
                        Nanozin.togglers[i].toggleCD = (float)Convert.ToDouble(words[i + 1]);
                    }
                }

                //Get ammo supplies
                line = sr.ReadLine();
                words = line.Split();

                if (words[1] != "X")
                {
                    size = words.Length - 1;

                    //Get rid of empty string
                    if (words[words.Length - 1] == "")
                        size -= 1;

                    for (int i = 0; i < size; i++)
                    {
                        Nanozin.ammos[i].supply = Convert.ToInt32(words[i+1]);
                    }
                }
            }

            //Set to pixel sizes
            Nanozin.levelWidth *= 64;
            Nanozin.levelHeight *= 64;
            Nanozin.levelCenter = new Vector2(Nanozin.levelWidth / 2, Nanozin.levelHeight / 2);

            //Fade in
            Nanozin.fadeLevel.SetValue(0f);

            //Set wind speed for clouds
            Nanozin.cloudVelocity = new Vector2(0, 0);
            while (Math.Abs(Nanozin.cloudVelocity.X) + Math.Abs(Nanozin.cloudVelocity.Y) < .6f)
                Nanozin.cloudVelocity = new Vector2(((((float)Nanozin.rand.Next() % 80f) - 40f) / 100f), ((((float)Nanozin.rand.Next() % 80f) - 40f) / 100f));

            //Starting cloud particles
            int randNum = (Nanozin.rand.Next() % 6) + 5;
            for (int i = 0; i < randNum; i++)
            {
                addTemplateParticle(Nanozin.cloudP, randInRadius( new Vector2(Nanozin.levelWidth / 2f, Nanozin.levelHeight / 2f), Nanozin.levelWidth / 2));
                
                //Shrink effect:
                Nanozin.particles[Nanozin.particles.Count - 1].mStartScale += 40f;
            }
        }

        static public void endPlay()
        {
            Nanozin.currentScreenTimer = 0;

            //Reset vars
            Nanozin.currentScreenTimer = 0;
            Nanozin.pulseIds = 0;
            Nanozin.plasmaIds = 0;

            if (!Nanozin.beatLevel)
            {
                //Save death particles
                int index = 0;

                while (index < Nanozin.particles.Count && Nanozin.particles[index].mTextureIndex != 3)
                {
                    index++;
                }

                if (index < Nanozin.particles.Count)
                {
                    int j = 0;
                    for (int i = index; i < index + Nanozin.particlesPerDeath; i++)
                    {
                        Nanozin.deathParticles.Add(new Particle());
                        Nanozin.deathParticles[j].replicate(Nanozin.particles[i]);
                        j++;
                    }
                }
            }

            //Delete all objects
            Nanozin.theSalvager = null;
            Nanozin.theRifle = null;
            Nanozin.fuelCells.Clear();
            Nanozin.ammos.Clear();
            Nanozin.plasmas.Clear();
            Nanozin.icePatches.Clear();
            Nanozin.wastes.Clear();
            Nanozin.walls.Clear();
            Nanozin.detectors.Clear();
            Nanozin.hardWalls.Clear();
            Nanozin.crackedWalls.Clear();
            Nanozin.bombWalls.Clear();
            Nanozin.nodes.Clear();
            Nanozin.receivers.Clear();
            Nanozin.togglers.Clear();
            Nanozin.mirrors.Clear();
            Nanozin.plasmaLancers.Clear();
            Nanozin.furnaces.Clear();
            Nanozin.wallExplosions.Clear();
            Nanozin.pressurePlates.Clear();
            Nanozin.plasmaFilms.Clear();
            Nanozin.explosionAreas.Clear();

            //Delete all Nanozin.particles
            Nanozin.particles.Clear();
        }

        static public void saveLevel()
        {
            string file = Nanozin.levelName + ".txt",
                   folder = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName, "GraphicsFinalProjectContent\\customLevels"),
                   currentLine = "";
            int index;
            Rectangle scanPos;

            //Misc Variables
            string playerRotation = ((int)(Nanozin.salvEditRot / (Math.PI / 2f))).ToString(),
                   playerHasRifle = "0",
                   playerStartAmmo = Nanozin.editorLevelAmmo.ToString(),
                   mirrorRotations = "",
                   lancerRotations = "",
                   lancerCoolDowns = "",
                   togglerStates = "",
                   togglerCoolDowns = "",
                   ammoSupplies = "";

            using (StreamWriter sw = new StreamWriter(Path.Combine(folder, file)))
            {
                sw.WriteLine("Width: " + (Nanozin.levelWidth / 64) + " Height: " + (Nanozin.levelHeight / 64) + " Background: " + Nanozin.currentBackground);
                
                //Scan level
                for (int j = 0; j < Nanozin.levelHeight / 64; j++ )
                {
                    for (int i = 0; i < Nanozin.levelWidth / 64; i++)
                    {
                        scanPos = new Rectangle(i * 64, j * 64, 64, 64);
                        
                        if (new Rectangle((int)Nanozin.salvagerEdit.X, (int)Nanozin.salvagerEdit.Y, 64, 64).Intersects(scanPos))
                        {
                            currentLine += "1  ";

                            //Player has rifle?
                            if (Nanozin.theRifle.mBoundingBox.Intersects(scanPos))
                                playerHasRifle = "1";
                            else
                                playerHasRifle = "0";
                        }
                        else if (Nanozin.theRifle.mBoundingBox.Intersects(scanPos))
                        {
                            currentLine += "14 ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "walls", 0) != -1)
                        {
                            currentLine += "2  ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "crackedWalls", 0) != -1)
                        {
                            currentLine += "3  ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "hardWalls", 0) != -1)
                        {
                            currentLine += "4  ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "nodes", 0) != -1)
                        {
                            currentLine += "5  ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "receivers", 0) != -1)
                        {
                            currentLine += "6  ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "mirrors", 0) != -1)
                        {
                            index = Functions.checkObjectCollision(scanPos, 0, 0, "mirrors", 0);
                            currentLine += "7  ";
                            mirrorRotations += ((int)(Nanozin.mirrors[index].mirrorRot / (Math.PI / 2f))).ToString() + " ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "pressurePlates", 0) != -1)
                        {
                            currentLine += "8  ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "plasmaFilms", 0) != -1)
                        {
                            currentLine += "9  ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "ammos", 0) != -1)
                        {
                            index = Functions.checkObjectCollision(scanPos, 0, 0, "ammos", 0);
                            currentLine += "10 ";
                            ammoSupplies += Nanozin.ammos[index].supply.ToString() + " ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "icePatches", 0) != -1)
                        {
                            currentLine += "11 ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "bombWalls", 0) != -1)
                        {
                            currentLine += "12 ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "fuelCells", 0) != -1)
                        {
                            currentLine += "13 ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "plasmaLancers", 0) != -1)
                        {
                            index = Functions.checkObjectCollision(scanPos, 0, 0, "plasmaLancers", 0);
                            currentLine += "15 ";
                            lancerRotations += ((int)(Nanozin.plasmaLancers[index].lancerRot / (Math.PI / 2f))).ToString() + " ";
                            lancerCoolDowns += Nanozin.plasmaLancers[index].coolDown.ToString() + " ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "togglers", 0) != -1)
                        {
                            index = Functions.checkObjectCollision(scanPos, 0, 0, "togglers", 0);
                            currentLine += "16 ";
                            togglerCoolDowns += Nanozin.togglers[index].toggleCD.ToString() + " ";

                            if (Nanozin.togglers[index].powering)
                                togglerStates += "1 ";
                            else
                                togglerStates += "0 ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "furnaces", 0) != -1)
                        {
                            currentLine += "17 ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "detectors", 0) != -1)
                        {
                            currentLine += "18 ";
                        }
                        else if (Functions.checkObjectCollision(scanPos, 0, 0, "wastes", 0) != -1)
                        {
                            currentLine += "19 ";
                        }
                        else
                        {
                            currentLine += "0  ";
                        }

                        //End of line, print it
                        if (i == (Nanozin.levelWidth / 64) - 1)
                        {
                            sw.WriteLine(currentLine);
                            currentLine = "";
                        }
                    }
                }

                //Set empty variables to X
                if (mirrorRotations == "")
                    mirrorRotations = "X";
                if (lancerRotations == "")
                    lancerRotations = "X";
                if (lancerCoolDowns == "")
                    lancerCoolDowns = "X";
                if (togglerStates == "")
                    togglerStates = "X";
                if (togglerCoolDowns == "")
                    togglerCoolDowns = "X";
                if (ammoSupplies == "")
                    ammoSupplies = "X";

                //Print misc variables
                sw.WriteLine("playerRotation: " + playerRotation);
                sw.WriteLine("playerHasRifle: " + playerHasRifle);
                sw.WriteLine("playerStartAmmo: " + playerStartAmmo);
                sw.WriteLine("mirrorRotations: " + mirrorRotations);
                sw.WriteLine("lancerRotations: " + lancerRotations);
                sw.WriteLine("lancerCoolDowns: " + lancerCoolDowns);
                sw.WriteLine("togglerStates: " + togglerStates);
                sw.WriteLine("togglerCoolDowns: " + togglerCoolDowns);
                sw.WriteLine("ammoSupplies: " + ammoSupplies);
            }
        }

        static public void loadLanguage(string lang)
        {
            string file = lang + "Text.txt",
                   folder = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName, "GraphicsFinalProjectContent"),
                   line;
            int i = 0;

            using (StreamReader sr = new StreamReader(Path.Combine(folder, file)))
            {
                line = sr.ReadLine();

                while (line != "EOF")
                {
                    Nanozin.theText[i] = line;

                    ++i;
                    line = sr.ReadLine();
                }
            }
        }

        static public int getSave()
        {
            string file = "saveFile.txt",
                   folder = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName, "GraphicsFinalProjectContent"),
                   line;

            using (StreamReader sr = new StreamReader(Path.Combine(folder, file)))
            {
                line = sr.ReadLine();

                return Convert.ToInt32(line);
            }
        }

        static public void setSave( int level )///////will be old
        {
            string file = "saveFile.txt",
            folder = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName, "GraphicsFinalProjectContent");

             using (StreamWriter sw = new StreamWriter(Path.Combine(folder, file)))
             {
                 sw.WriteLine(level);
             }
        }

        static public void addTemplateParticle(Particle templateP, Vector2 position)
        {
            int index = Nanozin.particles.Count;
            Nanozin.particles.Add(new Particle());
            Nanozin.particles[index].replicate(templateP);
            Nanozin.particles[index].mPosition = position;

            //Particle variation
            if (templateP == Nanozin.dustP)
            {
                int shade = 255;

                Nanozin.particles[index].mCurRotation = Nanozin.rand.Next() % (float)(Math.PI * 2);
                Nanozin.particles[index].mRotation = ((Nanozin.rand.Next() % 2) / 1000f) + .001f;
                if (Nanozin.rand.Next() % 2 == 0)
                    Nanozin.particles[index].mRotation *= -1;

                Nanozin.particles[index].mVelocity.X += ((float)((Nanozin.rand.Next() % 4) - 2f) / 10f);
                Nanozin.particles[index].mStartScale -= ((float)(Nanozin.rand.Next() % 20f) / 10f); //3.5 to 1.5
                Nanozin.particles[index].mEndScale = Nanozin.particles[index].mStartScale;

                shade -= (int)((3.5f - Nanozin.particles[index].mStartScale) * 125);
                Nanozin.particles[index].mStartColor = new Color(shade, shade, shade);
                Nanozin.particles[index].mEndColor = new Color(Nanozin.rand.Next() % shade, Nanozin.rand.Next() % shade, Nanozin.rand.Next() % shade);

                if (Nanozin.particles[index].mPosition.Y < 400)
                    Nanozin.particles[index].mVelocity.Y += .04f + (float)((Nanozin.rand.Next() % 8) / (float)100);
                else
                    Nanozin.particles[index].mVelocity.Y += -.04f + (float)((Nanozin.rand.Next() % 8) / (float)-100);
            }
            else if (templateP == Nanozin.chargeP)
            {
                Nanozin.particles[index].mVelocity.X = ((float)((Nanozin.rand.Next() % 500) + 0f) / 10f);
                Nanozin.particles[index].mVelocity.Y = ((float)((Nanozin.rand.Next() % 500) + 0f) / 10f);
                if (Nanozin.rand.Next() % 2 == 0)
                    Nanozin.particles[index].mVelocity.X *= -1;
                if (Nanozin.rand.Next() % 2 == 0)
                    Nanozin.particles[index].mVelocity.Y *= -1;

                //For knowing the speed to go reverse in
                Nanozin.particles[index].mStartVelocity = Nanozin.particles[index].mVelocity;

                Nanozin.particles[index].mStartScale += ((float)(Nanozin.rand.Next() % 15f) / 100f);
                Nanozin.particles[index].mEndScale = Nanozin.particles[index].mStartScale;

                if (Nanozin.currentScreen == 1)
                {
                    Nanozin.particles[index].mEndColor = new Color(Nanozin.rand.Next() % 256, Nanozin.rand.Next() % 256, Nanozin.rand.Next() % 256);
                }
            }
            else if (templateP == Nanozin.trailP)
            {
                Nanozin.particles[index].mVelocity.X = ((float)((Nanozin.rand.Next() % 20) + 1f) / 10f);
                Nanozin.particles[index].mVelocity.Y = ((float)((Nanozin.rand.Next() % 20) + 1f) / 10f);
                if (Nanozin.rand.Next() % 2 == 0)
                    Nanozin.particles[index].mVelocity.X *= -1;
                if (Nanozin.rand.Next() % 2 == 0)
                    Nanozin.particles[index].mVelocity.Y *= -1;

                Nanozin.particles[index].mStartScale += ((float)((Nanozin.rand.Next() % 15f) - 5f) / 100f);
            }
            else if (templateP == Nanozin.powerP)
            {
                Nanozin.particles[index].mAcceleration.X += ((float)((Nanozin.rand.Next() % 8) - 4f) / 10f);
                if (Nanozin.rand.Next() % 4 == 0)
                {
                    Nanozin.particles[index].mStartColor = new Color(0, 200, 200);
                    int change = (Nanozin.rand.Next() % 100) - 50;
                    Nanozin.particles[index].mStartColor.G += (byte)change;
                    Nanozin.particles[index].mStartColor.B += (byte)change;
                }
                else
                {
                    Nanozin.particles[index].mStartColor.R += (byte)((Nanozin.rand.Next() % 135) - 0);
                }

                Nanozin.particles[index].mEndColor = Nanozin.particles[index].mStartColor;
            }
            else if (templateP == Nanozin.burstP)
            {
                int factor = (Nanozin.rand.Next() % 3) * 2;

                Nanozin.particles[index].mVelocity.X = ((float)((Nanozin.rand.Next() % (100 + (factor * 100))) + 0f) / 10f);
                Nanozin.particles[index].mVelocity.Y = ((float)((Nanozin.rand.Next() % (100 + (factor * 100))) + 0f) / 10f);
                if (Nanozin.rand.Next() % 2 == 0)
                    Nanozin.particles[index].mVelocity.X *= -1;
                if (Nanozin.rand.Next() % 2 == 0)
                    Nanozin.particles[index].mVelocity.Y *= -1;

                Nanozin.particles[index].mStartAlpha -= ((float)(Nanozin.rand.Next() % 7) / 10f);

                Nanozin.particles[index].mStartScale += ((float)(Nanozin.rand.Next() % 60f) / 100f);
                Nanozin.particles[index].mEndScale = (Nanozin.rand.Next() % ((Nanozin.particles[index].mStartScale - .1f) * 10)) / 10;

                if (Nanozin.rand.Next() % 3 == 0)
                    Nanozin.particles[index].mStartColor = Color.Red;
                Nanozin.particles[index].mEndColor = Nanozin.particles[index].mStartColor;
            }
            else if (templateP == Nanozin.deathP)
            {
                Nanozin.particles[index].mStartScale += ((((float)Nanozin.rand.Next() % 35f) - 20f) / 100f);
                Nanozin.particles[index].mEndScale = Nanozin.particles[index].mStartScale;
                Nanozin.particles[index].mCurRotation = Nanozin.rand.Next() % (float)(Math.PI * 2);

                Nanozin.particles[index].mDuration += ((((float)Nanozin.rand.Next() % 4000f) - 2000f) / 1000f);

                while (Nanozin.particles[index].mRotation == 0)
                    Nanozin.particles[index].mRotation = Nanozin.rand.Next() % (((float)(Nanozin.rand.Next() % 80) - 40f) / 100f);
            }
            else if (templateP == Nanozin.fireP)
            {
                Nanozin.particles[index].mVelocity.X += ((((float)Nanozin.rand.Next() % 50f) - 25f) / 100f);
                Nanozin.particles[index].mStartColor.R += (byte)((Nanozin.rand.Next() % 120) - 30);
            }
            else if (templateP == Nanozin.fuelP)
            {
                if (Nanozin.rand.Next() % 4 == 0)
                    Nanozin.particles[index].mStartColor = new Color(0, 255, 255);
                else if (Nanozin.rand.Next() % 3 == 0)
                    Nanozin.particles[index].mStartColor = new Color(255, 0, 0);
                else if (Nanozin.rand.Next() % 4 == 0)
                    Nanozin.particles[index].mStartColor = new Color(255, 255, 255);

                Nanozin.particles[index].mEndColor = Nanozin.particles[index].mStartColor;
            }
            else if (templateP == Nanozin.splashP)
            {
                Nanozin.particles[index].mVelocity.X = ((((float)Nanozin.rand.Next() % 200f) - 100f) / 100f);
                Nanozin.particles[index].mVelocity.Y = ((((float)Nanozin.rand.Next() % 200f) - 100f) / 100f);
                Nanozin.particles[index].mStartScale += (((float)Nanozin.rand.Next() % 20f) / 100f);
                Nanozin.particles[index].mDuration += (((float)Nanozin.rand.Next() % 30f) / 100f);
            }
            else if (templateP == Nanozin.explosionP)
            {
                Nanozin.particles[index].mVelocity.X = ((((float)Nanozin.rand.Next() % 800f) - 400f) / 100f);
                Nanozin.particles[index].mVelocity.Y = ((((float)Nanozin.rand.Next() % 800f) - 400f) / 100f);
                Nanozin.particles[index].mStartScale += (((float)Nanozin.rand.Next() % 30f) / 10f);
                Nanozin.particles[index].mEndScale = Nanozin.particles[index].mStartScale + (((float)Nanozin.rand.Next() % 30f) / 10f);
                Nanozin.particles[index].mDuration += (((float)Nanozin.rand.Next() % 30f) / 10f);
                
                if (Nanozin.rand.Next() % 6 == 0)
                {
                    Nanozin.particles[index].mStartColor = Color.Black;
                    Nanozin.particles[index].mDuration *= 2f;
                    Nanozin.particles[index].mVelocity *= .25f;
                }
                else if (Nanozin.rand.Next() % 6 == 0)
                {
                    Nanozin.particles[index].mStartColor = Color.White;
                    Nanozin.particles[index].mVelocity *= 3f;
                    Nanozin.particles[index].mStartScale /= 3f;
                    Nanozin.particles[index].mEndScale /= 3f;
                    Nanozin.particles[index].mStartAlpha += .2f;
                }
                else if (Nanozin.rand.Next() % 6 == 0)
                {
                    Nanozin.particles[index].mStartColor = Color.White;
                    Nanozin.particles[index].mVelocity /= 3f;
                    Nanozin.particles[index].mStartScale *= 1.5f;
                    Nanozin.particles[index].mEndScale *= 1.5f;
                    Nanozin.particles[index].mStartAlpha += .2f;
                    Nanozin.particles[index].mDepth -= .2f;
                }
                else
                {
                    Nanozin.particles[index].mStartColor.G = (byte)(Nanozin.rand.Next() % 90);
                }
            }
            else if (templateP == Nanozin.cloudP)
            {
                Nanozin.particles[index].mSourceRectangle = new Rectangle((Nanozin.rand.Next() % 3) * 64, (Nanozin.rand.Next() % 3) * 64, 64, 64);
                Nanozin.particles[index].mVelocity = Nanozin.cloudVelocity;
                Nanozin.particles[index].mCurRotation = Nanozin.rand.Next() % (float)(Math.PI * 2f);
                Nanozin.particles[index].mStartScale += (((float)Nanozin.rand.Next() % 100f) / 10f) + 5f;
                Nanozin.particles[index].mEndScale = Nanozin.particles[index].mStartScale;
                Nanozin.particles[index].mStartAlpha = ((((float)Nanozin.rand.Next() % 20f) + 20f) / 100f);
                Nanozin.particles[index].mEndAlpha = Nanozin.particles[index].mStartAlpha;

                Nanozin.particles[index].mStartColor = new Color(Nanozin.rand.Next() % 256, Nanozin.rand.Next() % 256, Nanozin.rand.Next() % 256);
                Nanozin.particles[index].mEndColor = Nanozin.particles[index].mStartColor;

                //Flicker fix
                Nanozin.particles[index].mDepth += (((float)Nanozin.rand.Next() % 500f) / 10000f); //lol
            }
            else if (templateP == Nanozin.oozeP)
            {
                Nanozin.particles[index].mEndScale += (((float)Nanozin.rand.Next() % 15f) / 100f);
                Nanozin.particles[index].mStartColor.R += (byte)((Nanozin.rand.Next() % 80) - 40);
                Nanozin.particles[index].mStartColor.G += (byte)((Nanozin.rand.Next() % 80) - 40);
                Nanozin.particles[index].mStartColor.B += (byte)((Nanozin.rand.Next() % 80) - 40);
                Nanozin.particles[index].mStartColor *= .4f;
                Nanozin.particles[index].mEndColor = Nanozin.particles[index].mStartColor;
                Nanozin.particles[index].mAcceleration.Y -= ((Nanozin.rand.Next() % 70f) / 10000f);
                Nanozin.particles[index].mDuration -= ((Nanozin.rand.Next() % 100f) / 100f);
                Nanozin.particles[index].mVelocity.X = (((Nanozin.rand.Next() % 400f) - 200f) / 1000f);
            }
        }

        //Find a Nanozin.random position given position and radius
        static public Vector2 randInRadius(Vector2 position, int radius)
        {
            Vector2 placement = position;

            int leftorRight = (Nanozin.rand.Next() % 2) - 1;
            int upOrDown = (Nanozin.rand.Next() % 2) - 1;
            if (leftorRight == 0)
                leftorRight = 1;
            if (upOrDown == 0)
                upOrDown = 1;

            placement.X += (Nanozin.rand.Next() % radius) * leftorRight;
            placement.Y += (Nanozin.rand.Next() % radius) * upOrDown;

            return placement;
        }

        //Check for collision with all objects of a given list
        static public int checkObjectCollision(Rectangle bbox, int offsetX, int offsetY, string theList, int startIndex)
        {
            int foundCollision = -1;
            Rectangle newRect = bbox;

            newRect.X += offsetX;
            newRect.Y += offsetY;

            switch (theList)
            {
                case "salvager":
                    if (newRect.Intersects(Nanozin.theSalvager.mBoundingBox))
                        foundCollision = 1;
                    break;
                case "fuelCells":
                    for (int i = startIndex; i < Nanozin.fuelCells.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.fuelCells[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "plasmas":
                    for (int i = startIndex; i < Nanozin.plasmas.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.plasmas[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "bombWalls":
                    for (int i = startIndex; i < Nanozin.bombWalls.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.bombWalls[i].mBoundingBox) && Nanozin.bombWalls[i].triggeredTime < 0)
                            foundCollision = i;
                    }
                    break;
                case "ammos":
                    for (int i = startIndex; i < Nanozin.ammos.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.ammos[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "walls":
                    for (int i = startIndex; i < Nanozin.walls.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.walls[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "hardWalls":
                    for (int i = startIndex; i < Nanozin.hardWalls.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.hardWalls[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "crackedWalls":
                    for (int i = startIndex; i < Nanozin.crackedWalls.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.crackedWalls[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "wallExplosions":
                    for (int i = startIndex; i < Nanozin.wallExplosions.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.wallExplosions[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "nodes":
                    for (int i = startIndex; i < Nanozin.nodes.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.nodes[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "receivers":
                    for (int i = startIndex; i < Nanozin.receivers.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.receivers[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "togglers":
                    for (int i = startIndex; i < Nanozin.togglers.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.togglers[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "detectors":
                    for (int i = startIndex; i < Nanozin.detectors.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.detectors[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "pressurePlates":
                    for (int i = startIndex; i < Nanozin.pressurePlates.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.pressurePlates[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "mirrors":
                    for (int i = startIndex; i < Nanozin.mirrors.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.mirrors[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "plasmaLancers":
                    for (int i = startIndex; i < Nanozin.plasmaLancers.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.plasmaLancers[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "furnaces":
                    for (int i = startIndex; i < Nanozin.furnaces.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.furnaces[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "plasmaFilms":
                    for (int i = startIndex; i < Nanozin.plasmaFilms.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.plasmaFilms[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "icePatches":
                    for (int i = startIndex; i < Nanozin.icePatches.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.icePatches[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
                case "wastes":
                    for (int i = startIndex; i < Nanozin.wastes.Count && foundCollision == -1; i++)
                    {
                        if (newRect.Intersects(Nanozin.wastes[i].mBoundingBox))
                            foundCollision = i;
                    }
                    break;
            }

            return foundCollision;
        }

        //Check for surrounding presence with all objects in a given list
        static int checkObjectSurroundings(Rectangle bbox, int distance, string theList)
        {
            int foundCollision = -1;

            foundCollision = checkObjectCollision(bbox, distance, 0, theList, 0);
            if (foundCollision == -1)
                foundCollision = checkObjectCollision(bbox, -distance, 0, theList, 0);
            if (foundCollision == -1)
                foundCollision = checkObjectCollision(bbox, 0, distance, theList, 0);
            if (foundCollision == -1)
                foundCollision = checkObjectCollision(bbox, 0, -distance, theList, 0);

            return foundCollision;
        }

        //Check surroundings for one instance in a list
        static public bool checkInstanceSurroundings(Rectangle bbox, string theList, int index)
        {
            bool foundCollision = false;

            switch (theList)
            {
                case "walls":
                    if (Nanozin.walls[index].mBoundingBox.Intersects(new Rectangle(bbox.X + (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.walls[index].mBoundingBox.Intersects(new Rectangle(bbox.X - (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.walls[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y + (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height))
                    || Nanozin.walls[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y - (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height)))
                    {
                        foundCollision = true;
                    }
                    break;
                case "hardWalls":
                    if (Nanozin.hardWalls[index].mBoundingBox.Intersects(new Rectangle(bbox.X + (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.hardWalls[index].mBoundingBox.Intersects(new Rectangle(bbox.X - (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.hardWalls[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y + (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height))
                    || Nanozin.hardWalls[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y - (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height)))
                    {
                        foundCollision = true;
                    }
                    break;
                case "crackedWalls":
                    if (Nanozin.crackedWalls[index].mBoundingBox.Intersects(new Rectangle(bbox.X + (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.crackedWalls[index].mBoundingBox.Intersects(new Rectangle(bbox.X - (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.crackedWalls[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y + (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height))
                    || Nanozin.crackedWalls[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y - (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height)))
                    {
                        foundCollision = true;
                    }
                    break;
                case "nodes":
                    if (Nanozin.nodes[index].mBoundingBox.Intersects(new Rectangle(bbox.X + (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.nodes[index].mBoundingBox.Intersects(new Rectangle(bbox.X - (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.nodes[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y + (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height))
                    || Nanozin.nodes[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y - (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height)))
                    {
                        foundCollision = true;
                    }
                    break;
                case "receivers":
                    if (Nanozin.receivers[index].mBoundingBox.Intersects(new Rectangle(bbox.X + (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.receivers[index].mBoundingBox.Intersects(new Rectangle(bbox.X - (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.receivers[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y + (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height))
                    || Nanozin.receivers[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y - (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height)))
                    {
                        foundCollision = true;
                    }
                    break;
                case "detectors":
                    if (Nanozin.detectors[index].mBoundingBox.Intersects(new Rectangle(bbox.X + (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.detectors[index].mBoundingBox.Intersects(new Rectangle(bbox.X - (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.detectors[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y + (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height))
                    || Nanozin.detectors[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y - (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height)))
                    {
                        foundCollision = true;
                    }
                    break;
                case "togglers":
                    if (Nanozin.togglers[index].mBoundingBox.Intersects(new Rectangle(bbox.X + (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.togglers[index].mBoundingBox.Intersects(new Rectangle(bbox.X - (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.togglers[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y + (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height))
                    || Nanozin.togglers[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y - (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height)))
                    {
                        foundCollision = true;
                    }
                    break;
                case "pressurePlates":
                    if (Nanozin.pressurePlates[index].mBoundingBox.Intersects(new Rectangle(bbox.X + (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.pressurePlates[index].mBoundingBox.Intersects(new Rectangle(bbox.X - (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.pressurePlates[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y + (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height))
                    || Nanozin.pressurePlates[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y - (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height)))
                    {
                        foundCollision = true;
                    }
                    break;
                case "mirrors":
                    if (Nanozin.mirrors[index].mBoundingBox.Intersects(new Rectangle(bbox.X + (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.mirrors[index].mBoundingBox.Intersects(new Rectangle(bbox.X - (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.mirrors[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y + (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height))
                    || Nanozin.mirrors[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y - (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height)))
                    {
                        foundCollision = true;
                    }
                    break;
                case "plasmaFilms":
                    if (Nanozin.plasmaFilms[index].mBoundingBox.Intersects(new Rectangle(bbox.X + (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.plasmaFilms[index].mBoundingBox.Intersects(new Rectangle(bbox.X - (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.plasmaFilms[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y + (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height))
                    || Nanozin.plasmaFilms[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y - (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height)))
                    {
                        foundCollision = true;
                    }
                    break;
                case "icePatches":
                    if (Nanozin.icePatches[index].mBoundingBox.Intersects(new Rectangle(bbox.X + (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.icePatches[index].mBoundingBox.Intersects(new Rectangle(bbox.X - (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.icePatches[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y + (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height))
                    || Nanozin.icePatches[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y - (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height)))
                    {
                        foundCollision = true;
                    }
                    break;
                case "furnaces":
                    if (Nanozin.furnaces[index].mBoundingBox.Intersects(new Rectangle(bbox.X + (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.furnaces[index].mBoundingBox.Intersects(new Rectangle(bbox.X - (Nanozin.SPRITE_LENGTH / 2), bbox.Y, bbox.Width, bbox.Height))
                    || Nanozin.furnaces[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y + (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height))
                    || Nanozin.furnaces[index].mBoundingBox.Intersects(new Rectangle(bbox.X, bbox.Y - (Nanozin.SPRITE_LENGTH / 2), bbox.Width, bbox.Height)))
                    {
                        foundCollision = true;
                    }
                    break;
            }

            return foundCollision;
        }

        //Check for powersource in surroundings, returns pulse ID
        static public int checkForPowersource(Rectangle bbox)
        {
            //Checks for every power-transferable object
            for (int i = 0; i < Nanozin. plasmaFilms.Count; i++)
            {
                if (Nanozin.plasmaFilms[i].powered && checkInstanceSurroundings(bbox, "plasmaFilms", i) && Nanozin.plasmaFilms[i].lastPoweredTime < Nanozin.currentScreenTimer - .05f)
                    return Nanozin.plasmaFilms[i].lastPulseId;
            }

            for (int i = 0; i < Nanozin.nodes.Count; i++)
            {
                //Adds a buffer that power sources must have been powering for                                  .05 seconds
                if (Nanozin.nodes[i].powered && Nanozin.nodes[i].lastPoweredTime < Nanozin.currentScreenTimer - .05f && checkInstanceSurroundings(bbox, "nodes", i))
                    return Nanozin.nodes[i].lastPulseId;
            }

            for (int i = 0; i < Nanozin.detectors.Count; i++)
            {
                //Adds a buffer that power sources must have been powering for                                  .05 seconds
                if (Nanozin.detectors[i].powered && Nanozin.detectors[i].lastPoweredTime < Nanozin.currentScreenTimer - .05f && checkInstanceSurroundings(bbox, "detectors", i))
                    return Nanozin.detectors[i].lastPulseId;
            }

            for (int i = 0; i < Nanozin.togglers.Count; i++)
            {
                //Adds a buffer that power sources must have been powering for                                        .05 seconds
                if (Nanozin.togglers[i].powered && Nanozin.togglers[i].lastPoweredTime < Nanozin.currentScreenTimer - .05f && checkInstanceSurroundings(bbox, "togglers", i))
                    return Nanozin.togglers[i].lastPulseId;
            }

            //New power sources checked after transfering power sources
            for (int i = 0; i < Nanozin.pressurePlates.Count; i++)
            {
                if (Nanozin.pressurePlates[i].powering && checkInstanceSurroundings(bbox, "pressurePlates", i))
                {
                    return Nanozin.pulseIds++;
                }
            }

            for (int i = 0; i < Nanozin.receivers.Count; i++)
            {
                //Adds a buffer that power sources must have been powering for                                          .05 seconds
                if (Nanozin.receivers[i].powered && Nanozin.receivers[i].lastPoweredTime < Nanozin.currentScreenTimer - .05f && checkInstanceSurroundings(bbox, "receivers", i))
                {
                    return Nanozin.receivers[i].lastPulseId;
                }
            }

            return -1;
        }
    }
}
