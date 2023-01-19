
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using DisprzTraining.Data;
using DisprzTraining.Models;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.Collections.Generic;
using DisprzTraining.CustomExceptions;
namespace DisprzTraining.Tests
{
    public class IntegrationTestControllerService:IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public string URL="api/appointments";
        public IntegrationTestControllerService(){
            var integrationTest= new WebApplicationFactory<Program>();
            _client=integrationTest.CreateClient();
        }

        private void RemoveTestData()
        {
            EventData.meetingData.Clear();
        }
        private StringContent format(Appointment data){
            var serializeObject = JsonConvert.SerializeObject(data);
            var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            return stringContent;
        }
        [Fact]
        public async Task Create_Get_Delete_Appointment_ON_SUCCESS()
        {
            var appointment = new Appointment()
            {
                Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247462"),
                EventName = "Meeting",
                StartTime = new DateTime(2023, 02, 15, 15, 08, 0),
                EndTime = new DateTime(2023, 02, 15, 16, 30, 0),
                EventDescription = "must attend",
                receiverMail=new List<string>(){}
            };
            //Create Appointment
            var stringContent = format(appointment);
            var response =await _client.PostAsync("api/appointments", stringContent);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
            response?.Content?.Headers?.ContentType?.ToString());

            //Get Appointment
            var getResponse=await _client.GetAsync("api/appointments/15-02-2023");
            getResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK,getResponse.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
            response?.Content?.Headers?.ContentType?.ToString());
            var contentResponseType=getResponse.Content.ReadFromJsonAsync<List<Appointment>>();
            var ResponseValue=Assert.IsType<List<Appointment>>(contentResponseType.Result);
            Assert.Single(ResponseValue);
            Assert.Equal(appointment.Id,ResponseValue[0].Id);

            //Delete Appointment
            var deleteResponse=await _client.DeleteAsync("api/appointments/2023-02-15T15:08:00") ;
            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent,deleteResponse.StatusCode);
            var check=await _client.GetAsync("api/appointments/15-01-2023");
            check.EnsureSuccessStatusCode();
            Assert.Empty(check.Content.ReadFromJsonAsync<List<Appointment>>().Result);
        }

        [Fact]
        public async Task Update_SUCCESS(){
            var appointment = new Appointment()
            {
                Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247492"),
                EventName = "Meeting",
                StartTime = new DateTime(2023, 02, 15, 15, 08, 0),
                EndTime = new DateTime(2023, 02, 15, 16, 30, 0),
                EventDescription = "must attend",
                receiverMail=new List<string>(){}
            };
            var updateAppointment=new Appointment()
            {
                Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247492"),
                EventName = "Meeting",
                StartTime = new DateTime(2023, 02, 15, 17, 08, 0),
                EndTime = new DateTime(2023, 02, 15, 18, 30, 0),
                EventDescription = "must attend",
                receiverMail=new List<string>(){}
            };
            var updateAppointment1 = new Appointment()
            {
                Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247492"),
                EventName = "zoom Call",
                StartTime = new DateTime(2023, 02, 16, 15, 08, 0),
                EndTime = new DateTime(2023, 02, 16, 16, 30, 0),
                EventDescription = "must attend",
                receiverMail=new List<string>(){}
            };

            string date = appointment.StartTime.ToString("dd-MM-yyyy");
            string update = updateAppointment.StartTime.ToString("dd-MM-yyyy");
            string update1= updateAppointment1.StartTime.ToString("dd-MM-yyyy");
            var stringContent=format(appointment);
            await _client.PostAsync(URL,stringContent);
            var updateContent=format(updateAppointment);
            var result=await _client.PutAsync($"{URL}/{date}",updateContent);
            result.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK,result.StatusCode);
            var updatedResponse=await _client.GetAsync($"{URL}/{update}");
            Assert.Equivalent(updateAppointment,updatedResponse.Content.ReadFromJsonAsync<List<Appointment>>().Result[0]);
            var updateContent2=format(updateAppointment1);
            var result2=await _client.PutAsync($"{URL}/{update}",updateContent2);
            var updatedResponse2=await _client.GetAsync($"{URL}/{update1}");
            result2.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK,result2.StatusCode);
            Assert.Equivalent(updateAppointment1,updatedResponse2.Content.ReadFromJsonAsync<List<Appointment>>().Result[0]);
            RemoveTestData();
        }
        // [Fact]
        // public async Task Update_Failed(){
        //     var appointment = new Appointment()
        //     {
        //         Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247455"),
        //         EventName = "Meeting",
        //         StartTime = new DateTime(2023, 03, 15, 15, 08, 0),
        //         EndTime = new DateTime(2023, 03, 15, 16, 30, 0),
        //         EventDescription = "must attend",
        //         receiverMail=new List<string>(){"welcome@gmail.com"}
        //     };
        //     var appointment2 = new Appointment()
        //     {
        //         Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247433"),
        //         EventName = "Meeting",
        //         StartTime = new DateTime(2023, 03, 17, 15, 08, 0),
        //         EndTime = new DateTime(2023, 03, 17, 16, 30, 0),
        //         EventDescription = "must attend",
        //         receiverMail=new List<string>(){"welcome@gmail.com"}
        //     };
            
        //     var updateAppointment = new Appointment()
        //     {
        //         Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247455"),
        //         EventName = "zoom Call",
        //         StartTime = new DateTime(2023, 03, 17, 15, 18, 0),
        //         EndTime = new DateTime(2023, 03, 17, 16, 10, 0),
        //         EventDescription = "must attend",
        //         receiverMail=new List<string>(){}
        //     };
            
        //     var updateAppointment2 = new Appointment()
        //     {
        //         Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247455"),
        //         EventName = "zoom Call",
        //         StartTime = new DateTime(2023, 03, 17, 15, 18, 0),
        //         EndTime = new DateTime(2023, 03, 17, 15, 18, 0),
        //         EventDescription = "must attend",
        //         receiverMail=new List<string>(){}
        //     };
        //     var updateAppointment3=new Appointment()
        //     {
        //         Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247455"),
        //         EventName = "Meeting",
        //         StartTime = new DateTime(2023, 03, 15, 17, 08, 0),
        //         EndTime = new DateTime(2023, 03, 15, 16, 30, 0),
        //         EventDescription = "must attend",
        //         receiverMail=new List<string>(){}
        //     };
        //     var updateAppointment4 = new Appointment()
        //     {
        //         Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247412"),
        //         EventName = "zoom Call",
        //         StartTime = new DateTime(2023, 03, 15, 19, 18, 0),
        //         EndTime = new DateTime(2023, 03, 15, 20, 10, 0),
        //         EventDescription = "must attend",
        //         receiverMail=new List<string>(){}
        //     };

        //     string date = appointment.StartTime.ToString("dd-MM-yyyy");
        //     string update = updateAppointment.StartTime.ToString("dd-MM-yyyy");
        //     var stringContent=format(appointment);
        //     var stringContent2=format(appointment2);
        //     await _client.PostAsync(URL,stringContent);
        //     await _client.PostAsync(URL,stringContent2);
        //     var updateContent=format(updateAppointment);
        //     var overLapUpdate=await _client.PutAsync($"{URL}/{date}",updateContent);
        //     Assert.Equal(HttpStatusCode.Conflict,overLapUpdate.StatusCode);
        //     var responseMessage=Assert.IsType<CustomExceptions.MeetingOverLapException>(overLapUpdate.Content.ReadFromJsonAsync<CustomExceptions.MeetingOverLapException>().Result);
        //     Assert.Equivalent(new CustomExceptions.MeetingOverLapException{StatusCode=409,ErrorMessage="Already Meeting is there"},responseMessage);

        //     var updateContent2=format(updateAppointment2);
        //     var mismatchResult=await _client.PutAsync($"{URL}/{date}",updateContent2);
        //     Assert.Equal(HttpStatusCode.BadRequest,mismatchResult.StatusCode);
        //     var responseMessage2=Assert.IsType<CustomExceptions.DateTimeMisMatchException>(mismatchResult.Content.ReadFromJsonAsync<CustomExceptions.DateTimeMisMatchException>().Result);
        //     Assert.Equivalent(new CustomExceptions.DateTimeMisMatchException{StatusCode=400,ErrorMessage="Given EndTime is not greater than the StartTime"},responseMessage2);

        //     var updateContent3=format(updateAppointment3);
        //     var mismatchResult2=await _client.PutAsync($"{URL}/{date}",updateContent3);
        //     Assert.Equal(HttpStatusCode.BadRequest,mismatchResult2.StatusCode);
        //     var responseMessage3=Assert.IsType<CustomExceptions.DateTimeMisMatchException>(mismatchResult2.Content.ReadFromJsonAsync<CustomExceptions.DateTimeMisMatchException>().Result);
        //     Assert.Equivalent(new CustomExceptions.DateTimeMisMatchException{StatusCode=400,ErrorMessage="Given EndTime is not greater than the StartTime"},responseMessage3);

        //     var updateContent4=format(updateAppointment4);
        //     var IdNotFoundResult=await _client.PutAsync($"{URL}/{date}",updateContent4);
        //     Assert.Equal(HttpStatusCode.BadRequest,IdNotFoundResult.StatusCode);
        //     var responseMessage4=Assert.IsType<CustomExceptions.NotIdFoundException>(IdNotFoundResult.Content.ReadFromJsonAsync<CustomExceptions.NotIdFoundException>().Result);
        //     Assert.Equivalent(new CustomExceptions.NotIdFoundException{StatusCode=400,ErrorMessage="Meeting ID is not found"},responseMessage4);
        //     RemoveTestData();
        // }
        [Fact]
        public async Task GetHolidays(){
            string success_date="01-01-2023";
            var result= await _client.GetAsync($"{URL}/holiday/{success_date}");
            result.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK,result.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
            result?.Content?.Headers?.ContentType?.ToString());
            Assert.Equal(EventData.holidays[success_date],result?.Content.ReadFromJsonAsync<List<string>>().Result);
            string empty_date="09-01-2023";
             var result2= await _client.GetAsync($"{URL}/holiday/{empty_date}");
            result2.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK,result2.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
            result2?.Content?.Headers?.ContentType?.ToString());
            Assert.Empty(result2.Content.ReadFromJsonAsync<List<string>>().Result);
        }

        // [Fact]
        // public async Task Search_Success_Failure(){
        //     //Saerch the Appointment by  -- NAME --   and  -- DATE --
        //     var appointment = new Appointment()
        //     {
        //         Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247407"),
        //         EventName = "Meeting",
        //         StartTime = new DateTime(2023, 02, 19, 15, 08, 0),
        //         EndTime = new DateTime(2023, 02, 19, 16, 30, 0),
        //         EventDescription = "must attend",
        //         receiverMail=new List<string>(){"welcome@gmail.com"}
        //     };
        //     string data = "meet", type = "Name";
        //     string date = "15-02-2023", typ = "date";
        //     string failDate = "11-01-2023", faildata = "hello", InvalidDate = "2023-31-01";
        //     var stringContent=format(appointment);
        //     await _client.PostAsync(URL,stringContent);
        //     var result=await _client.GetAsync($"{URL}/search?search={data}&type={type}");
        //     result.EnsureSuccessStatusCode();
        //     // Assert.Equal("application/json; charset=utf-8", 
        //     // result?.Content?.Headers?.ContentType?.ToString());
        //     Assert.Equal(HttpStatusCode.OK,result.StatusCode);
        //     // Assert.Equivalent(appointment.Id,result.Content.ReadFromJsonAsync<List<Appointment>>().Result[0].Id);
        //     RemoveTestData();
        //     result.Dispose();
        // }
    }
}