using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MH4F
{
    class CharacterSelectList
    {

        private CharacterSelectNode[,] characterSelection;
         
        private Vector2 player1Selection;
        private Vector2 player2Selection;

        private int width;
        private int height;

        public CharacterSelectList()
        {
            characterSelection = new CharacterSelectNode[2, 2];
            characterSelection[0, 0] = new CharacterSelectNode("LongSword");
            width = 2;
            height = 2;
        }

        public void moveCharacterSelection(KeyboardState key, Dictionary<string, Keys> controls)
        {
            if (key.IsKeyDown(controls["right"]))
            {
                player1Selection.X = (player1Selection.X + 1) % width;
            }
            if (key.IsKeyDown(controls["left"]))
            {
                player1Selection.X = (player1Selection.X - 1) % width;
            }
            if (key.IsKeyDown(controls["up"]))
            {
                player1Selection.Y = (player1Selection.Y - 1) % height;
            }
            if (key.IsKeyDown(controls["down"]))
            {
                player1Selection.Y = (player1Selection.Y + 1) % height;
            }
        }

        public String selectCharacter()
        {
            return characterSelection[(int)player1Selection.X, (int)player1Selection.Y].CharacterId;
        }
    }
}
