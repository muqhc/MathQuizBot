module MathQuizBot.Command

open Discord
open Discord.WebSocket
open System.Threading.Tasks

type ICommand =
    abstract Name: string
    abstract Description: string
    abstract handle: SocketSlashCommand -> Task

let registerGuild (guildId: uint64) (command: ICommand) (client: DiscordSocketClient): System.Func<Task> = System.Func<_>(fun () -> task {
    let guild = client.GetGuild(guildId)
    let builder = SlashCommandBuilder()
    builder.Name <- command.Name
    builder.Description <- command.Description
    client.add_SlashCommandExecuted (fun it -> if it.Data.Name = command.Name then command.handle it else task{printf ""})
    builder.Build() |> guild.CreateApplicationCommandAsync |> Async.AwaitTask |> ignore
    })

let registerGlobal (command: ICommand) (client: DiscordSocketClient): System.Func<Task> = System.Func<_>(fun () -> task {
    let builder = SlashCommandBuilder()
    builder.Name <- command.Name
    builder.Description <- command.Description
    client.add_SlashCommandExecuted (fun it -> if it.Data.Name = command.Name then command.handle it else task{printf ""})
    builder.Build() |> client.CreateGlobalApplicationCommandAsync |> Async.AwaitTask |> ignore
    })


