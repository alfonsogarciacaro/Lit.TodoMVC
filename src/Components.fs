module Lit.TodoMVC.Components

open Browser.Types
open Lit

module private Util =
    let hmr = HMR.createToken()

    [<Fable.Core.ImportMember("@lit-labs/motion")>]
    let animate() = ()

    let onEnterOrEscape onEnter onEscape (ev: Event) =
        let ev = ev :?> KeyboardEvent
        match ev.key with
        | "Enter" -> onEnter ev
        | "Escape" -> onEscape ev
        | _ -> ()

    type Option<'T> with
        member this.Iter(f) =
            Option.iter f this

open Util

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
                <input class="input is-medium"
                    type="text"
                    aria-label="New todo description"
                    {Lit.ref inputRef}
                    @keyup={Ev(onEnterOrEscape addNewTodo ignore)}>
            </div>
            <div class="control">
                <button class="button is-primary is-medium"
                    aria-label="Add new todo"
                    @click={Ev addNewTodo}>
                    <i role="img" class="bi-plus-lg"></i>
                </button>
            </div>
        </div>
    """

[<HookComponent>]
let TodoEl dispatch (edit: Todo option) (todo: Todo) =
    Hook.useHmr(hmr)

    let isEditing =
        match edit with
        | Some edit -> edit.Id = todo.Id
        | None -> false

    let hasFocus = Hook.useRef(false)
    let inputRef = Hook.useRef<HTMLInputElement>()

    Hook.useEffectOnChange(isEditing, function
        | true when not hasFocus.Value ->
            inputRef.Value.Iter(fun i -> i.select())
        | _ -> ())

    let transition =
        Hook.useTransition(
            ms = 500,
            onComplete = (fun isIn ->
                if not isIn then DeleteTodo todo.Id |> dispatch),
            cssBefore = inline_css """.{
                opacity: 0;
                transform: scale(2);
            }""",
            cssAfter = inline_css """.{
                opacity: 0;
                transform: scale(0.1);
            }""")

    let style = transition.css + inline_css """.{
        border: 2px solid lightgray;
        border-radius: 10px;
        margin: 5px 0;
    }"""

    if isEditing then
        let applyEdit _ =
            inputRef.Value.Iter(fun input ->
                { todo with Description = input.value.Trim() }
                |> Some
                |> FinishEdit
                |> dispatch)

        let cancelEdit _ =
            FinishEdit None |> dispatch

        html $"""
            <div class="columns" style={style}>
                <div class="column is-10">
                    <input class="input"
                        type="text"
                        aria-label="Edit todo"
                        {Lit.ref inputRef}
                        value={todo.Description}
                        @keyup={Ev(onEnterOrEscape applyEdit cancelEdit)}
                        @blur={Ev cancelEdit}>
                </div>
                <div class="column is-2">
                    <button class="button is-primary" aria-label="Save edit"
                        @click={Ev applyEdit}>
                        <i role="img" class="bi-save"></i>
                    </button>
                </div>
            </div>"""
    else
        html $"""
            <div {animate()} class="columns" style={style}>
                <div class="column is-9">
                    <p class="subtitle"
                        style="cursor: pointer; user-select: none"
                        @dblclick={Ev(fun _ -> StartEdit todo |> dispatch)}>
                        {todo.Description}
                    </p>
                </div>
                <div class="column is-3">
                    <button class={Lit.classes ["button", true; "is-success", todo.Completed]}
                        aria-label={if todo.Completed then "Mark uncompleted" else "Mark completed"}
                        @click={Ev(fun _ -> ToggleCompleted todo.Id |> dispatch)}>
                        <i role="img" class="bi-check-lg"></i>
                    </button>
                    <button class="button is-primary" aria-label="Edit"
                        @click={Ev(fun _ -> StartEdit todo |> dispatch)}>
                        <i role="img" class="bi-pencil"></i>
                    </button>
                    <button class="button is-danger" aria-label="Delete"
                        @click={Ev(fun _ -> transition.triggerLeave())}>
                        <i role="img" class="bi-trash"></i>
                    </button>
                </div>
            </div>
        """
