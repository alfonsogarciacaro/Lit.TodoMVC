module Lit.TodoMVC.App

open Elmish
open Lit
open Components
open Util

let private hmr = HMR.createToken()

module Styles =
    // By extracting the styles we make them reusable by other web components if needed.
    // Note these are promises so they need to be awaited (there's also `importStyleSheetSync`).
    let bulma = LitElement.importStyleSheet("bulma/css/bulma.min.css")
    let bootsrapIcons = LitElement.importStyleSheet("bootstrap-icons/font/bootstrap-icons.css")

    // We could use external stylesheets, but in this case is problematic because the font references won't be imported
    // let bulma = LitElement.importExternalStyleSheet("https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.4/css/bulma.min.css")
    // let bootsrapIcons = LitElement.importExternalStyleSheet("https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/bootstrap-icons.css")

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
    let _ = LitElement.initAsync(fun cfg -> promise {
        let! bulma = Styles.bulma
        let! bootsrapIcons = Styles.bootsrapIcons
        cfg.styles <- [
            bulma
            bootsrapIcons
        ]
    })

    Hook.useHmr(hmr)
    let encode, decode = Hook.useMemo(generateThothCoders)
    let model, dispatch = Hook.useElmishWithLocalStorage(init, update, encode, decode, "todo-app")

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
