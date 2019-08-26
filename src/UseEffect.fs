module ReactHooksSample.UseEffect

open System
open Browser.Types
open Fable.React
open Fable.React.Props
open Thoth.Json

let decodeRepoItem =
    Decode.field "name" Decode.string

let decodeResonse = Decode.array decodeRepoItem

let githubUsers =
    [ "fable-compiler"; "fsprojects"; "nojaf" ]


let loadRepos updateRepos user =
    let url = sprintf "https://api.github.com/users/%s/repos" user
    Fetch.fetch url []
    |> Promise.bind (fun res -> res.text())
    |> Promise.map (fun json -> Decode.fromString decodeResonse json)
    |> Promise.mapResult updateRepos
    |> ignore

let effectComponent =
    FunctionComponent.Of(fun () ->
        let options =
            githubUsers
            |> List.map (fun name ->
                option [ Value name; Key name ] [ str name ]
            )
            |> (@) (List.singleton (option [ Value ""; Key "empty" ] []))

        let selectedOrg = Hooks.useState ("")
        let repos = Hooks.useState (Array.empty)
        let onChange (ev : Event) = selectedOrg.update (ev.Value)

        Hooks.useEffect (fun () ->
            match System.String.IsNullOrWhiteSpace(selectedOrg.current) with
            | true -> repos.update Array.empty
            | false -> loadRepos repos.update selectedOrg.current
        , [| selectedOrg |])

        let repoListItems =
            repos.current
            |> Array.sortWith (fun a b -> String.Compare(a, b, System.StringComparison.OrdinalIgnoreCase))
            |> Array.map (fun r -> li [ Key r ] [ str r ])

        div [ ClassName "content" ] [
            div [ ClassName "select" ] [
                select [ Value selectedOrg; OnChange onChange ] options
            ]
            ul [] repoListItems
        ]
    )