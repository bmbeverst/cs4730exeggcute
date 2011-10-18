// This project type requires the F# PowerPack at http://fsharppowerpack.codeplex.com/releases
// Learn more about F# at http://fsharp.net
// Original project template by Jomo Fisher based on work of Brian McNamara, Don Syme and Matt Valerio
// This posting is provided "AS IS" with no warranties, and confers no rights.
module Exeggcute.Parser
open System
open System.IO
open Microsoft.FSharp.Text.Lexing
open Exeggcute.src

open Ast
open Lexer
open Parser


/// Evaluate an expression
let rec evalNodeList expr =
    match expr with
    | Pair (a, b)    -> (evalNode a) :: (evalNodeList b)
    | Nil(node)      -> evalNode node :: []

and evalNode node = 
    match node with
    | Node(t,a) -> (evalType t, evalArgs a)

/// Evaluate an equation
and evalScript eq =
    match eq with
    | Script expr -> evalNodeList expr

and evalType t = 
    match t with
    | TypeName (x) -> x

and evalArgs args = 
    match args with 
    | ArgList(a,b) -> (parseArg a) :: (evalArgs b)
    | LastArg(a) ->  (parseArg a) :: []

and parseArg arg = 
    match arg with
    | Name(x) -> x
    | ParenArg(x) -> parseTuple x


and parseTuple tuple = 
    match tuple with
    | Vector3(x, y, z) -> MyString ""



printfn "Calculator"

let parseMyScript script =
    let text = File.ReadAllText script in
    let text = text.Substring( 0, (text.Length - 2)) in
    Printf.printf "String to parse:\n<%s>\n" text ;
    (*let text = "a a a\na a a\n" in*)
    try
        printfn "Lexing [%s]" text
        let lexbuff = LexBuffer<char>.FromString(text)
            
        printfn "Parsing..."
        let equation = Parser.start Lexer.tokenize lexbuff
            
        printfn "Evaluating Equation..."
        let result = evalScript equation
            
        printfn "Result: %s" (result.ToString())
            
    with ex ->
        printfn "Unhandled Exception: %s" ex.Message


;;