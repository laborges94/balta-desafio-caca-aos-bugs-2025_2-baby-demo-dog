using BugStore.Models;

namespace BugStore.Requests.Orders;

public class Create
{
    public Guid CustomerId { get; set; }
    public List<OrderLine> Lines { get; set; } = [];
}