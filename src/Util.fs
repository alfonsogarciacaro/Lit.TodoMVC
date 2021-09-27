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
    let __withLocalStorage (storedType: Type) (mapSave: 'model -> 'stored) (mapLoad: 'stored -> 'model) (program: Program<unit, 'model, 'msg, 'view>) =
        let encoder = Encode.Auto.generateBoxedEncoderCached(storedType)
        let decoder = Decode.Auto.generateBoxedDecoderCached(storedType)

        let mapInit (init: unit -> 'model * Cmd<'msg>) =
            fun () ->
                let defaultModel, cmd = init()
                localStorage.getItem(STORAGE_KEY)
                |> function null -> Error "No storage" | json -> Ok json
                |> Result.bind (Decode.fromString decoder)
                |> function
                    | Ok stored -> stored :?> 'stored |> mapLoad, cmd
                    | Error err ->
                        JS.console.warn("Cannot get initial state from localStorage", err)
                        defaultModel, cmd

        let mapUpdate (update: 'msg -> 'model -> 'model * Cmd<'msg>) =
            fun msg model ->
                assert false
                let newModel, cmd = update msg model
                localStorage.setItem(STORAGE_KEY, mapSave newModel |> encoder |> Encode.toString 0)
                newModel, cmd

        Program.map mapInit mapUpdate id id id program

    /// Load/save Elmish state to browser's localStorage. Uses Thoth.Json for serialization.
    ///
    /// Better used in apps that don't update the Elmish model on every key stroke to prevent hitting localStorage too many times.
    let inline withLocalStorage (program: Program<unit, 'model, 'msg, 'view>) =
        __withLocalStorage typeof<'model> id id program

    /// Load/save mapped Elmish state to browser's localStorage. Uses Thoth.Json for serialization.
    ///
    /// Better used in apps that don't update the Elmish model on every key stroke to prevent hitting localStorage too many times.
    let inline withLocalStorageMap (mapSave: 'model -> 'stored) (mapLoad: 'stored -> 'model) (program: Program<unit, 'model, 'msg, 'view>) =
        __withLocalStorage typeof<'stored> mapSave mapLoad program
