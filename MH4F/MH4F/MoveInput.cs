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
        public static Boolean checkStringInputToKeyInput(String input, KeyboardState keyboardState, Direction direction, Dictionary<string, Keys> controls)
        {
            if (input == "A" && keyboardState.IsKeyDown(controls["a"]))
            {
                return true;
            }
            if ( keyboardState.IsKeyDown(controls["right"]) && ((direction == Direction.Right && input == "6") || (direction == Direction.Left && input =="4")))
            {
                return true;
            }
            if (keyboardState.IsKeyDown(controls["right"]) && keyboardState.IsKeyDown(controls["down"]) && ((direction == Direction.Right && input == "3") || (direction == Direction.Left && input == "1")))
            {
                return true;
            }
            if (keyboardState.IsKeyDown(controls["left"]) && keyboardState.IsKeyDown(controls["down"]) && ((direction == Direction.Right && input == "1") || (direction == Direction.Left && input == "3")))
            {
                return true;
            }
            if (input == "2" && keyboardState.IsKeyDown(controls["down"]))
            {
                return true;
            }
            if (keyboardState.IsKeyDown(controls["left"]) && ((direction == Direction.Right && input == "4") || (direction == Direction.Left && input == "6")))
            {
                return true;
            }
            if (input == "5" && keyboardState.IsKeyUp(controls["down"]) && keyboardState.IsKeyUp(controls["up"]) && keyboardState.IsKeyUp(controls["right"]) && keyboardState.IsKeyUp(controls["left"]))
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

        // Boring getters and setters here
        //
    }
}
