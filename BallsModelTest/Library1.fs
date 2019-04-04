module Test.``Test 1``

open BallsModel

open MbUnit.Framework
open FsUnit.MbUnit

[<Test>]
let ``tt`` () =
  1 |> should equal 1