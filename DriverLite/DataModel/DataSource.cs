using System;
using System.Collections.ObjectModel;
using DriverLite.Common;

// ReSharper disable ExplicitCallerInfoArgument

namespace DriverLite.DataModel
{
    public enum Location
    {
        EmptyPark,
        Yard,
        Customer,
        ContainerTerminal
    };

    public enum Status
    {
        Planned,
        Accepted,
        Started,
        ConfirmedContainer,
        DepartedPickup,
        ArrivedDelivery,
        DepartedDelivery,
        Completed
    };

    public class MovementDetail : ObservableObj
    {
        private Guid _movementKey;
        public Guid MovementKey { get { return _movementKey; } set { SetField(ref _movementKey, value); } }

        private Location _pickupLoc;
        public Location PickupLoc { get { return _pickupLoc; } set { SetField(ref _pickupLoc, value); } }

        private string _pickupAddr;
        public string PickupAddr { get { return _pickupAddr; } set { SetField(ref _pickupAddr, value); } }

        private string _pickupAccName;
        public string PickupAccName { get { return _pickupAccName; } set { SetField(ref _pickupAccName, value); } }

        private Location _deliveryLoc;
        public Location DeliveryLoc { get { return _deliveryLoc; } set { SetField(ref _deliveryLoc, value); } }

        private string _deliveryAddr;
        public string DeliveryAddr { get { return _deliveryAddr; } set { SetField(ref _deliveryAddr, value); } }

        private string _deliveryAccName;
        public string DeliveryAccName { get { return _deliveryAccName; } set { SetField(ref _deliveryAccName, value); } }

        private Status _status;
        public Status Status { get { return _status; } set { SetField(ref _status, value); } }

        private string _containerNo;
        public string ContainerNo { get { return _containerNo; } set { SetField(ref _containerNo, value); } }

        public string Journey { get { return PickupLoc + " => " + DeliveryLoc; } }
        public string PickupDesc { get { return PickupAccName + ", " + PickupAddr; } }
        public string DeliveryDesc { get { return DeliveryAccName + ", " + DeliveryAddr; } }
    }

    public class DataSource
    {
        private static readonly ObservableCollection<MovementDetail> _details = new ObservableCollection<MovementDetail>
        {
            new MovementDetail
            {
                MovementKey = Guid.NewGuid(), 

                PickupLoc = Location.Yard,
                PickupAccName = "Rocke",
                PickupAddr = "1 Test St",
                DeliveryLoc = Location.EmptyPark,
                DeliveryAddr = "2 There St",
                DeliveryAccName = "PMC",
                ContainerNo = "OU812"
            },
            new MovementDetail
            {
                MovementKey = Guid.NewGuid(), 
                PickupLoc = Location.Yard,
                PickupAccName = "Tasman",
                PickupAddr = "1 Nutha St",
                DeliveryLoc = Location.EmptyPark,
                DeliveryAddr = "2 There St",
                DeliveryAccName = "PMC",
                ContainerNo = "122RE"
            }
        };

        public static ObservableCollection<MovementDetail> MovementDetails { get { return _details; } }
    }
}
