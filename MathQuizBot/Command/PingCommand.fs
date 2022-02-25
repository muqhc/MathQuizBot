namespace MathQuizBot.Commands

open Discord
open Discord.WebSocket
open MathQuizBot.Command

type PingCommand() =
    interface ICommand with
        member this.Name: string = "ping"
        member this.Description: string = "Ping Pong!"
        member this.handle(command: SocketSlashCommand): System.Threading.Tasks.Task = task {
            EmbedBuilder()
                .WithTitle("Pong!")
                .WithDescription((-) (System.DateTime.Now.Millisecond) (command.CreatedAt.Millisecond) |> sprintf "~ %d ms")
                .WithCurrentTimestamp()
                .Build()
            |> fun em -> command.RespondAsync ("",[|em|]) |> Async.AwaitTask |> ignore
            (command.User.Username, command.User.Id.ToString()) ||> printfn "'/ping' called by %s ( %s )"
            }
        

