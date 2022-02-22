using System;
using Xunit;
using RoomBookingApp.Core;
using Shouldly;

namespace RoomBookingApp.Core.Tests;

public class RoomBookingRequestProcessorTest
{
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
        var processor = new RoomBookingRequestProcessor();

        // Act
        RoomBookingResult result = processor.BookRoom(bookingRequest);

        // Assert
        result.ShouldNotBeNull();
        result.FullName.ShouldBe(bookingRequest.FullName);
        result.Email.ShouldBe(bookingRequest.Email);
        result.Date.ShouldBe(bookingRequest.Date);
    }
}