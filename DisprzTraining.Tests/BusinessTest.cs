
using DisprzTraining.DataAccess;
using Moq;
using DisprzTraining.Models;
using DisprzTraining.Business;
namespace DisprzTraining.Tests.TestControllers
{
    public class BusinessTest
    {
        [Fact]
        public async Task CreateAppointment_On_Success_ReturnTrue(){
            var Mock = new Mock<IAppointmentDAL>();
            var mockModel=new Appointment(){EventName="santhosh",StartTime=new DateTime().ToLocalTime(),
            EndTime=DateTime.Now.AddHours(2),EventDate="2022-01-02",EventDescription="hello"};
            Mock.Setup(service=>service.CreateAppointment(mockModel)).ReturnsAsync(true);
            var mockServices=new AppointmentBL(Mock.Object);
            var result=await mockServices.createAppointment(mockModel);
            Assert.Equal(result,true);
        }
        [Fact]
        public async Task CreateAppointment_On_DateTimeMisMatch_Exception()
        {
            //Arrange
            var Mock = new Mock<IAppointmentDAL>();
            var mockModel=new Appointment(){EventName="santhosh",StartTime=new DateTime().ToLocalTime(),
            EndTime=DateTime.Now,EventDate="2022-01-02",EventDescription="hello"};
            Mock.Setup(service => service.CreateAppointment(mockModel)).ThrowsAsync(new CustomExceptions.DateTimeMisMatchException());
            var mockServices = new AppointmentBL(Mock.Object);
            //Act
            var result = async () =>{await mockServices.createAppointment(mockModel);};
            //Assert
           await Assert.ThrowsAsync<CustomExceptions.DateTimeMisMatchException>(result);
      
            
        }
        [Fact]
        public async Task CreateAppointment_On_MeetingOverLap_Exception()
        {
            //Arrange
            var Mock = new Mock<IAppointmentDAL>();
            var mockModel=new Appointment(){EventName="santhosh",StartTime=new DateTime().ToLocalTime(),
            EndTime=DateTime.Now.AddHours(2),EventDate="2022-01-02",EventDescription="hello"};
            Mock.Setup(service => service.CreateAppointment(mockModel)).ThrowsAsync(new CustomExceptions.MeetingOverLapException());
            var mockServices = new AppointmentBL(Mock.Object);
            //Act
            var result = async () =>{await mockServices.createAppointment(mockModel);};
            //Assert
           await Assert.ThrowsAsync<CustomExceptions.MeetingOverLapException>(result); 
        }
        [Fact]
        public async  Task GetAppointment_On_Success_ReturnList(){
            var Mock = new Mock<IAppointmentDAL>();
            Mock.Setup(service=>service.GetAppointments()).ReturnsAsync(TestData.TestData.getAllAppoinmentsWithData());
            var mockServices=new AppointmentBL(Mock.Object);
            var result= await mockServices.getAppoinments();
            Assert.IsType<List<Appointment>>(result);
        }
        [Fact]
        public async Task DeleteAppointment_OnReturnAppointment(){
            var Mock =new Mock<IAppointmentDAL>();
            Mock.Setup(service=>service.DeleteAppointment(new Guid())).Returns(true);
            var mockServices=new AppointmentBL(Mock.Object);
            var result=await mockServices.DeleteAppointment(new Guid());
            Assert.Equal(true,result);
        }
        [Fact]
        public async Task DeleteAppointment_OnReturnEmpty(){
            var Mock =new Mock<IAppointmentDAL>();
            Mock.Setup(service=>service.DeleteAppointment(new Guid())).Returns(false);
            var mockServices=new AppointmentBL(Mock.Object);
            var result=await mockServices.DeleteAppointment(new Guid());
            Assert.Equal(false,result);
        }
        [Fact]
        public async Task GetAppointmentByRange_OnReturnList(){
            var Mock = new Mock<IAppointmentDAL>();
            Mock.Setup(service=>service.GetAppointmentsByRange(new DateTime())).Returns(TestData.TestData.getAllAppoinmentsWithData());
            var mockServices=new AppointmentBL(Mock.Object);
            var result= await mockServices.getAppointmentsByRange(new DateTime());
            Assert.IsType<List<Appointment>>(result);
        }


   }
}