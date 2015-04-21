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

        public List<String> InputCommand
        {
            get { return inputCommand; }
            set { inputCommand = value; }
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
    }
}
