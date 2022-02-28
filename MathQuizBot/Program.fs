module MathQuizBot.Program

open Discord
open Discord.WebSocket
open Discord.Commands
open System
open System.Diagnostics
open System.Threading
open System.Threading.Tasks

open MathQuizBot.Commands

let logging (log: LogMessage) = log.ToString() |> printfn "%s"

let run (token: string) (args: string[]) (someObj: obj) = 
    printfn "Bot Booting....."
    
    let client = new DiscordSocketClient()
    let commandServ = new CommandService()
    
    client.add_Log      (fun log -> task { logging log })
    commandServ.add_Log (fun log -> task { logging log })
    
    client.LoginAsync (TokenType.Bot, token) |> Async.AwaitTask |> ignore
    client.StartAsync () |> Async.AwaitTask |> ignore
    
    client.add_Ready <| Command.publishCmdGlobal client [|
        PingCommand ()
        FoodCommand client
        MathQuizCommand client
    |]
    Thread.Sleep(-1)

[<EntryPoint>]
let main (args: string[]): int =
    let token = match Environment.GetEnvironmentVariable "BOT_TOKEN" with
                    | null ->
                        printf "Write Bot Token : "
                        Console.ReadLine()
                    | it -> it
    let botFun = run token args
    let botThread = Thread(ParameterizedThreadStart botFun)
    botThread.Start()
    printfn "Main Thread End."
    0
