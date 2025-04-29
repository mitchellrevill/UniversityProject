using System.Net;
using System.Security.Claims;
using System.Text;
using Moq;
using Newtonsoft.Json;
using UniversityProject.Model;
using Xunit;

public class MethodHandleTest
{
    [Fact]
    public async Task Test_GetEmployees_Success()
    {
        // Arrange  
        var mockRequest = new Mock<HttpListenerRequest>();
        var mockResponse = new Mock<HttpListenerResponse>();
        var mockOutputStream = new Mock<Stream>();
        mockResponse.Setup(r => r.OutputStream).Returns(mockOutputStream.Object);

        mockRequest.Setup(r => r.HttpMethod).Returns("GET");
        mockRequest.Setup(r => r.Url).Returns(new Uri("http://localhost/GetAllEmployees"));

        // Act  
        await MethodHandle.GetEmployees(mockRequest.Object, mockResponse.Object);

        // Assert  
        mockResponse.VerifySet(r => r.StatusCode = (int)HttpStatusCode.OK);
        mockOutputStream.Verify(s => s.WriteAsync(It.IsAny<byte[]>(), 0, It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Test_InsertEmployee_Unauthorized()
    {
        // Arrange  
        var mockRequest = new Mock<HttpListenerRequest>();
        var mockResponse = new Mock<HttpListenerResponse>();
        var mockOutputStream = new Mock<Stream>();
        mockResponse.Setup(r => r.OutputStream).Returns(mockOutputStream.Object);

        mockRequest.Setup(r => r.HttpMethod).Returns("POST");
        mockRequest.Setup(r => r.Url).Returns(new Uri("http://localhost/insertEmployee"));
        mockRequest.Setup(r => r.Headers).Returns(new WebHeaderCollection { { "Authorization", "InvalidToken" } });

        // Act  
        await MethodHandle.InsertEmployee(mockRequest.Object, mockResponse.Object);

        // Assert  
        mockResponse.VerifySet(r => r.StatusCode = 401);
        mockOutputStream.Verify(s => s.WriteAsync(It.IsAny<byte[]>(), 0, It.IsAny<int>()), Times.Once);
    }



    [Fact]
    public async Task Test_ServeStaticFile_FileNotFound()
    {
        // Arrange  
        var mockRequest = new Mock<HttpListenerRequest>();
        var mockResponse = new Mock<HttpListenerResponse>();
        var mockOutputStream = new Mock<Stream>();
        mockResponse.Setup(r => r.OutputStream).Returns(mockOutputStream.Object);

        mockRequest.Setup(r => r.Url).Returns(new Uri("http://localhost/nonexistentfile.html"));

        // Act  
        await MethodHandle.ServeStaticFile(mockRequest.Object, mockResponse.Object, "nonexistentfile.html");

        // Assert  
        mockResponse.VerifySet(r => r.StatusCode = (int)HttpStatusCode.NotFound);
        mockOutputStream.Verify(s => s.WriteAsync(It.IsAny<byte[]>(), 0, It.IsAny<int>()), Times.Once);
    }
}
