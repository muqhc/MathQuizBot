namespace MathQuizBot.Commands

open Discord
open MathQuizBot.Command
open MathQuizBot.Modal
open MathQuizBot.Modals


type FoodCommand(client: WebSocket.DiscordSocketClient) =
    interface ICommand with
        member this.Options: SlashCommandOptionBuilder[] = [||]
        member this.Description: string = "Question What's your fav food?"
        member this.Name: string = "food"
        member this.handle(cmd: WebSocket.SocketSlashCommand): unit = 
            FoodModal() |> publishModal client
            |> cmd.RespondWithModalAsync |> Async.AwaitTask |> ignore
