namespace MathQuizBot.Commands

open Discord
open Discord.WebSocket
open MathQuizBot.Command

type PingCommand() =
    interface ICommand with
        member this.Options: SlashCommandOptionBuilder[] = null
        member this.Name: string = "ping"
        member this.Description: string = "Ping Pong!"
        member this.handle (cmd: SocketSlashCommand) =
            EmbedBuilder()
                .WithTitle("Pong!")
                .WithDescription((-) (System.DateTime.Now.Millisecond) (cmd.CreatedAt.Millisecond) |> sprintf "~ %d ms")
                .WithCurrentTimestamp()
                .Build() |> fun em -> cmd.RespondAsync (embed=em) |> Async.AwaitTask |> ignore
            (cmd.User.Username, cmd.User.Id.ToString()) ||> printfn "'/ping' called by %s(%s)"
        

