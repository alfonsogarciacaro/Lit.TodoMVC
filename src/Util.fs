namespace Lit.TodoMVC

module Program =
    open System
    open Fable.Core
    open Browser
    open Elmish
    open Thoth.Json

    let private STORAGE_KEY = "elmish-storage"

    // We need to inline so Fable can resolve the 'model type at compile time but To avoid inlining too much code,
    // a common pattern is to have a "hidden" (cannot be private) function that receives the resolved System.Type
    let __withLocalStorage (modelType: Type) (program: Program<unit, 'model, 'msg, 'view>) =
        let encoder = Encode.Auto.generateBoxedEncoderCached(modelType)
        let decoder = Decode.Auto.generateBoxedDecoderCached(modelType)

        let mapInit (init: unit -> 'model * Cmd<'msg>) =
            fun () ->
                let defaultModel, cmd = init()
                localStorage.getItem(STORAGE_KEY)
                |> function null -> Error "No storage" | json -> Ok json
                |> Result.bind (Decode.fromString decoder)
                |> function
                    | Ok model -> model :?> 'model, cmd
                    | Error err ->
                        JS.console.warn("Cannot get initial state from localStorage", err)
                        defaultModel, cmd

        let mapUpdate (update: 'msg -> 'model -> 'model * Cmd<'msg>) =
            fun msg model ->
                assert false
                let newModel, cmd = update msg model
                localStorage.setItem(STORAGE_KEY, encoder newModel |> Encode.toString 0)
                newModel, cmd

        Program.map mapInit mapUpdate id id id program

    /// Simple helper to load/save Elmish state to browser's localStorage.
    ///
    /// Better used in apps that don't update the Elmish model on every key stroke to prevent hitting localStorage too many times.
    ///
    /// For more complex scenarios we likely need more control over the model encoding/decoding.
    let inline withLocalStorage (program: Program<unit, 'model, 'msg, 'view>) =
        __withLocalStorage typeof<'model> program
