using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MH4F
{
    class CharacterSelectList
    {

        private CharacterSelectNode[,] characterSelection;
         
        private Vector2 player1Selection;
        private Vector2 player2Selection;

        Texture2D blankBox;

        private int width;
        private int height;

        KeyboardState prevState;

        public CharacterSelectList(ContentManager content)
        {
            blankBox = content.Load<Texture2D>("HealthBar2");
            characterSelection = new CharacterSelectNode[2, 1];
            characterSelection[0, 0] = new CharacterSelectNode("LongSword", new Vector2(100, 100), content.Load<Texture2D>("portraits/lsportrait"));
            characterSelection[1, 0] = new CharacterSelectNode("LongSword", new Vector2(700, 100), content.Load<Texture2D>("portraits/lsportrait"));
            width = 2;
            height = 1;
        }

        public void moveCharacterSelection(KeyboardState key, Dictionary<string, Keys> controls)
        {
            if (key.IsKeyDown(controls["right"]) && prevState.IsKeyUp(controls["right"]))
            {
                player1Selection.X = (player1Selection.X + 1) % width;
            }
            if (key.IsKeyDown(controls["left"]) && prevState.IsKeyUp(controls["left"]))
            {
                if (player1Selection.X == 0)
                {
                    player1Selection.X = width - 1;
                }
                else
                {
                    player1Selection.X = (player1Selection.X - 1) % width;
                }
                
            }
            if (key.IsKeyDown(controls["up"]) && prevState.IsKeyUp(controls["up"]))
            {
                if (player1Selection.Y == 0)
                {
                    player1Selection.Y = height - 1;
                }
                else
                {
                    player1Selection.Y = (player1Selection.Y - 1) % height;
                }
               
            }
            if (key.IsKeyDown(controls["down"]) && prevState.IsKeyUp(controls["down"]))
            {
                player1Selection.Y = (player1Selection.Y + 1) % height;
            }
            prevState = key;
        }

        public String selectCharacter()
        {
            return characterSelection[(int)player1Selection.X, (int)player1Selection.Y].CharacterId;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(CharacterSelectNode node in characterSelection)
            {
                if (node != null)
                {
                    node.Draw(spriteBatch, blankBox);
                }
                spriteBatch.Draw(blankBox, characterSelection[(int)player1Selection.X, (int)player1Selection.Y].DrawRectangle, new Rectangle(0, 0, 467, 44), Color.White);
            }

        }
    }
}
