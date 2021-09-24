module TodoTest

open Elmish
open Lit
open Expect
open Expect.Dom
open WebTestRunner

[<HookComponent>]
let Counter () =
    let value, setValue = Hook.useState 5

    html
        $"""
      <div>
        <p>Value: {value}</p>
        <button @click={Ev(fun _ -> value + 1 |> setValue)}>Increment</button>
        <button @click={Ev(fun _ -> value - 1 |> setValue)}>Decrement</button>
      </div>
    """

describe "Todo" <| fun () ->
    it "counter renders" <| fun () -> promise {
        use! el = Counter() |> render
        return! el.El |> Expect.matchHtmlSnapshot "counter"
    }
