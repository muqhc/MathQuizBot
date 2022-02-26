module MathQuizBot.Command

open Discord
open Discord.WebSocket
open System.Threading.Tasks

type ICommand =
    abstract Name: string
    abstract Description: string
    abstract handle: SocketSlashCommand -> unit

let publishCmdGuild (client: DiscordSocketClient) (guildIds: uint64[]) (commands: ICommand[]): System.Func<Task> = System.Func<_>(fun () -> task {
    let guilds = [|for id in guildIds do client.GetGuild id|]
    for command in commands do
        SlashCommandBuilder()
            .WithName(command.Name)
            .WithDescription(command.Description)
            .Build() |> fun cmd -> for guild in guilds do guild.CreateApplicationCommandAsync cmd |> Async.AwaitTask |> ignore
        client.add_SlashCommandExecuted (fun it -> task{if it.Data.Name = command.Name then command.handle it })
    })

let publishCmdGlobal (client: DiscordSocketClient) (commands: ICommand[]): System.Func<Task> = System.Func<Task>(fun () -> task {
    for command in commands do
        SlashCommandBuilder()
            .WithName(command.Name)
            .WithDescription(command.Description)
            .Build() |> client.CreateGlobalApplicationCommandAsync |> Async.AwaitTask |> ignore
        client.add_SlashCommandExecuted (fun it -> task{if it.Data.Name = command.Name then command.handle it })
    })


