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
    public class Floater
    {
        public Floater( Vector2 position, string text, SpriteFont font, Color color, float distance, float speed)
        {
            mPosition = position;
            mText = text;
            mFont = font;
            mColor = color;
            mDistance = distance;
            startDistance = distance;
            mSpeed = speed;
        }
        ~Floater() { }

        Vector2 mPosition;
        string mText;
        SpriteFont mFont;
        Color mColor;
        float mDistance,
              startDistance,
              mSpeed;

        public void draw( SpriteBatch sb)
        {
            Vector2 drawLocation = mPosition - (Nanozin.cameraPosition - Nanozin.SCREEN_MID);

            sb.DrawString(mFont, mText, drawLocation, mColor * (mDistance / startDistance));
        }

        public bool update()
        {
            bool done = false;

            mDistance -= mSpeed;
            mPosition.Y -= mSpeed;

            if (mDistance <= 0)
                done = true;

            return done;
        }

    };
}
