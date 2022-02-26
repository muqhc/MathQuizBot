namespace MathQuizBot.Modals

open Discord
open System
open System.Collections
open System.Collections.Immutable

open MathQuizBot.Modal

type FoodModal() =
    interface IModal with
        member this.Id: string = "fav-food"
        member this.Title: string = "Your Favorite Food"
        member this.Builder: ModalBuilder = ModalBuilder()
                                                .AddTextInput("What's your fav food?","food-name",placeholder="")
                                                .AddTextInput("Have any reason?","food-reason",TextInputStyle.Paragraph,"")
        member this.onSubmitted(smd: WebSocket.SocketModal) = 
            let foodName = smd.Data.Components.ToImmutableList().Find(fun data -> data.CustomId = "food-name").Value
            let foodReason = smd.Data.Components.ToImmutableList().Find(fun data -> data.CustomId = "food-reason").Value
            smd.RespondAsync (embed=
                EmbedBuilder()
                    .WithTitle( ( smd.User.Username, foodName) ||> sprintf "%s's fav food is \"%s\"")
                    .WithDescription( ( smd.User.Username, foodReason) ||> sprintf "%s said \"%s\"")
                    .Build()
            ) |> Async.AwaitTask |> ignore