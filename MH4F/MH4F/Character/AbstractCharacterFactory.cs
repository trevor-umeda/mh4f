using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace MH4F
{
    abstract class AbstractCharacterFactory
    {
        public abstract Player createCharacter(ContentManager content,int playerNumber, int xPosition, int yHeight, ComboManager comboManager, ThrowManager throwManager);

        protected void loadCharacterDataConfigs(String character, Player player, ContentManager content)
        {
            string[] files = Directory.GetFiles("Config/" + character+ "/Moves", "*.txt");

            Dictionary<String, Texture2D> spriteTextures = new Dictionary<String, Texture2D>();
            Dictionary<String, MoveInput> moveInputList = new Dictionary<String, MoveInput>(); 
            foreach (string fileName in files)
            {
                Console.WriteLine(fileName);
                bool parsingList = false;
                String tempListKey = "";
                Dictionary<String, Object> moveInfo = new Dictionary<String, Object>();
                List<String> tempListValue = new List<String>(); ;
                System.IO.Stream stream = TitleContainer.OpenStream(fileName);
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                while (sreader.Peek() >= 0)
                {
                    String spriteLine = sreader.ReadLine();
                   
                    if (!parsingList)
                    {
                        String[] sHb = spriteLine.Split('=');

                        if (sHb[1].Contains("["))
                        {
                            parsingList = true;
                            tempListKey = sHb[0].Trim();
                            tempListValue = new List<String>();                
                            tempListValue.Add(sHb[1].Split('[')[1].Trim());
                        }
                        else
                        {
                            moveInfo.Add(sHb[0].Trim(), sHb[1].Trim());
                        }
                    }
                    else
                    {
                        if (spriteLine.Contains("]"))
                        {
                            parsingList = false;              
                            tempListValue.Add(spriteLine.Split(']')[0].Trim());
                            moveInfo.Add(tempListKey, tempListValue);
                        }
                        else
                        {
                            tempListValue.Add(spriteLine.Trim());
                        }
                    }
                }
                moveParse(content, player, moveInfo, spriteTextures, moveInputList);
            }
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream("Config/"+ character + "/Gatling.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                int lineNumber = 0;
                List<String> moves = new List<String>(); ;
                while (sreader.Peek() >= 0)
                {
                    String movesInfo = sreader.ReadLine();
                    // If its the first line, use that to register the moves and determine priority
                    //
                    if (lineNumber == 0)
                    {
                        Console.WriteLine(movesInfo);
                        moves = new List<String>(movesInfo.Split('|'));
                        for (int i = 0; i < moves.Count; i++)
                        {
                            String moveName = moves[i].Trim();
                            if(moveInputList.ContainsKey(moves[i].Trim()))
                            {
                                 List<String> moveInput = moveInputList[moves[i].Trim()].InputCommand;
                                // TODO ATM this only looks at special moves, and the universal moves are handled outside. which is a bit bad
                                //
                                if (moveInput.Count > 1)
                                {
                                    player.RegisterGroundMove(moveName, moveInputList[moves[i].Trim()].InputCommand);
                                }
                            }               
                        }
                    }
                    // Otherwise register the gatlings
                    //
                    else
                    {
                        Console.WriteLine(movesInfo);
                        String[] sHb = movesInfo.Split('|');
                        List<MoveInput> moveInputs = new List<MoveInput>();
                        for (int i = 0; i < moves.Count; i++)
                        {
                            if (sHb[i].Trim().Equals("x") && moveInputList.ContainsKey(moves[i].Trim()))
                            {

                                moveInputs.Add(moveInputList[moves[i].Trim()]);
                            }
                            Console.WriteLine("Use " + moves[i].Trim() + " is " + sHb[i].Trim());
                        }

                        player.SpecialInputManager.registerGatling(sHb[0].Trim(), moveInputs);
                    }
                    lineNumber++;
                }         
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }
            player.Sprite.CurrentAnimation = "standing";
        }

        protected void moveParse(ContentManager content, Player player, Dictionary<String, Object> moveInfo, Dictionary<String, Texture2D> spriteTextures, Dictionary<String, MoveInput> moveInputList)
        {

            String name = (String)moveInfo["name"];

            Texture2D texture = null;
            if(!spriteTextures.TryGetValue((String)moveInfo["sprite"], out texture)) 
            {
                texture = content.Load<Texture2D>((String)moveInfo["sprite"]);
                spriteTextures.Add((String)moveInfo["sprite"], texture);
            }

            CharacterState moveState;
            Enum.TryParse((String)moveInfo["CharacterState"], true, out moveState);

            Move newMove;

            if (moveState == CharacterState.HIT || moveState == CharacterState.BLOCK)
            {
                newMove = new HitAnimation(
                    texture,
                    int.Parse((String)moveInfo["XImageStart"]),
                    int.Parse((String)moveInfo["YImageStart"]),
                    int.Parse((String)moveInfo["Width"]),
                    int.Parse((String)moveInfo["Height"]),
                    int.Parse((String)moveInfo["FrameCount"]),
                    int.Parse((String)moveInfo["Columns"]),
                    float.Parse((String)moveInfo["FrameLength"]),
                    moveState
                 );
            }
            else
            {
                newMove = new Move(
                    texture, 
                    int.Parse((String)moveInfo["XImageStart"]), 
                    int.Parse((String)moveInfo["YImageStart"]), 
                    int.Parse((String)moveInfo["Width"]),
                    int.Parse((String)moveInfo["Height"]),
                    int.Parse((String)moveInfo["FrameCount"]),
                    int.Parse((String)moveInfo["Columns"]),
                    float.Parse((String)moveInfo["FrameLength"]),
                    moveState
                );
            }

            if (moveInfo.ContainsKey("IsAttack"))
            {
                newMove.IsAttack = Boolean.Parse((String)moveInfo["IsAttack"]);
            }

            if (moveInfo.ContainsKey("NextAnimation"))
            {
                newMove.NextAnimation = (String)moveInfo["NextAnimation"];
            }

            if (moveInfo.ContainsKey("FrameLengthInfo"))
            {
                String framelengthString = (String)moveInfo["FrameLengthInfo"];
                List<String> frameLengthList = new List<String>(framelengthString.Split(','));
                List<int> frameLengthIntList = frameLengthList.ConvertAll(s => Int32.Parse(s));
                int[] frameLengthInfo = frameLengthIntList.ToArray();

                newMove.SetFrameLengthInfo(frameLengthInfo);
            }

            if (moveInfo.ContainsKey("Hitbox"))
            {
                List<String> hitInfo = (List<String>)moveInfo["Hitbox"];
                foreach (String hitBox in hitInfo)
                {
                    String[] hitBoxData = hitBox.Split(';');
                    // Console.WriteLine(int.Parse(hurtBoxData[0]));
                    newMove.AddHitboxInfo(int.Parse(hitBoxData[0]),
                        new Hitbox(hitBoxData[1], hitBoxData[2], hitBoxData[3], hitBoxData[4]));
                }
            }

            
            if (moveInfo.ContainsKey("Hurtbox"))
            {
                List<String> hurtInfo = (List<String>)moveInfo["Hurtbox"];
                foreach (String hurtBox in hurtInfo)
                {
                    String[] hurtBoxData = hurtBox.Split(';');
                   // Console.WriteLine(int.Parse(hurtBoxData[0]));
                    newMove.AddHurtboxInfo(int.Parse(hurtBoxData[0]),
                        new Hitbox(hurtBoxData[1], hurtBoxData[2], hurtBoxData[3], hurtBoxData[4]));
                }
            }
             
            Hitzone hitZone;
            //int test = (int)moveInfo["Blockstun"];

            if(moveInfo.ContainsKey("Hitzone")&& moveInfo.ContainsKey("Hitstun") && moveInfo.ContainsKey("Blockstun")){
                Enum.TryParse((String)moveInfo["Hitzone"], true, out hitZone);
                HitInfo hitMoveInfo = new HitInfo(int.Parse((String)moveInfo["Hitstun"]), int.Parse((String)moveInfo["Blockstun"]), hitZone);
                hitMoveInfo.Damage = int.Parse((String)moveInfo["Damage"]);
                hitMoveInfo.IsHardKnockDown = Boolean.Parse((String)moveInfo["IsHardKnockDown"]);
                hitMoveInfo.AirUntechTime = int.Parse((String)moveInfo["AirUntechTime"]);
                hitMoveInfo.AirYVelocity = int.Parse((String)moveInfo["AirYVelocity"]);
                hitMoveInfo.AirXVelocity = int.Parse((String)moveInfo["AirXVelocity"]);

                if (moveInfo.ContainsKey("ForceAirborne"))
                {
                    hitMoveInfo.ForceAirborne = Boolean.Parse((String)moveInfo["ForceAirborne"]);
                }
                if (moveInfo.ContainsKey("FreezeMove"))
                {
                    hitMoveInfo.FreezeOpponent = Boolean.Parse((String)moveInfo["FreezeMove"]);
                }
                if (moveInfo.ContainsKey("Unblockable"))
                {
                    hitMoveInfo.Unblockable = Boolean.Parse((String)moveInfo["Unblockable"]);
                }
                newMove.HitInfo = hitMoveInfo;
            }

            if (moveInfo.ContainsKey("Input"))
            {
                String moveInput = (String)moveInfo["Input"];
                String[] inputs = moveInput.Split(';');
                List<String> inputList = new List<String>(inputs);
                moveInputList.Add(name, new MoveInput(name, inputList));
            }
            player.Sprite.AddAnimation(name, newMove);
            Console.WriteLine("SDFSD");
        }

        protected void loadCharacterData(String character, Player player, ContentManager content)
        {
            // Load Sprite Info
            //
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(character + "Sprites.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                bool readingAnimations = false;
                Dictionary<String, Texture2D> spriteTextures = new Dictionary<String, Texture2D>();
                while (sreader.Peek() >= 0)
                {
                    String spriteLine = sreader.ReadLine();
                    Console.WriteLine(spriteLine);
                    if (spriteLine.Contains("-SPRITEANIMATIONS-"))
                    {
                        readingAnimations = true;
                    }
                    else if (spriteLine.Contains("-SPRITESHEET-"))
                    {
                        readingAnimations = false;
                    }
                    else if (!readingAnimations)
                    {
                        Texture2D test = content.Load<Texture2D>(spriteLine);
                        spriteTextures[spriteLine] = test;
                    }
                    else
                    {
                        String[] sHb = spriteLine.Split(';');
                        Console.WriteLine("Using " + sHb[0]);
                        //player2.Sprite.AddAnimation(standing, "standing", 0, 0, 144, 288, 8, 0.1f, CharacterState.STANDING);                       
                        CharacterState moveState;
                        Enum.TryParse(sHb[9], true, out moveState);
                        if (sHb.Length >= 11)
                        {
                            // Hacky way to add moves
                            //
                            if (sHb[10] == "true")
                            {
                                player.Sprite.AddAnimation(spriteTextures[sHb[0]], sHb[1], int.Parse(sHb[2]), int.Parse(sHb[3]), int.Parse(sHb[4]),
                                                          int.Parse(sHb[5]), int.Parse(sHb[6]), int.Parse(sHb[7]), float.Parse(sHb[8]), moveState, true);
                            }
                            else
                            {
                                player.Sprite.AddAnimation(spriteTextures[sHb[0]], sHb[1], int.Parse(sHb[2]), int.Parse(sHb[3]), int.Parse(sHb[4]),
                                                          int.Parse(sHb[5]), int.Parse(sHb[6]), int.Parse(sHb[7]), float.Parse(sHb[8]), moveState, sHb[10]);
                            }

                        }
                        else
                        {
                            player.Sprite.AddAnimation(spriteTextures[sHb[0]], sHb[1], int.Parse(sHb[2]), int.Parse(sHb[3]), int.Parse(sHb[4]),
                            int.Parse(sHb[5]), int.Parse(sHb[6]), int.Parse(sHb[7]), float.Parse(sHb[8]), moveState);
                        }

                    }



                }
                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }

            // Load hitbox info
            //
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(character + "Hitbox.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                while (sreader.Peek() >= 0)
                {
                    String hitboxInfo = sreader.ReadLine();
                    Console.WriteLine(hitboxInfo);
                    String[] sHb = hitboxInfo.Split(';');
                    Console.WriteLine(sHb[0]);

                    player.Sprite.AddHitbox(sHb[0], Convert.ToInt32(sHb[1]), new Hitbox(sHb[2], sHb[3], sHb[4], sHb[5]));
                }
                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }

            // Load Hurtbox info
            //
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(character + "Hurtbox.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                while (sreader.Peek() >= 0)
                {
                    String hurtboxInfo = sreader.ReadLine();
                    Console.WriteLine(hurtboxInfo);
                    String[] sHb = hurtboxInfo.Split(';');
                    Console.WriteLine(sHb[0]);
                    player.Sprite.AddHurtbox(sHb[0], Convert.ToInt32(sHb[1]), new Hitbox(sHb[2], sHb[3], sHb[4], sHb[5]));
                }
                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }

            // Load Move info
            //
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(character + "Moves.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                while (sreader.Peek() >= 0)
                {
                    String movesInfo = sreader.ReadLine();
                    if (movesInfo.Contains("- MoveName"))
                    {

                    }
                    else
                    {
                        Console.WriteLine(movesInfo);
                        String[] sHb = movesInfo.Split(';');
                        Console.WriteLine(sHb[0]);
                        Hitzone hitZone;
                        Enum.TryParse(sHb[3], true, out hitZone);
                        Boolean isHardKnockdown = false;
                        if (sHb[5] == "true")
                        {
                            isHardKnockdown = true;
                        }
                        HitInfo hitInfo = new HitInfo(int.Parse(sHb[1]), int.Parse(sHb[2]), hitZone);
                        hitInfo.IsHardKnockDown = isHardKnockdown;
                        hitInfo.Damage = int.Parse(sHb[4]);
                        hitInfo.AirUntechTime = int.Parse(sHb[6]);
                        hitInfo.AirXVelocity = int.Parse(sHb[7]);
                        hitInfo.AirYVelocity = -int.Parse(sHb[8]);
                        if (sHb.Length > 9)
                        {
                            if (sHb[9] == "true")
                            {
                                hitInfo.ForceAirborne = true;
                            }
                        }
                        if (sHb.Length > 10)
                        {
                            if (sHb[10] == "true")
                            {
                                hitInfo.FreezeOpponent = true;
                            }
                        }
                        if (sHb.Length > 11)
                        {
                            if (sHb[11] == "true")
                            {
                                hitInfo.Unblockable = true;
                            }
                        }
                        player.SetAttackMoveProperties(sHb[0], hitInfo);
                    }
                }
                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }

            // Load Generic Move Input data
            //
            Dictionary<String, MoveInput> moveInputList = new Dictionary<String, MoveInput>(); ;
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream("UniversalInputs.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data           
                while (sreader.Peek() >= 0)
                {
                    String movesInfo = sreader.ReadLine();

                    Console.WriteLine(movesInfo);
                    String[] moves = movesInfo.Split(' ');
                    moveInputList.Add(moves[0], new MoveInput(moves[0], new List<String> { moves[1] }));
                }
                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }

            // Load character specific input move data
            //
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(character + "MoveInputs.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data           
                while (sreader.Peek() >= 0)
                {
                    String movesInfo = sreader.ReadLine();

                    Console.WriteLine(movesInfo);
                    String[] moves = movesInfo.Split(' ');
                    String[] inputs = moves[1].Split(';');
                    List<String> inputList = new List<String>(inputs);
                    moveInputList.Add(moves[0], new MoveInput(moves[0], inputList));
                    //player.SetAttackMoveProperties(sHb[0], hitInfo);
                   // player1.RegisterGroundMove("fireball", new List<string> { "2", "3", "6", "A" });
                    player.RegisterGroundMove(moves[0], inputList);

                }
                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }

            // Load Gatling
            //
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(character + "Gatling.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                int lineNumber = 0;
                List<String> moves = new List<String>(); ;
                while (sreader.Peek() >= 0)
                {
                    String movesInfo = sreader.ReadLine();
                    if (lineNumber == 0)
                    {
                        Console.WriteLine(movesInfo);
                        moves = new List<String>(movesInfo.Split('|'));
                    }
                    else
                    {
                        Console.WriteLine(movesInfo);
                        String[] sHb = movesInfo.Split('|');
                        List<MoveInput> moveInputs = new List<MoveInput>();
                        for (int i = 0; i < moves.Count; i++)
                        {
                            if (sHb[i].Trim().Equals("x"))
                            {
                                moveInputs.Add(moveInputList[moves[i].Trim()]);
                            }
                            Console.WriteLine("Use " + moves[i].Trim() + " is " + sHb[i].Trim());
                        }
                        player.SpecialInputManager.registerGatling(sHb[0].Trim(), moveInputs);


                    }
                    lineNumber++;
                }

                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }

            player.Sprite.CurrentAnimation = "standing";
        }
    }
}
