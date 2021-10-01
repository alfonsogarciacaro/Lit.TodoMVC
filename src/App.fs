module Lit.TodoMVC.App

open Elmish
open Lit
open Util

Todos.register()
Virtualizer.register()

[<RequireQualifiedAccess>]
type Tab =
    | Todos
    | Virtualizer

[<LitElement("my-app")>]
let MyApp() =
    let _ = LitElement.init(fun cfg ->
        cfg.useShadowDom <- false
    )

    let activeTab, setActiveTab = Hook.useState(Tab.Todos)

    let renderTab tab text =
        html $"""<li class={if tab = activeTab then "is-active" else ""}>
            <a @click={Ev(fun ev ->
                ev.preventDefault()
                setActiveTab tab
            )}>
                {text}
            </a>
        </li>"""

    let content =
        match activeTab with
        | Tab.Todos -> html $"""<todo-app local-storage></todo-app>"""
        | Tab.Virtualizer -> html $"""<virtualizer-app></virtualizer-app>"""

    html $"""
        <div class="tabs">
          <ul>
            {renderTab Tab.Todos "Todos"}
            {renderTab Tab.Virtualizer "Virtual Scroll"}
          </ul>
        </div>
        {content}
    """
