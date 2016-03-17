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
    public class PlasmaLancer : Sprite
    {
        public PlasmaLancer( Vector2 position )
        {
            mTexture = Nanozin.plasmaLancerTexture;
            mPosition = position;
            mOrigin = new Vector2(0, 0);
            mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mSourceRectangle = new Rectangle(0, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mRotation = 0f;
            mScale = 1f;
            mDepth = .5f;
            mTint = Color.White;

            timeLastFired = -10f;
            coolDown = 4f;
            lancerRot = 0f;
            lancerPos = new Vector2(mPosition.X+32, mPosition.Y+32);
            rotateAmount = 0f;
            destroyed = false;
        }
        ~PlasmaLancer() { }

        public float timeLastFired,
                     coolDown,
                     lancerRot,
                     lightWhenPaused,
                     rotateAmount;
        public Vector2 lancerPos;
        public bool destroyed;

        public bool update()
        {
            //Editor rotation akin to a mirror
            if (Nanozin.levelEditing && rotateAmount > 0)
            {
                lancerRot += (float)(Math.PI / 30f);
                rotateAmount -= (float)(Math.PI / 30f);
                if (rotateAmount <= 0)
                    lancerRot += .001f;
                if (lancerRot >= Math.PI * 2)
                    lancerRot = 0;
            }

            if (Nanozin.currentScreenTimer > timeLastFired + coolDown || timeLastFired < 0)
            {
                if (Functions.checkForPowersource(mBoundingBox) != -1)
                {
                    timeLastFired = Nanozin.currentScreenTimer;
                    Nanozin.plasmas.Add(new Plasma(lancerPos, 6, "plasmaLancer", Functions.checkObjectCollision(mBoundingBox, 0, 0, "plasmaLancers", 0)));

                    if (!Nanozin.muted)
                        Nanozin.soundPlasmaFire.Play();

                    Nanozin.plasmas[Nanozin.plasmas.Count - 1].mRotation = lancerRot;
                    Nanozin.plasmas[Nanozin.plasmas.Count - 1].mVelocity = Nanozin.plasmas[Nanozin.plasmas.Count - 1].getVelocityFromRot(lancerRot);
                }
            }

            return destroyed;
        }

        public new void draw(SpriteBatch sb)
        {
            Vector2 drawLocation = mPosition - (Nanozin.cameraPosition - Nanozin.SCREEN_MID);
            Vector2 lancerLocation = lancerPos - (Nanozin.cameraPosition - Nanozin.SCREEN_MID);

            if (new Rectangle((int)drawLocation.X, (int)drawLocation.Y, 64, 64).Intersects(new Rectangle(0, 0, Nanozin.SCREEN_WIDTH, Nanozin.SCREEN_HEIGHT)))
            {
                sb.Draw(mTexture,
                        drawLocation,
                        mSourceRectangle,
                        mTint,
                        mRotation,
                        mOrigin,
                        mScale,
                        SpriteEffects.None,
                        mDepth);
                if (timeLastFired != -10f)
                {
                    if (!Nanozin.paused)
                        lightWhenPaused = (((timeLastFired - Nanozin.currentScreenTimer) + coolDown) / coolDown);

                    sb.Draw(mTexture,
                            drawLocation,
                            new Rectangle(64, 0, 64, 64),
                            mTint * lightWhenPaused,
                            mRotation,
                            mOrigin,
                            mScale,
                            SpriteEffects.None,
                            .51f);
                }
                sb.Draw(mTexture,
                        lancerLocation,
                        new Rectangle(Nanozin.SPRITE_LENGTH * 2, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH),
                        mTint,
                        lancerRot,
                        new Vector2(Nanozin.SPRITE_LENGTH / 2, Nanozin.SPRITE_LENGTH / 2),
                        mScale,
                        SpriteEffects.None,
                        .52f);
            }
        }
    }
}
