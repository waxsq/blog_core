using MediatR;

namespace Blog.Core.Records
{
    public record CategoryCountRecord(long Id, int Count) : IRequest;
}
