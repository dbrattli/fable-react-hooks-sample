module ReactHooksSample.UseReducer

open Fable.React
open Fable.React.Props

type Msg =
    | Increase
    | Decrease
    | Reset

type Model = { Value : int }

let intialState = { Value = 0 }

let update model msg =
    match msg with
    | Increase -> { model with Value = model.Value + 1 }
    | Decrease -> { model with Value = model.Value - 1 }
    | Reset -> intialState

let reducerComponent =
    FunctionComponent.Of(fun (initialState: Model) ->
        let model = Hooks.useReducer(update, initialState)

        div [] [
            button [ ClassName "button"; OnClick(fun _ -> model.update Increase) ] [ str "Increase" ]
            button [ ClassName "button"; OnClick(fun _ -> model.update Decrease) ] [ str "Decrease" ]
            button [ ClassName "button"; OnClick(fun _ -> model.update Reset) ] [ str "Reset" ]
            p [ ClassName "title is-2 has-text-centered" ] [ sprintf "%i" model.current.Value |> str ]
        ]
    )