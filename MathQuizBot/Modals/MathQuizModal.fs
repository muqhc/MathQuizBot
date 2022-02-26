namespace MathQuizBot.Modals

open Discord
open System.Collections.Immutable

open MathQuizBot.Modal
open MathQuizBot.MathQuiz

type MathQuizModal(client: WebSocket.DiscordSocketClient,difficulty: int) =
    member this.Quiz = genQuiz difficulty
    interface IModal with
        member this.Id: string = "math-quiz"
        member this.Title: string = "Math Quiz!"
        member this.Builder: ModalBuilder = ModalBuilder()
                                                .AddTextInput(this.Quiz.ToString()+" = ?","quiz-blank",placeholder="Write Number")
        member this.onSubmitted(smd: WebSocket.SocketModal): unit = 
            let input = smd.Data.Components.ToImmutableList().Find(fun data -> data.CustomId = "quiz-blank").Value
            QuizResultModal(this.Quiz,int input) |> publishModal client |> smd.RespondWithModalAsync |> Async.AwaitTask |> ignore
            

