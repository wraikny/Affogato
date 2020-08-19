#r "paket:
source https://api.nuget.org/v3/index.json
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.Core.Target
nuget Fake.Core.ReleaseNotes
nuget Fake.DotNet.AssemblyInfoFile
nuget Fake.DotNet.Testing.Expecto
nuget FAKE.IO.Zip //"

#load ".fake/build.fsx/intellisense.fsx"
#r "netstandard"

#load "./src/Affogato/VectorExtGenerator.fs"

open Fake.Core
open Fake.DotNet
open Fake.DotNet.Testing
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

let release = ReleaseNotes.load "RELEASE_NOTES.md"

release.AssemblyVersion |> Trace.tracefn "Auto version: %s"

Target.create "AssemblyInfo" (fun _ ->
  let info = [
    AssemblyInfo.Version release.AssemblyVersion
    AssemblyInfo.FileVersion release.AssemblyVersion
    AssemblyInfo.InformationalVersion release.NugetVersion
  ]
  !! "src/**/*.fsproj"
  |> Seq.iter(fun path ->
    let dir = System.IO.Path.GetDirectoryName(path)
    AssemblyInfoFile.createFSharp (sprintf "%s/AssemblyInfo.fs" dir) info)
)


Target.create "Generate" (fun _ ->
  File.writeNew "./src/Affogato/VectorExt.fs" VectorExtGenerator.source
  Trace.log "Generate finished"
)


Target.create "Test" (fun _ ->
  let testProjects = [
    "Affogato.Test"
  ]

  [ for x in testProjects ->
      sprintf "tests/%s/bin/Release/**/%s.dll" x x
  ] |> function
  | [] ->
    printfn "There is no test project"
  | x::xs ->
    Seq.fold (++) (!! x) xs
    |> Expecto.run id
)

let dotnet cmd arg = DotNet.exec id cmd arg |> ignore

Target.create "Tool" (fun _ ->
  dotnet "tool" "update paket"
  dotnet "tool" "update fake-cli"
)

Target.create "Clean" (fun _ ->
  !! "output"
  ++ "src/**/obj"
  ++ "tests/**/bin"
  ++ "tests/**/obj"
  |> Shell.cleanDirs
  Trace.log "Clean finished"
)

Target.create "Build" (fun _ ->
  !! "src/**/*.*proj"
  ++ "tests/**/*.*proj"
  |> Seq.iter (DotNet.build id)
  Trace.log "Build finished"
)

Target.create "Zip" (fun _ ->
  Shell.cleanDir @"./dist"

  let files =
    !! "output/Release/netstandard2.0/*.dll"
    ++ "output/Release/netstandard2.0/*.pdb"
  files
  |> Shell.copy "./dist"

  let zipFileName =
    sprintf @"./dist/Affogato.%s.zip" release.AssemblyVersion

  files
  |> Zip.zip "./" zipFileName
)

Target.create "Pack" (fun _ ->
  !! "src/**/*.*proj"
  |> Seq.iter (DotNet.pack (fun opt ->
    { opt with
        Configuration = DotNet.BuildConfiguration.Release
        OutputPath = Some "output/packages/"
        MSBuildParams = {
          opt.MSBuildParams with
            Properties = [
              "PackageVersion", release.NugetVersion
            ] @ opt.MSBuildParams.Properties
        }
    }
  ))
)

Target.create "All" ignore

// let inline ( *==> ) deps target =
//   for dep in deps do dep ==> target |> ignore
//   target

"Clean"
  ==> "AssemblyInfo"
  ==> "Generate"
  ==> "Build"
  ==> "All"

if not <| System.IO.Directory.Exists(@"./output/Release") then
  "Build" ==> "Pack" |> ignore
  "Build" ==> "Zip" |> ignore

Target.runOrDefault "All"
