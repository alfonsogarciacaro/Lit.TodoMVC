module Lit.TodoMVC.App

open Lit

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
        | Tab.Todos ->
            Lit.ofImport(Todos.register, fun _ ->
                html $"""<todo-app local-storage="todo-app"></todo-app>""")
        | Tab.Virtualizer ->
            Lit.ofImport(Virtualizer.register, fun _ ->
                html $"<contact-list></contact-list>")

    html $"""
        <div class="tabs" style="margin-bottom: 0.5rem;">
          <ul>
            {renderTab Tab.Todos "Todos"}
            {renderTab Tab.Virtualizer "Virtual Scroll"}
          </ul>
        </div>
        {content}
    """
