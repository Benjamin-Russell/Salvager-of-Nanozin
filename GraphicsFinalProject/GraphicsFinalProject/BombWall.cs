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
    public class BombWall : Wall
    {
        public BombWall(Vector2 position) : base(position)
        {
            mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH * 4, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            triggeredTime = -1f;
            triggerTime = .3f;
        }
        ~BombWall() { }

        public float triggeredTime,
                     triggerTime;

        public bool update()
        {
            if (triggeredTime < 0 && Functions.checkForPowersource(mBoundingBox) != -1)
            {
                triggeredTime = Nanozin.currentScreenTimer;

                if (!Nanozin.muted)
                    Nanozin.soundBombCharge.Play();
            }

            if (triggeredTime > 0)
            {
                if (Nanozin.currentScreenTimer > triggeredTime + triggerTime)
                {
                    for (int i = 0; i < 40; i++)
                    {
                        Functions.addTemplateParticle(Nanozin.explosionP, Functions.randInRadius(new Vector2(mPosition.X + 32, mPosition.Y + 32), 96));
                    }

                    for (int i = 0; i < 30; i++)
                    {
                        Functions.addTemplateParticle(Nanozin.explosionP, Functions.randInRadius(new Vector2(mPosition.X + 32, mPosition.Y + 32), 32));
                    }

                    Nanozin.explosionAreas.Add(new ExplosionArea(new Rectangle((int)mPosition.X - 64, (int)mPosition.Y - 64, 192, 192)));
                    Nanozin.explosionAreas.Add(new ExplosionArea(new Rectangle((int)mPosition.X, (int)mPosition.Y - 128, 64, 320)));
                    Nanozin.explosionAreas.Add(new ExplosionArea(new Rectangle((int)mPosition.X - 128, (int)mPosition.Y, 320, 64)));

                    if (!Nanozin.muted)
                        Nanozin.soundBombWall.Play();

                    return true;
                }
                else if (Nanozin.currentScreenTimer < triggeredTime + (triggerTime * .2f))
                    mSourceRectangle.X = 320;
                else if (Nanozin.currentScreenTimer < triggeredTime + (triggerTime * .4f))
                    mSourceRectangle.X = 384;
                else if (Nanozin.currentScreenTimer < triggeredTime + (triggerTime * .6f))
                    mSourceRectangle.Y = 0;
                else if (Nanozin.currentScreenTimer < triggeredTime + (triggerTime * .8f))
                    mSourceRectangle.X = 448;
                else
                    mSourceRectangle.Y = 64;

            }

            return false;
        }
    };
}