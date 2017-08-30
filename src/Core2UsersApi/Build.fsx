#r @"./packages/FAKE.Core/tools/FakeLib.dll"

open Fake
open Fake.Core
open Fake.Core.TargetOperators
open Fake.Core.Trace

let rootPublishDirectory = getBuildParamOrDefault "publishDirectory"  @"C:\CompiledSource"
let buildMode = getBuildParamOrDefault "buildMode" "Debug"

let mutable projectName = ""
let mutable publishDirectory = rootPublishDirectory @@ projectName
let mutable solutionFilePresent = true

Target.Create "Set Solution Name" (fun _ ->
              
    let findSolutionFile = TryFindFirstMatchingFile "*.sln" currentDirectory
    
    if findSolutionFile.IsSome then
        
        let solutionFileHelper = FileSystemHelper.fileInfo(findSolutionFile.Value)
            
        projectName <- solutionFileHelper.Name.Replace(solutionFileHelper.Extension, "")
        publishDirectory <- rootPublishDirectory @@  projectName
        
        trace ("PublishDirectory: " + publishDirectory)
        trace ("Project Name has been set to: " + projectName)
    else
        solutionFilePresent <- false

)

Target.Create "Build DNX Project"(fun _ ->
    
    DotNetCli.Build
        (fun p -> 
            { p with 
                Configuration = buildMode;
                Project = projectName
            })
    
    |> ignore
)

Target.Create "Package DNX" (fun _ ->

    DotNetCli.Publish
        (fun p -> 
            { p with 
                Configuration = buildMode;
                Project = projectName;
                Output = publishDirectory
            })
    
    |> ignore
    
)

"Set Solution Name"
 ==> "Build DNX Project"
 ==> "Package DNX"

Target.RunOrDefault  "Package DNX"