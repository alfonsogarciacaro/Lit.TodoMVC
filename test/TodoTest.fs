module TodoTest

open Elmish
open Expect
open Expect.Dom
open Expect.Elmish
open WebTestRunner
open Lit.TodoMVC.App

describe "Todo" <| fun () ->
    it "New todo" <| fun () -> promise {
        // Initialize the Elmish app with Program.runTest, this will return a container
        // that's also an observable reporting model updates
        use! container =
            Program.mkProgram init update view
            |> Program.runTest

        // Access the element from the container and do snapshot testing
        let el = container.El
        do! el |> Expect.matchHtmlSnapshot "before new todo"

        // We can get the form elements using the aria labels, same way as screen readers will do
        el.getTextInput("New todo description").value <- "Elmish test"
        el.getButton("Add new todo").click()

        // Get the updated model and confirm it contains the new todo uncompleted
        let! model = container.Await()
        let newTodo = model.Todos |> List.find (fun t -> t.Description = "Elmish test")
        newTodo |> Expect.isFalse "new todo complete" (fun t -> t.Completed)

        // Await for the element to update
        do! elementUpdated el
        do! el |> Expect.matchHtmlSnapshot "after new todo"
    }
