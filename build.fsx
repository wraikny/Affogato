#!dotnet fsi

#r "netstandard"
#r "nuget: MSBuild.StructuredLogger"
#r "nuget: Fake.Core"
#r "nuget: Fake.Core.Target"
#r "nuget: Fake.Core.ReleaseNotes"
#r "nuget: Fake.IO.FileSystem"
#r "nuget: Fake.DotNet.Cli"
#r "nuget: Fake.DotNet.AssemblyInfoFile"
#r "nuget: Fake.DotNet.Testing.Expecto"

#load "./src/Affogato/VectorExtGenerator.fs"

open Fake.Core
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.DotNet
open Fake.DotNet.Testing

// Boilerplate
System.Environment.GetCommandLineArgs()
|> Array.skip 2 // skip fsi.exe; build.fsx
|> Array.toList
|> Context.FakeExecutionContext.Create false __SOURCE_FILE__
|> Context.RuntimeContext.Fake
|> Context.setExecutionContext

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

  !! "src/**/*.*proj"
  ++ "tests/**/*.*proj"
  |> Seq.iter (DotNet.build (fun p -> { p with Configuration = DotNet.BuildConfiguration.Release }))

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

Target.runOrDefault "All"
