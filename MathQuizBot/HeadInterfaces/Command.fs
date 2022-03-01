module MathQuizBot.Command

open Discord
open Discord.WebSocket
open System.Threading.Tasks
open System

type ICommand =
    abstract Name: string
    abstract Description: string
    abstract Options: SlashCommandOptionBuilder[]
    abstract handle: SocketSlashCommand -> unit

let genCommand (command: ICommand) = 
    let builder = SlashCommandBuilder()
                    .WithName(command.Name)
                    .WithDescription(command.Description)
    (if command.Options = null then builder else builder.AddOptions(command.Options)).Build()

let publishCmdGuild (client: DiscordSocketClient) (guildIds: uint64[]) (commands: ICommand[]): System.Func<Task> = System.Func<_>(fun () -> task {
    let guilds = [|for id in guildIds do client.GetGuild id|]
    for command in commands do
        genCommand command
        |> fun cmd -> for guild in guilds do 
                        guild.CreateApplicationCommandAsync cmd
                        |> Async.AwaitTask |> ignore
        client.add_SlashCommandExecuted (fun it -> task{if it.Data.Name = command.Name then command.handle it })
    })

let publishCmdGlobal (client: DiscordSocketClient) (commands: ICommand[]): System.Func<Task> = System.Func<Task>(fun () -> task {
    for command in commands do
        genCommand command
        |> client.CreateGlobalApplicationCommandAsync
        |> Async.AwaitTask |> ignore
        client.add_SlashCommandExecuted (fun it -> task{if it.Data.Name = command.Name then command.handle it })
    })


