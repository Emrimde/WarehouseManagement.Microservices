using InventoryMicroservice.Core.Domain.RepositoryContracts;
using InventoryMicroservice.Core.RabbitMQ.PickingPublisher;
using InventoryMicroservice.Core.ServiceContracts;

namespace InventoryMicroservice.Core.Services;
public class InventoryReservationService : IInventoryReservationService
{
    private readonly IInventoryRepository _inventoryRepo;
    private readonly IRabbitMQPickingPublisher _publisher;
    public InventoryReservationService(IInventoryRepository inventoryRepo, IRabbitMQPickingPublisher publisher)
    {
        _inventoryRepo = inventoryRepo;
        _publisher = publisher;
    }
    public async Task ProcessOrderAsync(PickingMessage pickingMessage)
    {
        await Task.WhenAll(
    pickingMessage.Items.Select(item =>
        _inventoryRepo.ReserveQuantity(item.SKU, item.Quantity)
    ));

        _publisher.Publish("inventory.reservated", pickingMessage);
    }
}
