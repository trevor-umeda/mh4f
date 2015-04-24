using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace MH4F
{
    // Has the input of a special move
    //
    class MoveInput
    {
        List<String> inputCommand;

        // Should match the name of the sprite animation.
        //
        String name;

        int currentInputCommandIndex;

        public List<String> InputCommand
        {
            get { return inputCommand; }
            set { inputCommand = value; }
        }

        public int CurrentInputCommandIndex
        {
            get { return currentInputCommandIndex; }
            set { currentInputCommandIndex = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public MoveInput(String name, List<String> inputCommand)
        {
            this.name = name;
            this.inputCommand = inputCommand;
            currentInputCommandIndex = 0;
        }

        public void resetCurrentInputCommandIndex()
        {
            CurrentInputCommandIndex = 0;
        }

        public void moveCurrentInputCommandIndex()
        {
            CurrentInputCommandIndex++;
        }
        public static Boolean checkStringInputToKeyInput(String input, KeyboardState keyboardState)
        {
            if (input == "A" && keyboardState.IsKeyDown(Keys.A))
            {
                return true;
            }
            if (input == "6" && keyboardState.IsKeyDown(Keys.Right))
            {
                return true;
            }
            if (input == "3" && keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyDown(Keys.Down))
            {
                return true;
            }
            if (input == "2" && keyboardState.IsKeyDown(Keys.Down))
            {
                return true;
            }
            return false;
        }
        public static bool KeyboardPressed(KeyboardState keyboardState, KeyboardState lastKeyboardState, Keys key)
        {
            return (keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key));
        }

        public static bool KeyboardReleased(KeyboardState keyboardState, KeyboardState lastKeyboardState, Keys key)
        {
            return (keyboardState.IsKeyUp(key) && lastKeyboardState.IsKeyDown(key));
        }

        public static bool KeyboardDown(KeyboardState keyboardState, Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }
    }
}
