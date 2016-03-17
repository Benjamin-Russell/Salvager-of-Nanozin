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
    public class Waste : Sprite
    {
        public Waste(Vector2 position)
        {
            mTexture = Nanozin.wasteTexture;
            mPosition = position;
            mOrigin = new Vector2(0, 0);
            mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mSourceRectangle = new Rectangle(64 * ((Nanozin.rand.Next() % 7) + 1), 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mRotation = 0f;
            mScale = 1f;
            mDepth = .5f;
            mTint = Color.White;
        }
        ~Waste() { }

        public void update()
        {
            //Animate Radioactive Waste
            if (Nanozin.rand.Next() % 60 == 0)
            {
                int startX = mSourceRectangle.X,
                    newX = 64 * (Nanozin.rand.Next() % 8);

                while (startX == newX)
                {
                    newX = 64 * (Nanozin.rand.Next() % 8);
                }

                mSourceRectangle.X = newX;
            }

            //Spawn waste particles
            if (Nanozin.rand.Next() % 7 == 0)
            {
                Functions.addTemplateParticle(Nanozin.oozeP, Functions.randInRadius(new Vector2(mPosition.X + 32, mPosition.Y + 32), 32));
            }
        }
    };
}