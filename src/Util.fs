module Lit.TodoMVC.Util

open Fable.Core
open Browser
open Elmish

module Util =
    let mapInit decode storageKey (init: unit -> 'model * Cmd<'msg>) =
        fun () ->
            let defaultModel, cmd = init()
            match localStorage.getItem(storageKey) with
            | null -> defaultModel, cmd
            | json ->
                try
                    let stored = decode json
                    stored, cmd
                with e ->
                    JS.console.warn($"Cannot decode localStorage '{storageKey}'", e.Message)
                    defaultModel, cmd

    let mapUpdate encode storageKey (update: 'msg -> 'model -> 'model * Cmd<'msg>) =
        fun msg model ->
            let newModel, cmd = update msg model
            localStorage.setItem(storageKey, encode newModel)
            newModel, cmd

open Util

module Program =
    /// Load/save Elmish state to browser's localStorage. Uses Thoth.Json for serialization.
    ///
    /// Better used in apps that don't update the Elmish model on every key stroke to prevent hitting localStorage too many times.
    let withLocalStorage (encode: 'model -> string) (decode: string -> 'model) (storageKey: string) (program: Program<unit, 'model, 'msg, 'view>) =
        Program.map (mapInit decode storageKey) (mapUpdate encode storageKey) id id id program

type Lit.Hook with
    static member inline useElmishWithLocalStorage(init, update, encode, decode, storageKey) =
        let init = mapInit decode storageKey init
        let update = mapUpdate encode storageKey update
        Lit.Hook.getContext().useElmish(init, update)

let inline generateThothCoders<'T>() =
    let encoder =
        let enc = Thoth.Json.Encode.Auto.generateEncoder<'T>()
        fun model -> enc model |> Thoth.Json.Encode.toString 0

    let decoder =
        let dec = Thoth.Json.Decode.Auto.generateDecoder<'T>()
        fun json -> Thoth.Json.Decode.unsafeFromString dec json
    encoder, decoder

