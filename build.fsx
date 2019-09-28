#load ".fake/build.fsx/intellisense.fsx"
#load "./src/Affogato/VectorExtGenerator.fs"
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators


Target.create "Generate" (fun _ ->
  File.writeNew "./src/Affogato/VectorExt.fs" VectorExtGenerator.source
  Trace.log "Generate finished"
)

Target.create "Clean" (fun _ ->
  !! "src/**/bin"
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

Target.create "All" ignore

// let inline ( *==> ) deps target =
//   for dep in deps do dep ==> target |> ignore
//   target

"Generate"
  ==> "Build"

"Clean"
  ==> "Build"
  ==> "All"

Target.runOrDefault "All"
