module Lit.TodoMVC.Virtualizer

open Lit
open Util

let private hmr = HMR.createToken()

let register() = ()

type Contact =
    abstract name: string
    abstract mediumText: string
    abstract color: string

[<LitElement("contact-card")>]
let ContactCard() =
    let _, props = LitElement.init(fun cfg ->
        cfg.styles <- [
            css $"""
            :host {{
                display: block;
                width: 100%%;
                margin: 0.25em 0;
                cursor: pointer;
            }}
            div, details {{
                padding: 1em;
                color: white;
                display: block;
            }}
            """
        ]
        cfg.props <-
            {|
                contact = Prop.Of<Contact option>(None, attribute="")
            |}
    )

    Hook.useHmr(hmr)
    let isOpen, setOpen = Hook.useState(false)
    match props.contact.Value with
    | None -> Lit.nothing
    | Some contact ->
        let summaryStyle =
            if isOpen then "display: block"
            else "display: none"

        html $"""
            <details style="background: {contact.color}">
                <summary @click={Ev(fun _ -> setOpen(not isOpen))}>
                    {contact.name}
                </summary>
                <p style={summaryStyle}>{contact.mediumText}</p>
            </details>
    """

[<LitElement("contact-list")>]
let ContactList() =
    let _ = LitElement.init(fun cfg ->
        cfg.styles <- [
            css $"""
            :host {{
                display: block;
                height: 100%%;
            }}
            .contact-list {{
                height: 100%%;
            }}
            """
        ]
    )

    Hook.useHmr(hmr)
    let contacts, setContacts = Hook.useState([||])
    Hook.useEffectOnce(fun () ->
        fetchJson<Contact array>("contacts.json")
        |> Promise.iter setContacts
    )

    let renderContact (contact: Contact) _ =
        html $"""<contact-card .contact={contact}></contact-card>"""

    html $"""
        <div class="contact-list">
            {LitLabs.virtualizer.scroll(contacts, renderContact, LitLabs.virtualizer.Layout1d)}
        </div>
    """
