using System;
using System.Collections.Generic;
using System.Text;

namespace BikesIsland.Integrations.Common
{
    public static class OperationErrorDictionary
    {
        public static class BikeReservation
        { 
            public static OperationError BikeAlreadyReserved() =>
               new OperationError("Unfortunately the car was already reserved by another client in this specific term.");

            public static OperationError BikeDoesNotExist() =>
               new OperationError("Unfortunately the car specified in the reservation does not exist in out catalog.");
        }
    }
}
