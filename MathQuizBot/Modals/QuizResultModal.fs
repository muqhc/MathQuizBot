namespace MathQuizBot.Modals

open Discord

open MathQuizBot.Modal
open MathQuizBot.MathQuiz

open System.Collections.Generic

type QuizResultModal(quiz: Quiz,input: int) =
    interface IModal with
        member this.Id: string = "quiz-result"
        member this.Title: string = if quiz.calculate = input then "You Win!" else "Fail..."
        member this.Builder: Discord.ModalBuilder = ModalBuilder()
        member this.onSubmitted(smd: Discord.WebSocket.SocketModal): unit = ignore None

