module MathQuizBot.Components.Button

open Discord
open Discord.WebSocket

type IButton =
    abstract Id: string
    abstract Label: string
    abstract Builder: ButtonBuilder
    abstract onExcuted: SocketMessageComponent -> unit

let publishButton (client: DiscordSocketClient) (ibutton: IButton): ButtonBuilder =
    client.add_ButtonExecuted(fun smc -> task{if smc.Data.CustomId = ibutton.Id then ibutton.onExcuted smc})
    ibutton.Builder
        .WithCustomId(ibutton.Id)
        .WithLabel(ibutton.Label)

let publishedButton (ibutton: IButton): ButtonBuilder =
    ibutton.Builder
        .WithCustomId(ibutton.Id)
        .WithLabel(ibutton.Label)