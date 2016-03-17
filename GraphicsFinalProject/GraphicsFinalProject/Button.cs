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
    public class Button : Sprite
    {
        public Button( Vector2 position )
        {
            mTexture = Nanozin.buttonTexture;
            mPosition = position;
            mScale = .75f;
            mOrigin = new Vector2(0, 0);
            mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y + 16, (int)(mTexture.Width * mScale), (int)(mTexture.Height * mScale) - 32);
            mSourceRectangle = new Rectangle(0, 0, 512, 256);
            mRotation = 0f;
            mDepth = .49f;
            mAlpha = 0f;
            mTint = Color.White;
            mId = -1;

            wasClicked = false;
            visible = true;

            switch((int)mPosition.X)
            {
                case 250:
                    switch((int)mPosition.Y)
                    {
                        case 190:
                            mFunction = Nanozin.theText[4];
                            break;
                        case 400:
                            mFunction = Nanozin.theText[8];
                            break;
                    }
                    break;
                case 650:
                    switch((int)mPosition.Y)
                    {
                        case 190:
                            mFunction = Nanozin.theText[11];
                            break;
                        case 400:
                            mFunction = Nanozin.theText[15];
                            break;
                    }
                    break;
                case 450:
                    mFunction = Nanozin.theText[9];
                    break;
                case 32:
                    mScale = .5f;
                    mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y + 16, (int)(mTexture.Width * mScale), (int)(mTexture.Height * mScale) - 32);
                    
                    switch((int)mPosition.Y)
                    {
                        case 170:
                            mFunction = " Change Name";
                            break;
                        case 280:
                            mFunction = "  Level Width";
                            break;
                        case 390:
                            mFunction = "       Shift X";
                            break;
                        case 500:
                            mFunction = "    Load Level";
                            break;
                        case 610:
                            mFunction = "       Menu";
                            break;
                    }
                    break;
                case 300:
                    mScale = .5f;
                    mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y + 16, (int)(mTexture.Width * mScale), (int)(mTexture.Height * mScale) - 32);
                    
                    switch((int)mPosition.Y)
                    {
                        case 170:
                            mFunction = "     Play Test";
                            break;
                        case 280:
                            mFunction = "  Level Height";
                            break;
                        case 390:
                            mFunction = "       Shift Y";
                            break;
                        case 500:
                            mFunction = "    Save Level";
                            break;
                        case 610:
                            mFunction = "      Controls";
                            break;
                    }
                    break;
                case 775:
                    mScale = .5f;
                    mPosition.Y = 15 + (mId * 110);
                    mId = Nanozin.buttonIds++;

                    switch(mId)
                    {
                        case 0:
                            mSelect = Nanozin.editorTexture;
                            selectSource = new Rectangle(128, 0, 64, 64);
                            mName = "Selection Tool";
                            break;
                        case 1:
                            mSelect = Nanozin.salvagerTexture;
                            selectSource = new Rectangle(0, 0, 64, 64);
                            mName = "The Salvager";
                            break;
                        case 2:
                            mSelect = Nanozin.wallsTexture;
                            selectSource = new Rectangle(0, 0, 64, 64);
                            mName = "Wall";
                            break;
                        case 3:
                            mSelect = Nanozin.wallsTexture;
                            selectSource = new Rectangle(128, 64, 64, 64);
                            mName = "Cracked Wall";
                            break;
                        case 4:
                            mSelect = Nanozin.wallsTexture;
                            selectSource = new Rectangle(64, 64, 64, 64);
                            mName = "Hard Wall";
                            break;
                        case 5:
                            mSelect = Nanozin.wallsTexture;
                            selectSource = new Rectangle(64, 0, 64, 64);
                            mName = "Node";
                            break;
                        case 6:
                            mSelect = Nanozin.wallsTexture;
                            selectSource = new Rectangle(192, 64, 64, 64);
                            mName = "Plasma Receiver";
                            break;
                        case 7:
                            mSelect = Nanozin.mirrorTexture;
                            selectSource = new Rectangle(0, 0, 64, 64);
                            mName = "Mirror";
                            break;
                        case 8:
                            mSelect = Nanozin.pressurePlateTexture;
                            selectSource = new Rectangle(0, 0, 64, 64);
                            mName = "Pressure Plate";
                            break;
                        case 9:
                            mSelect = Nanozin.plasmaFilmTexture;
                            selectSource = new Rectangle(128, 0, 64, 64);
                            mName = "Plasma Film";
                            break;
                        case 10:
                            mSelect = Nanozin.ammoTexture;
                            selectSource = new Rectangle(0, 0, 64, 64);
                            mName = "Supply Depot";
                            break;
                        case 11:
                            mSelect = Nanozin.iceTexture;
                            selectSource = new Rectangle(0, 0, 64, 64);
                            mName = "Ice Patch";
                            break;
                        case 12:
                            mSelect = Nanozin.wallsTexture;
                            selectSource = new Rectangle(256, 64, 64, 64);
                            mName = "Bomb Wall";
                            break;
                        case 13:
                            mSelect = Nanozin.fuelCellTexture;
                            selectSource = new Rectangle(0, 0, 64, 64);
                            mName = "Fuel Cell";
                            break;
                        case 14:
                            mSelect = Nanozin.plasmaRifleTexture;
                            selectSource = new Rectangle(0, 0, 64, 64);
                            mName = "Plasma Rifle";
                            break;
                        case 15:
                            mSelect = Nanozin.plasmaLancerTexture;
                            selectSource = new Rectangle(0, 0, 64, 64);
                            mName = "Plasma Lancer";
                            break;
                        case 16:
                            mSelect = Nanozin.wallsTexture;
                            selectSource = new Rectangle(192, 64, 64, 64);
                            mName = "Toggler";
                            break;
                        case 17:
                            mSelect = Nanozin.furnaceTexture;
                            selectSource = new Rectangle(0, 0, 64, 64);
                            mName = "Furnace";
                            break;
                        case 18:
                            mSelect = Nanozin.detectorTexture;
                            selectSource = new Rectangle(0, 0, 64, 64);
                            mName = "Plasma Detector";
                            break;
                        case 19:
                            mSelect = Nanozin.wasteTexture;
                            selectSource = new Rectangle(0, 0, 64, 64);
                            mName = "Radioactive Waste";
                            break;
                    }
                    break;
            }
        }
        ~Button() { }

        public float mAlpha;
        string mFunction,
               mName;
        bool wasClicked,
             visible;
        int mId;
        Texture2D mSelect;
        Rectangle selectSource;

        public void update()
        {
            Vector2 mousePos = new Vector2(Nanozin.theMouse.X, Nanozin.theMouse.Y);
            mAlpha = Nanozin.buttonAlpha;
            visible = true;

            if (mScale == .5f && mPosition.X > Nanozin.SCREEN_MID.X)
            {
                mPosition.Y = 15 + (mId * 110) - Nanozin.buttonScrollY;
                mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y + 16, (int)(mTexture.Width * mScale), (int)(mTexture.Height * mScale) - 32);
            }

            if (mScale == .75f)
            {
                switch ((int)mPosition.X)
                {
                    case 250:
                        switch ((int)mPosition.Y)
                        {
                            case 190:
                                if (Nanozin.currentScreen == 1)
                                {
                                    if (Nanozin.currentMenu == 0)
                                    {
                                        mFunction = Nanozin.theText[4];
                                    }
                                    else if (Nanozin.currentMenu == 2)
                                    {
                                        mFunction = Nanozin.theText[5];
                                    }
                                    else if (Nanozin.currentMenu == 4)
                                    {
                                        mFunction = Nanozin.theText[6];
                                    }
                                    else
                                    {
                                        visible = false;
                                    }
                                }
                                else if (Nanozin.currentScreen == 2)
                                {
                                    if (Nanozin.paused && Nanozin.currentMenu == 0 && !Nanozin.levelEditing)
                                    {
                                        mFunction = Nanozin.theText[7];
                                    }
                                    else
                                    {
                                        visible = false;
                                    }
                                }
                                break;
                            case 400:
                                if (Nanozin.currentScreen == 1)
                                {
                                    if (Nanozin.currentMenu == 0)
                                    {
                                        mFunction = Nanozin.theText[8];
                                    }
                                    else if (Nanozin.currentMenu == 2)
                                    {
                                        mFunction = Nanozin.theText[9];
                                    }
                                    else if (Nanozin.currentMenu == 4)
                                    {
                                        mFunction = Nanozin.theText[9];
                                    }
                                    else
                                    {
                                        visible = false;
                                    }
                                }
                                else if (Nanozin.currentScreen == 2)
                                {
                                    if (Nanozin.paused && Nanozin.currentMenu == 0 && !Nanozin.levelEditing)
                                    {
                                        if (Nanozin.playTesting)
                                            mFunction = Nanozin.theText[27];
                                        else
                                            mFunction = Nanozin.theText[10];
                                    }
                                    else
                                    {
                                        visible = false;
                                    }
                                }
                                break;
                        }
                        break;
                    case 650:
                        switch ((int)mPosition.Y)
                        {
                            case 190:
                                if (Nanozin.currentScreen == 1)
                                {
                                    if (Nanozin.currentMenu == 0)
                                    {
                                        mFunction = Nanozin.theText[11];
                                    }
                                    else if (Nanozin.currentMenu == 2)
                                    {
                                        mFunction = Nanozin.theText[12];
                                    }
                                    else if (Nanozin.currentMenu == 4)
                                    {
                                        mFunction = Nanozin.theText[13];
                                    }
                                    else
                                    {
                                        visible = false;
                                    }
                                }
                                else if (Nanozin.currentScreen == 2)
                                {
                                    if (Nanozin.paused && Nanozin.currentMenu == 0 && !Nanozin.levelEditing)
                                    {
                                        mFunction = Nanozin.theText[14];
                                    }
                                    else
                                    {
                                        visible = false;
                                    }
                                }
                                break;
                            case 400:
                                if (Nanozin.currentScreen == 1)
                                {
                                    if (Nanozin.currentMenu == 0)
                                    {
                                        mFunction = Nanozin.theText[15];
                                    }
                                    else if (Nanozin.currentMenu == 2)
                                    {
                                        mFunction = Nanozin.theText[16];
                                    }
                                    else if (Nanozin.currentMenu == 4)
                                    {
                                        mFunction = Nanozin.theText[27];
                                    }
                                    else
                                    {
                                        visible = false;
                                    }
                                }
                                else if (Nanozin.currentScreen == 2)
                                {
                                    if (Nanozin.paused && Nanozin.currentMenu == 0 && !Nanozin.levelEditing)
                                    {
                                        mFunction = Nanozin.theText[16];
                                    }
                                    else
                                    {
                                        visible = false;
                                    }
                                }
                                break;
                        }
                        break;
                    case 450:
                        if (Nanozin.currentScreen == 1)
                        {
                            if (!(Nanozin.currentMenu == 1 || Nanozin.currentMenu == 3))
                            {
                                visible = false;
                            }
                            else
                            {
                                mFunction = Nanozin.theText[9];
                            }
                        }
                        else if (Nanozin.currentScreen == 2)
                        {
                            if (Nanozin.paused && Nanozin.currentMenu == 3)
                            {
                                mFunction = Nanozin.theText[9];
                            }
                            else
                            {
                                visible = false;
                            }
                        }
                        break;
                }
            }

            if (mScale == .5f && Nanozin.playTesting)
            {
                visible = false;
            }
            
            //If mouse hovering
            if (mBoundingBox.Intersects(new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1)))
            {
                //Clicked or not
                if (Nanozin.theMouse.LeftButton == ButtonState.Pressed)
                {
                    mAlpha -= .2f;
                    wasClicked = true;
                }
                else
                {
                    mAlpha += .4f;

                    if (wasClicked)
                    {
                        wasClicked = false;

                        //If scroll button
                        if (mScale == .5f && Nanozin.paused && !Nanozin.playTesting)
                        {
                            {
                                if (mPosition.X > Nanozin.SCREEN_MID.X)
                                {
                                    Nanozin.editorSelection = mId;
                                    Nanozin.paused = false;
                                    Nanozin.currentMenu = 0;
                                    Nanozin.currentScreenTimer = Nanozin.timePaused;
                                    Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                }
                                else
                                {
                                    switch ((int)mPosition.X)
                                    {
                                        case 32:
                                            switch ((int)mPosition.Y)
                                            {
                                                case 170:
                                                    //Change level name
                                                    Nanozin.paused = false;
                                                    Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                    Nanozin.theTextInput = new TextInput("getFileName", "Name your level:", Nanozin.levelName, 0);
                                                    break;
                                                case 280:
                                                    //Change level width
                                                    Nanozin.paused = false;
                                                    Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                    Nanozin.theTextInput = new TextInput("setLevelWidth", "Set Level Width:", (Nanozin.levelWidth / 64).ToString(), 0);
                                                    break;
                                                case 390:
                                                    //Shift X
                                                    Nanozin.paused = false;
                                                    Nanozin.currentMenu = 0;
                                                    Nanozin.currentScreenTimer = Nanozin.timePaused;
                                                    Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                    Nanozin.theTextInput = new TextInput("shiftX", "Shift X Axis:", "", 0);
                                                    break;
                                                case 500:
                                                    //Load Level
                                                    Nanozin.paused = false;
                                                    Nanozin.currentMenu = 0;
                                                    Nanozin.currentScreenTimer = 0f;
                                                    Nanozin.menuLastChanged = 0f;

                                                    Nanozin.theTextInput = new TextInput("loadLevel", "Load Level:", "", 0);
                                                    break;
                                                case 610:
                                                    //Back to Menu
                                                    Functions.endPlay();
                                                    Nanozin.levelEditing = false;
                                                    Nanozin.playTesting = false;
                                                    Nanozin.particles.Clear();
                                                    Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                    Nanozin.paused = false;
                                                    Nanozin.currentScreenTimer = 0;
                                                    Nanozin.currentScreen = 1;

                                                    Nanozin.levelWidth = 1280;
                                                    Nanozin.levelHeight = 768;
                                                    Nanozin.levelCenter = new Vector2(Nanozin.levelWidth / 2, Nanozin.levelHeight / 2);

                                                    Nanozin.cameraPosition = new Vector2(Nanozin.SCREEN_WIDTH / 2, Nanozin.SCREEN_HEIGHT / 2);

                                                    //Intro particles
                                                    for (int i = 0; i < 110; i++)
                                                        Functions.addTemplateParticle(Nanozin.dustP, new Vector2((float)((Nanozin.rand.Next() % Nanozin.SCREEN_WIDTH)), Nanozin.SCREEN_HEIGHT + 20 + (Nanozin.rand.Next() % 90)));
                                                    for (int i = 0; i < 110; i++)
                                                        Functions.addTemplateParticle(Nanozin.dustP, new Vector2((float)((Nanozin.rand.Next() % Nanozin.SCREEN_WIDTH)), -20 - (Nanozin.rand.Next() % 90)));

                                                    Nanozin.buttons.Clear();
                                                    break;
                                            }
                                            break;
                                        case 300:
                                            switch ((int)mPosition.Y)
                                            {
                                                case 170:
                                                    if (Nanozin.salvagerEdit != new Vector2(-64, -64))
                                                    {
                                                        //Play Test
                                                        Nanozin.paused = false;
                                                        Nanozin.menuLastChanged = 0f;

                                                        if (Nanozin.levelName == "?")
                                                        {
                                                            Nanozin.theTextInput = new TextInput("getFileName", "Name the Level:", "", 0);
                                                        }
                                                        else
                                                        {
                                                            Nanozin.particles.Clear();
                                                            Nanozin.playTesting = true;
                                                            Nanozin.levelEditing = false;
                                                            Functions.saveLevel();
                                                            Functions.endPlay();
                                                            Functions.startPlay();
                                                        }
                                                    }
                                                    break;
                                                case 280:
                                                    //Change level heights
                                                    Nanozin.paused = false;
                                                    Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                    Nanozin.theTextInput = new TextInput("setLevelHeight", "Set Level Height:", (Nanozin.levelHeight / 64).ToString(), 0);
                                                    break;
                                                case 390:
                                                    //Shift Y
                                                    Nanozin.paused = false;
                                                    Nanozin.currentMenu = 0;
                                                    Nanozin.currentScreenTimer = Nanozin.timePaused;
                                                    Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                    Nanozin.theTextInput = new TextInput("shiftY", "Shift Y Axis:", "", 0);
                                                    break;
                                                case 500:
                                                    //Save the level
                                                    if (Nanozin.levelName == "?")
                                                    {
                                                        Nanozin.paused = false;
                                                        Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                        Nanozin.theTextInput = new TextInput("getFileName", "Name Level to Save:", "", 0);
                                                    }
                                                    //If naming file, it's saved in textInput. Else:
                                                    else
                                                    {
                                                        Functions.saveLevel();
                                                    }
                                                    break;
                                                case 610:
                                                    mFunction = "       Menu";
                                                    break;
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        //If menu button
                        else if (mScale == .75f)
                        {
                            switch ((int)mPosition.X)
                            {
                                case 250:
                                    switch ((int)mPosition.Y)
                                    {
                                        case 190:
                                            if (Nanozin.currentMenu == 0)
                                            {
                                                if (Nanozin.currentScreen == 1)
                                                {
                                                    if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow)
                                                    {
                                                        //Go to play
                                                        Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                        Nanozin.currentMenu = 4;
                                                    }
                                                }
                                                else if (Nanozin.currentScreen == 2)
                                                {
                                                    if (Nanozin.paused && !Nanozin.levelEditing)
                                                    {
                                                        //Unpause
                                                        Nanozin.paused = false;
                                                        Nanozin.currentMenu = 0;
                                                        Nanozin.currentScreenTimer = Nanozin.timePaused;
                                                        Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                    }
                                                }
                                            }
                                            else if (Nanozin.currentMenu == 2)
                                            {
                                                if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow)
                                                {
                                                    //Fullscreen
                                                    Nanozin.menuLastChanged = Nanozin.currentScreenTimer;

                                                    if (Nanozin.graphics.IsFullScreen == false)
                                                        Nanozin.graphics.IsFullScreen = true;
                                                    else
                                                        Nanozin.graphics.IsFullScreen = false;

                                                    Nanozin.graphics.ApplyChanges();
                                                }
                                            }
                                            else if (Nanozin.currentMenu == 4)
                                            {
                                                if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow)
                                                {
                                                    //Play new game
                                                    Nanozin.currentLevel = 1;
                                                    Nanozin.currentScreen = 3;
                                                    Nanozin.loreAlpha = 0f;
                                                    Nanozin.currentMenu = 0;
                                                    Nanozin.currentScreenTimer = 0;
                                                    Nanozin.menuLastChanged = 0;
                                                    Nanozin.particles.Clear();
                                                }
                                            }
                                            break;
                                        case 400:
                                            if (Nanozin.currentMenu == 0)
                                            {
                                                if (Nanozin.currentScreen == 1)
                                                {
                                                    //Credits
                                                    if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow)
                                                    {
                                                        Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                        Nanozin.currentMenu = 1;
                                                    }
                                                }
                                                else if (Nanozin.currentScreen == 2)
                                                {
                                                    if (Nanozin.paused && !Nanozin.levelEditing && Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow)
                                                    {
                                                        //Back to Menu
                                                        if (!Nanozin.playTesting)
                                                        {
                                                            Functions.endPlay();
                                                            Nanozin.levelEditing = false;
                                                            Nanozin.particles.Clear();
                                                            Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                            Nanozin.paused = false;
                                                            Nanozin.currentScreenTimer = 0;
                                                            Nanozin.currentScreen = 1;

                                                            Nanozin.levelWidth = 1280;
                                                            Nanozin.levelHeight = 768;
                                                            Nanozin.levelCenter = new Vector2(Nanozin.levelWidth / 2, Nanozin.levelHeight / 2);

                                                            Nanozin.cameraPosition = new Vector2(Nanozin.SCREEN_WIDTH / 2, Nanozin.SCREEN_HEIGHT / 2);

                                                            //Intro particles
                                                            for (int i = 0; i < 110; i++)
                                                                Functions.addTemplateParticle(Nanozin.dustP, new Vector2((float)((Nanozin.rand.Next() % Nanozin.SCREEN_WIDTH)), Nanozin.SCREEN_HEIGHT + 20 + (Nanozin.rand.Next() % 90)));
                                                            for (int i = 0; i < 110; i++)
                                                                Functions.addTemplateParticle(Nanozin.dustP, new Vector2((float)((Nanozin.rand.Next() % Nanozin.SCREEN_WIDTH)), -20 - (Nanozin.rand.Next() % 90)));
                                                        }
                                                        else
                                                        {
                                                            //Back to editor
                                                            Functions.endPlay();
                                                            Functions.startPlay();

                                                            Nanozin.paused = false;
                                                            Nanozin.currentMenu = 0;
                                                            Nanozin.currentScreenTimer = 0f;
                                                            Nanozin.menuLastChanged = 0f;

                                                            Nanozin.cameraPosition = Nanozin.salvagerEdit;

                                                            Nanozin.playTesting = false;
                                                            Nanozin.levelEditing = true;

                                                            //Some object manipulation
                                                            if (Nanozin.theSalvager.mTexture == Nanozin.salvagerRifleTexture)
                                                                Nanozin.theRifle.mPosition = new Vector2(Nanozin.theSalvager.mPosition.X, Nanozin.theSalvager.mPosition.Y);
                                                            Nanozin.theSalvager = null;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (Nanozin.currentMenu == 2)
                                            {
                                                //Back
                                                if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow)
                                                {
                                                    Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                    Nanozin.currentMenu = 0;
                                                }
                                            }
                                            else if (Nanozin.currentMenu == 4)
                                            {
                                                //Back
                                                if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow)
                                                {
                                                    Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                    Nanozin.currentMenu = 0;
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case 650:
                                    switch ((int)mPosition.Y)
                                    {
                                        case 190:
                                            if (Nanozin.currentMenu == 0)
                                            {
                                                if (Nanozin.currentScreen == 1)
                                                {
                                                    //Go to Options
                                                    if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow)
                                                    {
                                                        Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                        Nanozin.currentMenu = 2;
                                                    }
                                                }
                                                else if (Nanozin.currentScreen == 2)
                                                {
                                                    if (Nanozin.paused)
                                                    {
                                                        //Restart Level
                                                        if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow && !Nanozin.levelEditing)
                                                        {
                                                            Nanozin.paused = false;
                                                            Functions.endPlay();
                                                            Functions.startPlay();
                                                            Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (Nanozin.currentMenu == 2)
                                            {
                                                //Change language
                                                if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow)
                                                {
                                                    Nanozin.menuLastChanged = Nanozin.currentScreenTimer;

                                                    if (Nanozin.currentLanguage == "English")
                                                        Nanozin.currentLanguage = "Spanish";
                                                    else
                                                        Nanozin.currentLanguage = "English";

                                                    Functions.loadLanguage(Nanozin.currentLanguage);
                                                }
                                            }
                                            else if (Nanozin.currentMenu == 4)
                                            {
                                                //Continue
                                                if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow)
                                                {
                                                    Nanozin.currentLevel = Functions.getSave();
                                                    Nanozin.currentScreen = 3;
                                                    Nanozin.currentMenu = 0;
                                                    Nanozin.currentScreenTimer = 0;
                                                    Nanozin.menuLastChanged = 0;
                                                    Nanozin.particles.Clear();
                                                }
                                            }
                                            break;
                                        case 400:
                                            if (Nanozin.currentMenu == 0)
                                            {
                                                if (Nanozin.currentScreen == 1)
                                                {
                                                    if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow)
                                                    {
                                                        //Exit
                                                        Nanozin.exitingGame = true;
                                                    }
                                                }
                                                else if (Nanozin.currentScreen == 2)
                                                {
                                                    if (Nanozin.paused)
                                                    {
                                                        if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow && !Nanozin.levelEditing)
                                                        {
                                                            //Go to controls
                                                            Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                            Nanozin.currentMenu = 3;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (Nanozin.currentMenu == 2)
                                            {
                                                if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow)
                                                {
                                                    //Go to controls
                                                    Nanozin.menuLastChanged = Nanozin.currentScreenTimer;
                                                    Nanozin.currentMenu = 3;
                                                }
                                            }
                                            else if (Nanozin.currentMenu == 4)
                                            {
                                                //Go to level editor
                                                if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow)
                                                {
                                                    Nanozin.currentScreen = 2;
                                                    Nanozin.currentMenu = 0;
                                                    Nanozin.editorLevelAmmo = 3;
                                                    Nanozin.levelEditing = true;
                                                    Nanozin.menuLastChanged = 0f;
                                                    Nanozin.transitioning = true;
                                                    Nanozin.currentScreenTimer = 0;
                                                    Nanozin.currentLevel = -1;
                                                    Nanozin.numFuelCells = 0;
                                                    Nanozin.beatLevel = false;
                                                    Nanozin.timeTransitionStarted = -1;
                                                    Nanozin.levelName = "?";
                                                    Nanozin.salvagerEdit = new Vector2(-64, -64);
                                                    Nanozin.theRifle = new Rifle();

                                                    //Scroll buttons
                                                    Nanozin.buttonIds = 0;
                                                    for (int i = 0; i < 20; i++)
                                                        Nanozin.buttons.Add(new Button(new Vector2(775, 0)));

                                                    //Other buttons
                                                    Nanozin.buttons.Add(new Button(new Vector2(32, 170)));
                                                    Nanozin.buttons.Add(new Button(new Vector2(32, 280)));
                                                    Nanozin.buttons.Add(new Button(new Vector2(32, 390)));
                                                    Nanozin.buttons.Add(new Button(new Vector2(32, 500)));
                                                    Nanozin.buttons.Add(new Button(new Vector2(32, 610)));

                                                    Nanozin.buttons.Add(new Button(new Vector2(300, 170)));
                                                    Nanozin.buttons.Add(new Button(new Vector2(300, 280)));
                                                    Nanozin.buttons.Add(new Button(new Vector2(300, 390)));
                                                    Nanozin.buttons.Add(new Button(new Vector2(300, 500)));
                                                    Nanozin.buttons.Add(new Button(new Vector2(300, 610)));

                                                    Nanozin.levelWidth = 1280;
                                                    Nanozin.levelHeight = 768;
                                                    Nanozin.levelCenter = new Vector2(Nanozin.levelWidth / 2, Nanozin.levelHeight / 2);
                                                    Nanozin.cameraPosition = new Vector2(Nanozin.SCREEN_WIDTH / 2, Nanozin.SCREEN_HEIGHT / 2);

                                                    //Delete all Nanozin.particles
                                                    Nanozin.particles.Clear();

                                                    //Fade in
                                                    Nanozin.fadeLevel.SetValue(0f);

                                                    //Set wind speed for clouds
                                                    Nanozin.cloudVelocity = new Vector2(0, 0);
                                                    while (Math.Abs(Nanozin.cloudVelocity.X) + Math.Abs(Nanozin.cloudVelocity.Y) < .6f)
                                                        Nanozin.cloudVelocity = new Vector2(((((float)Nanozin.rand.Next() % 80f) - 40f) / 100f), ((((float)Nanozin.rand.Next() % 80f) - 40f) / 100f));

                                                    //Starting cloud particles
                                                    int randNum = (Nanozin.rand.Next() % 6) + 2;
                                                    for (int i = 0; i < randNum; i++)
                                                    {
                                                        Functions.addTemplateParticle(Nanozin.cloudP, Functions.randInRadius(new Vector2(Nanozin.levelWidth / 2f, Nanozin.levelHeight / 2f), Nanozin.levelWidth / 2));
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case 450:
                                    if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow)
                                    {
                                        Nanozin.menuLastChanged = Nanozin.currentScreenTimer;

                                        if (Nanozin.currentScreen == 1)
                                        {
                                            if (Nanozin.currentMenu == 1)
                                            {
                                                Nanozin.currentMenu = 0;
                                            }
                                            else if (Nanozin.currentMenu == 3)
                                            {
                                                Nanozin.currentMenu = 2;
                                            }
                                        }
                                        else
                                        {
                                            Nanozin.currentMenu = 0;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                wasClicked = false;

                if (Nanozin.editorSelection == mId)
                    mAlpha += .4f;
            }

            if (Nanozin.currentScreen == 3)
            {
                visible = false;
            }
        }

        public new void draw(SpriteBatch sb)
        {
            Vector2 drawLocation = mPosition;

            if (visible)
            {
                sb.Draw(mTexture,
                        drawLocation,
                        mSourceRectangle,
                        mTint * mAlpha,
                        mRotation,
                        mOrigin,
                        mScale,
                        SpriteEffects.None,
                        mDepth);

                //Menu button
                if (mScale == .75f)
                {
                    sb.DrawString(Nanozin.fontMenu,
                                  mFunction,
                                  new Vector2(mPosition.X + 64, mPosition.Y + 58),
                                  Color.White * (mAlpha + .3f));
                }
                //Scroll button
                else if (mScale == .5f && mPosition.X > Nanozin.SCREEN_MID.X)
                {
                    float scale = 1f,
                          targetX = mPosition.X + 96,
                          targetY = mPosition.Y + 32;
                    Color theTint = Color.White;

                    switch(mId)
                    {
                        case 0:
                            theTint = Color.Lime;
                            break;
                        case 1:
                            scale = .8f;
                            targetX += 12;
                            break;
                    }

                    sb.Draw(mSelect,
                            new Vector2(targetX, targetY),
                            selectSource,
                            theTint,
                            0f,
                            new Vector2(0, 0),
                            scale,
                            SpriteEffects.None,
                            .5f);

                    sb.DrawString(Nanozin.fontButtonName,
                                  mName,
                                  new Vector2(mPosition.X + 256, mPosition.Y + 48),
                                  Color.White);

                    if (mId == 7)
                    {
                        sb.Draw(mSelect,
                                new Vector2(targetX, targetY),
                                new Rectangle(128, 0, 64, 64),
                                Color.White,
                                0f,
                                new Vector2(0, 0),
                                scale,
                                SpriteEffects.None,
                                .51f);
                    }
                    else if (mId == 15)
                    {
                        sb.Draw(Nanozin.plasmaLancerTexture,
                                new Vector2(targetX, targetY),
                                new Rectangle(128, 0, 64, 64),
                                Color.White,
                                0f,
                                new Vector2(0, 0),
                                scale,
                                SpriteEffects.None,
                                .51f);
                    }
                    else if (mId == 16)
                    {
                        sb.Draw(Nanozin.wallsTexture,
                                new Vector2(targetX, targetY),
                                new Rectangle(256, 0, 64, 64),
                                Color.White,
                                0f,
                                new Vector2(0, 0),
                                scale,
                                SpriteEffects.None,
                                .51f);
                    }
                }
                //Editor button
                else if (mScale == .5f)
                {
                    sb.DrawString(Nanozin.fontEditor,
                                  mFunction,
                                  new Vector2(mPosition.X + 32, mPosition.Y + 48),
                                  Color.White * (mAlpha + .3f));
                }
            }
        }
    };
}