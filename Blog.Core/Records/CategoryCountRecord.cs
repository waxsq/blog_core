using MediatR;

namespace Blog.Core.Records
{
    public record CategoryCountRecord(List<long> Ids, int Count) : IRequest;
}
