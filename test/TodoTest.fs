module TodoTest

open Elmish
open Expect
open Expect.Dom
open WebTestRunner
open Lit.TodoMVC.Entry
open TestUtil

describe "Todo" <| fun () ->
    it "New todo" <| fun () -> promise {
        let! el, obs =
            Program.mkProgram init update view
            |> Program.runTest

        let el = el.El
        // do! el |> Expect.matchHtmlSnapshot "before new todo"

        el.getTextInput("New todo description").value <- "Todo test"
        el.getButton("Add new todo").click()

        let! model = obs.Await()
        let newTodo = model.Todos |> List.find (fun t -> t.Description = "Todo test")
        newTodo |> Expect.isFalse "new todo complete" (fun t -> t.Completed)

        // do! elementUpdated el
        // do! el |> Expect.matchHtmlSnapshot "after new todo"
    }
