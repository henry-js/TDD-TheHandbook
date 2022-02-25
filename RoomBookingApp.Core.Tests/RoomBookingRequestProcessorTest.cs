using System;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;
using Xunit;

namespace RoomBookingApp.Core.Tests;

public class RoomBookingRequestProcessorTest
{
    private readonly RoomBookingRequestProcessor _processor;

    public RoomBookingRequestProcessorTest()
    {
        _processor = new RoomBookingRequestProcessor();
    }

    [Fact]
    public void Should_Return_Room_Booking_Response_With_Request_Values()
    {
        // Arrange
        var bookingRequest = new RoomBookingRequest
        {
            FullName = "John Doe",
            Email = "test@request,com",
            Date = new DateTime(2022, 02, 22),
        };

        // Act
        RoomBookingResult result = _processor.BookRoom(bookingRequest);

        // Assert
        result.ShouldNotBeNull();
        result.FullName.ShouldBe(bookingRequest.FullName);
        result.Email.ShouldBe(bookingRequest.Email);
        result.Date.ShouldBe(bookingRequest.Date);
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
}