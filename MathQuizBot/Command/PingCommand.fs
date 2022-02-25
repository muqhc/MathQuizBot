namespace MathQuizBot.Commands

open Discord.WebSocket
open MathQuizBot.Command

type PingCommand() =
    interface ICommand with
        member this.Name: string = "ping"
        member this.Description: string = "Ping Pong!"
        member this.handle(command: SocketSlashCommand): System.Threading.Tasks.Task = task {
            command.RespondAsync "Pong!" |> Async.AwaitTask |> ignore
            }
        

