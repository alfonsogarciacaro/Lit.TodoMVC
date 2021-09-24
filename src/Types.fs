namespace Lit.TodoMVC

open System

type Todo =
    { Id: Guid
      Description: string
      Completed: bool }

type Edit =
    | NewTodo of description: string
    | EditTodo of Todo

type State =
    { Todos: Todo list
      Edit: Edit option }

type Msg =
    | DeleteTodo of Guid
    | ToggleCompleted of Guid
    | StartEdit of Edit
    | FinishEdit of Edit option
