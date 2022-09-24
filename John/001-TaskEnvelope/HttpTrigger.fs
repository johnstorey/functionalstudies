module RetrieveOpenTasks

open System
open Common_Types
open TaskEnvelope

open Npgsql
open Dapper.FSharp
open Dapper.FSharp.PostgreSQL

// Register optional F# types.
OptionTypes.register()

let taskEnvelopeTable =
    // We can use this function to match tables
    // with different names to our record definitions.
    table'<CommonTaskData> "commontaskdata" |> inSchema "public"
let connectionString = "postgres://postgres:ppassword@localhost/fsharp"

let conn = new NpgsqlConnection(connectionString)

// Generate a TaskEnvelope.
let taskEnvelopes =
    [ {
        Id = (IdType)(Guid.NewGuid().ToString())
        Name = (NameType)"Yakkity Yak"
        Description = (DescriptionType)"Don't talk back"
        Bucket = TaskBucket.FlexType
        Scheduler = SchedulerTwoType
        AppointmentId = (AppointmentIdType)1
        IndividualId = (IndividualIdType)3
        SupportActivityId = (SupportActivityIdType)82
        State = OpenTask
    } ]

// If you were to use ASP.NET core
// you would be running a task or async method.
task {
    /// the `!` here indicates that we will wait
    /// for the `InsertAsync` operation to finish
    let! result =
        // here's the Dapper.FSharp magical DSL
        insert {
            into taskEnvelopeTable
            values taskEnvelopes
        }
        |> conn.InsertAsync

    /// If all goes well you should see
    /// `Rows Affected: 2` in your console
    printfn $"Rows Affected: %i{result}"
} |> ignore

//let readTasks (connectionString: string) : TaskEnvelope list =
// let readTasks (connectionString: string) : SupportActivityTask list =
//     connectionString
//     |> Sql.connect
//     |> Sql.query "SELECT * FROM taskenvelopes"
//     |> Sql.execute (fun read ->
//         {
//             Id = read.string "Id"
//             Name = read.string "Name"
//             Description = read.string "Description"
//             State = OpenTask
//             Bucket = FlexType
//             Scheduler = SchedulerOneType
//             AppointmentId = 1
//             IndividualId = 2
//             SupportActivityId = 4
//         })
//
// //let insertResults = writeTasks connectionString
// let tasks = readTasks connectionString
//
// for task in tasks do
//     printfn "Id(%s) -> {%s}" task.Id task.Description
