namespace MathQuizBot.Commands

open Discord
open System
open System.Collections.Immutable

open MathQuizBot.Command
open MathQuizBot.Modal
open MathQuizBot.Modals


type MathQuizCommand(client: WebSocket.DiscordSocketClient) =
    member this.DefaultDifficulty = 6
    interface ICommand with
        member this.Description: string = "Let's solve math quiz!"
        member this.Name: string = "quiz"
        member this.Options: SlashCommandOptionBuilder[] = 
            [|
                SlashCommandOptionBuilder()
                    .WithName("difficulty")
                    .WithRequired(false)
                    .WithType(ApplicationCommandOptionType.Integer)
                    .WithDescription("difficulty number ; the more high, more difficult")
            |]
        member this.handle(cmd: WebSocket.SocketSlashCommand): unit = 
            let value = try Convert.ToInt32(cmd.Data.Options.ToImmutableArray().[0].Value :?> int64) with 
                        | :? System.IndexOutOfRangeException -> this.DefaultDifficulty
            if value < 14 
                then
                    MathQuizModal(value,client) |> publishModal client
                    |> cmd.RespondWithModalAsync |> Async.AwaitTask |> ignore
                else
                    cmd.RespondAsync($"difficulty max is 13, not allow {value}",ephemeral=true) |> Async.AwaitTask |> ignore

