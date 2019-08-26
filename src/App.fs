module ReactHooksSample.App

open Fable.React
open Fable.React.Props
open ReactHooksSample


let app() =
    let page = Hooks.useState "useState()"

    let tabs =
        [ "useState()"; "useReducer()"; "useEffect()" ]
        |> List.map (fun tab ->
            li [ Class(if page.current = tab then "is-active" else ""); Key tab ] [
                a [ Href "#"; OnClick(fun _ -> page.update tab) ] [ str tab ]
            ]
        )

    let activePage =
        match page.current with
        | "useState()" -> ofFunction UseState.appComponent () []
        | "useReducer()" -> ofFunction UseReducer.reducerComponent UseReducer.intialState []
        | "useEffect()" -> ofFunction UseEffect.effectComponent () []
        | _ -> str "other page"

    div [] [
        div [ ClassName "tabs" ] [
            ul [] tabs
        ]
        activePage
    ]

mountById "app" (ofFunction app () [])
