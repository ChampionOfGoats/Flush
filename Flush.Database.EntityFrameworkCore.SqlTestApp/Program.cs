using System;
using System.Threading.Tasks;
using Flush.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Diagnostics;

namespace Flush.Database.EntityFrameworkCore.SqlTestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json", false)
                .Build();

            var services = new ServiceCollection();
            services.Configure<ApplicationDatabaseConfiguration>(
                configuration.GetSection(ApplicationDatabaseConfiguration.SECTION));
            services.AddLogging(options =>
            {
                options.AddDebug();
            });
            services.AddApplicationDatabaseEFCore();

            var provider = services.BuildServiceProvider();

            var adep = provider.GetRequiredService<IApplicationDatabaseProxy>();

            // Do this 50000 times to simulate load.
            // This is ~962 sessions a week, for a year.
            
            for (int i = 0; i < 50000; i++)
            {
                Console.WriteLine($"Performing cycle number {i + 1}...");

                // Create a room
                IRoom room = await adep.CreateRoom("MyFirstRoom");
                try
                {
                    // Create a session
                    var session = await adep.CreateSession(room.RoomUniqueId);
                    Debug.Assert(session is not null);

                    // VERIFY: Create session returns same session if one is active.
                    var session2 = await adep.CreateSession(room.RoomUniqueId);
                    Debug.Assert(session == session2);

                    // Create some users
                    var uniques = Enumerable.Range(0, 5).Select(i => Guid.NewGuid()).ToList();
                    foreach (var guid in uniques)
                    {
                        adep.CreateParticipant(room.RoomUniqueId, guid.ToString()).GetAwaiter().GetResult();
                    }

                    // Get the room again
                    var room2 = await adep.GetRoom(room.RoomUniqueId);
                    Debug.Assert(room2 is not null);

                    // Get the session again
                    var session3 = await adep.GetActiveSession(room.RoomUniqueId);
                    Debug.Assert(session3 is not null);

                    // Get the participants.
                    foreach (var guid in uniques)
                    {
                        var participant = await adep.GetParticipant(room.RoomUniqueId, guid.ToString());
                        Debug.Assert(participant is not null);
                    }

                    // Get ALL the participants
                    var participants = await adep.GetParticipants(room.RoomUniqueId);
                    Debug.Assert(participants is not null);

                    // Set the room name.
                    adep.SetRoomName(room.RoomUniqueId, "MySecondRoom").GetAwaiter().GetResult();
                    var room3 = await adep.GetRoom(room.RoomUniqueId);
                    Debug.Assert(room3 is not null && room3.Name == "MySecondRoom");

                    // VERIFY: Owner is null before setting.
                    var owner = uniques.First();
                    Debug.Assert(room3.OwnerUniqueId is null);

                    // Set the owner.
                    adep.SetRoomOwner(room.RoomUniqueId, owner.ToString()).GetAwaiter().GetResult();
                    var room4 = await adep.GetRoom(room.RoomUniqueId);
                    Debug.Assert(room4.OwnerUniqueId == owner.ToString());
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    adep.EndActiveSession(room.RoomUniqueId).GetAwaiter().GetResult();
                }
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
