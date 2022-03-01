namespace MathQuizBot.Components.Buttons

open Discord
open MathQuizBot.Components.Button
open MathQuizBot.MathQuiz
open MathQuizBot.Modal

type QuizRetryButton(client: WebSocket.DiscordSocketClient,isCorrect: bool,imodal: IModal) =
    interface IButton with
        member this.Id: string = $"retry-button:{this.GetHashCode()}"
        member this.Label: string = "Try again"
        member this.Builder: Discord.ButtonBuilder = ButtonBuilder()
                                                        .WithStyle(if isCorrect then ButtonStyle.Success else ButtonStyle.Danger)
        member this.onExcuted(smc: Discord.WebSocket.SocketMessageComponent): unit = 
            publishModal client imodal
            |> smc.RespondWithModalAsync
            |> Async.AwaitTask |> ignore
        

