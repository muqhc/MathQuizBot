module MathQuizBot.Program

open Discord
open Discord.WebSocket
open Discord.Commands
open System
open System.Threading

let logging (log: LogMessage) = log.ToString() |> printfn "%s"

let run (args: string[]) = 
    task {
        printfn "Bot Booting....."

        let client = new DiscordSocketClient()
        let commandServ = new CommandService()

        client.add_Log      (fun log -> task { logging log })
        commandServ.add_Log (fun log -> task { logging log })

        let token = match Environment.GetEnvironmentVariable "BOT_TOKEN" with
                    | null ->
                        printf "Write Bot Token : "
                        Console.ReadLine()
                    | it -> it
        client.LoginAsync (TokenType.Bot, token) |> Async.AwaitTask |> ignore
        client.StartAsync () |> Async.AwaitTask |> ignore

        client.add_Ready <| Command.registerGuild 946407404201979954UL (Commands.PingCommand()) client 

        Thread.Sleep(-1)
    }

[<EntryPoint>]
let main (args: string[]): int =
    run args |> ignore
    0
