using System.ComponentModel;
namespace DisprzTraining.Enums
{
    public enum ResponseMessage
    {
        [Description("Given StartTime is not greater than the endtime")]
        MismatchDate,
        [Description("Already Meeting is there")]
        DateTimeOverLap,
        [Description("successfully deleted")]
        DeleteSuccess,
        [Description(" No Meeting is in this time")]
        MeetingNotFound,
        [Description("No data is there")]
        NoData,
        [Description("Event Succesfully added")]
        Created,
        [Description("Meeting ID is not found")]
        IdNotFound,
        [Description("No Data results found")]
        DataNotFound
    }
}