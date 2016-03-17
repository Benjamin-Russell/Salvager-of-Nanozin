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
    public class CrackedWall : Wall
    {
        public CrackedWall(Vector2 position)
            : base(position)
        {
            mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH * 2, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            timeTriggered = -1;
        }
        ~CrackedWall() { }

        public float timeTriggered;

        public bool update()
        {
            bool done = false;

            if (timeTriggered != -1)
            {
                for (int i = 0; i < 4; i++)
                {
                    Functions.addTemplateParticle(Nanozin.trailP, Functions.randInRadius(new Vector2(mPosition.X + 32, mPosition.Y + 32), 32));
                }

                if (Nanozin.currentScreenTimer > timeTriggered + .2f)
                {
                    done = true;

                    int index;
                    index = Functions.checkObjectCollision(mBoundingBox, Nanozin.SPRITE_LENGTH, 0, "crackedWalls", 0);
                    if (index != -1 && Nanozin.crackedWalls[index].timeTriggered == -1)
                    {
                        Nanozin.crackedWalls[index].timeTriggered = Nanozin.currentScreenTimer;
                        if (!Nanozin.muted)
                            Nanozin.soundSizzle.Play();
                    }
                    index = Functions.checkObjectCollision(mBoundingBox, Nanozin.SPRITE_LENGTH * -1, 0, "crackedWalls", 0);
                    if (index != -1 && Nanozin.crackedWalls[index].timeTriggered == -1)
                    {
                        Nanozin.crackedWalls[index].timeTriggered = Nanozin.currentScreenTimer;
                        if (!Nanozin.muted)
                            Nanozin.soundSizzle.Play();
                    }
                    index = Functions.checkObjectCollision(mBoundingBox, 0, Nanozin.SPRITE_LENGTH, "crackedWalls", 0);
                    if (index != -1 && Nanozin.crackedWalls[index].timeTriggered == -1)
                    {
                        Nanozin.crackedWalls[index].timeTriggered = Nanozin.currentScreenTimer;
                        if (!Nanozin.muted)
                            Nanozin.soundSizzle.Play();
                    }
                    index = Functions.checkObjectCollision(mBoundingBox, 0, Nanozin.SPRITE_LENGTH * -1, "crackedWalls", 0);
                    if (index != -1 && Nanozin.crackedWalls[index].timeTriggered == -1)
                    {
                        Nanozin.crackedWalls[index].timeTriggered = Nanozin.currentScreenTimer;
                        if (!Nanozin.muted)
                            Nanozin.soundSizzle.Play();
                    }
                }
            }

            return done;
        }

        public new void draw(SpriteBatch sb)
        {
            Vector2 drawLocation = mPosition - (Nanozin.cameraPosition - Nanozin.SCREEN_MID);

            if (new Rectangle((int)drawLocation.X, (int)drawLocation.Y, 64, 64).Intersects(new Rectangle(0, 0, Nanozin.SCREEN_WIDTH, Nanozin.SCREEN_HEIGHT)))
            {
                Color tResult;
                if (timeTriggered != -1)
                    tResult = mTint * (((timeTriggered + .2f) - Nanozin.currentScreenTimer) / .2f);
                else
                    tResult = mTint;

                sb.Draw(Nanozin.wallsTexture,
                        drawLocation,
                        mSourceRectangle,
                        tResult,
                        mRotation,
                        mOrigin,
                        mScale,
                        SpriteEffects.None,
                        mDepth);
            }
        }
    };
}
