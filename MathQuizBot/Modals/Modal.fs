module MathQuizBot.Modal

open Discord
open Discord.WebSocket
open System.Threading.Tasks

type IModal =
    abstract Id: string
    abstract Title: string
    abstract Builder: ModalBuilder
    abstract onSubmitted: SocketModal -> unit

let publishModal (client: DiscordSocketClient) (imodel: IModal): Modal =
    client.add_ModalSubmitted (fun smd -> task{if smd.Data.CustomId = imodel.Id then imodel.onSubmitted smd })
    imodel.Builder
        .WithCustomId(imodel.Id)
        .WithTitle(imodel.Title)
        .Build()
