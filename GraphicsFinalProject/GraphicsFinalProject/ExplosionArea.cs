using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace NanozinProject
{
    public class ExplosionArea
    {
        public ExplosionArea( Rectangle box)
        {
            mBoundingBox = box;
        }
        ~ExplosionArea() { }

        public Rectangle mBoundingBox;

        public void update()
        {
            //Explode objects in range
            int maxObjects,
                numFound = 0,
                i = 0,
                index;

            //First the 3x3 areas check all around bombWall; 8 spots, then the 4x1 areas check 2 spaces each
            if (mBoundingBox.Width == 192)
                maxObjects = 8;
            else
                maxObjects = 2;

            //Kill player
            if (mBoundingBox.Intersects(Nanozin.theSalvager.mBoundingBox) && !Nanozin.theSalvager.dead)
            {
                Nanozin.theSalvager.dead = true;
                Nanozin.transitioning = true;
                Nanozin.timeTransitionStarted = Nanozin.currentScreenTimer;
                numFound++;

                //Death Particles
                for (int j = 0; j < Nanozin.particlesPerDeath; j++)
                {
                    Functions.addTemplateParticle(Nanozin.deathP, Nanozin.theSalvager.mPosition);
                    float speedX = 0,
                          speedY = 0;
                    int myX,
                        myY;

                    if (maxObjects == 8)
                    {
                        myX = mBoundingBox.X+64;
                        myY = mBoundingBox.Y+64;
                    }
                    else
                    {
                        if (mBoundingBox.Width > mBoundingBox.Height)
                        {
                            myX = mBoundingBox.X + 128;
                            myY = mBoundingBox.Y;
                        }
                        else
                        {
                            myX = mBoundingBox.X;
                            myY = mBoundingBox.Y + 128;
                        }
                    }

                    //Determine direction of death particles
                    if (Nanozin.theSalvager.mPosition.X - 32 > myX)
                    {
                        if (Nanozin.theSalvager.mPosition.Y - 32 > myY)
                        {
                            speedX = (((float)Nanozin.rand.Next() % 100f) + 50f) / 100f;
                            speedY = ((((float)Nanozin.rand.Next() % 120f) - 60f) / 100f) + 1.5f;
                        }
                        else if (Nanozin.theSalvager.mPosition.Y - 32 == myY)
                        {
                            speedX = (((float)Nanozin.rand.Next() % 100f) + 50f) / 100f;
                            speedY = (((float)Nanozin.rand.Next() % 120f) - 60f) / 100f;
                        }
                        else
                        {
                            speedX = (((float)Nanozin.rand.Next() % 100f) + 50f) / 100f;
                            speedY = ((((float)Nanozin.rand.Next() % 120f) - 60f) / 100f) - 1.5f;
                        }
                    }
                    else if (Nanozin.theSalvager.mPosition.X - 32 == myX)
                    {
                        if (Nanozin.theSalvager.mPosition.Y - 32 > myY)
                        {
                            speedY = (((float)Nanozin.rand.Next() % 100f) + 50f) / 100f;
                            speedX = (((float)Nanozin.rand.Next() % 120f) - 60f) / 100f;
                        }
                        else
                        {
                            speedY = (((float)Nanozin.rand.Next() % 100f) + 50f) / -100f;
                            speedX = (((float)Nanozin.rand.Next() % 120f) - 60f) / 100f;
                        }
                    }
                    else
                    {
                        if (Nanozin.theSalvager.mPosition.Y - 32 > myY)
                        {
                            speedY = (((float)Nanozin.rand.Next() % 100f) + 50f) / 100f;
                            speedX = ((((float)Nanozin.rand.Next() % 120f) - 60f) / 100f) - 1.5f;
                        }
                        else if (Nanozin.theSalvager.mPosition.Y - 32 == myY)
                        {
                            speedX = (((float)Nanozin.rand.Next() % 100f) + 50f) / -100f;
                            speedY = (((float)Nanozin.rand.Next() % 120f) - 60f) / 100f;
                        }
                        else
                        {
                            speedY = (((float)Nanozin.rand.Next() % 100f) + 50f) / -100f;
                            speedX = ((((float)Nanozin.rand.Next() % 120f) - 60f) / 100f) - 1.5f;
                        }
                    }

                    Nanozin.particles[Nanozin.particles.Count - 1].mVelocity = new Vector2(speedX, speedY);
                }
            }

            while (i < Nanozin.bombWalls.Count && numFound < maxObjects)
            {
                //checkObjectCollision ignores triggered bombWalls
                index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "bombWalls", i);
                if (index != -1)
                {
                    Nanozin.bombWalls[index].triggeredTime = Nanozin.currentScreenTimer;

                    if (!Nanozin.muted)
                        Nanozin.soundBombCharge.Play();

                    numFound++;
                    //bombWalls are not destroyed instantly, so next index to start at must be the current +1
                    i = index+1;
                }
                else
                {
                    i = Nanozin.bombWalls.Count;
                }
            }
            
            i = 0;
            while (i < Nanozin.ammos.Count && numFound < maxObjects)
            {
                index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "ammos", i);
                if (index != -1)
                {
                    //Particles
                    if (Nanozin.ammos[index].mSourceRectangle.X == 0)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            Functions.addTemplateParticle(Nanozin.burstP, Functions.randInRadius(Nanozin.ammos[index].mPosition, 24));
                        }
                    }
                    
                    Nanozin.ammos.RemoveAt(index);
                    numFound++;
                    i = index;
                }
                else
                {
                    i = Nanozin.ammos.Count;
                }
            }

            i = 0;
            while (i < Nanozin.walls.Count && numFound < maxObjects)
            {
                index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "walls", i);
                if (index != -1)
                {
                    Nanozin.walls.RemoveAt(index);
                    numFound++;
                    i = index;
                }
                else
                {
                    i = Nanozin.walls.Count;
                }
            }

            i = 0;
            while (i < Nanozin.detectors.Count && numFound < maxObjects)
            {
                index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "detectors", i);
                if (index != -1)
                {
                    Nanozin.detectors.RemoveAt(index);
                    numFound++;
                    i = index;
                }
                else
                {
                    i = Nanozin.detectors.Count;
                }
            }

            i = 0;
            while (i < Nanozin.hardWalls.Count && numFound < maxObjects)
            {
                index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "hardWalls", i);
                if (index != -1)
                {
                    Nanozin.hardWalls.RemoveAt(index);
                    numFound++;
                    i = index;
                }
                else
                {
                    i = Nanozin.hardWalls.Count;
                }
            }

            i = 0;
            while (i < Nanozin.crackedWalls.Count && numFound < maxObjects)
            {
                index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "crackedWalls", i);
                if (index != -1)
                {
                    Nanozin.crackedWalls.RemoveAt(index);
                    numFound++;
                    i = index;
                }
                else
                {
                    i = Nanozin.crackedWalls.Count;
                }
            }

            i = 0;
            while (i < Nanozin.nodes.Count && numFound < maxObjects)
            {
                index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "nodes", i);
                if (index != -1)
                {
                    Nanozin.nodes.RemoveAt(index);
                    numFound++;
                    i = index;
                }
                else
                {
                    i = Nanozin.nodes.Count;
                }
            }

            i = 0;
            while (i < Nanozin.receivers.Count && numFound < maxObjects)
            {
                index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "receivers", i);
                if (index != -1)
                {
                    Nanozin.receivers.RemoveAt(index);
                    numFound++;
                    i = index;
                }
                else
                {
                    i = Nanozin.receivers.Count;
                }
            }

            i = 0;
            while (i < Nanozin.togglers.Count && numFound < maxObjects)
            {
                index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "togglers", i);
                if (index != -1)
                {
                    Nanozin.togglers.RemoveAt(index);
                    numFound++;
                    i = index;
                }
                else
                {
                    i = Nanozin.togglers.Count;
                }
            }

            i = 0;
            while (i < Nanozin.pressurePlates.Count && numFound < maxObjects)
            {
                index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "pressurePlates", i);
                if (index != -1)
                {
                    Nanozin.pressurePlates.RemoveAt(index);
                    numFound++;
                    i = index;
                }
                else
                {
                    i = Nanozin.pressurePlates.Count;
                }
            }

            i = 0;
            while (i < Nanozin.plasmaLancers.Count && numFound < maxObjects)
            {
                index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "plasmaLancers", i);
                if (index != -1)
                {
                    Nanozin.plasmaLancers.RemoveAt(index);
                    numFound++;
                    i = index;
                }
                else
                {
                    i = Nanozin.plasmaLancers.Count;
                }
            }

            i = 0;
            while (i < Nanozin.mirrors.Count && numFound < maxObjects)
            {
                index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "mirrors", i);
                if (index != -1)
                {
                    Nanozin.mirrors.RemoveAt(index);
                    numFound++;
                    i = index;
                }
                else
                {
                    i = Nanozin.mirrors.Count;
                }
            }

            i = 0;
            while (i < Nanozin.furnaces.Count && numFound < maxObjects)
            {
                index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "furnaces", i);
                if (index != -1)
                {
                    Nanozin.furnaces.RemoveAt(index);
                    numFound++;
                    i = index;
                }
                else
                {
                    i = Nanozin.furnaces.Count;
                }
            }

            i = 0;
            while (i < Nanozin.fuelCells.Count && numFound < maxObjects)
            {
                index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "fuelCells", i);
                if (index != -1)
                {
                    for (int j = 0; j < 400; j++)
                    {
                        Functions.addTemplateParticle(Nanozin.burstP, Functions.randInRadius(Nanozin.fuelCells[index].mPosition, 24));
                    }
                    Nanozin.fuelCells.RemoveAt(index);
                    numFound++;
                    i = index;

                    if (!Nanozin.muted)
                        Nanozin.soundFuelCellBroken.Play();

                    Nanozin.transitioning = true;
                    Nanozin.timeTransitionStarted = Nanozin.currentScreenTimer;
                }
                else
                {
                    i = Nanozin.fuelCells.Count;
                }
            }
        }
    };
}
