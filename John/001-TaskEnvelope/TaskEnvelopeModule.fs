module TaskEnvelope

open System
open Microsoft.VisualBasic.CompilerServices

// Instead of a separate task for every combination of booleans (Open, Closed, Completed) instead let's model these as
// task states and there can be a TaskWorkflow that moves the task from state to state.

// .. maybe these should be separate from SupportActivityTaskData so they can optionally have different data.
// .. Feels like that path could lead to difficult to maintain code though.
type OpenTask = OpenTask of string
type ClosedTask = ClosedTask of string
type CompletedTask = CompletedTask of string

type TaskState =
    | OpenTask
    | ClosedTask
    | CompletedTask

// DESIGN ISSUE: I don't yet know the proper way to create a type for common data shared by all task types, so
// there is huge duplication here.

// Make sure we don't have any primitive types that will be difficult to extend later.
type IdType = IdType of string
type NameType = NameType of string
type DescriptionType = DescriptionType of string

// For property Bucket.
type FlexType = FlexType of int
type NotFlexType = NotFlexType of int
type TaskBucket =
    | FlexType
    | NotFlexType

// For property SchedulerType.
type SchedulerOneType = SchedulerOneType of int
type SchedulerTwoType = SchedulerTwoType of int
type SchedulerThreeType = SchedulerThreeType of int
type SchedulerType =
    | SchedulerOneType
    | SchedulerTwoType
    | ShcedulerThreeType

// Some common types.
type AppointmentIdType = AppointmentIdType of int
type IndividualIdType = IndividualIdType of int
type CompletionTimeType = CompletionTimeType of DateTime
type SubmissionTimeType = SubmissionTimeType of DateTime

type ByDateType = ByDateType of DateTime
type OnDateType = OnDateType of DateType
type ByDateOrOnDateType =
    | ByDateType
    | OnDateType

// In reality we should make a type for every possible combination of ByDateOrOnDate with OffsetFrom12AMStart, but let;s simplify here.
type OffsetFrom12AMStartType = OffsetFrom12AMStartType of TimeSpan
type OffsetFrom12AMEndType = OffsetFrom12AMEndType of TimeSpan

// Hold data for SupportActivityTask type.
type SupportActivityIdType = SupportActivityIdType of int
type SupportActivityTask = SupportActivityTaskData of bool

// Hold data for MedicalActivityTask type.
type MedicationIdType = MedicationIdType of int
type SigType = SigType of string
type AdministrationSpecialInstructionsType = AdministrationSpecialInstructionsType of string
type MedicationStrengthType = MedicationStrengthType of string
type GiveAmountType = GiveAmountType of int

// .. Not sure what these are for, so will make them strings.
type AdministrationRouteType = AdministrationRouteType of string
type MedicalTask = MedicalTaskData of bool
type StrengthUnitType = StrengthUnitType of string
type DoseFormType = DoseFormType of string

// Use the types to create the records.
type CommonTaskData = {
    // Common data types.
    Id : IdType
    Name : NameType
    Description : DescriptionType
    Bucket: TaskBucket
    Scheduler: SchedulerType // Shcouldn't this just be an ID to keep the domain objects discrete?
    State : TaskState
    AppointmentId : AppointmentIdType
    IndividualId : IndividualIdType
    SupportActivityId : SupportActivityIdType
    //CompletionDate : CompletionTimeType
    //SubmissionTime : SubmissionTimeType
}

type SupportActivityTaskData =
    CommonTaskData

 type MedicationSpecificTaskData = {
    // Medication specific.
    MedicationId : MedicationIdType
    Sig : SigType
    AdministrationRoute : AdministrationRouteType
    AdministrationSpecialInstructions : AdministrationSpecialInstructionsType
    StrengthUnit : StrengthUnitType
    DoseForm : DoseFormType
    GiveAmount : GiveAmountType
    }

// OR MAYBE
//
type MedicationTaskData = {
    Data : CommonTaskData
    MedicationSpecificData : MedicationSpecificTaskData
}

// type MedicationTaskData = {
//     CommonTaskData * MedicationSpecificTaskData
// }

type TaskEnvelope =
    SupportActivity :  SupportActivityTaskData
    | MedicalTask : MedicationTaskData
