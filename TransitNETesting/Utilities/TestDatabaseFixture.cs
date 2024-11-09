using Microsoft.EntityFrameworkCore;
using TransitNE.Data;
using TransitNE.Models;

namespace TransitNETesting.Utilities
{
    public class TestDatabaseFixture
    {
        private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=EFTestSample;Trusted_Connection=True;ConnectRetryCount=0";

        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public TestDatabaseFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var _context = CreateContext())
                    {
                        _context.Database.EnsureDeleted();
                        _context.Database.EnsureCreated();
                        _context.AddRange(
                            new StopModel { Lng = "-74.96281", Lat = "40.124528", StopId = "13520", StopName = "Street Rd & Kingston Way" },
                            new TrainModel { lat = "39.959355666667", lon = "75.185751166667", trainno = "1717", service = "LOCAL", dest = "Trenton", currentstop = "Gray 30th Street", nextstop = "North Philadelphia Amtrak", line = "Trenton", consist = "282,281,302", heading = "350.14680429744", late = 0, SOURCE = "Jefferson", TRACK = "1", TRACK_CHANGE = "" },
                            new RailScheduleModel { station = "Jefferson Station", sched_tm = "1:30 pm", est_tm = "1:10 pm", act_tm = "1:10 pm" },
                            new BusTrolleySchedule { Stopname = "Street Rd & Kingston Way", Route = "1", date = "8:47a", day = "Wed", Direction = "0", DateCalendar = "10/09/24 08:47 am", DirectionDesc = "Parx Casino via Decatur-Drummond" },
                            new TransitNEUser { Email = "email@email.com", UserName = "userName", FirstName = "firstName", LastName = "lastName" },
                            new BusInputModel {  Busline = "1", StopName = "Street Rd & Kingston Way" },
                            new BusTrolleyModel { Lat = "40.124528", Lon = "-74.96281", Label = "1", VehicleID = "123", BlockID = "123", Direction = "NW", Destination = "Broad", Heading = 123, Late = false, Original_late = false, Offeset_sec = "3", Trip = "123", Next_stop_name = "Broad", Next_stop_sequence = 123, Estimated_seat_availability = "full", Timestamp = null },
                            new BusTrolleyRouteModel { RouteNumber = "111", RouteName = "new new" },
                            new SeptaRailLines { RailLine = "111" }
                            );
                        _context.SaveChanges();
                    }
                    _databaseInitialized = true;
                }
            }
        }

        public TransitNEContext CreateContext()
        {
            return new TransitNEContext(
                        new DbContextOptionsBuilder<TransitNEContext>()
                            .UseSqlite("DataSource=:memory:") // Use SQLite in-memory
                            .Options);
        }
    }
}
