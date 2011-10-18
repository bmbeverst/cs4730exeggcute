namespace Ast
open System
open Exeggcute.src
open Exeggcute.src.scripting.action

type Node = 
    | Node of TypeName * ArgList

and EntryList =
    | Pair of Node * EntryList
    | Nil of Node

and TypeName =
    | TypeName of CommandType

and ArgList = 
    | ArgList of Arg * ArgList
    | LastArg of Arg

and Arg = 
    | Name of MyString
    | ParenArg of Tuple


and Tuple = 
    | Vector3 of Single * Single * Single

and Script =
    | Script of EntryList