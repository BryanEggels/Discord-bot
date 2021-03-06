﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.VoiceNext;
using System.IO;
using Mivos_Bot.Context;
using Mivos_Bot.Repository;
using Mivos_Bot.Models;


namespace Mivos_Bot
{
    public class Program
    {
        private static VoiceNextClient VoiceService { get; set; }

        
        static void Main(string[] args)
        {
            
            Run();
            Console.ReadLine();

        }
        public static async Task Run()
        {
            MessageRepository messagerepo = new MessageRepository(new MessageSQLContext());
            UserRepository userrepo = new UserRepository(new UserSQLContext());
            
            try
            {
                var discord = new DiscordClient(new DiscordConfig
                {
                    AutoReconnect = true,
                    DiscordBranch = Branch.Stable,
                    LargeThreshold = 250,
                    LogLevel = LogLevel.Unnecessary,
                    Token = File.ReadAllText("token.txt"),
                    TokenType = TokenType.Bot,
                    UseInternalLogHandler = false
                });

                
                discord.DebugLogger.LogMessageReceived += (o, e) =>
                {
                    Console.WriteLine($"[{e.TimeStamp}] [{e.Application}] [{e.Level}] {e.Message}");
                };

                discord.MessageCreated += async (e) => //triggered when a new message comes in
                {
                    
                    if (!e.Message.Author.IsBot ) //filter the bots
                    {
                        if (userrepo.GetMuted().Any(User => User.uid == e.Author.ID)) //see if the messageauthor is currently muted
                        {
                            await e.Message.Delete();

                        }
                         
                        else if (messagerepo.CheckDuplicate(e.Message, e.Guild.ID) && !Containscommand(e.Message.Content.Split(' '))) //check if the message is a duplicate
                        {
                            
                            userrepo.MuteUser(e.Message.Author);
                            User muteduser = userrepo.GetUser(e.Author.ID);
                            await e.Message.Respond($"\"{e.Message.Content}\" is duplicate content!\n{e.Message.Author.Username} has been muted untill {muteduser.Mute_Expired}, this is mute number {muteduser.Mutecount}!");
                        }
                        await Task.Delay(0);
                    }
                    Console.WriteLine(e.Guild.ID.ToString());
                };
                

                discord.PresenceUpdate += async (e) => //still testing
                {
                    
                    DiscordGuild guild = await discord.GetGuild(e.GuildID);
                    List<DiscordChannel> channels = await guild.GetChannels();
                    await channels[0].SendMessage($"Welcome! <@{e.User.ID}>");
                    
                    await Task.Delay(0);
                };
                addcommands(discord);

                await discord.Connect();
                await discord.UpdateStatus("Met de banhamer", -1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            await Task.Delay(-1);
        }
        public static void addcommands(DiscordClient p_discord)
        {
            
            p_discord.UseCommands(new CommandConfig
            {
                Prefix = "!",
                SelfBot = false,
            });
            UserRepository userrepo = new UserRepository(new UserSQLContext());
            MessageRepository msgrepo = new MessageRepository(new MessageSQLContext());
            p_discord.AddCommand("hello", async e =>
            {
                string[] msg = e.Message.Content.Split(' ');

                //if msg array contains more then 1 arrayobject then..
                if (msg.Length > 1 && msg[1].Length >= 3)

                {
                    bool ChannelContainsUser = false;
                    List<DiscordUser> members = new List<DiscordUser>();
                    //check if member is in the current 'guild'
                    foreach (DiscordMember dm in e.Guild.Members)
                    {
                        //check if msg[1] containst a part of the user
                        if (dm.User.Username.ToUpper().Contains(msg[1].ToUpper()))
                        {
                            if (dm.User.Username.ToUpper() == msg[1].ToUpper())
                            {
                                ChannelContainsUser = true;
                                await e.Message.Respond($"{e.Message.Author.Username } says hello to <@{dm.User.ID}> ");
                                break;
                            }
                            members.Add(dm.User);
                        }
                        if (members.Count > 1 && !ChannelContainsUser)
                        {
                            await e.Message.Respond($"more then 1 user with those characters in his name");
                            ChannelContainsUser = true;
                            break;
                        }

                    }
                    if (members.Count == 1)
                    {
                        ChannelContainsUser = true;
                        await e.Message.Respond($"{e.Message.Author.Username } says hello to <@{members[0].ID}> ");

                    }
                    else if (!ChannelContainsUser)
                    {
                        await e.Message.Respond("That user is not in the current channel");
                    }

                }
                else
                {
                    await e.Message.Respond($"Hello, {e.Message.Author.Mention}!");
                }


            });
            p_discord.AddCommand("reken", async e =>
            {
                string[] msg = e.Message.Content.Split(' ');
                try
                {
                    double num1 = Convert.ToDouble(msg[1]);
                    double num2 = Convert.ToDouble(msg[3]);
                    double num3 = 1;
                    if (msg.Length > 4)
                    {
                        num3 = Convert.ToDouble(msg[4]);
                    }

                    switch (msg[2])
                    {
                        case "+":
                            await e.Message.Respond(num1.ToString() + msg[2] + num2.ToString() + '=' + (num1 + num2).ToString());
                            break;
                        case "-":
                            await e.Message.Respond(num1.ToString() + msg[2] + num2.ToString() + '=' + (num1 - num2).ToString());
                            break;
                        case "*":
                        case "x":
                            await e.Message.Respond(num1.ToString() + msg[2] + num2.ToString() + '=' + (num1 * num2).ToString());
                            break;
                        case "/":
                            await e.Message.Respond(num1.ToString() + msg[2] + num2.ToString() + '=' + (num1 / num2).ToString());
                            break;
                        case "**":
                        case "^":
                            await e.Message.Respond(num1.ToString() + msg[2] + num2.ToString() + '=' + Math.Pow(num1, num2).ToString());
                            break;
                        case "^^":
                            await e.Message.Respond(num1.ToString() + msg[2] + num2.ToString() + '=' + Math.Pow(Math.Pow(num1, num2), num3).ToString());
                            break;
                        case "%":
                            await e.Message.Respond(num1.ToString() + msg[2] + num2.ToString() + '=' + (num1 % num2).ToString());
                            break;
                        case ">":
                        case "<":
                        case "==":
                        case "!=":
                            await e.Message.Respond(msg[1] + " " + msg[2] + " " + msg[3] + " " + "= " + Operator(msg[2], num1, num2).ToString());
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception exc)
                {
                    await e.Message.Respond(exc.Message);
                }

            });
            p_discord.AddCommand("join", async e =>
            {
                try
                {
                    var vcfg = new VoiceNextConfiguration
                    {
                        VoiceApplication = DSharpPlus.VoiceNext.Codec.VoiceApplication.Music

                    };
                    VoiceService = p_discord.UseVoiceNext(vcfg);

                    await VoiceService.ConnectAsync(await p_discord.GetChannelByID(272324215439491072));
                    Console.WriteLine("Joined voice channel");
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }

            });
            p_discord.AddCommand("dc", async e =>
            {
                try
                {
                    DiscordGuild guild = await p_discord.GetGuild(e.Channel.GuildID);

                    VoiceService.GetConnection(guild).Disconnect();
                    VoiceService.GetConnection(guild).Dispose();

                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            });
            p_discord.AddCommand("god", async e =>
            {
                if (e.Message.Author.ID == 261216517910167554)
                {
                    await e.Message.Respond("AND THE REAL C#GOD IS....\n" + e.Message.Author.Username);

                }
                else
                {
                    await e.Message.Respond("you're not teh real c#god.");
                }


            });
            p_discord.AddCommand("kkk", async e =>
            {
                await e.Message.Respond($"WHITE POWER, RIGHT { e.Message.Author.Username }?");
            });
            p_discord.AddCommand("help", async e =>
            {
                string prefix = "!";
                await e.Message.Respond($"currently available commands are: \n{prefix}hello <username> \n{prefix}reken 'nummer' 'operator' 'nummer' \n{prefix}god to see if you are a c# god\n{prefix}karma @username to give a user karma!\n"+
                    $"{prefix}dice 'minimumnumber' 'maximumnumber' (without the quotes) to generate a random number. {prefix}dice will automatically pick a number between 1 and 100.\n" +
                    $"\n\nThis bot also functions as Robot9000. This means that the bot will mute you if you post duplicate content that already has been posted in this server.\n"+
                    "The amount of time you get muted depends on the amount of punishments you already had.");

            });
            p_discord.AddCommand("666", async e =>
            {
                await e.Message.Respond("HAIL SATAN " + e.Message.Author.Username);
            });
            p_discord.AddCommand("blm", async e =>
            {
                await e.Message.Respond("BLACK HAS NO POWER, RIGHT " + e.Message.Author.Username + "?");
            });
            p_discord.AddCommand("play", async e =>
            {
                try
                {
                    DiscordGuild guild = await p_discord.GetGuild(e.Channel.GuildID);
                    var rand = new Random();
                    var bytes = new byte[32000];
                    rand.NextBytes(bytes);

                    await VoiceService.GetConnection(guild).SendAsync(bytes, 517, 16);
                    Console.Write("i just played something!");
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            });
            p_discord.AddCommand("karma", async e =>
            {
                string[] msg = e.Message.Content.Split(' ');
                List<DiscordUser> users = e.Message.Mentions;
                if (users.Count == 1)
                {
                    if (users[0].ID != e.Message.Author.ID)
                    {
                        int karma = AddKarma(users[0].ID);
                        await e.Message.Respond($"{users[0].Username} gained 1 karma!\n{users[0].Username} now has {karma} karma");
                    }
                    else {
                        await e.Message.Respond($"You just lost 1 karma");
                    }

                }
                else if (users.Count > 1)
                {
                    await e.Message.Respond($"Please only mention 1 user :)");
                }
                else
                {
                    await e.Message.Respond($"You have to at least mention 1 user");
                }

            });
            p_discord.AddCommand("dice", async e =>
            {
                string[] msg = e.Message.Content.Split(' ');
                if (msg.Length > 1)
                {
                    int random = Dice(Convert.ToInt32(msg[1]), Convert.ToInt32(msg[2]));
                    await e.Message.Respond(random.ToString());

                }
                else if (msg.Length == 1)
                {
                    int random = Dice(1, 101);
                    await e.Message.Respond(random.ToString());
                }
                else
                {
                    await e.Message.Respond("Please use 2 parameters divided by a space");
                }

            });
            p_discord.AddCommand("unmute", async e =>
            {
                await Task.Delay(0);
                if(e.Message.Author.ID == 261216517910167554 || e.Message.Author.ID == 239471183475638272)
                {
                    if (e.Message.Mentions.Count > 0)
                    {
                        foreach (DiscordUser user in e.Message.Mentions)
                        {
                            new UserRepository(new UserSQLContext()).Unmute(user.ID);
                            await e.Message.Respond($"{e.Message.Author.Username } unmuted <@{user.ID}> ");
                        }
                    }
                    else
                    {
                        await e.Message.Respond($"Please mention the user(s) you want to unmute!");
                    }
                }
                else
                {
                    await e.Message.Respond($"You ar not permitted to unmute users!");
                }
            });
            p_discord.AddCommand("mutereset", async e =>
            {
                if (e.Message.Author.ID == 261216517910167554 || e.Message.Author.ID == 239471183475638272)
                {
                    
                    if(userrepo.MuteCountreset())
                    {
                        userrepo.MuteReset();
                        await e.Message.Respond("Robot9000 mutes have been reset!");
                    }
                    else
                    {
                        await e.Message.Respond("Oops! looks like something went wrong. Please contact Mivo90");
                    }
                }
                else
                {
                    await e.Message.Respond("I'm sorry, I can not let you do that.");
                }
            });
            p_discord.AddCommand("messagereset", async e =>
            {
                if(e.Message.Author.ID == 261216517910167554 || e.Message.Author.ID == 239471183475638272)
                {
                    if (msgrepo.ResetMessages())
                    {
                        await e.Message.Respond("Robot9000 messages have been reset!");
                    }
                    else
                    {
                        await e.Message.Respond("Oops, couldn't reset the messages. Please contact Mivo90");
                    }
                    
                }
                else
                {
                    await e.Message.Respond("Nice try.");
                }
            });

        }
        public static Boolean Operator(string logic, double x, double y)
        {
            switch (logic)
            {
                case ">": return x > y;
                case "<": return x < y;
                case "==": return x == y;
                case "!=": return x != y;

                default: throw new Exception("invalid logic");
            }
        }
        public static int AddKarma(ulong userid)
        {
            List<string> karmalist = new List<string>();
            string[] karmaline = null;
            StreamReader sr = new StreamReader("Karma.txt");
            bool alreadyExists = false;
            string readline = null;
            int lineNumber = 0;
            while ((readline = sr.ReadLine()) != null) {
                karmalist.Add(readline);
                ++lineNumber;
            }
            sr.Close();
            sr.Dispose();
            foreach (string line in karmalist)
            {
                if (line.Contains(userid.ToString()))
                {
                    karmaline = line.Split(':');
                    alreadyExists = true;
                    int karma = Convert.ToInt32(karmaline[1]) + 1;
                    karmaline[1] = karma.ToString();
                    lineChanger(userid.ToString() + ":" + karmaline[1], "Karma.txt", lineNumber);
                    return karma;
                }
            }
            if (!alreadyExists)
            {
                StreamWriter sw = new StreamWriter("Karma.txt", true);
                sw.WriteLine(userid.ToString() + ":" + "1");
                sw.Close();
                return 1;
            }

            return 0; //0 represents that there went something wrong
        }
        static void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }
        public static int Dice(int from, int to)
        {
            Random r = new Random();
            if (from > to)
            {
                return r.Next(to, from + 1);
            }
            else if(to > from)
            {
                return r.Next(from, to + 1);
            }
            else
            {
                return from;
            }
        }
        public static bool Containscommand(string[] message)
        {
            if (message[0].StartsWith("!"))
            {
                message[0] = message[0].TrimStart('!');
                List<string> commands = new List<string>();
                commands.Add("help");
                commands.Add("dice");
                commands.Add("join");
                commands.Add("dc");
                commands.Add("god");
                commands.Add("kkk");
                commands.Add("hello");
                commands.Add("666");
                commands.Add("blm");
                commands.Add("play");
                commands.Add("karma");
                commands.Add("leaderboard");
                commands.Add("lb");
                commands.Add("unmute");
                commands.Add("mutereset");
                commands.Add("messagereset");


                foreach (string command in commands)
                {
                    if (command == message[0]) { return true; }
                }
            }
            return false;

        }


        
    }

}

