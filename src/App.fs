module Lit.TodoMVC.App

open Elmish
open Lit
open Components
open Util

let private hmr = HMR.createToken()

let init() =
    let todos = [ Todo.New("Learn F#"); Todo.New("Have fun with Lit!") ]
    { Todos = todos; Edit = None; Sort = false }, Cmd.none

let update msg model =
    match msg with
    | ToggleSort ->
        { model with Sort = not model.Sort }, Cmd.none

    | AddNewTodo description ->
        let todo = Todo.New(description)
        { model with Todos = todo::model.Todos ; Edit = None }, Cmd.none

    | DeleteTodo guid ->
        let todos = model.Todos |> List.filter (fun t -> t.Id <> guid)
        { model with Todos = todos }, Cmd.none

    | ToggleCompleted guid ->
        let todos = model.Todos |> List.map (fun t ->
            if t.Id = guid then { t with Completed = not t.Completed } else t)
        { model with Todos = todos }, Cmd.none

    | StartEdit edit  ->
        { model with Edit = Some edit }, Cmd.none

    | FinishEdit None ->
        { model with Edit = None }, Cmd.none

    | FinishEdit(Some t1) ->
        let todos = model.Todos |> List.map (fun t2 ->
            if t1.Id = t2.Id then t1 else t2)
        { model with Todos = todos; Edit = None }, Cmd.none

[<LitElement("todo-app")>]
let TodoApp() =
    let _, props = LitElement.init(fun cfg ->
        // We need a LitElement to use @lit-labs/motion/animate
        // But we don't use Shadow DOM so we can use global CSS rules
        cfg.useShadowDom <- false
        cfg.props <-
            {|
                localStorage = Prop.Of(false, attribute="local-storage")
            |}
    )

    Hook.useHmr(hmr)

    let encode, decode =
        Hook.useMemo(generateThothCoders)

    let model, dispatch =
        Hook.useElmishWithLocalStorage(
            init, update,
            encode, decode, "todo-app",
            disableStorage = not props.localStorage.Value)

    let todos =
        if not model.Sort then model.Todos
        else model.Todos |> List.sortBy (fun t -> t.Description.ToLower())

    html $"""
      <div style="margin: 0 auto; max-width: 800px; padding: 20px;">
        <div class="title">
            <slot name="title"></slot>
        </div>

        {NewTodoEl dispatch}

        <label class="checkbox">
          <input type="checkbox"
            ?checked={model.Sort}
            @change={Ev(fun _ -> dispatch ToggleSort)}>
          Sort by description
        </label>

        {todos |> Lit.mapUnique
            (fun t -> string t.Id)
            (TodoEl dispatch model.Edit)}
      </div>
    """

// Dummy trigger so the module can be imported if not loaded directly from a <script> tag
// (e.g. from the tests) and the component registered
let register() = ()