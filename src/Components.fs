module Lit.TodoMVC.Components

open Browser.Types
open Lit

let private hmr = HMR.createToken()

[<HookComponent>]
let NewTodoEl dispatch =
    Hook.useHmr(hmr)
    let inputRef = Hook.useRef<HTMLInputElement>()
    let addNewTodo _ =
        match inputRef.Value with
        | None -> ()
        | Some input ->
            let value = input.value
            input.value <- ""
            match value.Trim() with
            | "" -> ()
            | v -> v |> AddNewTodo |> dispatch

    html $"""
        <div class="field has-addons">
            <div class="control is-expanded">
                <input {Lit.ref inputRef}
                    type="text"
                    class="input is-medium"
                    aria-label="New todo description"
                    @keyup={Ev(fun ev ->
                        let key = (ev :?> KeyboardEvent).key
                        if key = "Enter" then
                            addNewTodo ev)} >
            </div>
            <div class="control">
                <button class="button is-primary is-medium" aria-label="Add new todo"
                    @click={Ev addNewTodo}>
                    <i class="fa fa-plus"></i>
                </button>
            </div>
        </div>
    """

[<HookComponent>]
let TodoEl dispatch (edit: Todo option) (todo: Todo) =
    Hook.useHmr(hmr)
    let inputRef = Hook.useRef<HTMLInputElement>()
    let style = inline_css """.{
        border: 2px solid lightgray;
        border-radius: 10px;
        margin: 5px 0;
    }"""
    match edit with
    | Some edit when edit.Id = todo.Id ->
        html $"""
            <div class="columns" style={style}>
                <div class="column is-10">
                    <input {Lit.ref inputRef} type="text" class="input" aria-label="Edit todo" value={todo.Description}>
                </div>
                <div class="column is-2">
                    <button class="button is-primary" aria-label="Save edit"
                        @click={Ev(fun _ ->
                            inputRef.Value
                            |> Option.iter (fun input ->
                                { todo with Description = input.value.Trim() }
                                |> Some
                                |> FinishEdit
                                |> dispatch))}>
                        <i class="fa fa-save"></i>
                    </button>
                </div>
            </div>"""
    | _ ->
        html $"""
            <div class="columns" style={style}>
                <div class="column is-9">
                    <p class="subtitle"
                        style="cursor: pointer; user-select: none;"
                        @dblclick={Ev(fun _ -> StartEdit todo |> dispatch)}>
                        {todo.Description}
                    </p>
                </div>
                <div class="column is-3">
                    <!-- TODO: Provide aria besides color to indicate if item is complete or not -->
                    <button class={Lit.classes ["button", true; "is-success", todo.Completed]} aria-label="Check"
                        @click={Ev(fun _ -> ToggleCompleted todo.Id |> dispatch)}>
                        <i class="fa fa-check"></i>
                    </button>
                    <button class="button is-primary" aria-label="Edit"
                        @click={Ev(fun _ -> StartEdit todo |> dispatch)}>
                        <i class="fa fa-edit"></i>
                    </button>
                    <button class="button is-danger" aria-label="Delete"
                        @click={Ev(fun _ -> DeleteTodo todo.Id |> dispatch)}>
                        <i class="fa fa-times"></i>
                    </button>
                </div>
            </div>
        """
