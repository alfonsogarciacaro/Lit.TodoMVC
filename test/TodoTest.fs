module TodoTest

open Fable.Core
open Expect
open Expect.Dom
open WebTestRunner
open Lit.TodoMVC

App.register()

type Browser.Types.HTMLElement with
    [<Emit("$0")>]
    member _.Props<'Props>(): 'Props = jsNative

module Expect =
    let some (x: 'T option): 'T =
        match x with
        | Some x -> x
        | None -> AssertionError.Throw("be some")

    let map (f: 'T1 -> 'T2) (x: 'T1): 'T2 = f x

describe "Todo" <| fun () ->
    it "New todo" <| fun () -> promise {
        use! container = render_html $"<todo-app></todo-app>"

        // Access the element from the container
        let el = container.El

        // We can get the form elements using the aria labels, same way as screen readers will do
        el.shadowRoot.getTextInput("New todo description").value <- "Elmish test"
        el.shadowRoot.getButton("Add new todo").click()

        // Await for the element to update
        do! elementUpdated el
        let props = el.Props<App.Props>()
        let newTodo =
            props.model.Value
            |> Expect.some
            |> Expect.map (fun m -> m.Todos)
            |> Expect.find "new todo" (fun t -> t.Description = "Elmish test")
        newTodo |> Expect.isFalse "new todo complete" (fun t -> t.Completed)

        do! el |> Expect.matchHtmlSnapshot "new-todo"
    }
