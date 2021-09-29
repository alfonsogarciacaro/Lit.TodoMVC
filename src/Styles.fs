// By extracting the styles we make them reusable by other web components if needed.
// Note these are promises so they need to be awaited (there's also `importStyleSheetSync`).
module Styles

open Lit

let bulma = LitElement.importStyleSheet("bulma/css/bulma.min.css")
let bootsrapIcons = LitElement.importStyleSheet("bootstrap-icons/font/bootstrap-icons.css")

// We could use external stylesheets, but in this case is problematic because the font references won't be imported
// let bulma = LitElement.importExternalStyleSheet("https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.4/css/bulma.min.css")
// let bootsrapIcons = LitElement.importExternalStyleSheet("https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/bootstrap-icons.css")
