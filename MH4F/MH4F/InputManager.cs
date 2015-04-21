using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections;
namespace MH4F
{
    class InputManager
    {

        InputQueue<KeyboardState> inputs;
        KeyboardState lastKeyboardState;
        int maxInputQueueSize = 7;

        List<MoveInput> moveList;

        public KeyboardState LastKeyboardState
        {
            get { return lastKeyboardState; }
        }
        public InputManager()
        {
            inputs = new InputQueue<KeyboardState>();
            lastKeyboardState = new KeyboardState();
            moveList = new List<MoveInput>();
        }

        public String checkMoves(KeyboardState newKeyboardState)
        {
            inputs.Enqueue(newKeyboardState);
            List<String> fireball = moveList[0].InputCommand;
            int x = moveList[0].InputCommand.Count - 1;
            foreach (KeyboardState n in inputs)
            {
                
                if( MoveInput.checkStringInputToKeyInput(fireball[x],n))
                {
                    x--;
                    if (x < 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Fireball");
                        return "TEST";
                    }
                    //System.Diagnostics.Debug.WriteLine("A PRESSED DOWN");
                }
                      
            }
            return "NULL";
        }

        public void registerMove(String name, List<String> input)
        {
            input.Reverse();
            moveList.Add(new MoveInput(name, input));
        }

        public bool KeyboardPressed(KeyboardState keyboardState, Keys key)
        {
            return (keyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key));
        }

        public bool KeyboardReleased(KeyboardState keyboardState, Keys key)
        {
            return (keyboardState.IsKeyUp(key) && LastKeyboardState.IsKeyDown(key));
        }

        public bool KeyboardDown(KeyboardState keyboardState, Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }
    }
}
