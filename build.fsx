#r "paket:
    storage: none
    source https://api.nuget.org/v3/index.json
    nuget Fake.Core.Target
    nuget Fake.Core.ReleaseNotes
    nuget Fake.DotNet.AssemblyInfoFile
    nuget Fake.DotNet.Cli
    nuget Fake.DotNet.Testing.NUnit
    nuget Fake.DotNet.NuGet
    nuget Fake.DotNet.MsBuild
    nuget Fake.Tools.Git
	nuget Fake.DotNet.Paket //"

#if !FAKE

#load ".fake/build.fsx/intellisense.fsx"
#r "netstandard"
#endif

#load "./src/Affogato/VectorExtGenerator.fs"

open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators


// Information about the project to be used at NuGet and in AssemblyInfo files
let authors = ["wrakny"]

let summary = "A package of libraries for generic math types and advanced algorithms for games"

let description =
  """A package of libraries for generic math types and advanced algorithms for games"""

let license = "Apache 2.0 License"
let tags = "F# fsharp math linearalgebra game"

let release = ReleaseNotes.load "RELEASE_NOTES.md"

release.AssemblyVersion |> Trace.tracefn "Auto version: %s"

Target.create "AssemblyInfo" (fun _ ->
  let info = [
    AssemblyInfo.Description summary
    AssemblyInfo.Version release.AssemblyVersion
    AssemblyInfo.FileVersion release.AssemblyVersion
    AssemblyInfo.InformationalVersion release.NugetVersion
    AssemblyInfo.Copyright license
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

Target.create "Pack" (fun _ ->
  !! "src/**/*.*proj"
  |> Seq.iter (DotNet.pack (fun opt ->
    { opt with
        Configuration = DotNet.BuildConfiguration.Release
        OutputPath = Some "../../output/packages/"
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

Target.runOrDefault "All"
