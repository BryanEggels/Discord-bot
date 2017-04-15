using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.VoiceNext;
using System.IO;



namespace Mivos_Bot
{
    class Program
    {
        private static VoiceNextClient VoiceService { get; set; }


        static void Main(string[] args)
        {
            Run();
            Console.ReadLine();

        }
        public static async Task Run()
        {
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


                addcommands(discord);
                
                await discord.Connect();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            

            await Task.Delay(-1);
        }
        public static void addcommands(DiscordClient p_discord)
        {

            p_discord.UseCommands(new CommandConfig
            {
                Prefix = "#",
                SelfBot = false,
            });
            p_discord.AddCommand("hello", async e =>
            {
                await e.Message.Respond($"Hello, {e.Message.Author.Mention}!");
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
                catch(Exception exc)
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
                    
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            });
            p_discord.AddCommand("god", async e =>
            {
                if(e.Message.Author.ID == 261216517910167554)
                {
                    await e.Message.Respond("AND THE REAL C#GOD IS....\n"+e.Message.Author.Username);
                    
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
                await e.Message.Respond("currently available commands are: \n#hello \n#reken 'nummer' 'operator' 'nummer' \n#god to see if you are a c# god ");

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

                    await VoiceService.GetConnection(guild).SendAsync(bytes,517);
                    Console.Write("i just played something!");
                } 
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
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



    }
}
