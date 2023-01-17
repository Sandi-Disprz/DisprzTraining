
using Microsoft.AspNetCore.Mvc;
using Moq;
using DisprzTraining.Controllers;
using DisprzTraining.Business;
using FluentAssertions;
using DisprzTraining.Models;
using DisprzTraining.CustomExceptions;
namespace DisprzTraining.Tests.TestControllers
{
    [Route("[controller]")]
    public class TestControllers : Controller
    {
        [Fact]
        public async Task CreateAppointment_Onsuccess_Return200(){
            var mockServices=new Mock<IAppointmentBL>();
            var mockModel=new Appointment(){EventName="santhosh",StartTime=new DateTime().ToLocalTime(),
            EndTime=DateTime.Now.AddHours(2),EventDate="2022-01-02"};
            mockServices.Setup(service=>service.createAppointment(mockModel)).ReturnsAsync(true);
            var appointmentService=new AppointmentsController(mockServices.Object);
            var result=(CreatedResult)await appointmentService.createAppointment(mockModel);
            result.StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task CreateAppointment_OnException_Return400BadRequest(){
            var mockServices=new Mock<IAppointmentBL>();
            var mockModel=new Appointment(){EventName="santhosh",StartTime=new DateTime().ToLocalTime(),
            EndTime=DateTime.Now,EventDate="2022-01-02"};
            mockServices.Setup(service=>service.createAppointment(mockModel)).ThrowsAsync(new CustomExceptions.DateTimeMisMatchException());
            var appointmentService=new AppointmentsController(mockServices.Object);
            
            var result=(BadRequestObjectResult)await appointmentService.createAppointment(mockModel);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateAppointment_OnException_Return409Conflict(){
            var mockServices=new Mock<IAppointmentBL>();
            var mockModel=new Appointment(){EventName="santhosh",StartTime=new DateTime().ToLocalTime(),
            EndTime=DateTime.Now,EventDate="2022-01-02"};
            mockServices.Setup(service=>service.createAppointment(mockModel)).ThrowsAsync(new CustomExceptions.MeetingOverLapException());
            var appointmentService=new AppointmentsController(mockServices.Object);
            
            var result=(ConflictObjectResult)await appointmentService.createAppointment(mockModel);
            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public async Task GetAllAppointments_OnSuccess_Return200(){
            var mockServices=new Mock<IAppointmentBL>();
            mockServices.Setup(service=>service.getAppoinments())
            .ReturnsAsync(TestData.TestData.getAllAppoinmentsWithData());
            var appointmentService=new AppointmentsController(mockServices.Object);
            var result=(OkObjectResult) await appointmentService.getAppointments();
            result.StatusCode.Should().Be(200);
            Assert.IsType<List<Appointment>> (result.Value);

        }

        [Fact]
        public async Task GetAllAppointments_OnFailure_Return400(){
            var mockServices=new Mock<IAppointmentBL>();
            mockServices.Setup(service=>service.getAppoinments())
            .ReturnsAsync(TestData.TestData.GetAllAppointments());
            var appointmentService=new AppointmentsController(mockServices.Object);
            var result=(NotFoundObjectResult)await appointmentService.getAppointments();
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetAllAppoinmentsById_OnSuccess_Return200(){
            var mockServices=new Mock<IAppointmentBL>();
            mockServices.Setup(service=>service.getAppointmentById(new Guid()))
            .ReturnsAsync(TestData.TestData.GetAppointmentById());
            var appointmentService=new AppointmentsController(mockServices.Object);
            var result =(OkObjectResult) await appointmentService.getAppointmnetById(new Guid());
            result.Value.Should().BeEquivalentTo(TestData.TestData.GetAppointmentById());
            result.StatusCode.Should().Be(200);
            Assert.IsType<Appointment>(result.Value);
        }

        
        [Fact]

        public async Task GetAllAppoinmentsById_OnFailure_Return404(){
            var mockServices=new Mock<IAppointmentBL>();
            mockServices.Setup(service=>service.getAppointmentById(new Guid()))
            .ThrowsAsync(new CustomExceptions.NotIdFoundException());
            var appointmentService=new AppointmentsController(mockServices.Object);
            var result=(BadRequestObjectResult)await appointmentService.getAppointmnetById(new Guid());
            Assert.IsType<BadRequestObjectResult>(result);
        
        }
        [Fact]
        public async Task GetAllAppoinmentsByDate_Onsuccess_Return200(){
            var mockServices=new Mock<IAppointmentBL>();
            mockServices.Setup(service=>service.getAppointmentByDate("2022-01-03")).
            ReturnsAsync(TestData.TestData.getAllAppoinmentsWithData());
            var appointmentService=new AppointmentsController(mockServices.Object);
            var result=(OkObjectResult) await appointmentService.getAppointmentByDate("2022-01-03");
            result.Value.Should().BeEquivalentTo(TestData.TestData.getAllAppoinmentsWithData());
            result.StatusCode. Should().Be(200);
            Assert.IsType<List<Appointment>>(result.Value);
        }
        [Fact]
        public async Task GetAllAppoinmentsByDate_OnFailure_Return204(){
            var mockServices=new Mock<IAppointmentBL>();
            mockServices.Setup(service=>service.getAppointmentByDate("2022-01-03")).
            ReturnsAsync(new List<Appointment>(){});
            var appointmentService=new AppointmentsController(mockServices.Object);
            var result=(NoContentResult) await appointmentService.getAppointmentByDate("2022-01-03");
            result.StatusCode. Should().Be(204);
        }
        [Fact]
        public async Task DeleteAppoinmentById_OnSuccess_Return200(){
            var mockServices=new Mock<IAppointmentBL>();
            mockServices.Setup(service=>service.DeleteAppointment(new Guid()))
            .ReturnsAsync(true);
            var appointmentService=new AppointmentsController(mockServices.Object);
            var result =(OkObjectResult) await appointmentService.deleteAppointment(new Guid());
            result.StatusCode.Should().Be(200);
        }
        [Fact]
        public async Task DeleteAppoinmentById_OnFailure_Return400(){
            var mockServices=new Mock<IAppointmentBL>();
            mockServices.Setup(service=>service.DeleteAppointment(new Guid()))
            .ReturnsAsync(false);
            var appointmentService=new AppointmentsController(mockServices.Object);
            var result =(BadRequestObjectResult) await appointmentService.deleteAppointment(new Guid());
            result.StatusCode.Should().Be(400);
        }
        [Fact]
        public async Task SearchAppointmentByName_OnSuccess_Return200(){
            var mockServices=new Mock<IAppointmentBL>();
            mockServices.Setup(service=>service.SearchAppointmentByName("santhosh"))
            .ReturnsAsync(TestData.TestData.getAllAppoinmentsWithData());
            var appointmentService=new AppointmentsController(mockServices.Object);
            var result=(OkObjectResult) await appointmentService.searchAppointmentByName("santhosh");
            result.Value.Should().BeEquivalentTo(TestData.TestData.getAllAppoinmentsWithData());
            result.StatusCode.Should().Be(200);
        }
         [Fact]
        public async Task SearchAppointmentByName_OnFailure_Return400(){
            var mockServices=new Mock<IAppointmentBL>();
            mockServices.Setup(service=>service.SearchAppointmentByName("santhosh"))
            .ReturnsAsync(new List<Appointment>(){});
            var appointmentService=new AppointmentsController(mockServices.Object);
            var result=(NotFoundResult) await appointmentService.searchAppointmentByName("santhosh");
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task UpdateAppointment_OnSuccess_Return200(){
            var mockServices=new Mock<IAppointmentBL>();
            mockServices.Setup(service=>service.UpdateAppointment(TestData.TestData.GetAppointmentById()))
            .ReturnsAsync(true);
            var appointmentService=new AppointmentsController(mockServices.Object);
            var result=(OkObjectResult) await appointmentService.Update(TestData.TestData.GetAppointmentById());
            result.StatusCode.Should().Be(200);
        }
        [Fact]
        public async Task UpdateAppointment_OnException_Return400(){
            var mockServices=new Mock<IAppointmentBL>();
            var mockModel=new Appointment(){EventName="santhosh",StartTime=new DateTime().ToLocalTime(),
            EndTime=DateTime.Now,EventDate="2022-01-02"};
            mockServices.Setup(service=>service.UpdateAppointment(mockModel))
            .ThrowsAsync(new CustomExceptions.DateTimeMisMatchException());
            var appointmentService=new AppointmentsController(mockServices.Object);
            var result=(BadRequestObjectResult) await appointmentService.Update(mockModel);
            result.StatusCode.Should().Be(400);
        }
        [Fact]
        public async Task UpdateAppointment_OnException_Return409(){
            var mockServices=new Mock<IAppointmentBL>();
            var mockModel=new Appointment(){EventName="santhosh",StartTime=new DateTime().ToLocalTime(),
            EndTime=DateTime.Now,EventDate="2022-01-02"};
            mockServices.Setup(service=>service.UpdateAppointment(mockModel))
            .ThrowsAsync(new CustomExceptions.MeetingOverLapException());
            var appointmentService=new AppointmentsController(mockServices.Object);
            var result=(ConflictObjectResult) await appointmentService.Update(mockModel);
            result.StatusCode.Should().Be(409);
        }
        [Fact]
        public async Task GetRange_OnReturn_List(){
            var mockServices=new Mock<IAppointmentBL>();
            mockServices.Setup(service=>service.getAppointmentsByRange(new DateTime()))
            .ReturnsAsync(TestData.TestData.getAllAppoinmentsWithData());
            var appointmentService=new AppointmentsController(mockServices.Object);
            var result=(OkObjectResult) await appointmentService.Range(new DateTime());
            result.StatusCode.Should().Be(200);
        }
    }
}