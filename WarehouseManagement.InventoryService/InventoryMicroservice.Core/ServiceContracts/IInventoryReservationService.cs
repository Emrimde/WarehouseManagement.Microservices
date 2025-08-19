using InventoryMicroservice.Core.RabbitMQ.PickingPublisher;

namespace InventoryMicroservice.Core.ServiceContracts
{
    public interface IInventoryReservationService
    {
        Task ProcessOrderAsync(PickingMessage pickingMessage);
    }
}