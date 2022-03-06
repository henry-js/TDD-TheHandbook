using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Domain;
using RoomBookingApp.Persistence;
using RoomBookingApp.Persistence.Repositories;
using Shouldly;
using Xunit;

namespace RoomBookingApp.Persistence.Tests;

public class RoomBookingServiceTest
{
    [Fact]
    public void Should_Return_Available_Rooms()
    {
        // Arrange
        var date = new DateTime(2022, 03, 06);

        var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
            .UseInMemoryDatabase(databaseName: "AvailableRoomTest")
            .Options;

        using var context = new RoomBookingAppDbContext(dbOptions);
        context.Add(new Room { Id = 1, Name = "Room 1" });
        context.Add(new Room { Id = 2, Name = "Room 2" });
        context.Add(new Room { Id = 3, Name = "Room 3" });

        context.Add(new RoomBooking { RoomId = 3, Date = date });
        context.Add(new RoomBooking { RoomId = 3, Date = date.AddDays(-1) });

        context.SaveChanges();

        var roomBookingService = new RoomBookingService(context);

        // Act
        var availableRooms = roomBookingService.GetAvailableRooms(date);

        // Assert
        availableRooms.Count().ShouldBe(2);
        availableRooms.ShouldContain(r => r.Id == 2);
        availableRooms.ShouldContain(r => r.Id == 3);
        availableRooms.ShouldNotContain(r => r.Id == 1);
    }
}