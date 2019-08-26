module ReactHooksSample.UseState

open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props
open Browser.Types

let useInputValue  initialValue =
    let value = Hooks.useState initialValue
    let onChange (e : Event) =
        let targetValue : string = e.target?value
        value.update (targetValue)

    let resetValue() = value.update (System.String.Empty)

    value, onChange, resetValue

type FormProps = { OnSubmit : string -> unit }

let formComponent =
    FunctionComponent.Of(fun (props: FormProps) ->
        let (value, onChange, resetValue) = useInputValue ""

        let onSubmit (ev : Event) =
            ev.preventDefault()
            props.OnSubmit(value.current)
            resetValue()

        form [ OnSubmit onSubmit; ] [
            input [ Value value.current; OnChange onChange; Placeholder "Enter todo"; ClassName "input" ]
        ]
    )
type Todo = { Text : string; Complete : bool }

let appComponent =
    FunctionComponent.Of(fun () ->
        let todos = Hooks.useState([])

        let toggleComplete i =
            todos.current
            |> List.mapi (fun k todo ->
                if k = i then { todo with Complete = not todo.Complete } else todo
            )
            |> todos.update

        let renderTodos =
            todos.current
            |> List.mapi (fun idx todo ->
                let style =
                    CSSProp.TextDecoration(if todo.Complete then "line-through" else "")
                    |> List.singleton

                let key = sprintf "todo_%i" idx

                div [ Key key; OnClick(fun _ -> toggleComplete idx) ] [
                    label [ ClassName "checkbox"; Style style ] [
                        input [ Type "checkbox"; Checked todo.Complete; OnChange (fun _ -> toggleComplete idx) ]
                        str todo.Text
                    ]
                ]
            )

        let onSubmit text =
            { Text = text; Complete = false }
            |> List.singleton
            |> (@) todos.current
            |> todos.update

        div [] [
            h1 [ Class "title is-4" ] [ str "Todos" ]
            ofFunction formComponent { OnSubmit = onSubmit } []
            div [ ClassName "notification" ] renderTodos
        ]
    )

