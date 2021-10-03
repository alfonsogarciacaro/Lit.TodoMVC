/* @web/test-runner snapshot v1 */
export const snapshots = {};

snapshots["new-todo"] = 
`<todo-app>
    <div style="margin: 0 auto; max-width: 800px; padding: 20px;">
    <div class="title">
    <slot name="title"></slot>
    </div>
    <div class="field has-addons">
    <div class="control is-expanded">
    <input class="input is-medium" type="text" aria-label="New todo description">
    </div>
    <div class="control">
    <button class="button is-primary is-medium" aria-label="Add new todo">
    <i role="img" class="bi-plus-lg"></i>
    </button>
    </div>
    </div>
    <label class="checkbox">
    <input type="checkbox">
    Sort by description
    </label>
        <div class="columns" style="transition-duration: 500ms;
                opacity: 0;
                transform: scale(2);
        border: 2px solid lightgray;
        border-radius: 10px;
        margin: 5px 0;
    ">
        <div class="column is-9">
        <p class="subtitle" style="cursor: pointer; user-select: none">
        Elmish test
        </p>
        </div>
        <div class="column is-3">
        <button class="button" aria-label="Mark completed">
        <i role="img" class="bi-check-lg"></i>
        </button>
        <button class="button is-primary" aria-label="Edit">
        <i role="img" class="bi-pencil"></i>
        </button>
        <button class="button is-danger" aria-label="Delete">
        <i role="img" class="bi-trash"></i>
        </button>
        </div>
        </div>
        <div class="columns" style="transition-duration: 500ms;
                opacity: 0;
                transform: scale(2);
        border: 2px solid lightgray;
        border-radius: 10px;
        margin: 5px 0;
    ">
        <div class="column is-9">
        <p class="subtitle" style="cursor: pointer; user-select: none">
        Learn F#
        </p>
        </div>
        <div class="column is-3">
        <button class="button" aria-label="Mark completed">
        <i role="img" class="bi-check-lg"></i>
        </button>
        <button class="button is-primary" aria-label="Edit">
        <i role="img" class="bi-pencil"></i>
        </button>
        <button class="button is-danger" aria-label="Delete">
        <i role="img" class="bi-trash"></i>
        </button>
        </div>
        </div>
        <div class="columns" style="transition-duration: 500ms;
                opacity: 0;
                transform: scale(2);
        border: 2px solid lightgray;
        border-radius: 10px;
        margin: 5px 0;
    ">
        <div class="column is-9">
        <p class="subtitle" style="cursor: pointer; user-select: none">
        Have fun with Lit!
        </p>
        </div>
        <div class="column is-3">
        <button class="button" aria-label="Mark completed">
        <i role="img" class="bi-check-lg"></i>
        </button>
        <button class="button is-primary" aria-label="Edit">
        <i role="img" class="bi-pencil"></i>
        </button>
        <button class="button is-danger" aria-label="Delete">
        <i role="img" class="bi-trash"></i>
        </button>
        </div>
        </div>
    </div>
    </todo-app>`;
/* end snapshot new-todo */

