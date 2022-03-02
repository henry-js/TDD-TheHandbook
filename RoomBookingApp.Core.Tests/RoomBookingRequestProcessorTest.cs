using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;
using Xunit;

namespace RoomBookingApp.Core.Tests;

public class RoomBookingRequestProcessorTest
{
    private readonly RoomBookingRequestProcessor _processor;
    private readonly RoomBookingRequest _bookingRequest;
    private readonly List<Room> _availableRooms;
    private readonly Mock<IRoomBookingService> _roomBookingServiceMock;

    public RoomBookingRequestProcessorTest()
    {
        // Arrange
        _bookingRequest = new RoomBookingRequest
        {
            FullName = "John Doe",
            Email = "test@request,com",
            Date = new DateTime(2022, 02, 22),
        };
        _availableRooms = new List<Room>() { new Room() { Id = 1, } };

        _roomBookingServiceMock = new Mock<IRoomBookingService>();
        _roomBookingServiceMock.Setup(s => s.GetAvailableRooms(_bookingRequest.Date))
            .Returns(_availableRooms);
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
        RoomBooking? savedBooking = null;
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
        savedBooking.RoomId.ShouldBe(_availableRooms[0].Id);
    }

    [Fact]
    public void Should_Not_Save_Room_Booking_Request_If_None_Available()
    {
        _availableRooms.Clear();
        _processor.BookRoom(_bookingRequest);
        _roomBookingServiceMock.Verify((service) => service.Save(It.IsAny<RoomBooking>()), Times.Never);
    }

    [Theory]
    [InlineData(BookingResultFlag.Failure, false)]
    [InlineData(BookingResultFlag.Success, true)]
    public void Should_Return_SuccessOrFailure_Flag_In_Result(BookingResultFlag bookingResultFlag, bool isAvailable)
    {
        if (!isAvailable)
        {
            _availableRooms.Clear();
        }

        var result = _processor.BookRoom(_bookingRequest);
        bookingResultFlag.ShouldBe(result.Flag);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(null, false)]
    public void Should_Return_RoomBookingId_In_Result(int? roomBookingId, bool isAvailable)
    {
        if (!isAvailable)
        {
            _availableRooms.Clear();
        }

        var result = _processor.BookRoom(_bookingRequest);
        result.RoomBookingId.ShouldBe(roomBookingId);
    }
}