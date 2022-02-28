using System;
using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;
using Xunit;

namespace RoomBookingApp.Core.Tests;

public class RoomBookingRequestProcessorTest
{
    private readonly RoomBookingRequestProcessor _processor;
    private readonly RoomBookingRequest _bookingRequest;
    private readonly Mock<IRoomBookingService> _roomBookingServiceMock;

    public RoomBookingRequestProcessorTest()
    {
        _bookingRequest = new RoomBookingRequest
        {
            FullName = "John Doe",
            Email = "test@request,com",
            Date = new DateTime(2022, 02, 22),
        };

        _roomBookingServiceMock = new Mock<IRoomBookingService>();
        _processor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);
    }

    [Fact]
    public void Should_Return_Room_Booking_Response_With_Request_Values()
    {
        // Arrange

        // Act
        RoomBookingResult result = _processor.BookRoom(_bookingRequest);

        // Assert
        result.ShouldNotBeNull();
        result.FullName.ShouldBe(_bookingRequest.FullName);
        result.Email.ShouldBe(_bookingRequest.Email);
        result.Date.ShouldBe(_bookingRequest.Date);
    }

    [Fact]
    public void Should_Throw_Exception_For_Null_Request()
    {
        // Arrange

        // Act
        // Assert
        var exception = Should.Throw<ArgumentNullException>(() => _processor.BookRoom(null));

        exception.ParamName.ShouldBe("bookingRequest");
    }

    [Fact]
    public void Should_Save_Room_Booking_Request()
    {
        // Arrange
        RoomBooking savedBooking = null;
        _roomBookingServiceMock.Setup(service => service.Save(It.IsAny<RoomBooking>()))
            .Callback<RoomBooking>((booking) => savedBooking = booking);

        // Act
        _processor.BookRoom(_bookingRequest);

        // Assert
        _roomBookingServiceMock.Verify((service) => service.Save(It.IsAny<RoomBooking>()), Times.Once);
        savedBooking.ShouldNotBeNull();
        savedBooking.FullName.ShouldBe(_bookingRequest.FullName);
        savedBooking.Email.ShouldBe(_bookingRequest.Email);
        savedBooking.Date.ShouldBe(_bookingRequest.Date);
    }

    public void Should_Not_Save_Room_Booking_Request_If_None_Available()
    {
        // Arrange
        RoomBooking savedBooking = null;
        _roomBookingServiceMock.Setup(service => service.Save(It.IsAny<RoomBooking>()))
            .Callback<RoomBooking>((booking) => savedBooking = booking);

        // Act
        _processor.BookRoom(_bookingRequest);

        // Assert
        _roomBookingServiceMock.Verify((service) => service.Save(It.IsAny<RoomBooking>()), Times.Once);
        savedBooking.ShouldNotBeNull();
        savedBooking.FullName.ShouldBe(_bookingRequest.FullName);
        savedBooking.Email.ShouldBe(_bookingRequest.Email);
        savedBooking.Date.ShouldBe(_bookingRequest.Date);
    }

}