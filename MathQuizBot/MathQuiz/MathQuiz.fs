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
        
        member this.opLet = 
            match this.op with
            | Op.Plus -> (+) this.backValue
            | Op.Minus -> (-) this.backValue
    end

let (|Calc|) (calc: Calc) = (calc.opLet, (calc.op |> unbox) + calc.backValue.ToString())

[<AbstractClass>]
type Quiz() = 
    abstract Seqs: Calc[]
    abstract StartNum: int

    member this.calculate = (this.StartNum,this.Seqs) ||> Array.fold (fun (state: int) (Calc(op,_)) -> op state )

    override this.ToString() =
        this.Seqs
        |> Array.map (function Calc(_,sign) -> sign)
        |> String.concat ""
        |> (+) (this.StartNum.ToString())

let genQuiz difficulty = { new Quiz() with 
        member this.StartNum = Random.Shared.Next(1,difficulty*5)
        member this.Seqs = [|
            for i in 1..difficulty do
                Calc(
                    Op.GetValues()[Random.Shared.Next(Enum.GetValues(typedefof<Op>).Length)],
                    Random.Shared.Next(1,difficulty*5)
                    )
            |]
    }
