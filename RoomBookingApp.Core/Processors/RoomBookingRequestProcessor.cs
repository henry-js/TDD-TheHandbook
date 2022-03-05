using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Domain;
using RoomBookingApp.Domain.BaseModels;

namespace RoomBookingApp.Core.Processors;

public class RoomBookingRequestProcessor
{
    private readonly IRoomBookingService _roomBookingService;

    public RoomBookingRequestProcessor(IRoomBookingService roomBookingService)
    {
        _roomBookingService = roomBookingService;
    }

    public RoomBookingResult BookRoom(RoomBookingRequest? bookingRequest)
    {
        if (bookingRequest == null)
        {
            throw new ArgumentNullException(nameof(bookingRequest));
        }

        var availableRooms = _roomBookingService.GetAvailableRooms(bookingRequest.Date);
        var result = CreateRoomBookingObject<RoomBookingResult>(bookingRequest);

        if (availableRooms.Any())
        {
            var availableRoom = availableRooms.First();

            var roomBooking = CreateRoomBookingObject<RoomBooking>(bookingRequest);
            roomBooking.RoomId = availableRoom.Id;
            _roomBookingService.Save(roomBooking);

            result.RoomBookingId = roomBooking.Id;
            result.Flag = BookingResultFlag.Success;
        }
        else
        {
            result.Flag = BookingResultFlag.Failure;
        }

        return result;
    }

    private static TRoomBooking CreateRoomBookingObject<TRoomBooking>(RoomBookingRequest bookingRequest)
        where TRoomBooking : RoomBookingBase, new()
    {
        return new TRoomBooking
        {
            FullName = bookingRequest.FullName,
            Email = bookingRequest.Email,
            Date = bookingRequest.Date
        };
    }
}