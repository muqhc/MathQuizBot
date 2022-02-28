module MathQuizBot.MathQuiz

open System

type Op = 
    | Plus = '+'
    | Minus = '-'

type Calc =
    struct
        val op: Op
        val backValue: int

        new(op: Op,value: int) = {
            op=op
            backValue=value
            }
        
        member this.calcLet = 
            match this.op with
            | Op.Plus -> (+) this.backValue
            | Op.Minus -> (+) (-this.backValue)
        
        member this.calcText = (this.op |> unbox<char>).ToString() + this.backValue.ToString()
    end

let (|Calc|) (calc: Calc) = (calc.calcLet, calc.calcText)

[<AbstractClass>]
type Quiz() = 
    abstract Seqs: Calc[]
    abstract StartNum: int

    member this.calculate = (this.StartNum,this.Seqs) ||> Array.fold (fun (state: int) (Calc(op,sign)) -> op state )

    override this.ToString() =
        this.Seqs
        |> Array.map (function Calc(_,sign) -> sign)
        |> String.concat ""
        |> (+) (this.StartNum.ToString())

let genQuiz difficulty =
    let scale = match Environment.GetEnvironmentVariable "DIFFICULTY_SCALE" with
                | null -> 6
                | it -> Convert.ToInt32 it
    let random = Random()
    let startNum = random.Next(1,difficulty*scale)
    let calcs = [|
                for i in 1..difficulty do
                    Calc(
                        Op.GetValues()[Random.Shared.Next(Enum.GetValues(typedefof<Op>).Length)],
                        Random.Shared.Next(1,difficulty*scale)
                        )
                |]
    { new Quiz() with 
        member this.StartNum = startNum
        member this.Seqs = calcs
    }
