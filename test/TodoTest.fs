module TodoTest

open Expect
open Expect.Dom
open Expect.Elmish
open WebTestRunner
open Lit.TodoMVC

App.register()

describe "Todo" <| fun () ->
    it "TodoApp Elmish" <| fun () -> promise {
        use app =
            Program.mkHidden App.init App.update
            |> Program.runTest

        AddNewTodo "Elmish test" |> app.Dispatch
        app.Model.Todos
        |> Expect.find "new todo" (fun t -> t.Description = "Elmish test")
        |> Expect.isFalse "completed" (fun t -> t.Completed)
    }

    it "TodoApp UI" <| fun () -> promise {
        use! container = render_html $"<todo-app></todo-app>"

        let el = container.El
        let newTodo = "Elmish test"
        el |> Expect.error "new todo before adding" (fun el -> el.getByText(newTodo))

        // We can get the form elements using the aria labels, same way as screen readers will do
        el.getTextInput("New todo description").value <- newTodo
        el.getButton("Add new todo").click()

        // Await for the element to update
        do! elementUpdated el

        el |> Expect.success "new todo found" (fun el -> el.getByText(newTodo))
        do! el |> Expect.matchHtmlSnapshot "new-todo"
    }
