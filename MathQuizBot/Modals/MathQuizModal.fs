namespace MathQuizBot.Modals

open Discord
open System
open System.Collections.Immutable

open MathQuizBot.Modal
open MathQuizBot.MathQuiz

type MathQuizModal(difficulty: int) =
    let quiz = genQuiz difficulty
    do printfn $"{difficulty} | {quiz.ToString()} = {quiz.calculate}"
    interface IModal with
        member this.Id: string = "math-quiz"
        member this.Title: string = "Math Quiz!"
        member this.Builder: ModalBuilder = ModalBuilder()
                                                .AddTextInput(quiz.ToString()+" = ?","quiz-blank",placeholder="Write Number")
        member this.onSubmitted(smd: WebSocket.SocketModal): unit = 
            let input = Convert.ToInt32(smd.Data.Components.ToImmutableList().Find(fun data -> data.CustomId = "quiz-blank").Value)
            let isCorrect = quiz.calculate = input
            printfn $"{difficulty} | {quiz.ToString()} = {quiz.calculate} | {input}"
            smd.RespondAsync(embed=
                EmbedBuilder()
                    .WithTitle(if isCorrect then "You Win!" else "Fail...")
                    .WithDescription(
                            ($"Your answer is {input} \n")+
                            (if isCorrect then "And " else "But ") + (quiz.ToString()) + ($" = {quiz.calculate}")
                        )
                    .WithColor(if isCorrect then Color.Green else Color.Red)
                    .WithAuthor(smd.User)
                    .WithCurrentTimestamp()
                    .Build()
            ) |> Async.AwaitTask |> ignore
            

