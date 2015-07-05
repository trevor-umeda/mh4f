using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
namespace MH4F
{
    public class OneButtonThrowManager : ThrowManager
    {

        private String FORWARD_THROW_INPUT = "6C";
        private String BACK_THROW_INPUT = "4C";

        private String FORWARD_THROW_WHIFF_MOVE = "forwardcattack";
        private String BACK_THROW_WHIFF_MOVE = "backcattack";

        private CharacterState player1State;
        private CharacterState player2State;

        public void updateCharacterState(int playerNum, CharacterState characterState)
        {
            if (playerNum == 1)
            {
                player1State = characterState;
            }
            else
            {
                player2State = characterState;
            }
        }

        public String ForwardThrowInput
        {
            get
            {
                return FORWARD_THROW_INPUT;
            }
        }

        public String BackThrowInput
        {
            get
            {
                return BACK_THROW_INPUT;
            }
            
        }

        public String ForwardThrowWhiffMove
        {
            get
            {
                return FORWARD_THROW_WHIFF_MOVE;
            }            
        }

        public String BackThrowWhiffMove
        {
            get
            {
                return BACK_THROW_WHIFF_MOVE;
            }
        }

        public bool isValidThrow(int playerNum)
        {
            // You have to check the opponent (the other players) state
            //
            if (playerNum == 1)
            {

            }
            return true;
        }
        
        private bool checkPlayerThrowState()
        {
            return true;
        }

        public CharacterState Player1State
        {
            get { return player1State; }
            set { player1State = value; }
        }

        public CharacterState Player2State
        {
            get { return player2State; }
            set { player2State = value; }
        }
    }
}
